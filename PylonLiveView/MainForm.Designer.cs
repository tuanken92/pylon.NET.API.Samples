namespace PylonLiveView
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainerImageView = new System.Windows.Forms.SplitContainer();
            this.splitContainerConfiguration = new System.Windows.Forms.SplitContainer();
            this.deviceListView = new System.Windows.Forms.ListView();
            this.imageListForDeviceList = new System.Windows.Forms.ImageList(this.components);
            this.TriggerSensor_Chbx = new System.Windows.Forms.CheckBox();
            this.TestOff_Btn = new System.Windows.Forms.Button();
            this.TestOn_Btn = new System.Windows.Forms.Button();
            this.SettingBtn = new System.Windows.Forms.Button();
            this.CloseCamera_Btn = new System.Windows.Forms.Button();
            this.GetFrame_Btn = new System.Windows.Forms.Button();
            this.num_queue_nbup = new System.Windows.Forms.NumericUpDown();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOneShot = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonContinuousShot = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.cogRecordDisplay1 = new Cognex.VisionPro.CogRecordDisplay();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.updateDeviceListTimer = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.exposureTimeSliderControl = new PylonLiveViewControl.FloatSliderUserControl();
            this.gainSliderControl = new PylonLiveViewControl.FloatSliderUserControl();
            this.heightSliderControl = new PylonLiveViewControl.IntSliderUserControl();
            this.widthSliderControl = new PylonLiveViewControl.IntSliderUserControl();
            this.pixelFormatControl = new PylonLiveViewControl.EnumerationComboBoxUserControl();
            this.testImageControl = new PylonLiveViewControl.EnumerationComboBoxUserControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImageView)).BeginInit();
            this.splitContainerImageView.Panel1.SuspendLayout();
            this.splitContainerImageView.Panel2.SuspendLayout();
            this.splitContainerImageView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerConfiguration)).BeginInit();
            this.splitContainerConfiguration.Panel1.SuspendLayout();
            this.splitContainerConfiguration.Panel2.SuspendLayout();
            this.splitContainerConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_queue_nbup)).BeginInit();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerImageView
            // 
            this.splitContainerImageView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerImageView.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerImageView.Location = new System.Drawing.Point(0, 0);
            this.splitContainerImageView.Name = "splitContainerImageView";
            // 
            // splitContainerImageView.Panel1
            // 
            this.splitContainerImageView.Panel1.Controls.Add(this.splitContainerConfiguration);
            this.splitContainerImageView.Panel1.Controls.Add(this.toolStrip);
            // 
            // splitContainerImageView.Panel2
            // 
            this.splitContainerImageView.Panel2.AutoScroll = true;
            this.splitContainerImageView.Panel2.Controls.Add(this.cogRecordDisplay1);
            this.splitContainerImageView.Panel2.Controls.Add(this.pictureBox);
            this.splitContainerImageView.Size = new System.Drawing.Size(1047, 592);
            this.splitContainerImageView.SplitterDistance = 261;
            this.splitContainerImageView.TabIndex = 0;
            this.splitContainerImageView.TabStop = false;
            // 
            // splitContainerConfiguration
            // 
            this.splitContainerConfiguration.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerConfiguration.Location = new System.Drawing.Point(0, 39);
            this.splitContainerConfiguration.Name = "splitContainerConfiguration";
            this.splitContainerConfiguration.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerConfiguration.Panel1
            // 
            this.splitContainerConfiguration.Panel1.Controls.Add(this.deviceListView);
            // 
            // splitContainerConfiguration.Panel2
            // 
            this.splitContainerConfiguration.Panel2.Controls.Add(this.TriggerSensor_Chbx);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.TestOff_Btn);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.TestOn_Btn);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.SettingBtn);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.CloseCamera_Btn);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.GetFrame_Btn);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.num_queue_nbup);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.exposureTimeSliderControl);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.gainSliderControl);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.heightSliderControl);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.widthSliderControl);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.pixelFormatControl);
            this.splitContainerConfiguration.Panel2.Controls.Add(this.testImageControl);
            this.splitContainerConfiguration.Size = new System.Drawing.Size(261, 553);
            this.splitContainerConfiguration.SplitterDistance = 98;
            this.splitContainerConfiguration.TabIndex = 1;
            this.splitContainerConfiguration.TabStop = false;
            // 
            // deviceListView
            // 
            this.deviceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceListView.HideSelection = false;
            this.deviceListView.LargeImageList = this.imageListForDeviceList;
            this.deviceListView.Location = new System.Drawing.Point(0, 0);
            this.deviceListView.MultiSelect = false;
            this.deviceListView.Name = "deviceListView";
            this.deviceListView.ShowItemToolTips = true;
            this.deviceListView.Size = new System.Drawing.Size(257, 94);
            this.deviceListView.TabIndex = 0;
            this.deviceListView.UseCompatibleStateImageBehavior = false;
            this.deviceListView.View = System.Windows.Forms.View.Tile;
            this.deviceListView.SelectedIndexChanged += new System.EventHandler(this.deviceListView_SelectedIndexChanged);
            this.deviceListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.deviceListView_KeyDown);
            // 
            // imageListForDeviceList
            // 
            this.imageListForDeviceList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListForDeviceList.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListForDeviceList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // TriggerSensor_Chbx
            // 
            this.TriggerSensor_Chbx.AutoSize = true;
            this.TriggerSensor_Chbx.Location = new System.Drawing.Point(7, 369);
            this.TriggerSensor_Chbx.Name = "TriggerSensor_Chbx";
            this.TriggerSensor_Chbx.Size = new System.Drawing.Size(95, 17);
            this.TriggerSensor_Chbx.TabIndex = 7;
            this.TriggerSensor_Chbx.Text = "Trigger Sensor";
            this.TriggerSensor_Chbx.UseVisualStyleBackColor = true;
            this.TriggerSensor_Chbx.CheckedChanged += new System.EventHandler(this.TriggerSensor_Chbx_CheckedChanged);
            // 
            // TestOff_Btn
            // 
            this.TestOff_Btn.Location = new System.Drawing.Point(60, 401);
            this.TestOff_Btn.Name = "TestOff_Btn";
            this.TestOff_Btn.Size = new System.Drawing.Size(42, 23);
            this.TestOff_Btn.TabIndex = 2;
            this.TestOff_Btn.Text = "OFF";
            this.TestOff_Btn.UseVisualStyleBackColor = true;
            this.TestOff_Btn.Click += new System.EventHandler(this.TestOff_Btn_Click);
            // 
            // TestOn_Btn
            // 
            this.TestOn_Btn.Location = new System.Drawing.Point(7, 401);
            this.TestOn_Btn.Name = "TestOn_Btn";
            this.TestOn_Btn.Size = new System.Drawing.Size(42, 23);
            this.TestOn_Btn.TabIndex = 2;
            this.TestOn_Btn.Text = "ON";
            this.TestOn_Btn.UseVisualStyleBackColor = true;
            this.TestOn_Btn.Click += new System.EventHandler(this.TestBtnOn_Click);
            // 
            // SettingBtn
            // 
            this.SettingBtn.Location = new System.Drawing.Point(133, 363);
            this.SettingBtn.Name = "SettingBtn";
            this.SettingBtn.Size = new System.Drawing.Size(76, 23);
            this.SettingBtn.TabIndex = 2;
            this.SettingBtn.Text = "Setting";
            this.SettingBtn.UseVisualStyleBackColor = true;
            this.SettingBtn.Click += new System.EventHandler(this.SettingBtn_Click);
            // 
            // CloseCamera_Btn
            // 
            this.CloseCamera_Btn.Location = new System.Drawing.Point(162, 421);
            this.CloseCamera_Btn.Name = "CloseCamera_Btn";
            this.CloseCamera_Btn.Size = new System.Drawing.Size(92, 23);
            this.CloseCamera_Btn.TabIndex = 2;
            this.CloseCamera_Btn.Text = "Close Camera";
            this.CloseCamera_Btn.UseVisualStyleBackColor = true;
            this.CloseCamera_Btn.Click += new System.EventHandler(this.CloseCamera_Btn_Click);
            // 
            // GetFrame_Btn
            // 
            this.GetFrame_Btn.Location = new System.Drawing.Point(133, 334);
            this.GetFrame_Btn.Name = "GetFrame_Btn";
            this.GetFrame_Btn.Size = new System.Drawing.Size(76, 23);
            this.GetFrame_Btn.TabIndex = 2;
            this.GetFrame_Btn.Text = "Get Frame";
            this.GetFrame_Btn.UseVisualStyleBackColor = true;
            this.GetFrame_Btn.Click += new System.EventHandler(this.GetFrame_Btn_Click);
            // 
            // num_queue_nbup
            // 
            this.num_queue_nbup.Location = new System.Drawing.Point(7, 337);
            this.num_queue_nbup.Name = "num_queue_nbup";
            this.num_queue_nbup.Size = new System.Drawing.Size(120, 20);
            this.num_queue_nbup.TabIndex = 1;
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOneShot,
            this.toolStripButtonContinuousShot,
            this.toolStripButtonStop});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(261, 39);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripButtonOneShot
            // 
            this.toolStripButtonOneShot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOneShot.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOneShot.Image")));
            this.toolStripButtonOneShot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOneShot.Name = "toolStripButtonOneShot";
            this.toolStripButtonOneShot.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonOneShot.Text = "One Shot";
            this.toolStripButtonOneShot.ToolTipText = "One Shot";
            this.toolStripButtonOneShot.Click += new System.EventHandler(this.toolStripButtonOneShot_Click);
            // 
            // toolStripButtonContinuousShot
            // 
            this.toolStripButtonContinuousShot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonContinuousShot.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonContinuousShot.Image")));
            this.toolStripButtonContinuousShot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonContinuousShot.Name = "toolStripButtonContinuousShot";
            this.toolStripButtonContinuousShot.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonContinuousShot.Text = "Continuous Shot";
            this.toolStripButtonContinuousShot.ToolTipText = "Continuous Shot";
            this.toolStripButtonContinuousShot.Click += new System.EventHandler(this.toolStripButtonContinuousShot_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStop.Image")));
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonStop.Text = "Stop Grab";
            this.toolStripButtonStop.ToolTipText = "Stop Grab";
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // cogRecordDisplay1
            // 
            this.cogRecordDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogRecordDisplay1.ColorMapLowerRoiLimit = 0D;
            this.cogRecordDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogRecordDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogRecordDisplay1.ColorMapUpperRoiLimit = 1D;
            this.cogRecordDisplay1.DoubleTapZoomCycleLength = 2;
            this.cogRecordDisplay1.DoubleTapZoomSensitivity = 2.5D;
            this.cogRecordDisplay1.Location = new System.Drawing.Point(388, 8);
            this.cogRecordDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogRecordDisplay1.MouseWheelSensitivity = 1D;
            this.cogRecordDisplay1.Name = "cogRecordDisplay1";
            this.cogRecordDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogRecordDisplay1.OcxState")));
            this.cogRecordDisplay1.Size = new System.Drawing.Size(480, 480);
            this.cogRecordDisplay1.TabIndex = 26;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(5, 8);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(370, 480);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // updateDeviceListTimer
            // 
            this.updateDeviceListTimer.Enabled = true;
            this.updateDeviceListTimer.Interval = 5000;
            this.updateDeviceListTimer.Tick += new System.EventHandler(this.updateDeviceListTimer_Tick);
            // 
            // exposureTimeSliderControl
            // 
            this.exposureTimeSliderControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exposureTimeSliderControl.DefaultName = "N/A";
            this.exposureTimeSliderControl.Location = new System.Drawing.Point(0, 264);
            this.exposureTimeSliderControl.MinimumSize = new System.Drawing.Size(225, 50);
            this.exposureTimeSliderControl.Name = "exposureTimeSliderControl";
            this.exposureTimeSliderControl.Size = new System.Drawing.Size(260, 50);
            this.exposureTimeSliderControl.TabIndex = 6;
            // 
            // gainSliderControl
            // 
            this.gainSliderControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gainSliderControl.DefaultName = "N/A";
            this.gainSliderControl.Location = new System.Drawing.Point(0, 214);
            this.gainSliderControl.MinimumSize = new System.Drawing.Size(225, 50);
            this.gainSliderControl.Name = "gainSliderControl";
            this.gainSliderControl.Size = new System.Drawing.Size(264, 50);
            this.gainSliderControl.TabIndex = 5;
            // 
            // heightSliderControl
            // 
            this.heightSliderControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.heightSliderControl.DefaultName = "N/A";
            this.heightSliderControl.Location = new System.Drawing.Point(0, 164);
            this.heightSliderControl.MinimumSize = new System.Drawing.Size(225, 50);
            this.heightSliderControl.Name = "heightSliderControl";
            this.heightSliderControl.Size = new System.Drawing.Size(264, 50);
            this.heightSliderControl.TabIndex = 4;
            // 
            // widthSliderControl
            // 
            this.widthSliderControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.widthSliderControl.DefaultName = "N/A";
            this.widthSliderControl.Location = new System.Drawing.Point(0, 114);
            this.widthSliderControl.MinimumSize = new System.Drawing.Size(225, 50);
            this.widthSliderControl.Name = "widthSliderControl";
            this.widthSliderControl.Size = new System.Drawing.Size(264, 50);
            this.widthSliderControl.TabIndex = 3;
            // 
            // pixelFormatControl
            // 
            this.pixelFormatControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pixelFormatControl.DefaultName = "N/A";
            this.pixelFormatControl.Location = new System.Drawing.Point(12, 57);
            this.pixelFormatControl.Name = "pixelFormatControl";
            this.pixelFormatControl.Size = new System.Drawing.Size(236, 57);
            this.pixelFormatControl.TabIndex = 1;
            // 
            // testImageControl
            // 
            this.testImageControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.testImageControl.DefaultName = "N/A";
            this.testImageControl.Location = new System.Drawing.Point(12, 0);
            this.testImageControl.Name = "testImageControl";
            this.testImageControl.Size = new System.Drawing.Size(236, 51);
            this.testImageControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 592);
            this.Controls.Add(this.splitContainerImageView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Pylon Live View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.splitContainerImageView.Panel1.ResumeLayout(false);
            this.splitContainerImageView.Panel1.PerformLayout();
            this.splitContainerImageView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImageView)).EndInit();
            this.splitContainerImageView.ResumeLayout(false);
            this.splitContainerConfiguration.Panel1.ResumeLayout(false);
            this.splitContainerConfiguration.Panel2.ResumeLayout(false);
            this.splitContainerConfiguration.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerConfiguration)).EndInit();
            this.splitContainerConfiguration.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.num_queue_nbup)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogRecordDisplay1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerImageView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonOneShot;
        private System.Windows.Forms.ToolStripButton toolStripButtonContinuousShot;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.SplitContainer splitContainerConfiguration;
        private System.Windows.Forms.ListView deviceListView;
        private System.Windows.Forms.Timer updateDeviceListTimer;
        private System.Windows.Forms.ImageList imageListForDeviceList;
        private PylonLiveViewControl.EnumerationComboBoxUserControl testImageControl;
        private PylonLiveViewControl.EnumerationComboBoxUserControl pixelFormatControl;
        private System.Windows.Forms.Button GetFrame_Btn;
        private System.Windows.Forms.NumericUpDown num_queue_nbup;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button CloseCamera_Btn;
        public PylonLiveViewControl.IntSliderUserControl widthSliderControl;
        public PylonLiveViewControl.IntSliderUserControl heightSliderControl;
        public PylonLiveViewControl.FloatSliderUserControl gainSliderControl;
        public PylonLiveViewControl.FloatSliderUserControl exposureTimeSliderControl;
        private System.Windows.Forms.Button SettingBtn;
        private System.Windows.Forms.Button TestOn_Btn;
        private Cognex.VisionPro.CogRecordDisplay cogRecordDisplay1;
        private System.Windows.Forms.CheckBox TriggerSensor_Chbx;
        private System.Windows.Forms.Button TestOff_Btn;
        private System.IO.Ports.SerialPort serialPort1;
    }
}

