using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

using System.Net;

namespace Common.Wcf
{
    //************************************************************************
    /// <summary>
    /// CustomMessageInspector
    /// </summary>
    //************************************************************************
    public class CustomMessageInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        // クッキーコンテナ
        private static readonly CookieContainer s_cookieContainer = new CookieContainer();

        #region IClientMessageInspector メンバ
        //************************************************************************
        /// <summary>
        /// リプライ受信時の処理
        /// </summary>
        /// <param name="reply">Message</param>
        /// <param name="correlationState">object</param>
        //************************************************************************
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            //var aaa = ((HttpResponseMessageProperty)reply.Properties[HttpResponseMessageProperty.Name]).Headers["Set-Cookie"];
        }

        //************************************************************************
        /// <summary>
        /// リクエスト送信時の処理
        /// </summary>
        /// <param name="request">Message</param>
        /// <param name="channel">IClientChannel</param>
        /// <returns></returns>
        //************************************************************************
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // ヘッダ情報の設定
            request.Headers.Add(MessageHeader.CreateHeader("FormId", "ns", InformationManager.ClientInfo.FormId));
            
            // クッキーコンテナを設定
            var cookieManager = channel.GetProperty<IHttpCookieContainerManager>();
	        if (cookieManager != null) cookieManager.CookieContainer = s_cookieContainer;

            return null;
        }
        #endregion

        #region IDispatchMessageInspector メンバ
        //************************************************************************
        /// <summary>
        /// リクエスト受信時の処理
        /// </summary>
        /// <param name="request">Message</param>
        /// <param name="channel">IClientChannel</param>
        /// <param name="instanceContext">InstanceContext</param>
        /// <returns></returns>
        //************************************************************************
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // ヘッダ情報の取得
            if (request.Headers.FindHeader("FormId", "ns") > 0)
                InformationManager.ClientInfo = new ClientInformation()
                {
                    FormId = request.Headers.GetHeader<string>("FormId", "ns")
                };
 
            return null;
        }

        //************************************************************************
        /// <summary>
        /// リプライ送信時の処理
        /// </summary>
        /// <param name="reply">Message</param>
        /// <param name="correlationState">object</param>
        //************************************************************************
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
        #endregion
    }
}
