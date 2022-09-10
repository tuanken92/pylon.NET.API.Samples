
namespace CTTV_VisionInspection.Dialog
{
    partial class DialogMessageList
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
            this.lbCaption = new System.Windows.Forms.Label();
            this.panelCaption = new System.Windows.Forms.Panel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.list_message = new System.Windows.Forms.ListBox();
            this.panelCaption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lbCaption
            // 
            this.lbCaption.AutoSize = true;
            this.lbCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCaption.ForeColor = System.Drawing.Color.ForestGreen;
            this.lbCaption.Location = new System.Drawing.Point(38, 4);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(139, 20);
            this.lbCaption.TabIndex = 0;
            this.lbCaption.Text = "Message Status";
            // 
            // panelCaption
            // 
            this.panelCaption.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panelCaption.Controls.Add(this.pictureBoxLogo);
            this.panelCaption.Controls.Add(this.lbCaption);
            this.panelCaption.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCaption.Location = new System.Drawing.Point(0, 0);
            this.panelCaption.Name = "panelCaption";
            this.panelCaption.Size = new System.Drawing.Size(373, 29);
            this.panelCaption.TabIndex = 7;
            this.panelCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCaption_MouseDown);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Location = new System.Drawing.Point(2, 2);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 1;
            this.pictureBoxLogo.TabStop = false;
            // 
            // timer
            // 
            this.timer.Interval = 15000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.Blue;
            this.btnOK.Image = global::PylonLiveView.Properties.Resources.btnOK;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.Location = new System.Drawing.Point(54, 209);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(265, 35);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "  OK";
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // list_message
            // 
            this.list_message.BackColor = System.Drawing.Color.White;
            this.list_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.list_message.ForeColor = System.Drawing.Color.Black;
            this.list_message.FormattingEnabled = true;
            this.list_message.HorizontalScrollbar = true;
            this.list_message.ItemHeight = 16;
            this.list_message.Location = new System.Drawing.Point(27, 43);
            this.list_message.Name = "list_message";
            this.list_message.Size = new System.Drawing.Size(320, 148);
            this.list_message.TabIndex = 8;
            // 
            // DialogMessageList
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(373, 253);
            this.Controls.Add(this.list_message);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.panelCaption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DialogMessageList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DialogMessageList";
            this.panelCaption.ResumeLayout(false);
            this.panelCaption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbCaption;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panelCaption;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ListBox list_message;
    }
}