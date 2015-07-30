using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

using System.Linq;

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
            var httpResponse = reply.Properties[HttpResponseMessageProperty.Name] as HttpResponseMessageProperty;
            string setCookie = httpResponse.Headers[HttpResponseHeader.SetCookie];

            if (!string.IsNullOrEmpty(setCookie))
                s_cookieContainer.SetCookies(new Uri(ConfigurationManager.AppSettings["baseAddress"]), setCookie);
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
            //var cookieManager = channel.GetProperty<IHttpCookieContainerManager>();
	        //if (cookieManager != null) cookieManager.CookieContainer = s_cookieContainer;

            if (s_cookieContainer.Count > 0)
            {
                if (!request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
                    request.Properties.Add(HttpRequestMessageProperty.Name, new HttpRequestMessageProperty());

                var httpRequest = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                httpRequest.Headers[HttpRequestHeader.Cookie] = s_cookieContainer.GetCookieHeader(channel.RemoteAddress.Uri);
            }

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
