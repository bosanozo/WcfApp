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

using Common;
using Common.Unity;

namespace Common.Forms
{
    //************************************************************************
    /// <summary>
    /// メイン画面の基底クラス
    /// </summary>
    //************************************************************************
    public partial class MainForm : BaseForm
    {
        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region イベントハンドラ
        //************************************************************************
        /// <summary>
        /// 検索ボタン押下
        /// </summary>
        //************************************************************************
        [ShowWaiting]
        protected virtual void button1_Click(object sender, EventArgs e)
        {
            var idList = new List<SelectId>() { new SelectId("CMSM組織", "A.組織CD") };

            // 検索パラメータ作成
            var paramList = new List<SelectParam>();
            if (textBox1.Text.Length > 0) paramList.Add(new SelectParam("組織CD", "= @組織CD", textBox1.Text));
            if (textBox2.Text.Length > 0) paramList.Add(new SelectParam("組織名", "Like @組織名", "%" + textBox2.Text + "%"));

            // 検索実行
            ApplicationMessage message;
            var result = CommonService.SelectList(idList, paramList, SelectType.Limited, out message);

            // 返却メッセージの表示
            if (message != null) CustomMessageBox.Show(message);

            // 検索結果を設定
            dataGridView1.DataSource = result.Tables[0];

            dataGridView1.Columns.Remove("排他用バージョン");
        }

        //************************************************************************
        /// <summary>
        /// 条件クリアボタン押下
        /// </summary>
        //************************************************************************
        private void button2_Click(object sender, EventArgs e)
        {
            foreach(Control c in tableLayoutPanel1.Controls)
            {
                if (c is TextBox) ((TextBox)c).Clear();
            }
        }

        //************************************************************************
        /// <summary>
        /// 編集ボタン押下
        /// </summary>
        //************************************************************************
        [ShowWaiting]
        protected virtual void button3_Click(object sender, EventArgs e)
        {
            // 画面表示時に引数として渡す操作モードの設定
            OperationMode opeMode;
            if (sender == button3) opeMode = OperationMode.New;
            else if (sender == button4) opeMode = OperationMode.Update;
            else if (sender == button5) opeMode = OperationMode.Delete;
            else opeMode = OperationMode.View;

            // 選択行を取得
            int selectIdx = -1;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                object val= dataGridView1["選択", i].Value;
                if (val != null && (bool)val)
                {
                    selectIdx = i;
                    break;
                }
            }

            // 選択行が無い場合
            if (selectIdx < 0)
            {
                // 修正、削除、参照の場合
                if (opeMode != OperationMode.New)
                {
                    CustomMessageBox.Show("WV104");
                    return;
                }
            }

            DataRow paramRow = null;
            // 選択行が１行の場合
            if (selectIdx >= 0) paramRow = ((DataTable)dataGridView1.DataSource).Rows[selectIdx];

            // 詳細画面の表示 
            using(var detailform = UnityContainerManager.Container.Resolve<DetailForm>())
            {
                detailform.SetParam(paramRow, opeMode);
                detailform.ShowDialog();
            }
        }

        //************************************************************************
        /// <summary>
        /// 登録ボタン押下
        /// </summary>
        //************************************************************************
        [ShowWaiting]
        protected virtual void button7_Click(object sender, EventArgs e)
        {
            // 登録確認OKの場合
            if (CustomMessageBox.Show("QV001") == DialogResult.Yes)
            {
                // 変更データ取得
                var changeData = ((DataTable)dataGridView1.DataSource).DataSet.GetChanges();

                // データの変更が無い場合はメッセージを表示
                if (changeData == null)
                {
                    CustomMessageBox.Show("WV106");
                    return;
                }

                // 登録実行
                ApplicationMessage message;
                var result = CommonService.Update(changeData, out message);

                // メッセージの表示
                CustomMessageBox.Show("IV003");
            }
        }
        #endregion
    }
}
