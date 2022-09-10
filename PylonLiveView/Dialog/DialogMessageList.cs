using PylonLiveView;
using CTTV_VisionInspection.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CTTV_VisionInspection.Dialog
{
    public partial class DialogMessageList : Form
    {
        public DialogMessageList()
        {
            InitializeComponent();
        }

        public DialogMessageList(string caption, List<string> message)
        {
            InitializeComponent();
            lbCaption.Text = caption.ToUpper();
            

            switch (caption)
            {
                case "success":
                    timer.Start();
                    pictureBoxLogo.Image = PylonLiveView.Properties.Resources.btnOK;
                    lbCaption.ForeColor = Color.ForestGreen;
                    break;
                case "warning":
                    lbCaption.ForeColor = Color.LightYellow;
                    pictureBoxLogo.Image = PylonLiveView.Properties.Resources.warning;
                    break;
                default:
                    lbCaption.ForeColor = Color.Red;
                    pictureBoxLogo.Image = PylonLiveView.Properties.Resources.err;
                    break;
            }

            list_message.Items.Clear();
            foreach(string messageItem in message)
            {
                list_message.Items.Add(messageItem);
            }
        }


        public DialogMessageList(string caption, List<int> message)
        {
            InitializeComponent();
            lbCaption.Text = caption.ToUpper();


            switch (caption)
            {
                case "success":
                    timer.Start();
                    pictureBoxLogo.Image = PylonLiveView.Properties.Resources.btnOK;
                    lbCaption.ForeColor = Color.ForestGreen;
                    break;
                case "warning":
                    lbCaption.ForeColor = Color.LightYellow;
                    pictureBoxLogo.Image = PylonLiveView.Properties.Resources.warning;
                    break;
                default:
                    lbCaption.ForeColor = Color.Red;
                    pictureBoxLogo.Image = PylonLiveView.Properties.Resources.err;
                    break;
            }

            list_message.Items.Clear();
            foreach (int messageItem in message)
            {
                list_message.Items.Add(messageItem);
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }

        private void panelCaption_MouseDown(object sender, MouseEventArgs e)
        {
            MyLib.DragWindow(this.Handle);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
