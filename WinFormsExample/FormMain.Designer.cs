namespace WinFormsExample
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TreeNode treeNode1        = new TreeNode("Model");
            TreeNode treeNode2        = new TreeNode("Entities");
            menuStrip1                = new MenuStrip();
            fileToolStripMenuItem     = new ToolStripMenuItem();
            exitToolStripMenuItem     = new ToolStripMenuItem();
            editToolStripMenuItem     = new ToolStripMenuItem();
            scriptToolStripMenuItem   = new ToolStripMenuItem();
            reloadToolStripMenuItem   = new ToolStripMenuItem();
            splitContainerMainView    = new SplitContainer();
            ersVisualization1         = new Ers.WinForms.ErsVisualization();
            ersLogger1                = new Ers.WinForms.ErsLogger();
            ersClock1                 = new Ers.WinForms.ErsClock();
            ersTreeView1              = new Ers.WinForms.ErsTreeView();
            ersRunControl1            = new Ers.WinForms.ErsRunControl();
            comboBox3DCameraMode      = new ComboBox();
            comboBox2D3D              = new ComboBox();
            panelLeft                 = new Panel();
            labelSpeedVsRealtime      = new Label();
            splitContainerRight       = new SplitContainer();
            propertyGrid1             = new Ers.WinForms.ErsPropertyGrid();
            panelMain                 = new Panel();
            splitContainerLeftMiddle  = new SplitContainer();
            splitContainerMiddleRight = new SplitContainer();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMainView).BeginInit();
            splitContainerMainView.Panel1.SuspendLayout();
            splitContainerMainView.Panel2.SuspendLayout();
            splitContainerMainView.SuspendLayout();
            panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerRight).BeginInit();
            splitContainerRight.Panel1.SuspendLayout();
            splitContainerRight.Panel2.SuspendLayout();
            splitContainerRight.SuspendLayout();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerLeftMiddle).BeginInit();
            splitContainerLeftMiddle.Panel1.SuspendLayout();
            splitContainerLeftMiddle.Panel2.SuspendLayout();
            splitContainerLeftMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMiddleRight).BeginInit();
            splitContainerMiddleRight.Panel1.SuspendLayout();
            splitContainerMiddleRight.Panel2.SuspendLayout();
            splitContainerMiddleRight.SuspendLayout();
            SuspendLayout();
            //
            // menuStrip1
            //
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name     = "menuStrip1";
            menuStrip1.Size     = new Size(1259, 33);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text     = "menuStrip1";
            //
            // fileToolStripMenuItem
            //
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            //
            // exitToolStripMenuItem
            //
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(141, 34);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            //
            // editToolStripMenuItem
            //
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { scriptToolStripMenuItem, reloadToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(58, 29);
            editToolStripMenuItem.Text = "Edit";
            //
            // scriptToolStripMenuItem
            //
            scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            scriptToolStripMenuItem.Size = new Size(230, 34);
            scriptToolStripMenuItem.Text = "Script";
            scriptToolStripMenuItem.Click += scriptToolStripMenuItem_Click;
            //
            // reloadToolStripMenuItem
            //
            reloadToolStripMenuItem.Name         = "reloadToolStripMenuItem";
            reloadToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            reloadToolStripMenuItem.Size         = new Size(230, 34);
            reloadToolStripMenuItem.Text         = "Reload";
            reloadToolStripMenuItem.Click += reloadToolStripMenuItem_Click;
            //
            // splitContainerMainView
            //
            splitContainerMainView.BorderStyle = BorderStyle.FixedSingle;
            splitContainerMainView.Dock        = DockStyle.Fill;
            splitContainerMainView.Location    = new Point(0, 0);
            splitContainerMainView.Name        = "splitContainerMainView";
            splitContainerMainView.Orientation = Orientation.Horizontal;
            //
            // splitContainerMainView.Panel1
            //
            splitContainerMainView.Panel1.Controls.Add(ersVisualization1);
            //
            // splitContainerMainView.Panel2
            //
            splitContainerMainView.Panel2.Controls.Add(ersLogger1);
            splitContainerMainView.Size             = new Size(701, 630);
            splitContainerMainView.SplitterDistance = 515;
            splitContainerMainView.SplitterWidth    = 3;
            splitContainerMainView.TabIndex         = 1;
            //
            // ersVisualization1
            //
            ersVisualization1.BackColor       = Color.FromArgb(178, 178, 255);
            ersVisualization1.Dock            = DockStyle.Fill;
            ersVisualization1.Location        = new Point(0, 0);
            ersVisualization1.Margin          = new Padding(1, 2, 1, 2);
            ersVisualization1.Name            = "ersVisualization1";
            ersVisualization1.RenderMode      = Ers.WinForms.RenderMode.Render2D;
            ersVisualization1.Size            = new Size(699, 513);
            ersVisualization1.TabIndex        = 0;
            ersVisualization1.TargetFrameTime = 16F;
            //
            // ersLogger1
            //
            ersLogger1.BorderStyle                     = BorderStyle.None;
            ersLogger1.Dock                            = DockStyle.Fill;
            ersLogger1.Font                            = new Font("Consolas", 9F);
            ersLogger1.FullRowSelect                   = true;
            ersLogger1.GridLines                       = true;
            ersLogger1.HeaderFront                     = new Font("Segoe UI", 9F);
            ersLogger1.Location                        = new Point(0, 0);
            ersLogger1.MultiSelect                     = false;
            ersLogger1.Name                            = "ersLogger1";
            ersLogger1.OwnerDraw                       = true;
            ersLogger1.Size                            = new Size(699, 110);
            ersLogger1.TabIndex                        = 0;
            ersLogger1.UseCompatibleStateImageBehavior = false;
            ersLogger1.View                            = View.Details;
            ersLogger1.VirtualMode                     = true;
            //
            // ersClock1
            //
            ersClock1.BackColor         = SystemColors.Control;
            ersClock1.BezelColor        = Color.Black;
            ersClock1.DateFormat        = "ddd MMM dd yyyy";
            ersClock1.DigitalTimeFormat = "HH:mm:ss";
            ersClock1.Dock              = DockStyle.Bottom;
            ersClock1.GradientColor1    = Color.White;
            ersClock1.GradientColor2    = Color.LightGray;
            ersClock1.GradientMode      = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            ersClock1.HourHandColor     = Color.Black;
            ersClock1.HourMarkerColor   = Color.Black;
            ersClock1.Location          = new Point(0, 286);
            ersClock1.Margin            = new Padding(1, 2, 1, 2);
            ersClock1.MinuteHandColor   = Color.Black;
            ersClock1.Name              = "ersClock1";
            ersClock1.RunningTimeFormat = "hh:mm:ss";
            ersClock1.SecondHandColor   = Color.Black;
            ersClock1.Size              = new Size(298, 342);
            ersClock1.TabIndex          = 2;
            //
            // ersTreeView1
            //
            ersTreeView1.Dock              = DockStyle.Fill;
            ersTreeView1.ExpandAllOnCreate = true;
            ersTreeView1.Location          = new Point(0, 0);
            ersTreeView1.Name              = "ersTreeView1";
            treeNode1.Name                 = "";
            treeNode1.Text                 = "Model";
            treeNode1.ToolTipText          = "Model containers and simulators";
            treeNode2.Name                 = "";
            treeNode2.Text                 = "Entities";
            treeNode2.ToolTipText          = "All entities in the model";
            ersTreeView1.Nodes.AddRange(new TreeNode[] { treeNode1, treeNode2 });
            ersTreeView1.ShowNodeToolTips = true;
            ersTreeView1.Size             = new Size(250, 309);
            ersTreeView1.TabIndex         = 4;
            //
            // ersRunControl1
            //
            ersRunControl1.Dock     = DockStyle.Bottom;
            ersRunControl1.Interval = 1;
            ersRunControl1.Location = new Point(0, 179);
            ersRunControl1.Margin   = new Padding(1, 2, 1, 2);
            ersRunControl1.Name     = "ersRunControl1";
            ersRunControl1.Running  = false;
            ersRunControl1.Size     = new Size(298, 107);
            ersRunControl1.StepSize = 1UL;
            ersRunControl1.TabIndex = 3;
            //
            // comboBox3DCameraMode
            //
            comboBox3DCameraMode.Dock              = DockStyle.Top;
            comboBox3DCameraMode.DropDownStyle     = ComboBoxStyle.DropDownList;
            comboBox3DCameraMode.FormattingEnabled = true;
            comboBox3DCameraMode.Items.AddRange(new object[] { "Spherical", "First Person", "Flying" });
            comboBox3DCameraMode.Location = new Point(0, 33);
            comboBox3DCameraMode.Name     = "comboBox3DCameraMode";
            comboBox3DCameraMode.Size     = new Size(298, 33);
            comboBox3DCameraMode.TabIndex = 1;
            comboBox3DCameraMode.SelectedIndexChanged += comboBox3DCameraMode_SelectedIndexChanged;
            //
            // comboBox2D3D
            //
            comboBox2D3D.Dock              = DockStyle.Top;
            comboBox2D3D.DropDownStyle     = ComboBoxStyle.DropDownList;
            comboBox2D3D.FormattingEnabled = true;
            comboBox2D3D.Items.AddRange(new object[] { "2D", "3D" });
            comboBox2D3D.Location = new Point(0, 0);
            comboBox2D3D.Name     = "comboBox2D3D";
            comboBox2D3D.Size     = new Size(298, 33);
            comboBox2D3D.TabIndex = 0;
            comboBox2D3D.SelectedIndexChanged += comboBox2D3D_SelectedIndexChanged;
            //
            // panelLeft
            //
            panelLeft.BorderStyle = BorderStyle.FixedSingle;
            panelLeft.Controls.Add(labelSpeedVsRealtime);
            panelLeft.Controls.Add(ersRunControl1);
            panelLeft.Controls.Add(comboBox3DCameraMode);
            panelLeft.Controls.Add(comboBox2D3D);
            panelLeft.Controls.Add(ersClock1);
            panelLeft.Dock     = DockStyle.Fill;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name     = "panelLeft";
            panelLeft.Size     = new Size(300, 630);
            panelLeft.TabIndex = 2;
            //
            // labelSpeedVsRealtime
            //
            labelSpeedVsRealtime.AutoSize = true;
            labelSpeedVsRealtime.Dock     = DockStyle.Bottom;
            labelSpeedVsRealtime.Location = new Point(0, 154);
            labelSpeedVsRealtime.Margin   = new Padding(4, 0, 4, 0);
            labelSpeedVsRealtime.Name     = "labelSpeedVsRealtime";
            labelSpeedVsRealtime.Size     = new Size(234, 25);
            labelSpeedVsRealtime.TabIndex = 4;
            labelSpeedVsRealtime.Text     = "SimulationSpeedVSRealtime";
            //
            // splitContainerRight
            //
            splitContainerRight.Dock        = DockStyle.Fill;
            splitContainerRight.FixedPanel  = FixedPanel.Panel2;
            splitContainerRight.Location    = new Point(0, 0);
            splitContainerRight.Name        = "splitContainerRight";
            splitContainerRight.Orientation = Orientation.Horizontal;
            //
            // splitContainerRight.Panel1
            //
            splitContainerRight.Panel1.Controls.Add(ersTreeView1);
            //
            // splitContainerRight.Panel2
            //
            splitContainerRight.Panel2.Controls.Add(propertyGrid1);
            splitContainerRight.Size             = new Size(250, 630);
            splitContainerRight.SplitterDistance = 309;
            splitContainerRight.SplitterWidth    = 3;
            splitContainerRight.TabIndex         = 3;
            //
            // propertyGrid1
            //
            propertyGrid1.Dock     = DockStyle.Fill;
            propertyGrid1.Location = new Point(0, 0);
            propertyGrid1.Name     = "propertyGrid1";
            propertyGrid1.Size     = new Size(250, 318);
            propertyGrid1.TabIndex = 0;
            //
            // panelMain
            //
            panelMain.Controls.Add(splitContainerMainView);
            panelMain.Dock     = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Name     = "panelMain";
            panelMain.Size     = new Size(701, 630);
            panelMain.TabIndex = 5;
            //
            // splitContainerLeftMiddle
            //
            splitContainerLeftMiddle.Dock       = DockStyle.Fill;
            splitContainerLeftMiddle.FixedPanel = FixedPanel.Panel1;
            splitContainerLeftMiddle.Location   = new Point(0, 33);
            splitContainerLeftMiddle.Name       = "splitContainerLeftMiddle";
            //
            // splitContainerLeftMiddle.Panel1
            //
            splitContainerLeftMiddle.Panel1.Controls.Add(panelLeft);
            //
            // splitContainerLeftMiddle.Panel2
            //
            splitContainerLeftMiddle.Panel2.Controls.Add(splitContainerMiddleRight);
            splitContainerLeftMiddle.Size             = new Size(1259, 630);
            splitContainerLeftMiddle.SplitterDistance = 300;
            splitContainerLeftMiddle.TabIndex         = 6;
            //
            // splitContainerMiddleRight
            //
            splitContainerMiddleRight.Dock       = DockStyle.Fill;
            splitContainerMiddleRight.FixedPanel = FixedPanel.Panel2;
            splitContainerMiddleRight.Location   = new Point(0, 0);
            splitContainerMiddleRight.Name       = "splitContainerMiddleRight";
            //
            // splitContainerMiddleRight.Panel1
            //
            splitContainerMiddleRight.Panel1.Controls.Add(panelMain);
            //
            // splitContainerMiddleRight.Panel2
            //
            splitContainerMiddleRight.Panel2.Controls.Add(splitContainerRight);
            splitContainerMiddleRight.Size             = new Size(955, 630);
            splitContainerMiddleRight.SplitterDistance = 701;
            splitContainerMiddleRight.TabIndex         = 0;
            //
            // FormMain
            //
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode       = AutoScaleMode.Font;
            ClientSize          = new Size(1259, 663);
            Controls.Add(splitContainerLeftMiddle);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name          = "FormMain";
            Text          = "Windows Forms example";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainerMainView.Panel1.ResumeLayout(false);
            splitContainerMainView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMainView).EndInit();
            splitContainerMainView.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            splitContainerRight.Panel1.ResumeLayout(false);
            splitContainerRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerRight).EndInit();
            splitContainerRight.ResumeLayout(false);
            panelMain.ResumeLayout(false);
            splitContainerLeftMiddle.Panel1.ResumeLayout(false);
            splitContainerLeftMiddle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerLeftMiddle).EndInit();
            splitContainerLeftMiddle.ResumeLayout(false);
            splitContainerMiddleRight.Panel1.ResumeLayout(false);
            splitContainerMiddleRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMiddleRight).EndInit();
            splitContainerMiddleRight.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

#endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem scriptToolStripMenuItem;
        private SplitContainer splitContainerMainView;
        private Ers.WinForms.ErsVisualization ersVisualization1;
        private Ers.WinForms.ErsClock ersClock1;
        private Ers.WinForms.ErsTreeView ersTreeView1;
        private Ers.WinForms.ErsRunControl ersRunControl1;
        private ComboBox comboBox3DCameraMode;
        private ComboBox comboBox2D3D;
        private SplitContainer splitContainerLeftMiddle;
        private Panel panelLeft;
        private SplitContainer splitContainerRight;
        private Panel panelMain;
        private SplitContainer splitContainerMiddleRight;
        private Ers.WinForms.ErsPropertyGrid propertyGrid1;
        private Ers.WinForms.ErsLogger ersLogger1;
        private ToolStripMenuItem reloadToolStripMenuItem;
        private Label labelSpeedVsRealtime;
    }
}
