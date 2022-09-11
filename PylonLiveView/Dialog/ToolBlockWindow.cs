using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using CTTV_VisionInspection.Common;
using OpenCvSharp.Extensions;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace VisionPro_Tut.Windows
{
    public partial class ToolBlockWindow : Form
    {

        #region declera variable, object
        private TYPE_OF_TOOLBLOCK type_of_block;
        public CogToolBlock toolblock;
        private string path_vpp_file;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private CogToolBlockEditV2 cogToolBlockEditV21;
        private ToolStripMenuItem runToolStripMenuItem;
        #endregion


        public ToolBlockWindow()
        {
            Console.WriteLine("Tool block window default");
            InitializeComponent();
        }

        public ToolBlockWindow(TYPE_OF_TOOLBLOCK type_of_block)
        {
            

            switch (type_of_block)
            {
                case TYPE_OF_TOOLBLOCK.AcqFifo:
                    this.path_vpp_file = MyParam.common_param.file_tool_acq;
                    break;

                case TYPE_OF_TOOLBLOCK.ImageProcess:
                    this.path_vpp_file = MyParam.common_param.file_tool_process;
                    break;

                default:
                    MyLib.ShowDlgWarning("Not support!");
                    break;
            }
            Console.WriteLine("Tool block window create, path vpp_file = {0}, type_of_block = {1}", this.path_vpp_file, type_of_block);
            this.type_of_block = type_of_block;
            this.toolblock = new CogToolBlock();

            InitializeComponent();
        }

        

        private void MenuClick_Event(object sender, EventArgs e)
        {
            var menu_item = sender as ToolStripMenuItem;
            Console.WriteLine(menu_item.Text);
            switch (menu_item.Text)
            {
                case "Load":
                    string file_image = MyLib.OpenFileDialog(eTypeFile.File_Image);
                    //MyParam.toolBlockProcess.Inputs["Image"].Value = new CogImage8Grey(MyParam.mat.ToBitmap()); ;
                    MyParam.toolBlockProcess.Inputs["Image"].Value = new CogImage8Grey(new System.Drawing.Bitmap(file_image));
                    break;
                case "Save":
                    Console.WriteLine($"Save file {this.path_vpp_file}");
                    switch (this.type_of_block)
                    {
                        case TYPE_OF_TOOLBLOCK.AcqFifo:
                            File.WriteAllBytes(this.path_vpp_file, MyLib.Serialize(MyParam.toolBlockAcq));
                            break;
                        case TYPE_OF_TOOLBLOCK.ImageProcess:
                            File.WriteAllBytes(this.path_vpp_file, MyLib.Serialize(MyParam.toolBlockProcess));
                            break;
                        case TYPE_OF_TOOLBLOCK.Other:
                            File.WriteAllBytes(this.path_vpp_file, MyLib.Serialize(this.toolblock));
                            Console.WriteLine($"{this.path_vpp_file}-> {this.type_of_block}!");
                            break;
                    }
                    break;
                case "Save As":

                    SaveFileDialog saveToolBlock = new SaveFileDialog();
                    saveToolBlock.Filter = "ToolBlock |*.vpp|All File|*.*";
                    saveToolBlock.Title = "Save a Toolblock file";
                    saveToolBlock.RestoreDirectory = true;
                    saveToolBlock.InitialDirectory = String.Format($"{MyDefine.workingDirectory}\\Configs");
                    saveToolBlock.ShowDialog();
                    if (saveToolBlock.FileName != null)
                    {
                        Console.WriteLine(saveToolBlock.FileName);
                        
                        switch (this.type_of_block)
                        {
                            case TYPE_OF_TOOLBLOCK.AcqFifo:
                                File.WriteAllBytes(saveToolBlock.FileName, MyLib.Serialize(MyParam.toolBlockAcq));
                                break;
                            case TYPE_OF_TOOLBLOCK.ImageProcess:
                                File.WriteAllBytes(saveToolBlock.FileName, MyLib.Serialize(MyParam.toolBlockProcess));
                                break;
                            case TYPE_OF_TOOLBLOCK.Other:
                                File.WriteAllBytes(saveToolBlock.FileName, MyLib.Serialize(this.toolblock));
                                break;
                        }
                    }
                    break;
                case "OK":
                    switch (this.type_of_block)
                    {
                        case TYPE_OF_TOOLBLOCK.AcqFifo:
                        case TYPE_OF_TOOLBLOCK.ImageProcess:
                            Close();
                            DialogResult = DialogResult.OK;
                            break;
                        case TYPE_OF_TOOLBLOCK.Other:
                            Close();
                            break;
                    }
                    break;
                case "Run":
                    switch (this.type_of_block)
                    {
                        case TYPE_OF_TOOLBLOCK.AcqFifo:
                            MyParam.toolBlockAcq.Run();
                            break;
                        case TYPE_OF_TOOLBLOCK.ImageProcess:
                            MyParam.toolBlockProcess.Run();
                            break;
                        case TYPE_OF_TOOLBLOCK.Other:
                            this.toolblock.Run();
                            break;
                    }
                    
                    break;
            }
        }

        private void ToolBlockWindow_Load(object sender, EventArgs e)
        {
            //Load Toolblock and show on window
            

            Console.WriteLine("ToolBlockWindow_Load");

            cogToolBlockEditV21.LocalDisplayVisible = false;

            switch (this.type_of_block)
            {
                case TYPE_OF_TOOLBLOCK.AcqFifo:
                    cogToolBlockEditV21.Subject = MyParam.toolBlockAcq;
                    break;
                case TYPE_OF_TOOLBLOCK.ImageProcess:
                    cogToolBlockEditV21.Subject = MyParam.toolBlockProcess;
                    break;
                case TYPE_OF_TOOLBLOCK.Other:
                    cogToolBlockEditV21.Subject = this.toolblock;
                    break;
            }

            cogToolBlockEditV21.SubjectChanged += new EventHandler(cogToolBlockEditV21_SubjectChanged);

        }

        private void ToolBlockWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Close and release resource
            Console.WriteLine("ToolBlockWindow_FormClosing");
        }

        void cogToolBlockEditV21_SubjectChanged(object sender, EventArgs e)
        {
            // The application is meant to be used with the TB.vpp so whenever the user changes the TB
            Console.WriteLine("cogToolBlockEditV21_SubjectChanged");
        }




        #region GUI
        private Label lbStatus;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem oKToolStripMenuItem;
        private MenuStrip menuStrip1;
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            Console.WriteLine($"Dispose = {disposing}");

            cogToolBlockEditV21.SubjectChanged -= new EventHandler(cogToolBlockEditV21_SubjectChanged);
            if (disposing && (components != null))
            {
                components.Dispose();
                Console.WriteLine("components.Dispose();");
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
            this.lbStatus = new System.Windows.Forms.Label();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cogToolBlockEditV21 = new Cognex.VisionPro.ToolBlock.CogToolBlockEditV2();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV21)).BeginInit();
            this.SuspendLayout();
            // 
            // lbStatus
            // 
            this.lbStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbStatus.Location = new System.Drawing.Point(0, 445);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(807, 23);
            this.lbStatus.TabIndex = 1;
            this.lbStatus.Text = "Status";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.MenuClick_Event);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.MenuClick_Event);
            // 
            // oKToolStripMenuItem
            // 
            this.oKToolStripMenuItem.Name = "oKToolStripMenuItem";
            this.oKToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.oKToolStripMenuItem.Text = "OK";
            this.oKToolStripMenuItem.Click += new System.EventHandler(this.MenuClick_Event);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.runToolStripMenuItem,
            this.oKToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(807, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.saveAsToolStripMenuItem.Text = "Save As";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.MenuClick_Event);
            // 
            // cogToolBlockEditV21
            // 
            this.cogToolBlockEditV21.AllowDrop = true;
            this.cogToolBlockEditV21.ContextMenuCustomizer = null;
            this.cogToolBlockEditV21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogToolBlockEditV21.Location = new System.Drawing.Point(0, 24);
            this.cogToolBlockEditV21.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogToolBlockEditV21.Name = "cogToolBlockEditV21";
            this.cogToolBlockEditV21.ShowNodeToolTips = true;
            this.cogToolBlockEditV21.Size = new System.Drawing.Size(807, 421);
            this.cogToolBlockEditV21.SuspendElectricRuns = false;
            this.cogToolBlockEditV21.TabIndex = 3;
            // 
            // ToolBlockWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 468);
            this.Controls.Add(this.cogToolBlockEditV21);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ToolBlockWindow";
            this.Text = "Tool Block Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolBlockWindow_FormClosing);
            this.Load += new System.EventHandler(this.ToolBlockWindow_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV21)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #endregion


    }
}
