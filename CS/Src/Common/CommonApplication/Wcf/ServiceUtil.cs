using System;
using System.Linq;

using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Channels;

namespace Common.Wcf
{
    //************************************************************************
    /// <summary>
    /// WCFサービスユーティリティ
    /// </summary>
    //************************************************************************
    public class ServiceUtil
    {
        //************************************************************************
        /// <summary>
        /// 指定されたサービスインターフェースからChannelProxyを作成する。
        /// </summary>
        /// <param name="argType">サービスインターフェース</param>
        /// <returns>ChannelProxy</returns>
        //************************************************************************
        public static object GetServiceProxy(Type argType) 
        {
            // ServiceContractを取得
            var svcAttr = argType.CustomAttributes.Single(attr => attr.AttributeType == typeof(ServiceContractAttribute));
            var argName = svcAttr.NamedArguments.SingleOrDefault(arg => arg.MemberName == "Name");

            // Nameで指定されている場合はNameの値から、指定されていない場合はクラス名からサービス名を取得
            string serviceName = argName.MemberInfo != null ? argName.TypedValue.Value.ToString().Substring(1) : argType.Name.Substring(1);

            // URI作成
            var epAddr = new EndpointAddress(ConfigurationManager.AppSettings["baseAddress"] + serviceName + ".svc");

            var factoryType = typeof(ChannelFactory<>).MakeGenericType(argType);
            ChannelFactory factory = null;

            // 構成ファイルにある場合は構成ファイルから生成
            var clientSection = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
            for (int i = 0; i < clientSection.Endpoints.Count; i++)
            {
                if (clientSection.Endpoints[i].Name == serviceName)
                {
                    // Addressが設定されている場合は、設定値を使用する
                    var createArgs = clientSection.Endpoints[i].Address == null ?
                        new object[] { serviceName, epAddr } : new object[] { serviceName };

                    // ChannelFactory作成
                    factory = (ChannelFactory)Activator.CreateInstance(factoryType, createArgs);
                    break;
                }
            }

            // 構成ファイルに無い場合はデフォルトで生成
            if (factory == null)
            {
                // Binding作成
                var binding = new CustomBinding("DefaultBinding");
                // ChannelFactory作成
                factory = (ChannelFactory)Activator.CreateInstance(factoryType, new object[] { binding, epAddr });
            }

            // エンドポイント・ビヘイビアの追加
            factory.Endpoint.Behaviors.Add(new CustomEndpointBehavior());

            // プロキシを作成して返却
            return factoryType.GetMethod("CreateChannel", new Type[] { }).Invoke(factory, null);
        }        
    }
}
