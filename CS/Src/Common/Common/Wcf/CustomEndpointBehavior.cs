using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Common.Wcf
{
    //************************************************************************
    /// <summary>
    /// CustomEndpointBehavior
    /// </summary>
    //************************************************************************
    public class CustomEndpointBehavior : IEndpointBehavior
    {
        #region IEndpointBehavior メンバ
        //************************************************************************
        /// <summary>
        /// IEndpointBehaviorのメソッド
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        //************************************************************************
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        //************************************************************************
        /// <summary>
        /// クライアント側メッセージインスペクタの登録
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        //************************************************************************
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new CustomMessageInspector());
        }

        //************************************************************************
        /// <summary>
        /// サーバ側メッセージインスペクタの登録
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        //************************************************************************
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CustomMessageInspector());
        }

        //************************************************************************
        /// <summary>
        /// IEndpointBehaviorのメソッド
        /// </summary>
        /// <param name="endpoint"></param>
        //************************************************************************
        public void Validate(ServiceEndpoint endpoint)
        {
        }
        #endregion
    }
}
