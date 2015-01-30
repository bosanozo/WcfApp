using System;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Common.Unity
{
    //************************************************************************
    /// <summary>
    /// 処理中を表示するメソッドにつけるAttribute
    /// </summary>
    //************************************************************************
    [AttributeUsage(AttributeTargets.Method)]
    public class ShowWaitingAttribute : HandlerAttribute
    {
        //************************************************************************
        /// <summary>
        /// ShowWaitingHandlerを作成する。
        /// </summary>
        /// <param name="container">IUnityContainer</param>
        /// <returns>ShowWaitingHandler</returns>
        //************************************************************************
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new ShowWaitingHandler();
        }
    }
}
