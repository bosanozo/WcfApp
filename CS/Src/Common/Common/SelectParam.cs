/*******************************************************************************
 * 【共通部品】
 *
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2014.1.30, 新規作成
 ******************************************************************************/
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// 検索条件クラス
    /// </summary>
    //************************************************************************
    [DataContract]
    public class SelectParam
    {
        #region プロパティ
        /// <summary>項目名</summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>検索条件SQL</summary>
        [DataMember]
        public string Condtion { get; set; }

        /// <summary>プレースフォルダに設定するFrom値</summary>
        [DataMember]
        public object FromValue { get; set; }

        /// <summary>プレースフォルダに設定するTo値</summary>
        [DataMember]
        public object ToValue { get; set; }

        /// <summary>左辺項目名(leftcol = @name)</summary>
        [DataMember]
        public string LeftCol { get; set; }

        /// <summary>検索条件を追加する検索ID</summary>
        /// <remarks>未指定の場合は全テーブルの検索に条件を追加する。</remarks>
        [DataMember]
        public string SelectId { get; set; }
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argName">項目名</param>
        /// <param name="argCondtion">検索条件SQL</param>
        /// <param name="argFrom">プレースフォルダに設定するFrom値</param>
        /// <param name="argTo">プレースフォルダに設定するTo値</param>
        //************************************************************************
        public SelectParam(string argName, string argCondtion, object argFrom, object argTo = null)
        {
            Name = argName;
            Condtion = argCondtion;
            FromValue = argFrom;
            ToValue = argTo;
        }
        #endregion

        //************************************************************************
        /// <summary>
        /// 検索パラメータ作成
        /// </summary>
        /// <returns>検索パラメータ</returns>
        //************************************************************************
        public static List<SelectParam> CreateSelectParam(
            string Name, string Code, string Params, string DbCodeCol, string DbNameCol, string CodeId)
        {
            // 画面の条件を取得
            var formParam = new List<SelectParam>();

            if (!string.IsNullOrEmpty(Name))
                formParam.Add(new SelectParam("Name", "LIKE @Name", "%" + Name + "%"));

            if (!string.IsNullOrEmpty(Code))
                formParam.Add(new SelectParam("Code", "= @Code", Code));

            // 検索コード名
            var codeCol = Regex.Replace(CodeId, "(From|To)", "");
            var nameCol = Regex.Replace(codeCol, "(CD|ID)", "名");

            // 項目名の置き換え
            foreach (var p in formParam)
            {
                if (p.Name == "Code") p.Name = string.IsNullOrEmpty(DbCodeCol) ?
                    codeCol : DbCodeCol;
                else if (p.Name == "Name")
                {
                    p.Name = string.IsNullOrEmpty(DbNameCol) ?
                       nameCol : DbNameCol;
                    p.Condtion = "LIKE @" + p.Name;
                    p.FromValue = "%" + p.FromValue + "%";
                }
            }

            // 検索パラメータ作成
            var param = new List<SelectParam>();

            // 追加パラメータがある場合、追加する
            if (!string.IsNullOrEmpty(Params))
            {
                foreach (string p in Params.Split())
                {
                    object value;

                    // "#"から始まる場合はUserInfoから設定
                    if (p[0] == '#')
                    {
                        PropertyInfo pi = InformationManager.UserInfo.GetType().GetProperty(p.Substring(1));
                        value = pi.GetValue(InformationManager.UserInfo, null);
                    }
                    // セルの値を取得
                    else value = p;

                    // パラメータ追加
                    param.Add(new SelectParam(null, null, value));
                }
            }

            // 画面の条件を追加
            param.AddRange(formParam);

            return param;
        }
    }
}
