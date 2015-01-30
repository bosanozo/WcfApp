using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSM汎用基準値項目設定
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSM汎用基準値項目設定 : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSM汎用基準値項目設定()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>組織別フラグ</summary>
        [Key]
        [Column(Order=1)]
        public bool 組織別フラグ { get; set; }

        /// <summary>分類CD</summary>
        [Key]
        [Column(Order=2)]
        public string 分類CD { get; set; }

        /// <summary>データ区分</summary>
        [Key]
        [Column(Order=3)]
        public string データ区分 { get; set; }

        /// <summary>基準値NO</summary>
        [Key]
        [Column(Order=4)]
        public int 基準値NO { get; set; }

        /// <summary>項目名</summary>
        [Required]
        public string 項目名 { get; set; }

        /// <summary>桁数</summary>
        public int 桁数 { get; set; }

        /// <summary>必須フラグ</summary>
        public bool 必須フラグ { get; set; }

        /// <summary>ゼロ埋めフラグ</summary>
        public bool ゼロ埋めフラグ { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
