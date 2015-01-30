using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSTシステム情報
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSTシステム情報 : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSTシステム情報()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>記事NO</summary>
        [Key]
        [Column(Order=1)]
        public int 記事NO { get; set; }

        /// <summary>組織CD</summary>
        [Required]
        public string 組織CD { get; set; }

        /// <summary>重要度</summary>
        [Required]
        public string 重要度 { get; set; }

        /// <summary>内容</summary>
        public string 内容 { get; set; }

        /// <summary>表示期間From</summary>
        public DateTime? 表示期間From { get; set; }

        /// <summary>表示期間To</summary>
        public DateTime? 表示期間To { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
