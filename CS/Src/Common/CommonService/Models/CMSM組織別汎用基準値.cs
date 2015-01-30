using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSM組織別汎用基準値
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSM組織別汎用基準値 : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSM組織別汎用基準値()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>組織CD</summary>
        [Key]
        [Column(Order=1)]
        public string 組織CD { get; set; }

        /// <summary>分類CD</summary>
        [Key]
        [Column(Order=2)]
        public string 分類CD { get; set; }

        /// <summary>基準値CD</summary>
        [Key]
        [Column(Order=3)]
        public string 基準値CD { get; set; }

        /// <summary>分類名</summary>
        [Required]
        public string 分類名 { get; set; }

        /// <summary>基準値名</summary>
        [Required]
        public string 基準値名 { get; set; }

        /// <summary>基準値1</summary>
        public decimal? 基準値1 { get; set; }

        /// <summary>基準値2</summary>
        public decimal? 基準値2 { get; set; }

        /// <summary>基準値3</summary>
        public decimal? 基準値3 { get; set; }

        /// <summary>基準値4</summary>
        public decimal? 基準値4 { get; set; }

        /// <summary>文字項目1</summary>
        public string 文字項目1 { get; set; }

        /// <summary>文字項目2</summary>
        public string 文字項目2 { get; set; }

        /// <summary>文字項目3</summary>
        public string 文字項目3 { get; set; }

        /// <summary>文字項目4</summary>
        public string 文字項目4 { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
