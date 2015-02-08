using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common
{
    /// <summary>
    /// メッセージ定義ルート
    /// </summary>
    [XmlRoot("DocumentElement")]
    public class MessagesModel
    {
        [XmlElement]
        public List<MessageModel> Message { get; set; }
    }

    /// <summary>
    /// メッセージ定義
    /// </summary>
    public class MessageModel
    {
        [XmlElement]
        public string Code { get; set; }

        [XmlElement]
        public string Format { get; set; }
    }
}
