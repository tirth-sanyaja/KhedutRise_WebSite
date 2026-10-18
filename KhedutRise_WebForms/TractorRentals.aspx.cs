using System;
using System.Data;

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
