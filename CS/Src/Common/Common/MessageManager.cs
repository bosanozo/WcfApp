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
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// メッセージソース
    /// </summary>
    //************************************************************************
    public class MessageManager
    {
        private static MessagesModel s_messages;

        /// <summary>
        /// メッセージファイル
        /// </summary>
        public static string MessageFileDir { get; set; }

        //************************************************************************
        /// <summary>
        /// メッセージ定義文字列を返す。
        /// </summary>
        /// <param name="argMessageCode">メッセージコード</param>
        /// <param name="argParams">パラメータ</param>
        /// <returns>メッセージ</returns>
        //************************************************************************
        public static string GetMessage(string argMessageCode, params object[] argParams)
        {
            // メッセージ定義の読み込み
            if (s_messages == null)
            {
                s_messages = new MessagesModel();
                s_messages.Message = new List<MessageModel>();

                XmlSerializer serializer = new XmlSerializer(typeof(MessagesModel));

                string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory +
                    Path.DirectorySeparatorChar + MessageFileDir, "Message*.xml");

                foreach (string file in files)
                {
                    MessagesModel messages;
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                        messages = (MessagesModel)serializer.Deserialize(fs);

                    s_messages.Message.AddRange(messages.Message);
                }
            }

            // メッセージ定義の取得
            MessageModel msg = s_messages.Message.Single(m => m.Code == argMessageCode);
            return string.Format(msg.Format, argParams);
        }
    }
}
    