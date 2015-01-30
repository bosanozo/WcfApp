/*******************************************************************************
 * 【共通部品】
 * 
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2014.7.1, 新規作成
 ******************************************************************************/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    //************************************************************************
    /// <summary>
    /// テーブル共通項目（作成・更新情報）
    /// </summary>
    //************************************************************************
    public class UpdateInfo
    {
        [Column("作成日時")]
        public DateTime CreateDate { get; set; }

        [Column("作成者ID")]
        public string CreateId { get; set; }

        [Column("作成者IP")]
        public string CreateHost { get; set; }

        [Column("作成PG")]
        public string CreatePg { get; set; }

        [Column("更新日時")]
        public DateTime UpdateDate { get; set; }

        [Column("更新者ID")]
        public string UpdateId { get; set; }

        [Column("更新者IP")]
        public string UpdateHost { get; set; }

        [Column("更新PG")]
        public string UpdatePg { get; set; }

        [Timestamp]
        [Column("排他用バージョン")]
        public byte[] Version { get; set; }

        [NotMapped]
        public int RowNumber { get; set; }
    }
}
