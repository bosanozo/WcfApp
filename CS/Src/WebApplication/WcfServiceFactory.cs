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
    /// WCF�T�[�r�X��Unity�R���e�i����쐬����t�@�N�g��
    /// </summary>
    //************************************************************************
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        //************************************************************************
        /// <summary>
        /// Unity�R���e�i��ݒ肷��B
        /// </summary>
        //************************************************************************
        protected override void ConfigureContainer(IUnityContainer container)
        {
            // Interception���g��
            container.AddNewExtension<Interception>();

            // Interception�̐ݒ�
            container.Configure<Interception>().AddPolicy("Server")
                .AddMatchingRule(new CustomAttributeMatchingRule(typeof(UseTransactionAttribute), true));

            // Injection�̐ݒ�
            // IBaseService���p������C���^�[�t�F�[�X��o�^
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => t.GetInterfaces().Contains(typeof(IBaseService))),
                WithMappings.FromMatchingInterface,
                getInjectionMembers: t => new InjectionMember[] 
                    {
                        new Interceptor<InterfaceInterceptor>(),
                        new InterceptionBehavior<PolicyInjectionBehavior>()
                    });

            // DbContext��o�^
            container.RegisterType<BaseDbContext>(new HierarchicalLifetimeManager());
        }
    }    
}