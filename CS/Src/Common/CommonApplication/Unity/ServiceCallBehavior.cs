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
                // ダイアログを作成
                using (var dlg = new WaitingDialog())
                {
                    // ハンドリングしたメソッドを非同期実行
                    IAsyncResult ar = getNext().BeginInvoke(input, getNext, (asyncResult) =>
                    {
                        // 結果を取得
                        returnMessage = getNext().EndInvoke(asyncResult);

                        // UIスレッドで処理を実行
                        form.Invoke((MethodInvoker)dlg.Close);
                    }, null);

                    // ダイアログ表示待ち
                    if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(Properties.Settings.Default.DialogWaitTime)))
                        dlg.ShowDialog();

                    // 結果がまだ無い場合の対処
                    while (returnMessage == null) Thread.Sleep(10);
                }
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
