using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ers;

namespace WinFormsExample
{
    public partial class FormScriptEditor : Form
    {
        public FormScriptEditor() { InitializeComponent(); }

        private void button1_Click(object sender, EventArgs e)
        {
            SubModel subModel = SubModel.GetSubModel();
            string code = erS4dScriptEditor1.Text;
            subModel.RunSimpleString(code);
        }
    }
}
