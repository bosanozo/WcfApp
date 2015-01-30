/*******************************************************************************
 * 【共通部品】
 *
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2011.10.20, 新規作成
 ******************************************************************************/
using System;
using System.IO;
using System.Windows.Forms;

using Common;

namespace Common.Forms
{
    //************************************************************************
    /// <summary>
    /// メッセージボックスクラス
    /// </summary>
    //************************************************************************
    public class CustomMessageBox
    {
        //************************************************************************
        /// <summary>
        /// 指定されたメッセージコードのメッセージをダイアログ表示する。
        /// </summary>
        /// <param name="argMessage">メッセージ</param>
        /// <returns>ダイアログリザルト</returns>
        //************************************************************************
        public static DialogResult Show(ApplicationMessage argMessage)
        {
            return Show(argMessage.MessageCd, argMessage.Params);
        }

        //************************************************************************
        /// <summary>
        /// 指定されたメッセージコードのメッセージをダイアログ表示する。
        /// </summary>
        /// <param name="argCode">メッセージコード</param>
        /// <param name="argParams">パラメータ</param>
        /// <returns>ダイアログリザルト</returns>
        //************************************************************************
        public static DialogResult Show(string argCode, params object[] argParams)
        {
            return Show(argCode, MessageBoxDefaultButton.Button1, argParams);
        }

        //************************************************************************
        /// <summary>
        /// 発生した例外をエラーダイアログに表示する。
        /// </summary>
        /// <param name="argException">発生した例外</param>
        //************************************************************************
        protected void Show(Exception argException)
        {
            // 業務エラーの場合
            if (argException is BusinessException)
            {
                // ダイアログ表示
                CustomMessageBox.Show(((BusinessException)argException).ApplicationMessage);
            }
            // その他の場合
            else
            {
                string msgCd = "EV001";

                if (argException is FileNotFoundException)
                    msgCd = "W";
                else if (argException is IOException)
                    msgCd = "EV003";
       
                // ダイアログ表示
                CustomMessageBox.Show(msgCd, CommonUtil.GetExceptionMessage(argException));
            }
        }

        //************************************************************************
        /// <summary>
        /// 指定されたメッセージコードのメッセージをダイアログ表示する。
        /// </summary>
        /// <param name="argCode">メッセージコード</param>
        /// <param name="argDefaultButton">既定のボタン(MessageBoxDefaultButtonの一つ)</param>
        /// <param name="argParams">パラメータ</param>
        /// <returns>ダイアログリザルト</returns>
        //************************************************************************
        public static DialogResult Show(string argCode, MessageBoxDefaultButton argDefaultButton,
            params object[] argParams)
        {
            if (argCode == null || argCode.Length < 1) return DialogResult.Cancel;

            // キャプション、ボタン、アイコンの設定
            string caption = "メッセージ";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.None;
            switch (argCode[0])
            {
                case 'E':
                    caption = "エラー";
                    icon = MessageBoxIcon.Error;
                    break;
                case 'W':
                    caption = "警告";
                    icon = MessageBoxIcon.Warning;
                    break;
                case 'Q':
                    caption = "確認";
                    buttons = MessageBoxButtons.YesNo;
                    icon = MessageBoxIcon.Question;
                    break;
                case 'I':
                    icon = MessageBoxIcon.Information;
                    break;
            }

            // メッセージボックス表示
            DialogResult result = MessageBox.Show(
                MessageManager.GetMessage(argCode, argParams), caption, buttons, icon, argDefaultButton);

            // フォーカスがファンクションキーの場合に移動する
            Form form = Form.ActiveForm;
            //if (form != null && form.ActiveControl is GrapeCity.Win.Input.FunctionKey)
            //    form.SelectNextControl(form, true, true, true, true);

            return result;
        }
    }
}
