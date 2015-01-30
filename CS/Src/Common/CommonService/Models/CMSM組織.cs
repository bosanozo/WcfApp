using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSM組織
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSM組織 : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSM組織()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>組織CD</summary>
        [Key]
        [Column(Order=1)]
        public string 組織CD { get; set; }

        /// <summary>組織名</summary>
        [Required]
        public string 組織名 { get; set; }

        /// <summary>組織階層区分</summary>
        [Required]
        public string 組織階層区分 { get; set; }

        /// <summary>上位組織CD</summary>
        [Required]
        public string 上位組織CD { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
