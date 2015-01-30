using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.DataAccess;
using Common.Unity;

namespace Common.Service
{
    //************************************************************************
    /// <summary>
    /// 共通処理サービス
    /// </summary>
    //************************************************************************
    public class CommonService : ICommonService 
    {
        #region private変数
        private CommonDA m_dataAccess;
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argDataAccess">データアクセス層インスタンス</param>
        //************************************************************************
        public CommonService(CommonDA argDataAccess)
        {
            m_dataAccess = argDataAccess;
        }
        #endregion

        #region サービスインターフェース実装
        //************************************************************************
        /// <summary>
        /// ユーザ情報を取得する。
        /// </summary>
        /// <returns>ユーザ情報</returns>
        //************************************************************************
        public UserInformation GetUserInfo()
        {
            return InformationManager.UserInfo;
        }

        //************************************************************************
        /// <summary>
        /// 指定された検索IDのSQLファイルからSELECT文を作成し、検索を実行する。
        /// </summary>
        /// <param name="argSelectIdList">検索IDリスト</param>
        /// <param name="argParamList">検索条件リスト</param>
        /// <param name="argSelectType">検索種別</param>
        /// <param name="argMessage">返却メッセージ</param>
        /// <returns>検索結果</returns>
        //************************************************************************
        public DataSet Select(List<SelectId> argSelectIdList, List<SelectParam> argParamList,
            SelectType argSelectType, out ApplicationMessage argMessage)
        {
            // 最大検索件数取得
            int maxRow = m_dataAccess.GetMaxRow();

            // 検索実行
            bool isOver;
            var result = m_dataAccess.Select(argSelectIdList, argParamList, argSelectType, maxRow, out isOver);

            argMessage = null;
            // 検索結果なし
            if (result.Tables[0].Rows.Count == 0) argMessage = new ApplicationMessage("IV001");
            // 最大検索件数オーバー
            else if (isOver) argMessage = new ApplicationMessage("IV002");

            System.Threading.Thread.Sleep(5000);

            return result;
        }

        //************************************************************************
        /// <summary>
        /// データセットのデータを登録する。
        /// </summary>
        /// <param name="argDataSet">登録データ</param>
        /// <param name="argMessage">返却メッセージ</param>
        /// <returns>処理件数</returns>
        //************************************************************************
        [UseTransaction]
        public int Update(DataSet argDataSet, out ApplicationMessage argMessage)
        {
            argMessage = null;

            return m_dataAccess.Update(argDataSet);
        }
        #endregion
    }
}
