using MEFDemoSimple.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MEFDemoSimple
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListBox1.DataBind();
                TextBox1.Text = Manager.ContentProvider.Get();                
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            TextBox2.Text = Manager.Execute(ListBox1.SelectedValue, TextBox1.Text);
        }

        protected void ListBox1_DataBinding(object sender, EventArgs e)
        {
            ListBox l = sender as ListBox;
            if (l != null)
            {
                l.DataValueField = "Name";
                l.DataTextField = "Name";
                l.DataSource = Manager.Operations;
            }
        }
    }
}