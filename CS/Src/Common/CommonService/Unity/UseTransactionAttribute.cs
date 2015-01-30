using System;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Common.Unity
{
    //************************************************************************
    /// <summary>
    /// トランザクションを設定するメソッドにつけるAttribute
    /// </summary>
    //************************************************************************
    [AttributeUsage(AttributeTargets.Method)]
    public class UseTransactionAttribute : HandlerAttribute
    {
        //************************************************************************
        /// <summary>
        /// TransactionHandlerを作成する。
        /// </summary>
        /// <param name="container">IUnityContainer</param>
        /// <returns>TransactionHandler</returns>
        //************************************************************************
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new TransactionHandler();
        }
    }
}