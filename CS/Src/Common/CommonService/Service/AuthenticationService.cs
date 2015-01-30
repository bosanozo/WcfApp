using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.DataAccess;
using Common.Unity;

namespace Common.Service
{
    //************************************************************************
    /// <summary>
    /// 共通処理サービス
    /// </summary>
    //************************************************************************
    public class AuthenticationService : IAuthenticationService
    {
        #region private変数
        private CommonDA m_dataAccess;
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argDataAccess">データアクセス層インスタンス</param>
        //************************************************************************
        public AuthenticationService(CommonDA argDataAccess)
        {
            m_dataAccess = argDataAccess;
        }
        #endregion

        #region サービスインターフェース実装
        public bool ValidateUser(string username, string password, string customCredential)
        {
 	        throw new NotImplementedException();
        }

        //************************************************************************
        /// <summary>
        /// ログイン処理を行う。
        /// </summary>
        /// <param name="argUserName">ユーザID</param>
        /// <param name="argPassword">パスワード</param>
        /// <returns>true:成功, false:失敗</returns>
        //************************************************************************
        public bool Login(string username, string password, string customCredential, bool isPersistent)
        {
            return m_dataAccess.Login(username, password);
        }

        public bool IsLoggedIn()
        {
 	        throw new NotImplementedException();
        }

        public void Logout()
        {
 	        throw new NotImplementedException();
        }
        #endregion    
    }
}
