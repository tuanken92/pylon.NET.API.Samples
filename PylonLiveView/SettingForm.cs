using Basler.Pylon;
using CTTV_VisionInspection.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PylonLiveView
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();

            FileCAMConfig_Txt.Text = MyParam.common_param.file_cam_config;
        }

        private void LoadCAMConfig_Btn_Click(object sender, EventArgs e)
        {
            if (!MyLib.IsCameraConnected())
            {
                MyLib.ShowDlgWarning("Check connection with Camera!");
                return;
            }

            string file_cam_config = MyLib.OpenFileDialog(eTypeFile.File_Config, String.Format($"{MyDefine.workingDirectory}\\Configs"));
            if(file_cam_config != null)
                MyParam.common_param.file_cam_config = file_cam_config;

            Console.WriteLine("Reading file {0} back to camera device parameters ...", MyParam.common_param.file_cam_config);
            // Just for demonstration, read the content of the file back to the camera device parameters.
            MyParam.camera.Parameters.Load(MyParam.common_param.file_cam_config, ParameterPath.CameraDevice);
        }

        private void SaveCAMConfig_Btn_Click(object sender, EventArgs e)
        {
            if(!MyLib.IsCameraConnected())
            {
                MyLib.ShowDlgWarning("Check connection with Camera!");
                return;
            }

            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Save Camera Config";
                    saveFileDialog.InitialDirectory = String.Format($"{MyDefine.workingDirectory}\\Configs");
                    saveFileDialog.FileName = "CameraParameters.pfs";
                    saveFileDialog.Filter = "pfs files (*.pfs)|*.pfs|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        MyParam.common_param.file_cam_config = saveFileDialog.FileName;
                        FileCAMConfig_Txt.Text = saveFileDialog.FileName;

                        Console.WriteLine("Saving camera device parameters to file {0} ...", MyParam.common_param.file_cam_config);
                        // Save the content of the camera device parameters in the file.
                        MyParam.camera.Parameters.Save(MyParam.common_param.file_cam_config, ParameterPath.CameraDevice);

                    }

                }
            }
            catch(Exception ex)
            {
                MyLib.ShowDlgError(ex.Message);
            }
            
        }

        private void LoadToolProcess_Btn_Click(object sender, EventArgs e)
        {
            string file_toolblock = MyLib.OpenFileDialog(eTypeFile.File_ToolBlock, String.Format($"{MyDefine.workingDirectory}\\Configs"));
            if (file_toolblock != null)
            {
                MyParam.common_param.file_tool_process = MyDefine.file_tool_process;
                ToolProcess_Txt.Text = file_toolblock;
            }

            Console.WriteLine("Reading file {0} back to camera device parameters ...", MyParam.common_param.file_tool_process);
            // Just for demonstration, read the content of the file back to the camera device parameters.
            //MyParam.camera.Parameters.Load(MyParam.common_param.file_cam_config, ParameterPath.CameraDevice);
        }
    }
}
