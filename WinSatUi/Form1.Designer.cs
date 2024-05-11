namespace WinSatUi
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.startButton = new MetroFramework.Controls.MetroButton();
            this.versionBox = new System.Windows.Forms.PictureBox();
            this.metroProgressSpinner1 = new MetroFramework.Controls.MetroProgressSpinner();
            this.cpuValLabel = new MetroFramework.Controls.MetroLabel();
            this.memValLabel = new MetroFramework.Controls.MetroLabel();
            this.graphicsValLabel = new MetroFramework.Controls.MetroLabel();
            this.gameValLabel = new MetroFramework.Controls.MetroLabel();
            this.diskValLabel = new MetroFramework.Controls.MetroLabel();
            this.cpuLabel = new MetroFramework.Controls.MetroLabel();
            this.memLabel = new MetroFramework.Controls.MetroLabel();
            this.graphicsLabel = new MetroFramework.Controls.MetroLabel();
            this.gameLabel = new MetroFramework.Controls.MetroLabel();
            this.diskLabel = new MetroFramework.Controls.MetroLabel();
            this.valueBox = new System.Windows.Forms.GroupBox();
            this.recentButton = new MetroFramework.Controls.MetroButton();
            this.stopButton = new MetroFramework.Controls.MetroButton();
            this.buildLabel = new MetroFramework.Controls.MetroLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.versionBox)).BeginInit();
            this.valueBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            resources.ApplyResources(this.startButton, "startButton");
            this.startButton.Name = "startButton";
            this.startButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.startButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.startButton.UseSelectable = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // versionBox
            // 
            resources.ApplyResources(this.versionBox, "versionBox");
            this.versionBox.Name = "versionBox";
            this.versionBox.TabStop = false;
            // 
            // metroProgressSpinner1
            // 
            resources.ApplyResources(this.metroProgressSpinner1, "metroProgressSpinner1");
            this.metroProgressSpinner1.Maximum = 100;
            this.metroProgressSpinner1.Name = "metroProgressSpinner1";
            this.metroProgressSpinner1.Speed = 3F;
            this.metroProgressSpinner1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroProgressSpinner1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroProgressSpinner1.UseSelectable = true;
            // 
            // cpuValLabel
            // 
            resources.ApplyResources(this.cpuValLabel, "cpuValLabel");
            this.cpuValLabel.Name = "cpuValLabel";
            this.cpuValLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.cpuValLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // memValLabel
            // 
            resources.ApplyResources(this.memValLabel, "memValLabel");
            this.memValLabel.Name = "memValLabel";
            this.memValLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.memValLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // graphicsValLabel
            // 
            resources.ApplyResources(this.graphicsValLabel, "graphicsValLabel");
            this.graphicsValLabel.Name = "graphicsValLabel";
            this.graphicsValLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.graphicsValLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // gameValLabel
            // 
            resources.ApplyResources(this.gameValLabel, "gameValLabel");
            this.gameValLabel.Name = "gameValLabel";
            this.gameValLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.gameValLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // diskValLabel
            // 
            resources.ApplyResources(this.diskValLabel, "diskValLabel");
            this.diskValLabel.Name = "diskValLabel";
            this.diskValLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.diskValLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // cpuLabel
            // 
            resources.ApplyResources(this.cpuLabel, "cpuLabel");
            this.cpuLabel.Name = "cpuLabel";
            this.cpuLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.cpuLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            this.toolTip1.SetToolTip(this.cpuLabel, resources.GetString("cpuLabel.ToolTip"));
            // 
            // memLabel
            // 
            resources.ApplyResources(this.memLabel, "memLabel");
            this.memLabel.Name = "memLabel";
            this.memLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.memLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            this.toolTip1.SetToolTip(this.memLabel, resources.GetString("memLabel.ToolTip"));
            // 
            // graphicsLabel
            // 
            resources.ApplyResources(this.graphicsLabel, "graphicsLabel");
            this.graphicsLabel.Name = "graphicsLabel";
            this.graphicsLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.graphicsLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            this.toolTip1.SetToolTip(this.graphicsLabel, resources.GetString("graphicsLabel.ToolTip"));
            // 
            // gameLabel
            // 
            resources.ApplyResources(this.gameLabel, "gameLabel");
            this.gameLabel.Name = "gameLabel";
            this.gameLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.gameLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            this.toolTip1.SetToolTip(this.gameLabel, resources.GetString("gameLabel.ToolTip"));
            // 
            // diskLabel
            // 
            resources.ApplyResources(this.diskLabel, "diskLabel");
            this.diskLabel.Name = "diskLabel";
            this.diskLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.diskLabel.Theme = MetroFramework.MetroThemeStyle.Light;
            this.toolTip1.SetToolTip(this.diskLabel, resources.GetString("diskLabel.ToolTip"));
            // 
            // valueBox
            // 
            this.valueBox.Controls.Add(this.memLabel);
            this.valueBox.Controls.Add(this.diskLabel);
            this.valueBox.Controls.Add(this.cpuValLabel);
            this.valueBox.Controls.Add(this.gameLabel);
            this.valueBox.Controls.Add(this.memValLabel);
            this.valueBox.Controls.Add(this.graphicsLabel);
            this.valueBox.Controls.Add(this.graphicsValLabel);
            this.valueBox.Controls.Add(this.gameValLabel);
            this.valueBox.Controls.Add(this.cpuLabel);
            this.valueBox.Controls.Add(this.diskValLabel);
            resources.ApplyResources(this.valueBox, "valueBox");
            this.valueBox.Name = "valueBox";
            this.valueBox.TabStop = false;
            // 
            // recentButton
            // 
            resources.ApplyResources(this.recentButton, "recentButton");
            this.recentButton.Name = "recentButton";
            this.recentButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.recentButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.recentButton.UseSelectable = true;
            this.recentButton.Click += new System.EventHandler(this.recentButton_Click);
            // 
            // stopButton
            // 
            resources.ApplyResources(this.stopButton, "stopButton");
            this.stopButton.Name = "stopButton";
            this.stopButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.stopButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.stopButton.UseSelectable = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // buildLabel
            // 
            resources.ApplyResources(this.buildLabel, "buildLabel");
            this.buildLabel.Name = "buildLabel";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buildLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.recentButton);
            this.Controls.Add(this.valueBox);
            this.Controls.Add(this.metroProgressSpinner1);
            this.Controls.Add(this.versionBox);
            this.Controls.Add(this.startButton);
            this.Icon = global::WinSatUi.Properties.Resources.WinSatUi;
            this.Name = "Form1";
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.versionBox)).EndInit();
            this.valueBox.ResumeLayout(false);
            this.valueBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroButton startButton;
        private System.Windows.Forms.PictureBox versionBox;
        private MetroFramework.Controls.MetroProgressSpinner metroProgressSpinner1;
        private MetroFramework.Controls.MetroLabel cpuValLabel;
        private MetroFramework.Controls.MetroLabel memValLabel;
        private MetroFramework.Controls.MetroLabel graphicsValLabel;
        private MetroFramework.Controls.MetroLabel gameValLabel;
        private MetroFramework.Controls.MetroLabel diskValLabel;
        private MetroFramework.Controls.MetroLabel cpuLabel;
        private MetroFramework.Controls.MetroLabel memLabel;
        private MetroFramework.Controls.MetroLabel graphicsLabel;
        private MetroFramework.Controls.MetroLabel gameLabel;
        private MetroFramework.Controls.MetroLabel diskLabel;
        private System.Windows.Forms.GroupBox valueBox;
        private MetroFramework.Controls.MetroButton recentButton;
        private MetroFramework.Controls.MetroButton stopButton;
        private MetroFramework.Controls.MetroLabel buildLabel;
        public System.Windows.Forms.ToolTip toolTip1;
    }
}

