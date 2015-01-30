using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel.Security;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using Common.Forms;

namespace Common.Unity
{
    //************************************************************************
    /// <summary>
    /// サービス呼び出しをハンドリングする。
    /// </summary>
    //************************************************************************
    public class ServiceCallBehavior : IInterceptionBehavior
    {
        //************************************************************************
        /// <summary>
        /// サービス呼び出しをハンドリングする。
        /// </summary>
        /// <param name="input">IMethodInvocation</param>
        /// <param name="getNext">GetNextInterceptionBehaviorDelegate</param>
        /// <returns>IMethodReturn</returns>
        //************************************************************************
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // ハンドリングしたメソッド実行
            IMethodReturn returnMessage = InvokeMethod(input, getNext);

            // 例外処理
            if (returnMessage.Exception != null)
            {
                var securityEx = returnMessage.Exception as MessageSecurityException;
                if (securityEx != null)
                {
                    using (var loginDialog = UnityContainerManager.Container.Resolve<LoginDialog>())
                    {
                        if (loginDialog.ShowDialog() == DialogResult.OK)
                        {
                            // ログイン画面消去
                            Application.DoEvents();
                            // メソッド再実行
                            returnMessage = InvokeMethod(input, getNext);
                        }
                    }
                    //MessageBox.Show(faultEx.Detail.Type + "\n" + faultEx.Detail.Message);                    
                }
            }
            return returnMessage;
        }

        //************************************************************************
        /// <summary>
        /// ハンドリングしたメソッド実行し、実行時間が長い場合、処理中ダイアログを表示する。
        /// 認証エラーが発生した場合、ログインダイアログを表示する。
        /// </summary>
        /// <param name="input">IMethodInvocation</param>
        /// <param name="getNext">GetNextHandlerDelegate</param>
        /// <returns>IMethodReturn</returns>
        //************************************************************************
        private IMethodReturn InvokeMethod(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn returnMessage = null;

            // フォームを取得
            var form = Form.ActiveForm;

            if (form != null)
            {
                var loc = new System.Drawing.Point(form.Left + (form.Width - 300) / 2, form.Top + (form.Height - 200) / 2);
                // ダイアログを作成
                var dlg = new WaitingDialog() { DispLocation = loc };
                //dlg.DispLocation = new System.Drawing.Point(form.Left + (form.Width - dlg.Width) / 2, form.Top + (form.Height - dlg.Height) / 2);

                var cts = new CancellationTokenSource();

                var task = Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < Properties.Settings.Default.DialogWaitTime * 1000 / 500; i++)
                    {
                        // キャンセル要求がきていたら中断
                        if (cts.IsCancellationRequested) break;
                        Thread.Sleep(500);
                    }

                    // ダイアログを表示
                    if (returnMessage == null) dlg.ShowDialog();
                }, cts.Token);

                // メソッド実行
                returnMessage = getNext()(input, getNext);

                // ダイアログ表示キャンセル
                cts.Cancel();
                //task.Wait();

                // ダイアログを消去
                if (dlg.IsHandleCreated) dlg.Invoke((MethodInvoker)dlg.Dispose);
            }
            else
                returnMessage = getNext()(input, getNext);

            return returnMessage;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Enumerable.Empty<Type>();
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }

}
