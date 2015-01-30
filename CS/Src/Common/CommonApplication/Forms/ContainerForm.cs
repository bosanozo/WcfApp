using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

using Common.Unity;

namespace Common.Forms
{
    //************************************************************************
    /// <summary>
    /// メイン画面のコンテナ画面
    /// </summary>
    //************************************************************************
    public partial class ContainerForm : Form
    {
        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public ContainerForm()
        {
            InitializeComponent();
        }
        #endregion

        #region イベントハンドラ
        //************************************************************************
        /// <summary>
        /// フォームロード
        /// </summary>
        //************************************************************************
        private void ContainerForm_Load(object sender, EventArgs e)
        {
            DockContent menu = UnityContainerManager.Container.Resolve<MenuForm>();
            menu.Show(dockPanel1, DockState.DockLeftAutoHide);

            BaseForm form = UnityContainerManager.Container.Resolve<MainForm>();
            form.Show(dockPanel1, DockState.Document);

            // ヘッダの設定
            labelUserName.Text = InformationManager.UserInfo.Name;
            labelFormId.Text = "【" + form.GetType().BaseType.Name + "】";
            labelFormName.Text = form.FormName;
        }
        #endregion
    }
}
