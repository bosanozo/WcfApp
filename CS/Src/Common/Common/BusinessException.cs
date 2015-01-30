/*******************************************************************************
 * 【共通部品】
 *
 * 作成者: 豆蔵／田中 望
 * 改版履歴:
 * 2014.1.30, 新規作成
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// 業務アプリケーション例外クラス
    /// </summary>
    //************************************************************************
    [Serializable]
    public class BusinessException : Exception
    {
        #region プロパティ
        /// <summary>
        /// メッセージ
        /// </summary>
        public ApplicationMessage ApplicationMessage
        {
            get { return (ApplicationMessage)Data["Message"]; }
            set { Data.Add("Message", value); }
        }
        #endregion

        #region コンストラクタ
        // 親クラスから引き継いだコンストラクタ
        public BusinessException() { }
        public BusinessException(string argMessage) : base(argMessage) { }
        public BusinessException(string argMessage, Exception argInnerException)
            : base(argMessage, argInnerException) { }
        public BusinessException(SerializationInfo argSerializationInfo, StreamingContext argStreamingContext)
            : base(argSerializationInfo, argStreamingContext) { }

        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argMessage">メッセージデータ</param>
        //************************************************************************
        public BusinessException(ApplicationMessage argMessage)
        {
            ApplicationMessage = argMessage;
        }

        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argMessage">メッセージデータ</param>
        /// <param name="argInnerException">例外発生の元となった例外</param>
        //************************************************************************
        public BusinessException(ApplicationMessage argMessage, Exception argInnerException)
            : base(argInnerException.Message, argInnerException)
        {
            ApplicationMessage = argMessage;
        }
        #endregion

        //************************************************************************
        /// <summary>
        /// メッセージ文字列を返す。
        /// </summary>
        //************************************************************************
        public override string ToString()
        {
            if (ApplicationMessage != null)
            {
                // 全メッセージ文字列を連結
                StringBuilder builder = new StringBuilder(ApplicationMessage.ToString());

                // メッセージがエラー以外の場合は簡略化する
                if (ApplicationMessage.MessageCd.Length >= 1 && ApplicationMessage.MessageCd[0] != 'E')
                {
                    if (InnerException != null)
                        builder.AppendLine().Append(InnerException.GetType().FullName)
                            .Append(": ").Append(InnerException.Message);
                }
                else builder.AppendLine().Append(base.ToString());

                return builder.ToString();
            }
            else return base.ToString();
        }
    }
}
