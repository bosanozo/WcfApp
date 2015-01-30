using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSTシステム利用状況
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSTシステム利用状況 : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSTシステム利用状況()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>処理日時</summary>
        public DateTime 処理日時 { get; set; }

        /// <summary>画面ID</summary>
        [Required]
        public string 画面ID { get; set; }

        /// <summary>画面名 </summary>
        public string 画面名  { get; set; }

        /// <summary>ユーザID</summary>
        [Required]
        public string ユーザID { get; set; }

        /// <summary>端末ID</summary>
        [Required]
        public string 端末ID { get; set; }

        /// <summary>APサーバ</summary>
        [Required]
        public string APサーバ { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
