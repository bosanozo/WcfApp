﻿/*******************************************************************************
 * 【共通部品】
 * 
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2014.7.1, 新規作成
 ******************************************************************************/
namespace Common.Models
{
    //************************************************************************
    /// <summary>
    /// テーブル共通項目（作成・更新情報）があることを示すインターフェース
    /// </summary>
    //************************************************************************
    public interface IHasUpdateInfo
    {
        UpdateInfo UpdateInfo { get; set; }
    }
}
