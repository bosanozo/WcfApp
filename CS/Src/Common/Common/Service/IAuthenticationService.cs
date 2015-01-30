using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common.Service
{
    //************************************************************************
    /// <summary>
    /// 認証サービスのインターフェース
    /// </summary>
    //************************************************************************
    [ServiceContract(Namespace = "http://asp.net/ApplicationServices/v200")]
    public interface IAuthenticationService : IBaseService
    {
        [OperationContract(Action = "http://asp.net/ApplicationServices/v200/AuthenticationService/ValidateUser")]
        bool ValidateUser(string username, string password, string customCredential);

        [OperationContract(Action = "http://asp.net/ApplicationServices/v200/AuthenticationService/Login")]
        bool Login(string username, string password, string customCredential, bool isPersistent);

        [OperationContract(Action = "http://asp.net/ApplicationServices/v200/AuthenticationService/IsLoggedIn")]
        bool IsLoggedIn();

        [OperationContract(Action = "http://asp.net/ApplicationServices/v200/AuthenticationService/Logout")]
        void Logout(); 
    }
}
