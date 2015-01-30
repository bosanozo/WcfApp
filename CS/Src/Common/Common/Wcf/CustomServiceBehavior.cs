using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Common.Wcf
{
    //************************************************************************
    /// <summary>
    /// CustomServiceBehavior
    /// </summary>
    //************************************************************************
    public class CustomServiceBehavior : IServiceBehavior
    {
        private IErrorHandler errorHandler = new CustomErrorHandler();

        #region IServiceBehavior メンバ
        //************************************************************************
        /// <summary>
        /// IServiceBehaviorのメソッド
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        //************************************************************************
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        //************************************************************************
        /// <summary>
        /// エラーハンドラの登録
        /// </summary>
        /// <param name="serviceDescription">ServiceDescription</param>
        /// <param name="serviceHostBase">ServiceHostBase</param>
        //************************************************************************
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // エラーハンドラを設定
            foreach (ChannelDispatcherBase dispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher dispatcher = dispatcherBase as ChannelDispatcher;
                if (dispatcher == null) continue;

                dispatcher.ErrorHandlers.Add(this.errorHandler);
            }
        }

        //************************************************************************
        /// <summary>
        /// IServiceBehaviorのメソッド
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        //************************************************************************
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
        #endregion
    }
}
