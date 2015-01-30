using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

using log4net;

using Common;
using Common.Service;
using Common.Unity;

namespace Common.Forms
{
    //************************************************************************
    /// <summary>
    /// 画面の基底クラス
    /// </summary>
    //************************************************************************
    public partial class BaseForm : DockContent
    {
        #region ロガーフィールド
        private ILog m_logger;
        #endregion

        #region private 変数
        #endregion

        #region プロパティ
        /// <summary>ロガー</summary>
        protected ILog Log
        {
            get { return m_logger; }
        }

        /// <summary>共通処理サービス</summary>
        protected ICommonService CommonService
        {
            get
            {
                return UnityContainerManager.Container.Resolve<ICommonService>();
            } 
        }

        /// <summary>確認ダイアログなしクローズフラグ</summary>
        [Category("共通部品")]
        [Description("確認ダイアログなしクローズフラグ")]
        public bool CloseNoConfirm { get; set; }

        /// <summary>画面名</summary>
        [Category("共通部品")]
        [Description("画面名")]
        [DefaultValue("画面名")]
        public string FormName { get; set; }

        /// <summary>操作時刻</summary>
        [Browsable(false)]
        public DateTime OperationTime { get; set; }
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public BaseForm()
        {
            InitializeComponent();

            // ロガーを取得
            m_logger = LogManager.GetLogger(this.GetType());
        }
        #endregion

        #region エラー表示関連メソッド
        #endregion
    }
}
