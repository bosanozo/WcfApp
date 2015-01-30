using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //**************************************************************************
    /// <summary>
    /// CMST監査証跡
    /// </summary>
    //**************************************************************************
    [Serializable]
    public class CMST監査証跡 : IHasUpdateInfo
    {
        /// <summary>コンストラクタ</summary>
        public CMST監査証跡()
        {
            UpdateInfo = new UpdateInfo();
        }

        /// <summary>テーブル名</summary>
        public string テーブル名 { get; set; }

        /// <summary>更新区分</summary>
        public string 更新区分 { get; set; }

        /// <summary>キー</summary>
        public string キー { get; set; }

        /// <summary>内容</summary>
        public string 内容 { get; set; }

        /// <summary>共通項目（更新情報）</summary>
        public UpdateInfo UpdateInfo { get; set; }
    }
}
