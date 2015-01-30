using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Common.Forms
{
    //************************************************************************
    /// <summary>
    /// 例外情報等を表示するためのダイアログです
    /// </summary>
    //************************************************************************
    public partial class ErrorDialog : Form
    {
        private string m_TitleMessage;
        private string m_HeaderMessage;
        private string m_MessageSummary;
        private string m_MessageDetail;

        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public ErrorDialog()
        {
            InitializeComponent();
        }

        //************************************************************************
        /// <summary>
        /// ダイアログのタイトルバーに表示するメッセージ
        /// </summary>
        //************************************************************************
        public string TitleMessage
        {
            set { 
                m_TitleMessage = value;
                this.Text = m_TitleMessage;
            }
        }
        //************************************************************************
        /// <summary>
        /// ダイアログのヘッダ部に表示するメッセージ
        /// </summary>
        //************************************************************************
        public string HeaderMessage
        {
            set 
            { 
                m_HeaderMessage = value;
                this.labelHeader.Text = m_HeaderMessage;
            }
        }
        //************************************************************************
        /// <summary>
        /// ダイアログに表示するメッセージ（概要）
        /// </summary>
        //************************************************************************
        public string MessageSummary
        {
            set 
            { 
                m_MessageSummary = value;
                this.textBoxMessageSummary.Text = m_MessageSummary;
            }
        }
        //************************************************************************
        /// <summary>
        /// ダイアログに表示するメッセージ（詳細）
        /// </summary>
        //************************************************************************
        public string MessageDetail
        {
            set
            {
                m_MessageDetail = value;
                this.textBoxMessageDetail.Text = m_MessageDetail;
            }
        }
        //************************************************************************
        /// <summary>
        /// Form OnLoad
        /// </summary>
        /// <param name="e"></param>
        //************************************************************************
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            // ステータスバーに現在の日付時刻を表示
            this.toolStripStatusLabelDateTime.Text 
                = System.DateTime.Now.ToShortDateString() + " " 
                + System.DateTime.Now.ToLongTimeString();
        }
        //************************************************************************
        /// <summary>
        /// 「ＯＫ」ボタン　Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //************************************************************************
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        //************************************************************************
        /// <summary>
        /// 「中止」ボタン　Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //************************************************************************
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }
    }
}