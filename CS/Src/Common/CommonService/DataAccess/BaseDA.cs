using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using log4net;

namespace Common.DataAccess
{
    //************************************************************************
    /// <summary>
    /// 基底データアクセス層
    /// </summary>
    //************************************************************************
    public class BaseDA
    {
        #region ロガーフィールド
        private ILog m_logger;
        #endregion
            
        #region プロパティ
        /// <summary>ロガー</summary>
        protected ILog Log
        {
            get { return m_logger; }
        }

        /// <summary>DBプロバイダファクトリ</summary>
        protected DbProviderFactory ProviderFactory { get; set; }

        /// <summary>データアダプタ</summary>
        protected IDbDataAdapter DataAdapter { get; set; }
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public BaseDA()
        {
            // ロガーを取得
            m_logger = LogManager.GetLogger(this.GetType());

            // DBプロバイダファクトリ作成
            ProviderFactory = DbProviderFactories.GetFactory(Properties.Settings.Default.DbProviderFactory);
            // データアダプタはfactoryから作成する
            DataAdapter = ProviderFactory.CreateDataAdapter();
        }
        #endregion

        #region protectedメソッド
        //************************************************************************
        /// <summary>
        /// DBプロバイダファクトリに関連付けられたCommandを作成する。
        /// </summary>
        /// <param name="argCommandText">Commandに設定するSQL文</param>
        /// <param name="argDbContext">DbContext</param>
        /// <returns>DBプロバイダファクトリに関連付けられたCommand</returns>
        //************************************************************************
        protected IDbCommand CreateCommand(string argCommandText, DbContext argDbContext)
        {
            IDbCommand cmd = ProviderFactory.CreateCommand();
            cmd.CommandText = argCommandText;
            cmd.Connection = argDbContext.Database.Connection;

            return cmd;
        }

        //************************************************************************
        /// <summary>
        /// DBプロバイダファクトリに関連付けられたCommandパラメータを作成する。
        /// </summary>
        /// <param name="argParameterName">パラメータ名</param>
        /// <param name="argValue">値</param>
        /// <returns>DBプロバイダファクトリに関連付けられたCommandパラメータ</returns>
        //************************************************************************
        protected IDbDataParameter CreateCmdParam(string argParameterName, object ParameterName)
        {
            IDbDataParameter param = ProviderFactory.CreateParameter();
            param.ParameterName = argParameterName;
            param.Value = ParameterName;
            return param;
        }

        //************************************************************************
        /// <summary>
        /// StringBuilderに検索条件を追加する。
        /// </summary>
        /// <param name="argWhereSb">検索条件を追加するStringBuilder</param>
        /// <param name="argParam">検索条件</param>
        /// <remarks>argWhereSbの長さが0の場合は項目名から文字列を追加する。
        /// 0でない場合はANDから文字列を追加する。</remarks>
        //************************************************************************
        protected void AddWhere(StringBuilder argWhereSb, IEnumerable<SelectParam> argParam)
        {
            // 追加の条件
            foreach (var param in argParam)
            {
                if (string.IsNullOrEmpty(param.Condtion)) continue;
                
                if (argWhereSb.Length > 0) argWhereSb.AppendLine().Append("AND ");
                if (!string.IsNullOrEmpty(param.Name))
                {
                    // テーブルの指定がない場合は、Aをつける
                    if (!param.Name.Contains('.')) argWhereSb.Append("A.");
                    argWhereSb.AppendFormat("{0} ", param.Name);
                }
                argWhereSb.Append(param.Condtion);
            }
        }

        //************************************************************************
        /// <summary>
        /// Commandに検索パラメータを設定する。
        /// </summary>
        /// <param name="argCmd">IDbCommand</param>
        /// <param name="argParam">検索条件</param>
        //************************************************************************
        protected void SetParameter(IDbCommand argCmd, IEnumerable<SelectParam> argParam)
        {
            Regex regex = new Regex("@\\w+");

            foreach (var param in argParam)
            {
                // プレースフォルダ名を取得
                MatchCollection mc = regex.Matches(param.Condtion);
                if (mc.Count == 0) continue;

                if (param.FromValue != null && param.ToValue != null)
                {
                    argCmd.Parameters.Add(CreateCmdParam(mc[0].Value, param.FromValue));
                    argCmd.Parameters.Add(CreateCmdParam(mc[1].Value, param.ToValue));
                }
                else
                {
                    if (param.FromValue != null) argCmd.Parameters.Add(CreateCmdParam(mc[0].Value, param.FromValue));
                    if (param.ToValue != null) argCmd.Parameters.Add(CreateCmdParam(mc[0].Value, param.ToValue));
                }
            }
        }
        #endregion
    }
}
