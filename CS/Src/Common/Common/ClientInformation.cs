using System;
using System.Web;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// ユーザ情報を格納するためのクラス
    /// </summary>
    //************************************************************************
    [Serializable]
    public class ClientInformation
    {
        /// <summary>画面ID</summary>
        public string FormId { get; set; }

        /// <summary>
        /// ホスト名
        /// </summary>
        public string HostName
        {
            get
            {
                return HttpContext.Current != null ?
                    HttpContext.Current.Request.UserHostAddress : System.Environment.MachineName;
            }
        }
    }
}
