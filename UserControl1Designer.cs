#define ExcelDataReader

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using System.Reflection;

using Excel;
using LinqToExcel;

namespace WindowsFormsApplication1
{
    internal class UserControl1Designer : ParentControlDesigner
    {
        public UserControl1Designer()
        {
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            // スマートタグ追加
            ActionLists.Add(new UC1DesignerActionList(component));
        }
    }

    // スマートタグ
    public class UC1DesignerActionList : DesignerActionList
    {
        private UserControl1 m_panel;

        [Editor(typeof(MyFileNameEditor), typeof(UITypeEditor))]
        public string FileName
        {
            get { return m_panel.FileName; }
            set { m_panel.FileName = value; }
        }

        public string SheetName
        {
            get { return m_panel.SheetName; }
            set { m_panel.SheetName = value; }
        }

        public int ColumnCount
        {
            get { return m_panel.ColumnCount; }
            set { m_panel.ColumnCount = value; }
        }

        public UC1DesignerActionList(IComponent component) : base(component)
        {
            m_panel = (UserControl1)component;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();

            //Define static section header entries.
            items.Add(new DesignerActionHeaderItem("Appearance"));

            items.Add(new DesignerActionPropertyItem("FileName", "ファイル名(.xlsx)", "Appearance", "項目定義"));
            items.Add(new DesignerActionPropertyItem("SheetName", "シート", "Appearance", "シート"));
            items.Add(new DesignerActionPropertyItem("ColumnCount", "列数", "Appearance"));

            items.Add(new DesignerActionMethodItem(this, "AddControl", "AddControl実行", "Appearance", true));
            return items;
        }

        /// <summary>
        /// コントロールを追加します。
        /// </summary>
        private void AddControl()
        {
            try
            {
#if ExcelDataReader
                DataTable table = null;
                using (var sr = File.OpenRead(FileName))
                using (var er = ExcelReaderFactory.CreateOpenXmlReader(sr))
                {
                    er.IsFirstRowAsColumnNames = true;
                    var result = er.AsDataSet();
                    table = result.Tables[0];
                }
#else
                LinqToExcel.Query.ExcelQueryable<Row> sheet;
                using (var excel = new ExcelQueryFactory(FileName))
                {
                    excel.ReadOnly = true;
                    //excel.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;
                    //comboBoxSheet.DataSource = excel.GetWorksheetNames();
                    sheet = excel.Worksheet("Sheet1");
                }
#endif

                var host = GetService(typeof(IDesignerHost)) as IDesignerHost;

                using (var tran = host.CreateTransaction("AddControl"))
                {
                    try
                    {
                        m_panel.SuspendLayout();

                        // 子コントロールをクリア
                        while (m_panel.Controls.Count > 0)
                            host.DestroyComponent(m_panel.Controls[0]);

                        string prevName = null;
                        FlowLayoutPanel panel = null;
                        int colIdx = ColumnCount - 1;

                        int top = 0, left = 0;
#if ExcelDataReader
                        foreach(DataRow row in table.Rows)
#else
                        foreach (var row in sheet)
#endif
                        {
                            bool sameRow = prevName == row[0].ToString();
                            if (!sameRow)
                            {
                                // 列数が最大の場合は次の行に折り返し
                                if (colIdx >= ColumnCount - 1)
                                {
                                    colIdx = 0;
                                    top += 26;
                                    left = 0;
                                }
                                else
                                {
                                    colIdx++;
                                    left += 2;
                                }

                                // ラベル
                                var label = host.CreateComponent(typeof(Label)) as Label;
                                label.Text = prevName = row[0].ToString();
                                label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                                label.Top = top;
                                label.Left = left;
                                m_panel.Controls.Add(label);
                                left += label.Width;

                                // パネル
                                panel = host.CreateComponent(typeof(FlowLayoutPanel)) as FlowLayoutPanel;
                                panel.AutoSize = true;
                                //panel.Dock = DockStyle.Fill;
                                panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                                panel.Top = top;
                                panel.Left = left;
                                m_panel.Controls.Add(panel);

                                int colspan;
                                if (row["ColSpan"] != DBNull.Value && (colspan = Convert.ToInt32(row["ColSpan"])) > 1)
                                {
                                    if (colspan > ColumnCount - colIdx) colspan = ColumnCount - colIdx; 
                                    colIdx += colspan - 1;
                                }
                            }

                            // コントロール
                            var controlType = Type.GetType("System.Windows.Forms." + row[2].ToString());
                            var control = host.CreateComponent(controlType) as Control;
                            var text = row["Text"].ToString();
                            if (text.Length > 0) control.Text = text;
                            if (control is TextBox)
                            {
                                int len;
                                if (row[3] != DBNull.Value && (len = Convert.ToInt32(row[3])) > 0)
                                    (control as TextBox).MaxLength = len;
                            }
                            panel.Controls.Add(control);
                            left += control.Width + control.Margin.Left + control.Margin.Right;
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Cancel();
                        throw;
                    }
                    finally
                    {
                        m_panel.ResumeLayout();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
