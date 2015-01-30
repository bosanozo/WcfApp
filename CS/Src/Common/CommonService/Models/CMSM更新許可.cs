using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSM更新許可
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSM更新許可 : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSM更新許可()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>組織CD</summary>
        [Key]
        [Column(Order=1)]
        public string 組織CD { get; set; }

        /// <summary>ロールID</summary>
        [Key]
        [Column(Order=2)]
        public string ロールID { get; set; }

        /// <summary>画面ID</summary>
        [Key]
        [Column(Order=3)]
        public string 画面ID { get; set; }

        /// <summary>許否フラグ</summary>
        public bool? 許否フラグ { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
