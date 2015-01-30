/*******************************************************************************
 * 【共通部品】
 * 
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2014.7.1, 新規作成
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

using log4net;

using Common;
using Common.Models;

namespace Common.DataAccess
{
    //************************************************************************
    /// <summary>
    /// DbContextの基底クラス
    /// </summary>
    //************************************************************************
    public class BaseDbContext : DbContext
    {
        #region ロガーフィールド
        private ILog m_logger;
        #endregion

        #region プロパティ
        public virtual DbSet<CMSMメニュー> CMSMメニュー { get; set; }
        public virtual DbSet<CMSMユーザ> CMSMユーザ { get; set; }
        public virtual DbSet<CMSMユーザロール> CMSMユーザロール { get; set; }
        public virtual DbSet<CMSM組織> CMSM組織 { get; set; }
        public virtual DbSet<CMSM汎用基準値> CMSM汎用基準値 { get; set; }
        #endregion

        /// <summary>PKEY制約違反エラーNO</summary>
        private const int PKEY_ERR = 2627;

        private const string DB_ERR = "EV002";

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public BaseDbContext()
            : base("Default")
        {
            // ロガーを取得
            m_logger = LogManager.GetLogger(this.GetType());
        }
        #endregion

        #region overrideメソッド
        //************************************************************************
        /// <summary>
        /// 変更を永続化する。
        /// </summary>
        /// <returns></returns>
        //************************************************************************
        public override int SaveChanges()
        {
            DateTime changeDate = DateTime.Now;
            var changeSet = ChangeTracker.Entries<IHasUpdateInfo>();

            if (changeSet != null)
            {
                foreach (var entry in changeSet)
                {
                    // 状態に応じて更新情報を設定する
                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        var updInfo = entry.Entity.UpdateInfo;

                        if (entry.State == EntityState.Added)
                        {
                            updInfo.CreateDate = changeDate;
                            updInfo.CreatePg = InformationManager.ClientInfo.FormId;
                            updInfo.CreateHost = InformationManager.ClientInfo.HostName;
                            updInfo.CreateId = InformationManager.UserInfo.Id;
                        }

                        updInfo.UpdateDate = changeDate;
                        updInfo.UpdatePg = InformationManager.ClientInfo.FormId;
                        updInfo.UpdateHost = InformationManager.ClientInfo.HostName;
                        updInfo.UpdateId = InformationManager.UserInfo.Id;
                    }
                }
            }

            int ret = 0;

            try
            {
                ret = base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                ApplicationMessage message;

                var entity = e.Entries.First();

                if (entity.Entity is IHasUpdateInfo)
                {
                    // データベース値を検索
                    DbPropertyValues dbValues = Entry(entity.Entity).GetDatabaseValues();

                    if (dbValues != null)
                    {
                        DbPropertyValues updInfo = (DbPropertyValues)dbValues["UpdateInfo"];

                        if (entity.State == EntityState.Modified)
                            message = new ApplicationMessage("WV002",
                                updInfo["UpdateId"], updInfo["UpdateDate"], updInfo["UpdatePg"], updInfo["UpdateHost"]);
                        else if (entity.State == EntityState.Deleted)
                            message = new ApplicationMessage("WV004",
                                updInfo["UpdateId"], updInfo["UpdateDate"], updInfo["UpdatePg"], updInfo["UpdateHost"]);
                        else
                            message = new ApplicationMessage(DB_ERR, CommonUtil.GetExceptionMessage(e));
                    }
                    else
                    {
                        if (entity.State == EntityState.Modified)
                            message = new ApplicationMessage("WV003");
                        else if (entity.State == EntityState.Deleted)
                            message = new ApplicationMessage("WV005");
                        else
                            message = new ApplicationMessage(DB_ERR, CommonUtil.GetExceptionMessage(e));
                    }

                    message.RowField = new RowField(entity.Entity.GetType().Name, ((IHasUpdateInfo)entity.Entity).UpdateInfo.RowNumber);
                }
                else
                {
                    message = new ApplicationMessage(DB_ERR, CommonUtil.GetExceptionMessage(e));
                }

                throw new BusinessException(message, e);
            }
            catch (DbUpdateException e)
            {
                string msgCd = DB_ERR;
                Exception ex = e;

                var innerEx = e.InnerException.InnerException as System.Data.SqlClient.SqlException;

                if (innerEx != null)
                {
                    if (innerEx.Number == PKEY_ERR) msgCd = "WV001";
                    ex = innerEx;
                }

                object entity = e.Entries.First().Entity;
                var rowField = new RowField(entity.GetType().Name, ((IHasUpdateInfo)entity).UpdateInfo.RowNumber);

                var message = new ApplicationMessage(msgCd, rowField, CommonUtil.GetExceptionMessage(ex));
                throw new BusinessException(message, ex);
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var err in e.EntityValidationErrors)
                {
                    var verr = err.ValidationErrors.First();
                    sb.AppendFormat("{0}:{1}", err.Entry.Entity.GetType().Name, verr.ErrorMessage);
                }

                object entity = e.EntityValidationErrors.First().Entry.Entity;
                var rowField = new RowField(entity.GetType().Name, ((IHasUpdateInfo)entity).UpdateInfo.RowNumber);

                var message = new ApplicationMessage(DB_ERR, rowField, sb.ToString());
                throw new BusinessException(message, e);
            }

            return ret;
        }
        #endregion
    }
}
