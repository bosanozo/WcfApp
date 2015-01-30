using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Common.Models;

namespace Common.DataAccess
{
    //************************************************************************
    /// <summary>
    /// 共通処理データアクセス層
    /// </summary>
    //************************************************************************
    public class CommonDA : BaseDA
    {
        #region private変数
        private BaseDbContext m_dbContext;
        #endregion

        #region SELECT文作成用SQL
        /// <summary>
        /// 共通項目(作成・更新情報)
        /// </summary>
        private const string UPDATE_INFO_COLS =
            "A.作成日時," +
            "A.作成者ID," +
            "US1.ユーザ名 作成者名," +
            "A.作成者IP," +
            "A.作成PG," + "\r\n" +
            "A.更新日時," +
            "A.更新者ID," +
            "US2.ユーザ名 更新者名," +
            "A.更新者IP," +
            "A.更新PG," +
            "A.排他用バージョン";

        /// <summary>
        /// 作成者名、更新者名取得用JOIN
        /// </summary>
        private const string UPDATE_INFO_JOIN =
            "LEFT JOIN CMSMユーザ US1 ON US1.ユーザID = A.作成者ID" + "\r\n" +
            "LEFT JOIN CMSMユーザ US2 ON US2.ユーザID = A.更新者ID";

        /// <summary>
        /// 最大検索件数の項目
        /// </summary>
        private const string ROWNUMBER_COL =
            "ROW_NUMBER() OVER (ORDER BY {0}) - 1 ROWNUMBER";

        /// <summary>
        /// 最大検索件数の条件
        /// </summary>
        private const string ROWNUMBER_CONDITION =
            "WHERE ROWNUMBER <= @最大検索件数 ";
 
        /// <summary>
        /// SELECT文
        /// </summary>
        private const string SELECT_SQL =
            "SELECT " +
            "'0' 削除, A.* " +
            "FROM ({0}) A " +
            "{1}" +
            "ORDER BY ROWNUMBER";

        /// <summary>
        /// 適用期間チェックSELECT文
        /// </summary>
        private const string SELECT_SPAN_SQL =
            "SELECT NULL FROM {0} WHERE 適用終了日 >= TO_CHAR(@1, 'YYYYMMDD') AND 適用開始日 <= TO_CHAR(@1, 'YYYYMMDD')";
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argDbContext">DbContext</param>
        //************************************************************************
        public CommonDA(BaseDbContext argDbContext)
        {
            m_dbContext = argDbContext;
        }
        #endregion

        #region publicメソッド
        //************************************************************************
        /// <summary>
        /// ログイン処理を行う。
        /// </summary>
        /// <param name="argUserName">ユーザID</param>
        /// <param name="argPassword">パスワード</param>
        /// <returns>true:成功, false:失敗</returns>
        //************************************************************************
        public bool Login(string argUserName, string argPassword)
        {
            bool success = m_dbContext.CMSMユーザ.Where(r => r.ユーザID == argUserName && r.パスワード == argPassword).Count() > 0;

            if (success) InformationManager.UserInfo = GetUserInfo(argUserName);

            return success;
        }

        //************************************************************************
        /// <summary>
        /// ユーザ情報を検索する。
        /// </summary>
        /// <param name="argId">ユーザID</param>
        /// <returns>ユーザ情報</returns>
        //************************************************************************
        public UserInformation GetUserInfo(string argId)
        {
            var result = m_dbContext.Database.SqlQuery<UserInformation>(Properties.Resources.UserInfo, argId).First();

            var roles = from r in m_dbContext.CMSMユーザロール
                        where r.ユーザID == argId
                        select r.ロールID;

            result.Roles = roles.ToArray();

            return result;
        }

        //************************************************************************
        /// <summary>
        /// 最大検索件数を返す。
        /// </summary>
        /// <param name="argId">画面ID</param>
        /// <returns>最大検索件数</returns>
        //************************************************************************
        public int GetMaxRow(string argId = null)
        {
            if (argId == null) argId = InformationManager.ClientInfo.FormId;

            var result = from r in m_dbContext.CMSM汎用基準値
                         where r.分類CD == "V001" && r.基準値CD == argId
                         select r.基準値1;

            if (result.Count() > 0 && result.First().HasValue)
                return Convert.ToInt32(result.First().Value);
            else return 1000;
        }

