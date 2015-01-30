using System;
using System.IO;
using System.Windows.Forms;
using System.ServiceModel;

using Microsoft.Practices.Unity.InterceptionExtension;

using log4net;

using Common;
using Common.Forms;
using Common.Service;

namespace Common.Unity
{
    //************************************************************************
    /// <summary>
    /// 処理中ダイアログを表示する。
    /// </summary>
    //************************************************************************
    public class ShowWaitingHandler : ICallHandler
    {
        #region ロガーフィールド
        private ILog m_logger;
        #endregion

        /// <summary>
        /// Order
        /// </summary>
        public int Order { get; set; }

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public ShowWaitingHandler()
        {
            // ロガーを取得
            m_logger = LogManager.GetLogger(this.GetType());
        }
        #endregion

        //************************************************************************
        /// <summary>
        /// メソッドの実行中カーソルを処理中に変更する。
        /// </summary>
        /// <param name="input">IMethodInvocation</param>
        /// <param name="getNext">GetNextHandlerDelegate</param>
        /// <returns>IMethodReturn</returns>
        //************************************************************************
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {           
            IMethodReturn returnMessage = null;

            // フォームを取得
            var form = (Form)input.Target;

            // 画面IDを設定
            InformationManager.ClientInfo.FormId = form.GetType().BaseType.Name;

            // フォーム操作不可
            form.Enabled = false;

            // カーソルを処理中に変更
            var orgCursor = form.Cursor;
            form.Cursor = Cursors.WaitCursor;

            // メソッド実行
            returnMessage = getNext()(input, getNext);

            // フォームをアクティブにする
            form.Activate();

            // 処理中に溜まったイベントを処理
            Application.DoEvents();

            // カーソルを戻す
            form.Cursor = orgCursor;

            // フォーム操作可
            form.Enabled = true;

            // 例外処理
            if (returnMessage.Exception != null)
            {
                ApplicationMessage message;

                if (returnMessage.Exception is FaultException<ApplicationMessage>)
                {
                    var faultEx = returnMessage.Exception as FaultException<ApplicationMessage>;
                    message = faultEx.Detail;
                    m_logger.Error(message, faultEx);
                }
                else if (returnMessage.Exception is FaultException<ExceptionDetail>)
                {
                    var faultEx = returnMessage.Exception as FaultException<ExceptionDetail>;
                    message = new ApplicationMessage("EV001", GetDialogMessage(faultEx.Detail));
                    m_logger.Error(message, faultEx);
                }
                else
                {
                    // 業務エラーの場合
                    if (returnMessage.Exception is BusinessException)
                        message = ((BusinessException)returnMessage.Exception).ApplicationMessage;
                    // その他の場合
                    else
                    {
                        string msgCd = "EV001";

                        if (returnMessage.Exception is FileNotFoundException)
                            msgCd = "W";
                        else if (returnMessage.Exception is IOException)
                            msgCd = "EV003";

                        message = new ApplicationMessage(msgCd, CommonUtil.GetExceptionMessage(returnMessage.Exception));
                    }
                    m_logger.Error(message, returnMessage.Exception);
                }

                CustomMessageBox.Show(message);

                returnMessage.Exception = null;
            }

            return returnMessage;
        }

        //************************************************************************
        /// <summary>
        /// ダイアログに表示するメッセージを返す。
        /// </summary>
        /// <param name="argExDetail">ExceptionDetail</param>
        /// <returns>ダイアログに表示するメッセージ</returns>
        //************************************************************************
        private string GetDialogMessage(ExceptionDetail argExDetail)
        {
            return argExDetail.Message + (argExDetail.InnerException != null ?
                System.Environment.NewLine + GetDialogMessage(argExDetail.InnerException) : null);
        }
    }
}
