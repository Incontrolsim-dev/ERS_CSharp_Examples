using System;

namespace WinFormsExample
{
    public partial class FormLauncher : Form
    {
        public FormLauncher() { InitializeComponent(); }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            FormMain formMain = new FormMain();
            formMain.Show();
        }
    }
}
