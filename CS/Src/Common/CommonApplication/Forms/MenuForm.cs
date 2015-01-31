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
            UserInformation uinfo = InformationManager.UserInfo;

            // メニューレベル１の検索
            var table = CommonService.Select("CMSMメニューレベル1", uinfo.SoshikiCd, uinfo.Id).Tables[0];

            foreach (DataRow row in table.Rows)
            {
                if (row["許否フラグ"].ToString() == "True")
                {
                    var node = new TreeNode(row["画面名"].ToString());
                    node.Tag = row["メニューID"];
                    treeView1.Nodes.Add(node);

                    // メニューレベル２の検索
                    var table2 = CommonService.Select("CMSMメニューレベル2", uinfo.SoshikiCd, uinfo.Id, node.Tag).Tables[0];

                    foreach (DataRow row2 in table2.Rows)
                    {
                        if (row["許否フラグ"].ToString() != "False")
                        {
                            var childNode = new TreeNode(row2["画面名"].ToString());
                            childNode.Tag = row["メニューID"];
                            node.Nodes.Add(childNode);
                        }
                    }
                }
            }
        }

        //************************************************************************
        /// <summary>
        /// メニューツリークリック
        /// </summary>
        //************************************************************************
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            BaseForm form = UnityContainerManager.Container.Resolve<MainForm>();
            form.Show(DockPanel, DockState.Document);
        }
        #endregion
    }
}
