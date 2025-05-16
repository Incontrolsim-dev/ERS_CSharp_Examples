namespace WinFormsExample
{
    partial class FormLauncher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

#region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonStart = new Button();
            SuspendLayout();
            //
            // buttonStart
            //
            buttonStart.Dock                    = DockStyle.Fill;
            buttonStart.Font                    = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonStart.Location                = new Point(0, 0);
            buttonStart.Name                    = "buttonStart";
            buttonStart.Size                    = new Size(800, 450);
            buttonStart.TabIndex                = 0;
            buttonStart.Text                    = "Start";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            //
            // FormLauncher
            //
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode       = AutoScaleMode.Font;
            ClientSize          = new Size(800, 450);
            Controls.Add(buttonStart);
            Name = "FormLauncher";
            Text = "FormLauncher";
            ResumeLayout(false);
        }

#endregion

        private Button buttonStart;
    }
}