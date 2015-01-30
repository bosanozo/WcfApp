using System;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

using Common.Unity;
using Common.Service;

namespace Common.Forms
{
    //************************************************************************
    /// <summary>
    /// ログインダイアログ
    /// </summary>
    //************************************************************************
    public partial class LoginDialog : BaseForm
	{
        #region private変数
        private Exception m_exception;
        private IAuthenticationService m_service;
        #endregion

        #region コンストラクタ
        //************************************************************************
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argAuthService">認証サービス</param>
        //************************************************************************
        public LoginDialog(IAuthenticationService argAuthService)
		{
			InitializeComponent();
            m_service = argAuthService;
        }
        #endregion

        #region プロパティ
        //************************************************************************
        /// <summary>
        /// メッセージ
        /// </summary>
        //************************************************************************
        public string Message
		{
			get { return labelMessage.Text; }
			set { labelMessage.Text = value; }
		}

        //************************************************************************
        /// <summary>
        /// サブメッセージ
        /// </summary>
        //************************************************************************
        public string SubMessage
		{
			get { return labelSubMessage.Text; }
			set { labelSubMessage.Text = value; }
		}

        //************************************************************************
        /// <summary>
        /// ログイン処理で発生した例外
        /// </summary>
        //************************************************************************
        public Exception Exception
        {
            get { return m_exception; }
        }

        //************************************************************************
        /// <summary>
        /// ユーザＩＤ（入力不可）
        /// </summary>
        /// <remarks>設定された場合、ユーザＩＤ入力欄は ReadOnly に設定されます</remarks>
        //************************************************************************
        public bool UserIdFixed
		{
			get { return textBoxUserId.ReadOnly; }
			set
			{
				textBoxUserId.ReadOnly = value;
				textBoxUserId.TabStop = !value;
			}
		}

        //************************************************************************
        /// <summary>
        /// ユーザＩＤ
        /// </summary>
        //************************************************************************
        public string UserId
        {
            get { return textBoxUserId.Text; }
            set { textBoxUserId.Text = value; }
        }
        #endregion

        #region イベントハンドラ
        //************************************************************************
        /// <summary>
        /// 「ログイン」ボタン　Click
        /// </summary>
        //************************************************************************
        [ShowWaiting]
        protected virtual void buttonLogin_Click(object sender, EventArgs e)
		{
            // 必須入力のチェック
            if (textBoxUserId.Text.Length == 0)
            {
                // メッセージ（詳細）
                SubMessage = labelUserId.Text + "を入力してください。";
                textBoxUserId.Focus();
                return;
            }
            else
            {
                // メッセージ（詳細）
                SubMessage = string.Empty;
            }

            //Form prevDefaultOwner = AutoWaitingDialogWorker.DefaultOwnerForm;
            try
            {
                // AutoWaitingDialogWorker.DefaultOwnerForm = this;

                // 画面入力されたユーザの認証を実行
                bool success = m_service.Login(textBoxUserId.Text, textBoxPassword.Text, null, false);
                if (success)
                {
                    // ユーザ情報取得
                    if (InformationManager.UserInfo == null)
                        InformationManager.UserInfo =  CommonService.GetUserInfo();

                    var uinfo = InformationManager.UserInfo;

                    // Principal設定
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(uinfo.Id), uinfo.Roles);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    SubMessage = labelUserId.Text + "またはパスワードが不正です。";
                }
            }
            catch (Exception ex)
            {
                m_exception = ex;
                DialogResult = DialogResult.Abort;
            }
            finally
            {
                //AutoWaitingDialogWorker.DefaultOwnerForm = prevDefaultOwner;
            }
        }

        //************************************************************************
        /// <summary>
        /// コード値の頭０埋めを行う。
        /// </summary>
        //************************************************************************
        private void textBoxUserId_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (textBoxUserId.Text.Length > 0) textBoxUserId.Text =
                textBoxUserId.Text.PadLeft(textBoxUserId.MaxLength, '0');
        }
        #endregion
    }
}