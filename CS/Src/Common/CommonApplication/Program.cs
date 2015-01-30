using System;
using System.Windows.Forms;

using Microsoft.Practices.Unity;

using Common.Forms;
using Common.Unity;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// エントリポイントクラス
    /// </summary>
    //************************************************************************
    static class Program
    {
        //************************************************************************
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        //************************************************************************
        [STAThread]
        static void Main()
        {
            try
            {
                // 共通初期設定
                if (!Initialization.InitialSetting()) return;

                // UnityでFormを生成
                var form = UnityContainerManager.Container.Resolve<ContainerForm>();

                // 画面表示
                Application.Run(form);
            }
            catch (Exception ex)
            {
                // 例外処理
                //（集約例外ハンドラに処理させるため、ThreadExceptionイベントを起こす）
                Application.OnThreadException(ex);
            }
        }
    }
}
