using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Unity.Wcf;

using Common;
using Common.DataAccess;
using Common.Service;
using Common.Unity;

namespace WebApp
{
    //************************************************************************
    /// <summary>
    /// WCFサービスをUnityコンテナから作成するファクトリ
    /// </summary>
    //************************************************************************
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        //************************************************************************
        /// <summary>
        /// Unityコンテナを設定する。
        /// </summary>
        //************************************************************************
        protected override void ConfigureContainer(IUnityContainer container)
        {
            // Interceptionを拡張
            container.AddNewExtension<Interception>();

            // Interceptionの設定
            container.Configure<Interception>().AddPolicy("Server")
                .AddMatchingRule(new CustomAttributeMatchingRule(typeof(UseTransactionAttribute), true));

            // Injectionの設定
            // IBaseServiceを継承するインターフェースを登録
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => t.GetInterfaces().Contains(typeof(IBaseService))),
                WithMappings.FromMatchingInterface,
                getInjectionMembers: t => new InjectionMember[] 
                    {
                        new Interceptor<InterfaceInterceptor>(),
                        new InterceptionBehavior<PolicyInjectionBehavior>()
                    });

            // DbContextを登録
            container.RegisterType<BaseDbContext>(new HierarchicalLifetimeManager());
        }
    }    
}