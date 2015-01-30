using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSMユーザロール
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSMユーザロール : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSMユーザロール()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>ユーザID</summary>
        [Key]
        [Column(Order=1)]
        public string ユーザID { get; set; }

        /// <summary>ロールID</summary>
        [Key]
        [Column(Order=2)]
        public string ロールID { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
