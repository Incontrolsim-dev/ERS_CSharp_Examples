namespace WinFormsExample
{
    partial class FormScriptEditor
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
            erS4dScriptEditor1 = new Ers.WinForms.Ers4DScriptEditor();
            splitContainer1 = new SplitContainer();
            runButton = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // erS4dScriptEditor1
            // 
            erS4dScriptEditor1.Dock = DockStyle.Fill;
            erS4dScriptEditor1.Location = new Point(0, 0);
            erS4dScriptEditor1.Margin = new Padding(1);
            erS4dScriptEditor1.Name = "erS4dScriptEditor1";
            erS4dScriptEditor1.Size = new Size(560, 227);
            erS4dScriptEditor1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(erS4dScriptEditor1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(runButton);
            splitContainer1.Size = new Size(560, 270);
            splitContainer1.SplitterDistance = 227;
            splitContainer1.TabIndex = 1;
            // 
            // runButton
            // 
            runButton.Location = new Point(6, 8);
            runButton.Name = "runButton";
            runButton.Size = new Size(75, 23);
            runButton.TabIndex = 0;
            runButton.Text = "Run";
            runButton.UseVisualStyleBackColor = true;
            runButton.Click += button1_Click;
            // 
            // FormScriptEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(560, 270);
            Controls.Add(splitContainer1);
            Margin = new Padding(2);
            Name = "FormScriptEditor";
            Text = "Script editor";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Ers.WinForms.Ers4DScriptEditor erS4dScriptEditor1;
        private SplitContainer splitContainer1;
        private Button runButton;
    }
}