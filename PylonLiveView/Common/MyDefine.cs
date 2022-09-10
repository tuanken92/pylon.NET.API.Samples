using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CTTV_VisionInspection.Common
{

    public class MyDefine
    {
        public static string version = "Version 0.0.6 \r\n 24/08/2022";
        public static int NUM_THREAD = 2;

        

        //limit
        public static int X_Limit_Down = -34397;
        public static int X_Limit_Up = 315320;
        public static int Y_Limit_Down = -1000;
        public static int Y_Limit_Up = 26410;
        
        public static int Speed_Min = 0;
        public static int X_Jog_Speed = 5001;
        public static int X_Rel_Speed = 50001;
        public static int X_Abs_Speed = 250001;

        public static int Y_Jog_Speed = 1001;
        public static int Y_Rel_Speed = 5001;
        public static int Y_Abs_Speed = 25001;


        public static uint WM_LBUTTONDOWN = 0x201;
        public static uint WM_LBUTTONUP = 0x202;

        public static int ERROR_PLC_CODE = -999;

        public static readonly string workingDirectory = Environment.CurrentDirectory;
        public static readonly string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
        public static readonly string workspaceDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        public static readonly string resources_folder = String.Format($"{workingDirectory}\\resources");
        public static readonly string model_folder = String.Format($"C:\\CTTV_VisionInspection_Data\\Models");
        public static readonly string dowload_folder = String.Format($"C:\\CTTV_VisionInspection_Data\\Downloads");
        public static readonly string image_folder = String.Format($"C:\\CTTV_VisionInspection_Data\\Images");
        public static readonly string job_folder = String.Format($"C:\\CTTV_VisionInspection_Data\\Jobs");
        public static readonly string text_folder = String.Format($"C:\\CTTV_VisionInspection_Data\\Texts");

        public static readonly string regex_get_image_file = @"[^\s]+(.*?)\.(jpg|jpeg|png|gif|bmp|JPG|JPEG|PNG|GIF|BMP)$";
        public static readonly string regex_get_ip = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";


        #region Path file json
        public static readonly string file_y_axis_test_loop = String.Format($"{workingDirectory}\\Configs\\y_axis_test_loop.json");
        public static readonly string file_x_axis_test_loop = String.Format($"{workingDirectory}\\Configs\\x_axis_test_loop.json");

        public static readonly string file_minio_client = String.Format($"{workingDirectory}\\Configs\\minio_client.json");
        public static readonly string file_camera = String.Format($"{workingDirectory}\\Configs\\camera.json");
        public static readonly string file_model = String.Format($"{workingDirectory}\\Configs\\model.json");
        public static readonly string file_plc_assignment = String.Format($"{workingDirectory}\\Configs\\plc_assignment.json");
        //public static readonly string path_result = String.Format($"{workingDirectory}\\Configs\\plc_assignment.json");
        //public static readonly string path_save_texts = String.Format("{0}\\Texts", projectDirectory);

        public static readonly string file_brand = String.Format($"{workingDirectory}\\Configs\\brand.json");
        public static readonly string file_category = String.Format($"{workingDirectory}\\Configs\\category.json");
        public static readonly string file_user = String.Format($"{workingDirectory}\\Configs\\user.json");
        public static readonly string file_customer = String.Format($"{workingDirectory}\\Configs\\customer.json");
        public static readonly string file_warehouse = String.Format($"{workingDirectory}\\Configs\\warehouse.json");
        public static readonly string file_product = String.Format($"{workingDirectory}\\Configs\\product.json");
        public static readonly string file_unit = String.Format($"{workingDirectory}\\Configs\\unit.json");
        public static readonly string file_setting = String.Format($"{workingDirectory}\\Configs\\setting.json");
        public static readonly string file_import_product_manager = String.Format($"{workingDirectory}\\Configs\\product_import_manager.json");
        public static readonly string file_export_product_manager = String.Format($"{workingDirectory}\\Configs\\product_export_manager.json");
        public static readonly string import_product_tmp = String.Format($"{workingDirectory}\\Data\\Import\\") + @"product_import_{0}_{1}_{2}.json";
        public static readonly string export_product_tmp = String.Format($"{workingDirectory}\\Data\\Export\\") + @"product_export_{0}_{1}_{2}.json";


        public static readonly string file_config = String.Format($"{workingDirectory}\\Configs\\config_param.json");
        public static readonly string file_camera_config = String.Format($"{workingDirectory}\\Configs\\CameraParameters.pfs");
        public static readonly string file_tool_process = String.Format($"{workingDirectory}\\Configs\\Tool_Process.vpp");
        public static readonly string file_tool_acq = String.Format($"{workingDirectory}\\Configs\\Tool_Acq.vpp");
        public static readonly string file_excel = String.Format($"{workingDirectory}\\Data\\ImportData.xlsx");

        public static readonly string file_config_format_data = String.Format($"{workingDirectory}\\Data\\configs\\format_data.json");
        public static readonly string file_config_common_param = String.Format($"{workingDirectory}\\Data\\configs\\common_param.json");
        public static readonly string file_config_filter_window = String.Format($"{workingDirectory}\\Data\\configs\\filter_window.json");
        public static readonly string path_load_img_database = @"C:\Program Files\Cognex\VisionPro\Images";
        public static readonly string path_load_vpp_file = @"C:\Users\Admin\Desktop\Vpp_file";
        public static readonly string path_save_images = String.Format("{0}\\Images", projectDirectory);

        public static readonly string key_thh = @"https://tanhungha.com.vn/";
        public static readonly string hash_key = "";
        #endregion

        #region api
        public static string API_OK = "success";
        public static string API_NG = "error";
        public static string API_Warning = "warning";
        public static string API_LOSS_CONNECTION = "network";
        public static string dev_pass = "tuanna@2022";
        public static string user_pass = "cttv@2022";
        #endregion

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);

        public static bool CheckLibrary(string fileName)
        {
            return LoadLibrary(fileName) == IntPtr.Zero;
        }



    }
}
