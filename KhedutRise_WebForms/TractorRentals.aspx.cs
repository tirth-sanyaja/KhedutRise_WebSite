using System;
using System.Data;
using System.Text;

namespace KhedutRise
{
    public partial class TractorRentals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEquipments("");
            }
        }

        private void LoadEquipments(string search)
        {
            try
            {
                DataTable dt = EquipmentRepository.GetEquipments(search);
                if (dt != null && dt.Rows.Count > 0)
                {
                    rptEquipments.DataSource = dt;
                    rptEquipments.DataBind();
                    rptEquipments.Visible = true;
                    pnlNoEquipment.Visible = false;
                }
                else
                {
                    rptEquipments.Visible = false;
                    pnlNoEquipment.Visible = true;
                }
            }
            catch (Exception)
            {
                rptEquipments.Visible = false;
                pnlNoEquipment.Visible = true;
            }
        }

        // Build attachment badge HTML for each tractor's listed attachments
        public string BuildAttachmentBadges(object attachmentsObj)
        {
            string raw = (attachmentsObj ?? "").ToString().Trim();
            if (string.IsNullOrEmpty(raw)) return "<span class=\"text-muted small\">No attachments listed</span>";

            string[] parts = raw.Split(',');
            StringBuilder sb = new StringBuilder();
            foreach (string part in parts)
            {
                string name = part.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    sb.AppendFormat(
                        "<span class=\"badge me-1 mb-1\" style=\"background:#d4edda; color:#155724; border:1px solid #c3e6cb; font-weight:600; font-size:0.75rem; padding:4px 8px; border-radius:6px;\">{0}</span>",
                        name);
                }
            }
            return sb.ToString();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            LoadEquipments(search);
        }

        protected void btnResetSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadEquipments("");
        }
    }
}
