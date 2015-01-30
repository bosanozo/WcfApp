using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMSMユーザ
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMSMユーザ : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMSMユーザ()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>ユーザID</summary>
        [Key]
        [Column(Order=1)]
        public string ユーザID { get; set; }

        /// <summary>ユーザ名</summary>
        [Required]
        public string ユーザ名 { get; set; }

        /// <summary>パスワード</summary>
        [Required]
        public string パスワード { get; set; }

        /// <summary>メールアドレス</summary>
        public string メールアドレス { get; set; }

        /// <summary>組織CD</summary>
        [Required]
        public string 組織CD { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
