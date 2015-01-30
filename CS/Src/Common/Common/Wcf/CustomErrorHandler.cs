using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

using log4net;

using Common.Service;

namespace Common.Wcf
{
    //************************************************************************
    /// <summary>
    /// CustomErrorHandler
    /// </summary>
    //************************************************************************
    public class CustomErrorHandler : IErrorHandler
    {
        #region ロガーフィールド
        private ILog m_logger;
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        //************************************************************************
        public CustomErrorHandler()
        {
            // ロガーを取得
            m_logger = LogManager.GetLogger(this.GetType());
        }
        #endregion

        #region IErrorHandler メンバ
        //************************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error">Exception</param>
        /// <returns></returns>
        //************************************************************************
        public bool HandleError(Exception error)
        {
            // ログ出力
            if (error is BusinessException)
                m_logger.Error(((BusinessException)error).ApplicationMessage, error.InnerException);
            else
                m_logger.Error(error);

            // イベントログへの出力実施！
            /*
            string errorType = error.GetType().Name;
            string errorMessage = error.Message;
            string errorStackTrace = error.StackTrace;
            string evtSrc = "app";

            if (!EventLog.SourceExists(evtSrc)) EventLog.CreateEventSource(evtSrc, "");

            EventLog.WriteEntry(evtSrc, string.Format("{0}.{1}() {2}\r\n{3}",
                error.TargetSite.DeclaringType.Name, error.TargetSite.Name,
                errorMessage, error.ToString()), EventLogEntryType.Error);
            */
            return false;
        }

        //************************************************************************
        /// <summary>
        /// BusinessExceptionの場合FaultException<ApplicationMessage>を生成する。
        /// </summary>
        /// <param name="error">Exception</param>
        /// <param name="version">MessageVersion</param>
        /// <param name="message">Message</param>
        //************************************************************************
        public void ProvideFault(Exception error, MessageVersion version, ref Message message)
        {
            if (error is BusinessException)
            {
                FaultReason reason = new FaultReason("サーバ側でアプリケーションエラーが発生しました。");
                FaultException<ApplicationMessage> fe
                    = new FaultException<ApplicationMessage>(((BusinessException)error).ApplicationMessage, reason);
                MessageFault fault = fe.CreateMessageFault();
                message = Message.CreateMessage(version, fault, fe.Action);
            }
        }
        #endregion
    }
}
