/*******************************************************************************
 * 【共通部品】
 *
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2015.1.26, 新規作成
 ******************************************************************************/
using System.Runtime.Serialization;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// 検索IDクラス
    /// </summary>
    //************************************************************************
    [DataContract]
    public class SelectId
    {
        #region プロパティ
        /// <summary>検索ID</summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>ソート条件</summary>
        [DataMember]
        public string Order { get; set; }
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argId">検索ID</param>
        /// <param name="argOrder">ソート条件</param>
        //************************************************************************
        public SelectId(string argId, string argOrder)
        {
            Id = argId;
            Order = argOrder;
        }
        #endregion

    }
}
