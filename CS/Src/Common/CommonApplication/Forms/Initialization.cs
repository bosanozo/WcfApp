using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.ServiceModel;
using System.Threading;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using log4net;

using Common.Service;
using Common.Unity;
using Common.Wcf;

namespace Common.Forms
{
    //************************************************************************
    /// <summary>
    /// 初期設定関連クラス
    /// </summary>
    //************************************************************************
    public class Initialization
	{
        //************************************************************************
		/// <summary>
		/// Windowsアプリケーションの初期設定
		/// </summary>
		//************************************************************************
        public static bool InitialSetting()
		{
			// 集約例外ハンドラの設定
			Application.ThreadException += Application_ThreadException;

            // 表示スタイルの設定
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            Application.CurrentCulture = new CultureInfo("ja-JP", false);

            #region Unity設定
            // Unityコンテナ取得
            var container = UnityContainerManager.Container;

            // Interceptionを拡張
            container.AddNewExtension<Interception>();

            // Interceptionの設定
            PolicyDefinition policyDef = container.Configure<Interception>().AddPolicy("Client");
            policyDef.AddMatchingRule(new CustomAttributeMatchingRule(typeof(ShowWaitingAttribute), true));

            // Injection設定
            foreach (string file in
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Common*.dll", SearchOption.AllDirectories))
            {
                var asm = Assembly.LoadFile(file);

                // IBaseServiceを継承するInterfaceを登録
                if (Properties.Settings.Default.ServiceCall == "Local")
                    container.RegisterTypes(
                        asm.ExportedTypes.Where(t => t.IsClass == true && t.GetInterfaces().Contains(typeof(IBaseService))),
                        WithMappings.FromMatchingInterface,
                        //getLifetimeManager: t => new PerResolveLifetimeManager(),
                        getInjectionMembers: t => new InjectionMember[] 
                        {
                            new Interceptor<InterfaceInterceptor>(),
                            new InterceptionBehavior<PolicyInjectionBehavior>()
                        });
                else
                    container.RegisterTypes(
                        asm.ExportedTypes.Where(t => t.IsInterface == true && t.GetInterfaces().Contains(typeof(IBaseService))),
                        getInjectionMembers: t => new InjectionMember[] 
                        {
                            new InjectionFactory(c => ServiceUtil.GetServiceProxy(t)),
                            new Interceptor<InterfaceInterceptor>(),
                            new InterceptionBehavior<PolicyInjectionBehavior>(),
                            new InterceptionBehavior<ServiceCallBehavior>()
                        });
            }

            // BaseFormを継承するクラスを登録
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(c => c.IsSubclassOf(typeof(BaseForm))),
                getInjectionMembers: t => new InjectionMember[] 
                {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>()
                });

            // Local設定の追加
            if (Properties.Settings.Default.ServiceCall == "Local")
            {
                // TransactionAttributeを登録
                var t = Type.GetType("Common.Unity.UseTransactionAttribute, CommonService");
                policyDef.AddMatchingRule(new CustomAttributeMatchingRule(t, true));

                // DbContextを登録
                var t2 = Type.GetType("Common.DataAccess.BaseDbContext, CommonService");
                container.RegisterType(t2, new PerResolveLifetimeManager());
            }
            #endregion

            // 未認証の場合は、ログイン実行
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                InformationManager.ClientInfo = new ClientInformation();

                using (var login = container.Resolve<LoginDialog>())
                {
                    // ログイン画面を表示してログインを実行
                    var result = login.ShowDialog();

                    if (result == DialogResult.Abort && login.Exception != null)
                    {
                        // スロー
                        throw login.Exception;
                    }
                    else if (result != DialogResult.OK) return false;
                }               
            }

            return true;
		}

        //************************************************************************
        /// <summary>
        /// 集約例外ハンドラ
        /// </summary>
        /// <remarks>
        /// アプリケーション中でキャッチされなかった例外情報を捕捉します。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //************************************************************************
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // 例外情報をログ出力
            var logger = LogManager.GetLogger("Common.ThreadException");
            logger.Error(e.Exception);

            // 例外情報をダイアログ表示
            DialogResult ret;
            using (ErrorDialog fm = new ErrorDialog())
            {
                fm.MessageSummary = e.Exception.Message;
                fm.MessageDetail = e.Exception.ToString();
                ret = fm.ShowDialog();
            }
            // 中止の場合は、アプリケーション終了
            if (ret == DialogResult.Abort)
            {
                Application.ExitThread();
            }
        }
    }
}
