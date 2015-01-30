using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSMメニュー
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSMメニュー : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSMメニュー()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>メニューID</summary>
        [Key]
        [Column(Order=1)]
        public string メニューID { get; set; }

        /// <summary>上位メニューID</summary>
        [Required]
        public string 上位メニューID { get; set; }

        /// <summary>画面名</summary>
        public string 画面名 { get; set; }

        /// <summary>URL</summary>
        public string URL { get; set; }

        /// <summary>オプション</summary>
        public string オプション { get; set; }

        /// <summary>空欄フラグ</summary>
        public bool? 空欄フラグ { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
