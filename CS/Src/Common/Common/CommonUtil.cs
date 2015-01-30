/*******************************************************************************
 * 【共通部品】
 *
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2014.1.30, 新規作成
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// ユーティリティクラス
    /// </summary>
    //************************************************************************
    public static class CommonUtil
    {
        //************************************************************************
        /// <summary>
        /// ダイアログに表示するメッセージを返す。
        /// </summary>
        /// <param name="argExDetail">ExceptionDetail</param>
        /// <returns>ダイアログに表示するメッセージ</returns>
        //************************************************************************
        public static string GetExceptionMessage(Exception argEx)
        {
            return argEx.Message + (argEx.InnerException != null ?
                System.Environment.NewLine + GetExceptionMessage(argEx.InnerException) : null);
        }

        //************************************************************************
        /// <summary>
        /// DataRowから行番号を取得する。
        /// </summary>
        /// <param name="argDataRow">DataRow</param>
        /// <returns>行番号(行番号なしの場合は0を返す。)</returns>
        //************************************************************************
        public static int GetRowNumber(DataRow argDataRow)
        {
            int rowNumber = 0;
            // 行番号を取得
            if (argDataRow.Table.Columns.Contains("ROWNUMBER"))
            {
                DataRowVersion version =
                    argDataRow.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;
                rowNumber = Convert.ToInt32(argDataRow["ROWNUMBER", version]);
            }

            return rowNumber;
        }

        //************************************************************************
        /// <summary>
        /// DataColumnに対応した型の値を取得する。
        /// </summary>
        /// <param name="dcol">DataColumn</param>
        /// <param name="value">値の文字列</param>
        /// <returns>DataColumnに対応した型の値</returns>
        //************************************************************************
        public static object GetDataColumnVal(DataColumn dcol, string value)
        {
            if (value.Length == 0) return DBNull.Value;

            object result;

            // 型に応じて、値を比較し、DataTableに値を設定する
            switch (dcol.DataType.Name)
            {
                case "bool":
                case "Boolean":
                    // できてない
                    result = value == "true";  //Convert.ToBoolean(value);
                    break;

                case "decimal":
                    result = Convert.ToDecimal(value);
                    break;

                case "int32":
                case "Byte":
                    result = Convert.ToInt32(value);
                    break;

                case "DateTime":
                    result = Convert.ToDateTime(value);
                    break;

                default:
                    result = value;
                    break;
            }

            return result;
        }
    }
}
