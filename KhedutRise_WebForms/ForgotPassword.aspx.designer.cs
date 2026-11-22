namespace KhedutRise
{
    public partial class ForgotPassword
    {
        protected global::System.Web.UI.WebControls.Label lblResetMsg;
        protected global::System.Web.UI.WebControls.Panel pnlRequestOtp;
        protected global::System.Web.UI.WebControls.TextBox txtResetEmail;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvResetEmail;
        protected global::System.Web.UI.WebControls.RegularExpressionValidator revResetEmail;
        protected global::System.Web.UI.WebControls.Button btnSendResetOtp;
        protected global::System.Web.UI.WebControls.Panel pnlVerifyAndReset;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divTargetEmail;
        protected global::System.Web.UI.WebControls.TextBox txtResetOtp;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvResetOtp;
        protected global::System.Web.UI.WebControls.RegularExpressionValidator revResetOtp;
        protected global::System.Web.UI.WebControls.TextBox txtNewPassword;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvNewPassword;
        protected global::System.Web.UI.WebControls.TextBox txtConfirmPassword;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvConfirmPassword;
        protected global::System.Web.UI.WebControls.CompareValidator cvPasswords;
        protected global::System.Web.UI.WebControls.Button btnSubmitReset;
        protected global::System.Web.UI.WebControls.LinkButton btnResendResetOtp;
        protected global::System.Web.UI.WebControls.HiddenField hfCooldownRemaining;
        protected global::System.Web.UI.WebControls.HiddenField hfResetEmail;
    }
}
