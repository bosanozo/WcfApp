using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common.Forms
{
    //************************************************************************
    /// <summary>
    /// 処理中ダイアログ
    /// </summary>
    //************************************************************************
    public partial class WaitingDialog : Form
    {
        private DateTime m_startTime;

        /// <summary>
        /// 表示位置
        /// </summary>
        public Point DispLocation { get; set; }

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public WaitingDialog()
        {
            InitializeComponent();
        }

        public WaitingDialog(string argServiceName) : this()
        {
            label2.Text = argServiceName;
        }
        #endregion

        //************************************************************************
        /// <summary>
        /// 画面表示
        /// </summary>
        //************************************************************************
        private void ServiceCallingDialog_Shown(object sender, EventArgs e)
        {
            // 表示位置設定
            Left = DispLocation.X;
            Top = DispLocation.Y;
            //Activate();

            // タイマー起動
            var timer1 = new Timer();
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
            timer1.Start();

            // 経過時間初期設定
            m_startTime = DateTime.Now.AddSeconds(Properties.Settings.Default.DialogWaitTime * -1);
            button1.Enabled = false;
        }

        //************************************************************************
        /// <summary>
        /// タイマー動作
        /// </summary>
        //************************************************************************
        private void timer1_Tick(object sender, EventArgs e)
        {
            var timer1 = new Timer();
            var span = DateTime.Now - m_startTime;
            label1.Text = string.Format("{0:mm\\:ss}", span);
            if (span.TotalSeconds > 10) button1.Enabled = true;
        }
    }
}
