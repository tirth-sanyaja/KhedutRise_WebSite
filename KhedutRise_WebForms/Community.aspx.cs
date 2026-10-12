using System;

namespace KhedutRise
{
    // Page 25: Community - Farmer discussion forums and insights with validation
    public partial class Community : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnPostQuestion_Click(object sender, EventArgs e)
        {
            string topic = txtTopic.Text.Trim();
            string category = ddlTopicCategory.SelectedValue;
            string details = txtQuestionDetails.Text.Trim();

            if (string.IsNullOrWhiteSpace(topic) || topic.Length < 3)
            {
                lblForumMsg.Text = "Please enter a meaningful discussion topic title.";
                lblForumMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            if (string.IsNullOrWhiteSpace(details) || details.Length < 10)
            {
                lblForumMsg.Text = "Please provide question details (minimum 10 characters).";
                lblForumMsg.CssClass = "alert alert-danger d-block p-2 text-center small mb-3";
                return;
            }

            lblForumMsg.Text = "Question posted to community under <b>[" + category + "]</b>! Local farmers and agri-experts can now reply.";
            lblForumMsg.CssClass = "alert alert-success d-block p-2 text-center small mb-3";

            txtTopic.Text = "";
            txtQuestionDetails.Text = "";
        }
    }
}
