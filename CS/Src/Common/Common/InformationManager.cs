using System.Web;

namespace Common
{
    //************************************************************************
    /// <summary>
    /// ユーザ情報へのアクセスを提供します。
    /// </summary>
    //************************************************************************
    public class InformationManager
    {
        private static UserInformation s_userInfo = null;
        private static ClientInformation s_clientInfo = null;

        //************************************************************************
        /// <summary>
        /// ユーザ情報
        /// </summary>
        //************************************************************************
        public static UserInformation UserInfo
        {
            get
            {
                return HttpContext.Current != null ?
                    HttpContext.Current.Session["UserInformation"] as UserInformation : s_userInfo;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Session["UserInformation"] = value;
                else
                    s_userInfo = value;
            }
        }

        //************************************************************************
        /// <summary>
        /// クライアント情報
        /// </summary>
        //************************************************************************
        public static ClientInformation ClientInfo
        {
            get
            {
                return HttpContext.Current != null ?
                    HttpContext.Current.Items["ClientInformation"] as ClientInformation : s_clientInfo;
            }
            set
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Items["ClientInformation"] = value;
                else
                    s_clientInfo = value;
            }
        }
    }
}
