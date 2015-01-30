using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Common;
using Common.Unity;

namespace Common.Forms
{
    //************************************************************************
    /// <summary>
    /// 詳細画面の基底クラス
    /// </summary>
    //************************************************************************
    public partial class DetailForm : BaseForm
    {
        #region private変数
        // 選択されたデータ
        private DataRow m_selectedRow;
        // 操作モード
        private OperationMode m_opeMode;
        // 検索結果
        private DataSet m_dataSet;
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public DetailForm()
        {
            InitializeComponent();
        }
        #endregion

        public void SetParam(DataRow argSelectedRow, OperationMode argOpeMode)
        {
            // 選択されたデータを記憶
            m_selectedRow = argSelectedRow;
            // 操作モードを設定
            m_opeMode = argOpeMode;
        }

        #region イベントハンドラ
        //************************************************************************
        /// <summary>
        /// フォームロード
        /// </summary>
        //************************************************************************
        private void DetailForm_Load(object sender, EventArgs e)
        {
            var idList = new List<SelectId>() { new SelectId("CMSM組織", "A.組織CD") };

            // 検索パラメータ作成
            var paramList = new List<SelectParam>();
            object p1 = m_selectedRow != null ? m_selectedRow["組織CD"] : "";
            paramList.Add(new SelectParam("組織CD", "= @組織CD", p1));

            // 検索実行
            ApplicationMessage message;
            m_dataSet = CommonService.Select(idList, paramList, SelectType.All, out message);

            DataTable result = m_dataSet.Tables[0];

            // 新規の場合
            if (m_opeMode == OperationMode.New)
            {
                // 検索結果なしの場合、空行を追加
                if (result.Rows.Count == 0)
                {
                    // 空行を作成
                    DataRow newRow = result.NewRow();
                    // 初期値の設定
                    newRow["ROWNUMBER"] = 0m;
                    // 空行を追加
                    result.Rows.Add(newRow);
                    // 状態を初期化
                    result.AcceptChanges();
                }
            }
            // 新規以外の場合、返却メッセージの表示
            else if (message != null) CustomMessageBox.Show(message);

            // 検索結果なしの場合は、画面を閉じる
            if (result.Rows.Count == 0)
            {
                Close();
                return;
            }

            var idList2 = new List<SelectId>() { new SelectId("CMSM汎用基準値", "基準値CD") };

            // 検索パラメータ作成
            var paramList2 = new List<SelectParam>() { new SelectParam("分類CD", "= @分類CD", "M001") };

            // 検索実行
            var dataSet = CommonService.Select(idList2, paramList2, SelectType.All, out message);
            // コンボボックスにDataSourceを設定
            comboBox1.DataSource = dataSet.Tables["CMSM汎用基準値"];

            // モードによる画面表示の変更
            switch (m_opeMode)
            {
                case OperationMode.New:
                    FormName += "　新規";
                    break;

                case OperationMode.Update:
                    FormName += "　修正";
                    // 条件部を操作不可にする
                    Protect(tableLayoutPanel1);
                    break;

                case OperationMode.Delete:
                    FormName += "　削除確認";
                    // 全体を操作不可にする
                    Protect(tableLayoutPanel1);
                    Protect(tableLayoutPanel2);
                    // ボタンの名称を変更
                    button7.Text = "削除実行";
                    break;

                case OperationMode.View:
                    FormName += "　参照";
                    // 全体を操作不可にする
                    Protect(tableLayoutPanel1);
                    Protect(tableLayoutPanel2);
                    // 登録ボタンを使用不可にする
                    button7.Enabled = false;
                    break;
            }

            Text = FormName;

            // 検索結果をデータソースに設定
            textBox1.DataBindings.Add("Text", result, "組織CD");
            textBox2.DataBindings.Add("Text", result, "組織名");
            comboBox1.DataBindings.Add("SelectedValue", result, "組織階層区分");
        }

        public void Protect(Control argControl)
        {
            foreach(Control c in argControl.Controls)
            {
                if (c is TextBox)
                {
                    TextBox t = c as TextBox;
                    t.ReadOnly = true;
                    t.BackColor = SystemColors.Control;
                }
                else if (c is ComboBox) c.Enabled = false;
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
                // 編集の確定
                m_dataSet.Tables[0].Rows[0].EndEdit();

                // 変更データ取得
                DataSet updateData = m_dataSet.GetChanges();

                // 新規の場合、状態を新規に設定
                if (m_opeMode == OperationMode.New)
                {
                    updateData.AcceptChanges();
 
                    foreach (DataTable table in updateData.Tables)
                        foreach (DataRow row in table.Rows) row.SetAdded();
                }
                // 削除の場合、データをコピーして削除データを作成
                else if (m_opeMode == OperationMode.Delete)
                {
                    updateData = m_dataSet.Copy();
                    foreach (DataTable table in updateData.Tables)
                        foreach (DataRow row in table.Rows) row["削除"] = "1";
                }

                // データの変更が無い場合はメッセージを表示
                if (updateData == null)
                {
                    CustomMessageBox.Show("WV106");
                    return;
                }

                // 登録実行
                ApplicationMessage message;
                var result = CommonService.Update(updateData, out message);

                // 修正の場合は更新情報を最新にする
                if (m_opeMode == OperationMode.Update)
                {
                    //SetUpdateParamsWithName(updateData.Rows[0]);
                    // 更新者情報を変更
                    //SetUpdateInfo(multiRow);
                }

                // 新規、修正の場合は状態を初期化する
                if (m_opeMode == OperationMode.New || m_opeMode == OperationMode.Update)
                    m_dataSet.AcceptChanges();

                // メッセージの表示
                if (m_opeMode == OperationMode.Delete)
                {
                    // 削除完了メッセージを表示
                    if (CustomMessageBox.Show("IV004") == DialogResult.OK)
                        Close();
                }
                // 登録完了メッセージを表示
                else CustomMessageBox.Show("IV003");

                // フォーカスを入力可能最上位にセット
                if (m_opeMode == OperationMode.New)
                    textBox1.Focus();
                else
                    textBox2.Focus();
            }
        }
        #endregion
    }
}
