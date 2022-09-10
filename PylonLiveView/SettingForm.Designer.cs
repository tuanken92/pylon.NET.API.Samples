namespace PylonLiveView
{
    partial class SettingForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.FileCAMConfig_Txt = new System.Windows.Forms.TextBox();
            this.LoadCAMConfig_Btn = new System.Windows.Forms.Button();
            this.SaveCAMConfig_Btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ToolProcess_Txt = new System.Windows.Forms.TextBox();
            this.LoadToolProcess_Btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Config";
            // 
            // FileCAMConfig_Txt
            // 
            this.FileCAMConfig_Txt.Location = new System.Drawing.Point(76, 13);
            this.FileCAMConfig_Txt.Name = "FileCAMConfig_Txt";
            this.FileCAMConfig_Txt.Size = new System.Drawing.Size(538, 20);
            this.FileCAMConfig_Txt.TabIndex = 2;
            // 
            // LoadCAMConfig_Btn
            // 
            this.LoadCAMConfig_Btn.Location = new System.Drawing.Point(633, 12);
            this.LoadCAMConfig_Btn.Name = "LoadCAMConfig_Btn";
            this.LoadCAMConfig_Btn.Size = new System.Drawing.Size(75, 23);
            this.LoadCAMConfig_Btn.TabIndex = 3;
            this.LoadCAMConfig_Btn.Text = "Load";
            this.LoadCAMConfig_Btn.UseVisualStyleBackColor = true;
            this.LoadCAMConfig_Btn.Click += new System.EventHandler(this.LoadCAMConfig_Btn_Click);
            // 
            // SaveCAMConfig_Btn
            // 
            this.SaveCAMConfig_Btn.Location = new System.Drawing.Point(633, 41);
            this.SaveCAMConfig_Btn.Name = "SaveCAMConfig_Btn";
            this.SaveCAMConfig_Btn.Size = new System.Drawing.Size(75, 23);
            this.SaveCAMConfig_Btn.TabIndex = 3;
            this.SaveCAMConfig_Btn.Text = "Save";
            this.SaveCAMConfig_Btn.UseVisualStyleBackColor = true;
            this.SaveCAMConfig_Btn.Click += new System.EventHandler(this.SaveCAMConfig_Btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tool Process";
            // 
            // ToolProcess_Txt
            // 
            this.ToolProcess_Txt.Location = new System.Drawing.Point(17, 100);
            this.ToolProcess_Txt.Name = "ToolProcess_Txt";
            this.ToolProcess_Txt.Size = new System.Drawing.Size(597, 20);
            this.ToolProcess_Txt.TabIndex = 2;
            // 
            // LoadToolProcess_Btn
            // 
            this.LoadToolProcess_Btn.Location = new System.Drawing.Point(633, 99);
            this.LoadToolProcess_Btn.Name = "LoadToolProcess_Btn";
            this.LoadToolProcess_Btn.Size = new System.Drawing.Size(75, 23);
            this.LoadToolProcess_Btn.TabIndex = 3;
            this.LoadToolProcess_Btn.Text = "Load";
            this.LoadToolProcess_Btn.UseVisualStyleBackColor = true;
            this.LoadToolProcess_Btn.Click += new System.EventHandler(this.LoadToolProcess_Btn_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 325);
            this.Controls.Add(this.SaveCAMConfig_Btn);
            this.Controls.Add(this.LoadToolProcess_Btn);
            this.Controls.Add(this.LoadCAMConfig_Btn);
            this.Controls.Add(this.ToolProcess_Txt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FileCAMConfig_Txt);
            this.Controls.Add(this.label1);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FileCAMConfig_Txt;
        private System.Windows.Forms.Button LoadCAMConfig_Btn;
        private System.Windows.Forms.Button SaveCAMConfig_Btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ToolProcess_Txt;
        private System.Windows.Forms.Button LoadToolProcess_Btn;
    }
}