        //************************************************************************
        /// <summary>
        /// 指定された検索IDのSQLファイルからSELECT文を作成し、検索を実行する。
        /// </summary>
        /// <param name="argSelectIdList">検索IDリスト</param>
        /// <param name="argParamList">検索条件リスト</param>
        /// <param name="argSelectType">検索種別</param>
        /// <param name="argMaxRow">最大検索件数</param>
        /// <param name="argIsOver">最大検索件数オーバーフラグ</param>
        /// <returns>検索結果</returns>
        //************************************************************************
        public DataSet Select(List<SelectId> argSelectIdList, List<SelectParam> argParamList,
            SelectType argSelectType, int argMaxRow, out bool argIsOver)
       {
            // データセットの作成
            DataSet result = new DataSet();

            var regex = new Regex("-- (\\w+) [[]検索条件[]]");

            foreach (var selectId in argSelectIdList)
            {
                // ２テーブル目以降は最大検索件数の条件をつけない
                var selectType = argSelectIdList.IndexOf(selectId) > 0 && argSelectType == SelectType.Limited ?
                    SelectType.All : argSelectType; 

                // SQL文を取得
                var sqltext = Properties.Resources.ResourceManager.GetString(selectId.Id);

                // 検索条件があるかチェック
                var match = regex.Match(sqltext);

                // SQL文を取得
                var sql = new StringBuilder(sqltext);

                // 共通項目、ROWNUMBERの項目を設定
                sql.Replace("-- [共通項目]", "," + UPDATE_INFO_COLS + "," + Environment.NewLine +
                    string.Format(ROWNUMBER_COL, selectId.Order));

                // ROWNUMBERの項目を設定
                sql.Replace("-- [ROWNUMBER]", "," + string.Format(ROWNUMBER_COL, selectId.Order));

                // 共通項目のJOINを設定
                sql.Replace("-- [共通JOIN]", UPDATE_INFO_JOIN);

                // 検索IDが一致するものと検索IDなしを抽出
                var param = from p in argParamList
                        where p.SelectId == selectId.Id
                           || string.IsNullOrEmpty(p.SelectId)
                        select p;

                // WHERE句作成
                if (match.Success)
                {
                    StringBuilder where = new StringBuilder();
                    AddWhere(where, param);
                    if (where.Length > 0)
                        sql.Replace("-- " + match.Groups[1].Value + " [検索条件]", match.Groups[1].Value + " " + where);
                }

                // SELECT文の設定
                IDbCommand cmd = CreateCommand(string.Format(SELECT_SQL, sql,
                    selectType == SelectType.Limited ? ROWNUMBER_CONDITION : ""), m_dbContext);
                DataAdapter.SelectCommand = cmd;

                // パラメータの設定
                SetParameter(cmd, param);
                // 最大検索件数のパラメータ
                if (selectType == SelectType.Limited)
                    cmd.Parameters.Add(CreateCmdParam("最大検索件数", argMaxRow));

                // データの取得
                DataAdapter.Fill(result);
                // テーブル名を設定
                result.Tables["Table"].TableName = selectId.Id;
            }

            // 最初のデータテーブルで検索件数オーバーを判定
            int cnt = result.Tables[0].Rows.Count;

            // 最大検索件数オーバーの場合、最終行を削除
            if (argSelectType == SelectType.Limited && cnt >= argMaxRow)
            {
                argIsOver = true;
                result.Tables[0].Rows.RemoveAt(cnt - 1);
            }
            else argIsOver = false;

            // 検索結果の返却
            return result;
        }

        //************************************************************************
        /// <summary>
        /// データセットのデータを登録する。
        /// </summary>
        /// <param name="argDataSet">登録データ</param>
        /// <returns>処理件数</returns>
        //************************************************************************
        public int Update(DataSet argDataSet)
        {
            int cnt = 0;

            foreach (DataTable table in argDataSet.Tables)
            {
                // 動的に処理対象のエンティティに応じた型を取得
                var entityType = Type.GetType("Common.Models." + table.TableName);
                var listType = typeof(List<>).MakeGenericType(entityType);
                var method = listType.GetMethod("get_Item", new Type[] { typeof(int) });

                // Listに変換
                var entityList = AutoMapper.Mapper.DynamicMap(
                    table.CreateDataReader(), typeof(IDataReader), listType);

                // 各レコード毎の処理
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var entity = method.Invoke(entityList, new object[] { i });
                    DataRow row = table.Rows[i];

                    // レコードの状態に応じてDbContextに登録
                    if (row.RowState == DataRowState.Added)
                        m_dbContext.Entry(entity).State = EntityState.Added;
                    else
                    {
                        // 更新情報を設定
                        var huEntity = entity as IHasUpdateInfo;
                        if (huEntity != null)
                        {
                            var updateInfo = huEntity.UpdateInfo;
                            updateInfo.CreateDate = (DateTime)row["作成日時"];
                            updateInfo.CreateId = row["作成者ID"].ToString();
                            updateInfo.CreateHost = row["作成者IP"].ToString();
                            updateInfo.CreatePg = row["作成PG"].ToString();
                            updateInfo.Version = (byte[])row["排他用バージョン"];
                            updateInfo.RowNumber = CommonUtil.GetRowNumber(row);
                        }

                        if (row["削除"].ToString() == "1")
                            m_dbContext.Entry(entity).State = EntityState.Deleted;
                        else
                            m_dbContext.Entry(entity).State = EntityState.Modified;
                    }
                }
            }

            m_dbContext.SaveChanges();

            return cnt;
        }
        #endregion
    }
}
