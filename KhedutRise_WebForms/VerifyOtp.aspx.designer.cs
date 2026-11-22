namespace KhedutRise
{
    public partial class VerifyOtp
    {
        protected global::System.Web.UI.WebControls.Label lblVerifyMsg;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl badgePurpose;
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divTargetEmail;
        protected global::System.Web.UI.WebControls.TextBox txtOtpCode;
        protected global::System.Web.UI.WebControls.RequiredFieldValidator rfvOtp;
        protected global::System.Web.UI.WebControls.RegularExpressionValidator revOtp;
        protected global::System.Web.UI.WebControls.Button btnConfirmOtp;
        protected global::System.Web.UI.WebControls.LinkButton btnResendOtp;
        protected global::System.Web.UI.WebControls.HyperLink hlBack;
        protected global::System.Web.UI.WebControls.HiddenField hfCooldownRemaining;
        protected global::System.Web.UI.WebControls.HiddenField hfPurpose;
        protected global::System.Web.UI.WebControls.HiddenField hfEmail;
        protected global::System.Web.UI.WebControls.HiddenField hfIdent;
        protected global::System.Web.UI.WebControls.HiddenField hfRole;
    }
}
