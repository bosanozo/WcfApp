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
    /// メニュー画面
    /// </summary>
    //************************************************************************
    public partial class MenuForm : BaseForm
    {
        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public MenuForm()
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
        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        //************************************************************************
        /// <summary>
        /// メニューツリークリック
        /// </summary>
        //************************************************************************
        private void treeView1_Click(object sender, EventArgs e)
        {
            BaseForm form = UnityContainerManager.Container.Resolve<MainForm>();
            form.Show(DockPanel, DockState.Document);
        }
        #endregion

    }
}
