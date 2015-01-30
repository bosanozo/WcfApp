using System.Transactions;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Common.Unity
{
    //************************************************************************
    /// <summary>
    /// トランザクションを設定する。
    /// </summary>
    //************************************************************************
    public class TransactionHandler : ICallHandler
    {
        /// <summary>
        /// Order
        /// </summary>
        public int Order { get; set; }

        //************************************************************************
        /// <summary>
        /// メソッドをTransactionScope内で実行する。
        /// </summary>
        /// <param name="input">IMethodInvocation</param>
        /// <param name="getNext">GetNextHandlerDelegate</param>
        /// <returns>IMethodReturn</returns>
        //************************************************************************
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn returnMessage;

            using (TransactionScope scope = new TransactionScope())
            {
                //Use logging in production
                returnMessage = getNext()(input, getNext);
                //If Exception is thrown rollback
                if (returnMessage.Exception == null)
                {
                    scope.Complete();
                }
            }

            return returnMessage;
        }
    }
}