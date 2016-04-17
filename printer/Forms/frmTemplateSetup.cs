using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DogeAddress.Forms
{
    public partial class frmTemplateSetup : Form
    {

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void frmTemplateSetup_Load(object sender, EventArgs e)
        {
            ddlPaperSize.DataSource = Enum.GetValues(typeof(PdfSharp.PageSize));

        }


    }
}
