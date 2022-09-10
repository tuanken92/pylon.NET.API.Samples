
namespace CTTV_VisionInspection.Dialog
{
    partial class DialogYesNo
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
            this.lbCaption = new System.Windows.Forms.Label();
            this.lbInformation = new System.Windows.Forms.Label();
            this.panelCaption = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelCaption.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCaption
            // 
            this.lbCaption.AutoSize = true;
            this.lbCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCaption.ForeColor = System.Drawing.Color.Orange;
            this.lbCaption.Location = new System.Drawing.Point(3, 4);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(111, 20);
            this.lbCaption.TabIndex = 0;
            this.lbCaption.Text = "Exit Program";
            this.lbCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCaption_MouseDown);
            // 
            // lbInformation
            // 
            this.lbInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInformation.ForeColor = System.Drawing.Color.White;
            this.lbInformation.Location = new System.Drawing.Point(30, 49);
            this.lbInformation.Name = "lbInformation";
            this.lbInformation.Size = new System.Drawing.Size(279, 120);
            this.lbInformation.TabIndex = 4;
            this.lbInformation.Text = "Are you sure want to Exit?";
            this.lbInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCaption
            // 
            this.panelCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(76)))));
            this.panelCaption.Controls.Add(this.lbCaption);
            this.panelCaption.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCaption.Location = new System.Drawing.Point(0, 0);
            this.panelCaption.Name = "panelCaption";
            this.panelCaption.Size = new System.Drawing.Size(338, 29);
            this.panelCaption.TabIndex = 7;
            this.panelCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCaption_MouseDown);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = global::PylonLiveView.Properties.Resources.btnCancel;
            this.btnCancel.Location = new System.Drawing.Point(209, 181);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "  Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Image = global::PylonLiveView.Properties.Resources.btnOK;
            this.btnOK.Location = new System.Drawing.Point(24, 181);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 35);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "  OK";
            this.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // DialogYesNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(76)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(338, 228);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbInformation);
            this.Controls.Add(this.panelCaption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DialogYesNo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DialogYesNo_Dark";
            this.panelCaption.ResumeLayout(false);
            this.panelCaption.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbCaption;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lbInformation;
        private System.Windows.Forms.Panel panelCaption;
    }
}