using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSM業務日付
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSM業務日付 : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSM業務日付()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>組織CD</summary>
        [Key]
        [Column(Order=1)]
        public string 組織CD { get; set; }

        /// <summary>差分日数</summary>
        public int 差分日数 { get; set; }

        /// <summary>切替時間</summary>
        [Required]
        public string 切替時間 { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
