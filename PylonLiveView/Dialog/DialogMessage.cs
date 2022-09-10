using CTTV_VisionInspection;
using CTTV_VisionInspection.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CTTV_VisionInspection.Dialog
{
    public partial class DialogMessage : Form
    {
        public DialogMessage()
        {
            InitializeComponent();
        }

        public DialogMessage(string caption, string message)
        {
            InitializeComponent();
            lbCaption.Text = caption.ToUpper();
            lbInformation.Text = message;

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
