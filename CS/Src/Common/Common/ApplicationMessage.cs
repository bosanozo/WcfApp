/*******************************************************************************
 * 【共通部品】
 *
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2014.1.30, 新規作成
 ******************************************************************************/
using System;
using System.Runtime.Serialization;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// メッセージデータ
    /// </summary>
    //************************************************************************
    [Serializable]
    [DataContract]
    public class ApplicationMessage
    {
        #region プロパティ
        /// <summary>
        /// メッセージコード
        /// </summary>
        [DataMember]
        public string MessageCd { get; set; }

        /// <summary>
        /// データテーブル名、行番号、フィールドデータ
        /// </summary>
        [DataMember]
        public RowField RowField { get; set; }

        /// <summary>
        /// パラメータ
        /// </summary>
        [DataMember]
        public object[] Params { get; set; }
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// メッセージデータを生成する。
        /// </summary>
        /// <param name="argMsgCode">メッセージコード</param>
        /// <param name="argParams">パラメータ</param>
        //************************************************************************
        public ApplicationMessage(string argMsgCode, params object[] argParams)
        {
            MessageCd = argMsgCode;
            Params = argParams;
        }

        //************************************************************************
        /// <summary>
        /// コンストラクタ（行番号指定）
        /// メッセージデータを生成する。
        /// </summary>
        /// <param name="argMsgCode">メッセージコード</param>
        /// <param name="argRowField">行番号</param>
        /// <param name="argParams">パラメータ</param>
        //************************************************************************
        public ApplicationMessage(string argMsgCode, RowField argRowField,
            params object[] argParams) : this(argMsgCode, argParams)
        {
            RowField = argRowField;
        }
        #endregion

        //************************************************************************
        /// <summary>
        /// メッセージ文字列を返す。
        /// </summary>
        //************************************************************************
        public override string ToString()
        {
            return MessageManager.GetMessage(MessageCd, Params);
        }
    }
}
