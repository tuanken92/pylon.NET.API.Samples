using Basler.Pylon;
using Cognex.VisionPro;
using Cognex.VisionPro.ID;
using Cognex.VisionPro.ResultsAnalysis;
using Cognex.VisionPro.ToolBlock;
using CTTV_VisionInspection.Dialog;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using PylonLiveView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CTTV_VisionInspection.Common.SvLogger;

namespace CTTV_VisionInspection.Common
{
    public enum TYPE_OF_TOOLBLOCK
    {
        AcqFifo = 0,
        ImageProcess = 1,
        Other = 2,
    }

    public enum eModeRun
    {
        EasyBuilder,
        SpreadSheet
    }
    public enum eMachineStatus
    {
        Waiting,
        Start,
        Stop,
        Pause,
        Resume,
        Checking,
        Goto_Conner,
        EMG,
    }
    public enum eTaskLoop
    {
        Task_ScanIO,
        Task_MergeImage,
        Task_MotionUpdateUI,
        Task_TeachingUpdateUI,
        Task_LoopX,
        Task_LoopY,
        Task_LoopXY,
        Task_LoopVision,
    }
    public enum eTestProcessing
    {
        None = 0,
        Wait_Signal,
        Servo_On,
        Go_Home,
        Check_Reach_Home,
        Move_To_Conner,
        Check_Reach_Conner,
        Delay_Moving_Conner,
        Trigger_Conner,
        Check_Done_Conner,
        Process_Conner,
        Move_To_Point,
        Check_Reach_Position,
        Delay_Moving,
        Trigger_Vision,
        Check_Done_Vision,
        Finish,
        Stop,
        Pause,
        EMG,
        Result

    }

    public enum eTypeList
    {
        List_None,
        List_Camera,
        List_Model,
        List_Webcam
    }
    public enum eTypeFile
    {
        File_Config,
        File_ToolBlock,
        File_Image,
        File_Excel,
        File_SerialNumber,
        File_Job,
        File_Json
    }


    public enum eViewWindow
    {
        View_None = -1,
        View_Auto,
        View_Manual,
        View_Teaching,
        View_Manager,
        View_Log,
        View_Infor,

        SubView_LightController,
        SubView_MotionController,
        SubView_CameraController,
        SubView_Parameter,
        SubView_ModelManager,
        SubView_CameraManager,
        SubView_MinIOManager,
        SubView_IOController
    }

    public enum eFilter
    {
        Filter_AnyText = 0,
        Filter_Prefix,
        Filter_Regex
    }

    public enum ePLCType
    {
        X,
        Y,
        M,
        D
    }


    public enum ePLCIndex
    {
        //jog+
        Y_Run_Jog_Pos,
        Y_Running_Jog_Pos,

        //jog-
        Y_Run_Jog_Neg,
        Y_Running_Jog_Neg,

        //jog speed
        Y_Jog_Speed,

        //home
        Y_Go_Home,
        Y_Done_Home,
        Y_Home_Speed,
        Y_Cur_Pos,

        //run relative
        Y_Run_Relative,
        Y_Done_Rel,
        Y_Rel_Speed,
        Y_Rel_Offset,

        //run absolute
        Y_Run_Abs,
        Y_Done_Abs,
        Y_Abs_Speed,
        Y_Abs_Position,

        //Reach Limit
        Y_Reach_Limit_Pos,
        Y_Reach_Limit_Neg,

        //reset, alarm, on
        Y_Servo_Reset,
        Y_Servo_On,
        Y_Driver_Error,


        //jog+
        X_Run_Jog_Pos,
        X_Running_Jog_Pos,

        //jog-
        X_Run_Jog_Neg,
        X_Running_Jog_Neg,

        //jog speed
        X_Jog_Speed,

        //home
        X_Go_Home,
        X_Done_Home,
        X_Home_Speed,
        X_Cur_Pos,

        //run relative
        X_Run_Relative,
        X_Done_Rel,
        X_Rel_Speed,
        X_Rel_Offset,

        //run absolute
        X_Run_Abs,
        X_Done_Abs,
        X_Abs_Speed,
        X_Abs_Position,

        //Reach Limit
        X_Reach_Limit_Pos,
        X_Reach_Limit_Neg,

        //reset, alarm, on
        X_Servo_Reset,
        X_Servo_On,
        X_Driver_Error,


        //button
        Start_Btn,
        Stop_Btn,
        Pause_Btn,
        EMG_Btn,
        Mode_Btn,
        Soft_Start,

        //output
        Soft_Stop,
        Relay_0,
        Relay_1,
        Trigger_Cam,
        Trigger_Light,

        //lamp tower
        Lamp_Red,
        Lamp_Yellow,
        Lamp_Green,
        Lamp_Buzz,

        //Trigger cam
        Cam_Trigger,
        Cam_Trigger_Enable,

        //Button
        Stop_Signal,
        Pause_Signal,
        Start_Resume_Signal,
        EMG_Signal,

        //result
        Result_OK,
        Result_NG

    }


    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public async Task<IDisposable> LockAsync()
        {
            await _semaphore.WaitAsync();
            return new Handler(_semaphore);
        }

        private sealed class Handler : IDisposable
        {
            private readonly SemaphoreSlim _semaphore;
            private bool _disposed = false;

            public Handler(SemaphoreSlim semaphore)
            {
                _semaphore = semaphore;
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    _semaphore.Release();
                    _disposed = true;
                }
            }
        }
    }


    public class StepControl
    {
        public eTestProcessing Cur_Processing;
        public eTestProcessing Old_Processing;

        public StepControl()
        {
            Cur_Processing = eTestProcessing.None;
            Old_Processing = eTestProcessing.None;
        }

        public void SetStep(eTestProcessing step)
        {
            if (Cur_Processing == step)
            {
                Console.WriteLine("Dupplicate Step");
                return;
            }
            //Update step
            Old_Processing = Cur_Processing;
            Cur_Processing = step;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Old step = {Old_Processing}");
            Console.WriteLine($"Cur step = {Cur_Processing}");
        }
    }
    public class TaskLoop
    {

        #region Implementation of INotifyPropertyChanged

        //public event PropertyChangedEventHandler PropertyChanged;

        //private void OnPropertyChanged(string propertyName)
        //{
        //    if (this.PropertyChanged != null)
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        #endregion

        private CancellationTokenSource _cancelSource;
        private readonly AsyncLock _lock = new AsyncLock();

        public CancellationTokenSource CancelSource { get => _cancelSource; set => _cancelSource = value; }

        public TaskLoop()
        {
            CancelSource = new CancellationTokenSource();
        }

        ~TaskLoop()
        {
            CancelSource?.Dispose();
        }


        public void ResetToken()
        {
            CancelSource?.Dispose();
            CancelSource = new CancellationTokenSource();
        }

        public void StopLoop()
        {
            CancelSource?.Cancel();
        }



        public async Task RunLoop(int interval, Action action)
        {
            if (action == null)
                return;

            using (await _lock.LockAsync())
            {
                while (!CancelSource.IsCancellationRequested)
                {
                    await Task.Run(() => action());
                    await Task.Delay(interval);
                }
            }
        }

    }



    public class SaveLoadParameter
    {
        public static void Save_Parameter(object param)
        {
            //save
            if (MyLib.File_Is_Exist(MyDefine.file_config))
            {
                Save_Parameter(param, MyDefine.file_config);
            }
            else
            {
                //create folder
                FileInfo fileInfo = new FileInfo(MyDefine.file_config);
                if (!fileInfo.Exists)
                    Directory.CreateDirectory(fileInfo.Directory.FullName);

                //create file
                using (FileStream f = File.Create(MyDefine.file_config))
                {
                    f.Close();
                    Console.WriteLine($"Create file {MyDefine.file_config}");
                }

                //save param to file
                Save_Parameter(param, MyDefine.file_config);
            }
        }

        public static object Load_Parameter(object param)
        {
            if (MyLib.File_Is_Exist(MyDefine.file_config))
            {
                using (StreamReader file = File.OpenText(MyDefine.file_config))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    param = serializer.Deserialize(file, param.GetType());
                }
            }
            else
            {
                MyLib.ShowDlgError($"Not found {MyDefine.file_config}");
            }

            return param;
        }

        public static void Save_Parameter(object param, string file_name)
        {
            //save
            if (MyLib.File_Is_Exist(file_name))
            {
                // serialize JSON directly to a file
                Console.WriteLine("Save parameter to file " + file_name);
                using (StreamWriter file = File.CreateText(file_name))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, param);
                }
            }
            else
            {
                //create folder
                FileInfo fileInfo = new FileInfo(file_name);
                if (!fileInfo.Exists)
                    Directory.CreateDirectory(fileInfo.Directory.FullName);

                //create file
                using (FileStream f = File.Create(file_name))
                {
                    f.Close();
                    Console.WriteLine($"Create file {file_name}");
                }

                // serialize JSON directly to a file
                Console.WriteLine("Save parameter to file " + file_name);
                using (StreamWriter file = File.CreateText(file_name))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, param);
                }
            }
        }

        public static object Load_Parameter(object param, string file_name)
        {
            if (MyLib.File_Is_Exist(file_name))
            {
                using (StreamReader file = File.OpenText(file_name))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    param = serializer.Deserialize(file, param.GetType());
                }
            }
            else
            {
                MyLib.ShowDlgError($"Not found {file_name}");
            }

            return param;
        }
    }

    public class MyWebcam
    {

    }


    public class PLCParam
    {
        public string ip { get; set; }
        public int port_pc { get; set; }
        public int port_cam { get; set; }
        public bool is_using { get; set; }
        public bool ignore_dupplicate_point { get; set; }
        public bool reach_point_signal { get; set; }
        public bool ignore_output_result { get; set; }
        public bool is_bypass_ok { get; set; }


        public PLCParam()
        {
            ip = "192.168.1.115";
            port_pc = 12289;
            port_cam = 12288;
            is_using = false;
            ignore_dupplicate_point = true;
            reach_point_signal = false;
            ignore_output_result = true;
            is_bypass_ok = false;
        }
    }

    public class CameraParam
    {
        public string ip { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public bool is_using { get; set; }
        public bool is_save_img { get; set; }

        public string step_cell { get; set; }
        public string result_cell { get; set; }
        public string split_data { get; set; }
        public int timeout_result { get; set; }

        public CameraParam()
        {
            ip = "192.168.1.111";
            user = "admin";
            pass = "";
            is_using = false;
            is_save_img = true;

            step_cell = "C15";
            result_cell = "A15";

            split_data = "_";
            timeout_result = 5;



        }
    }

    public class CommonParam
    {
        public bool auto_sensor_trigger { get; set; }

        public int time_merge_image { get; set; }
        public int time_scan_io { get; set; }
        public string file_cam_config { get; set; }
        public string file_tool_process { get; set; }
        public string file_tool_acq { get; set; }

        public int num_frame { get; set; }
        public int cur_frame { get; set; }
        public int processed_frame { get; set; }
        public int frame_width { get; set; }
        public int frame_height { get; set; }

        public string output_text_file { get; set; }
        
        [JsonIgnore]
        public Queue<byte[]> queue_data = new Queue<byte[]>();

        public CommonParam()
        {
            auto_sensor_trigger = false;

            time_merge_image = 10;
            time_scan_io = 100;
            num_frame = 2;
            frame_width = 4096;
            frame_height = 256;
            cur_frame = 0;
            processed_frame = 0;
            output_text_file = MyDefine.text_folder;
            file_cam_config = MyDefine.file_camera_config;
            file_tool_process = MyDefine.file_tool_process;
            file_tool_acq = MyDefine.file_tool_acq;
        }

    }

    public class ResultReport
    {
        public Stopwatch watchProcess;
        public bool bResult;
        public string pCode;
        public string sCode;
        public string timeStart;
        public string timeStop;
        public ResultReport()
        {
            watchProcess = new Stopwatch();
            bResult = false;
            pCode = "";
            sCode = "";
            timeStart = "";
            timeStop = "";
        }


        public string GetData()
        {
            //12539303,08201121,14:27:35,14:27:49,14.11,NG
            //Barcode1, barcode2, timeStart, timeEnd, CycleTime, Result

            string data = String.Format("{0},{1},{2},{3},{4},{5}\n",
                                         pCode, sCode, timeStart, timeStop, watchProcess.Elapsed.TotalSeconds.ToString(), bResult ? "OK" : "NG");

            return data;
        }

        public void ClearData()
        {
            watchProcess.Restart();
            bResult = false;
            pCode = "";
            sCode = "";
            timeStart = "";
            timeStop = "";
        }

        public void Begin()
        {
            ClearData();
            watchProcess.Start();
            timeStart = DateTime.Now.ToString("hh:mm:ss");
        }
        

        public bool WriteData()
        {
            bool bResult = true;

            try
            {
                MyLib.WriteTextCTTVResult(GetData());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                bResult = false;
            }

            return bResult;

        }

    }

    public static class MyParam
    {
        public static Cognex.VisionPro.Display.CogDisplay cogDisplay;
        public static Cognex.VisionPro.CogRecordDisplay cogRecordDisplay;
        public static object lockObject = new object();
        public static Mat mat = new Mat();
        public static bool bIsFirstImg = false;

        public static Stopwatch watch = new Stopwatch();
        public static CommonParam common_param = new CommonParam();
        public static ResultReport report_result = new ResultReport();
        
        public static List<TaskLoop> taskLoops = new List<TaskLoop>();


        public static CogToolBlock toolBlockProcess;
        public static CogToolBlock toolBlockAcq;

        public static Camera camera = null;
        //public static MainForm mainForm = new MainForm();
        static MyParam()
        {
            for (int i = 0; i < MyDefine.NUM_THREAD; i++)
            {
                taskLoops.Add(new TaskLoop());
            }
        }


    }
    public class MyLib
    {

        public static void ContinuousShot()
        {
            try
            {
                // Start the grabbing of images until grabbing is stopped.
                Configuration.AcquireContinuous(MyParam.camera, null);
                MyParam.camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                MyLib.ShowDlgError(exception.Message);
            }
        }
        public static void Stop()
        {
            // Stop the grabbing.
            try
            {
                MyParam.camera.StreamGrabber.Stop();
            }
            catch (Exception exception)
            {
                MyLib.ShowDlgError(exception.Message);
            }
        }

        static bool isStartFrame = false;
        public static void StartGetFrame()
        {
            if(isStartFrame)
            {
                return;
            }
            Console.WriteLine("---------------> Start Get Frame");
            long max_buffer = MyParam.camera.Parameters[PLCameraInstance.MaxNumBuffer].GetMaximum();
            MyParam.camera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(1000);

            MyParam.common_param.cur_frame = 0;
            MyParam.common_param.processed_frame = 0;
            MyParam.bIsFirstImg = false;
            MyParam.mat.Release();
            MyParam.common_param.queue_data.Clear();
            //MyParam.common_param.queue_data = new Queue<byte[]>(MyParam.common_param.num_frame);
            Console.WriteLine("num_queue = {0}, max buffer = {1}", 1000, max_buffer);
            Console.WriteLine("width = {0}, height = {1}", MyParam.common_param.frame_width, MyParam.common_param.frame_height);
            MainProcess.RunLoopMergeImage();
            //start
            ContinuousShot();
            isStartFrame = true;
        }

        public static void StopGetFrame()
        {
            Console.WriteLine("---------------> Stop Get Frame");
            Stop(); // Stop the grabbing of images.
            //MainProcess.trigger_status = false;
            isStartFrame = false;


        }
        public static void InitObject(int tool_index)
        {
            switch (tool_index)
            {
                case 0:
                    if (!File.Exists(MyParam.common_param.file_tool_acq))
                        MyParam.common_param.file_tool_acq= MyDefine.file_tool_acq;

                    MyParam.toolBlockAcq = CogSerializer.LoadObjectFromFile(MyParam.common_param.file_tool_acq) as CogToolBlock;
                    MyParam.toolBlockAcq.Ran += ToolBlockAcq_Ran;
                    MyParam.toolBlockAcq.Changed += ToolBlockAcq_Changed;
                    MyParam.toolBlockAcq.Running += ToolBlockAcq_Running;
                    ToolBlock_PrintInfor(MyParam.toolBlockAcq);
                    break;

                case 1:
                    if (!File.Exists(MyParam.common_param.file_tool_process))
                        MyParam.common_param.file_tool_process = MyDefine.file_tool_process;
                    MyParam.toolBlockProcess = CogSerializer.LoadObjectFromFile(MyParam.common_param.file_tool_process) as CogToolBlock;
                    if (MyParam.toolBlockProcess == null)
                    {
                        MyLib.ShowDlgError("toolBlockProcess == null!");
                        return;
                    }

                    MyParam.toolBlockProcess.Changed += ToolBlockProcess_Changed;
                    MyParam.toolBlockProcess.Running += ToolBlockProcess_Running;
                    MyParam.toolBlockProcess.Ran += ToolBlockProcess_Ran;

                    ToolBlock_PrintInfor(MyParam.toolBlockProcess);
                    break;

            }



        }
        public static void RemoveEvent(int tool_index)
        {

            switch (tool_index)
            {
                case 0:
                    if (MyParam.toolBlockAcq == null)
                        return;
                    MyParam.toolBlockAcq.Ran -= ToolBlockAcq_Ran;
                    MyParam.toolBlockAcq.Changed -= ToolBlockAcq_Changed;
                    MyParam.toolBlockAcq.Running -= ToolBlockAcq_Running;

                    break;

                case 1:
                    if (MyParam.toolBlockProcess == null)
                        return;
                    MyParam.toolBlockProcess.Changed -= ToolBlockProcess_Changed;
                    MyParam.toolBlockProcess.Running -= ToolBlockProcess_Running;
                    MyParam.toolBlockProcess.Ran -= ToolBlockProcess_Ran;
                    break;

            }

            
        }


        public static void ReleaseObject()
        {

            if (MyParam.toolBlockProcess == null)
                return;

            if (MyParam.toolBlockProcess != null)
                MyParam.toolBlockProcess.Dispose();
        }

        private static void ToolBlockProcess_Ran(object sender, EventArgs e)
        {
            Console.WriteLine("ToolBlockProcess_Ran");

            //var result = (String)(MyParam.toolBlockProcess.Outputs["IDCode"].Value);
            //Console.WriteLine(result);

            //CogResultsAnalysisTool result_tool = (CogResultsAnalysisTool)(MyParam.toolBlockProcess.Tools["CogResultsAnalysisTool1"]);
            //ICogRecord temp_result = result_tool.CreateLastRunRecord();
            //foreach (ICogRecord x in temp_result.SubRecords)
            //    Console.WriteLine(x.Annotation);

            
            //foreach(ICogTool cogTool in MyParam.toolBlockProcess.Tools)
            //{
            //    if (cogTool.RunStatus.Exception != null)
            //    {
            //        Console.WriteLine("---Exception---");
            //        Console.WriteLine(cogTool.Name);
            //        Console.WriteLine(cogTool.RunStatus.Exception.Message);
            //    }
            //}


            
            //Assign picture to display
            ICogRecord temp = MyParam.toolBlockProcess.CreateLastRunRecord();
            
            foreach(ICogRecord x in temp.SubRecords)
                Console.WriteLine(x.Annotation);
            
            //ICogRecord tempResult = temp.SubRecords["CogFixtureTool1.OutputImage"];
            ICogRecord tempResult = temp.SubRecords[temp.SubRecords.Count-1];
            //if (result_tool.Result.Decision == CogToolResultConstants.Accept)
            //{
            //}
            //else
            //{
            //    tempResult = temp.SubRecords["CogPMAlignTool1.InputImage"];
            //    var image = tempResult.Content as CogImage8Grey;
            //    MyLib.Save_BitMap(image.ToBitmap());

            //}


            
            //Console.WriteLine(result_tool.Result.Decision);

            
            if(tempResult != null)
            {
                MyParam.cogRecordDisplay.Record = tempResult;
                MyParam.cogRecordDisplay.Fit(true);
            }



            //ReleaseImg(temp);
        }


        static void ReleaseImg(ICogRecord temp)
        {
            ICogRecord temp1 = temp.SubRecords["CogFixtureTool1.OutputImage"];
            var image = temp1.Content as CogImage8Grey;
            image.Dispose();

            ICogRecord temp2 = temp.SubRecords["CogPMAlignTool1.InputImage"];
            image = temp2.Content as CogImage8Grey;
            image.Dispose();
        }
        private static void ToolBlockProcess_Running(object sender, EventArgs e)
        {
            Console.WriteLine("ToolBlockProcess_Running");
        }

        private static void ToolBlockProcess_Changed(object sender, CogChangedEventArgs e)
        {
            Console.WriteLine("ToolBlockProcess_Changed");
        }

        private static void ToolBlockAcq_Running(object sender, EventArgs e)
        {
            Console.WriteLine("ToolBlockAcq_Running");
        }

        private static void ToolBlockAcq_Changed(object sender, CogChangedEventArgs e)
        {
            Console.WriteLine("ToolBlockAcq_Changed");
        }

        private static void ToolBlockAcq_Ran(object sender, EventArgs e)
        {
            Console.WriteLine("ToolBlockAcq_Ran");
        }

        public static void ToolBlock_PrintInfor(CogToolBlock toolblock)
        {
            int numTools = toolblock.Tools.Count;
            Console.WriteLine($"-------------Toolblock {toolblock.Name} begin----------------");
            Console.WriteLine("-------------element");
            for (int i = 0; i < numTools; i++)
            {
                Console.WriteLine($"{toolblock.Tools[i].Name}");

                //cur record
                Cognex.VisionPro.ICogRecord tmpRecord = toolblock.Tools[i].CreateCurrentRecord();
                Console.WriteLine($"\ttmpRecord currentRecord = {tmpRecord.Annotation}");
                for (int j = 0; j < tmpRecord.SubRecords.Count; j++)
                {
                    Console.WriteLine($"\t\tj = {j}: {tmpRecord.SubRecords[j].Annotation}");
                }


                //lastest record
                tmpRecord = toolblock.Tools[i].CreateLastRunRecord();
                Console.WriteLine($"\ttmpRecord LastRecord = {tmpRecord.Annotation}");
                for (int j = 0; j < tmpRecord.SubRecords.Count; j++)
                {
                    Console.WriteLine($"\t\tj = {j}: {tmpRecord.SubRecords[j].Annotation}");
                }
            }

            Console.WriteLine("-------------input");
            int numInputs = toolblock.Inputs.Count;
            for (int i = 0; i < numInputs; i++)
            {
                Console.WriteLine($"{toolblock.Inputs[i].Name}");
            }

            Console.WriteLine("-------------output");
            int numOutputs = toolblock.Outputs.Count;
            for (int i = 0; i < numOutputs; i++)
            {
                Console.WriteLine($"{toolblock.Outputs[i].Name}");
            }

            Console.WriteLine($"-------------Toolblock {toolblock.Name} end----------------");
        }

        public static bool IsCameraConnected()
        {
            bool bResult = false;
            if (MyParam.camera == null)
                return bResult;

            if(MyParam.camera.IsOpen)
                bResult = true;
                
            return bResult;
        }
        public static void Display(OpenCvSharp.Mat mat, System.Windows.Forms.PictureBox pictureBox)
        {
            if (mat == null)
                return;

            Bitmap bitmap = mat.ToBitmap();

            // Assign a temporary variable to dispose the bitmap after assigning the new bitmap to the display control.
            Bitmap bitmapOld = pictureBox.Image as Bitmap;
            // Provide the display control with the new bitmap. This action automatically updates the display.
            pictureBox.Image = bitmap;
            if (bitmapOld != null)
            {
                // Dispose the bitmap.
                bitmapOld.Dispose();
            }
        }

        public static void Display(CogImage8Grey cogImage8Grey, Cognex.VisionPro.Display.CogDisplay cogDisplay1, bool result=true)
        {
            

            CogGraphicLabel cglCaption = new CogGraphicLabel();
            Font myFont = new Font("Comic Sans MS", 16, FontStyle.Bold);

            // Set it's text and alignment properties
            cglCaption.Text = result ? "PASS" : "FAIL";
            cglCaption.Color = result ? CogColorConstants.Green : CogColorConstants.Red;

            cglCaption.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
            // .NET fonts are read only, so create a new font and then
            // push this font object onto the Font property of the label
            // myFont = new Font("Comic Sans MS", 18, FontStyle.Bold);                        

            cglCaption.Font = myFont;

            // Set its space to be '*' - anchored to the image
            cglCaption.SelectedSpaceName = "*";

            // Position the label over the CogDisplay
            cglCaption.X = 50;
            cglCaption.Y = 50;

            // Add the label to the CogDisplay
            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.InteractiveGraphics.Add(cglCaption, cglCaption.Text, false);
            cogDisplay1.Image = cogImage8Grey;
            cglCaption.Dispose();
            myFont.Dispose();
        }


        public static void textBox_KeyPress_Number(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) /*&& (e.KeyChar != '.')*/)
            {
                e.Handled = true;
            }

            // only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        public static bool IsNumber(string str)
        {
            double db;
            return double.TryParse(str, out db);
        }

        public static bool WriteTextCTTVResult(string data_result)
        {
            MyLib.CreateFolder(MyParam.common_param.output_text_file);
            //String fileName = String.Format("{0}\\result_{1}.txt", MyDefine.path_save_texts, DateTime.Now.ToString("yyyyMMdd_hhmmss"));
            String fileName = String.Format("{0}\\TT_{1}.txt", MyParam.common_param.output_text_file, DateTime.Now.ToString("MMddyyyy"));

            bool bResult = true;

            try
            {
                if (!File.Exists(fileName))
                {
                    Log.Debug(data_result);
                    //File.Create(fileName);
                    File.WriteAllBytes(fileName, Encoding.ASCII.GetBytes(data_result));

                    //using (TextWriter tw = new StreamWriter(fileName))
                    //{
                    //    tw.WriteLine(data_result);
                    //    tw.Close();
                    //}

                }
                else
                {
                    //overwrite
                    using (var tw = new StreamWriter(fileName, true))
                    {
                        tw.Write(data_result);
                        //tw.Flush();
                        tw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                bResult = false;
            }




            return bResult;

        }
        

        


       
        public static void CloseAllTaskLoop()
        {
            for (int i = 0; i < MyDefine.NUM_THREAD; i++)
            {
                MyParam.taskLoops[i].StopLoop();
            }
        }



       

        private static Random rnd = new Random();
        private const int UNIT_MB = 1024 * 1024;

        // Create a file of given size from random byte array
        public static string CreateFile(int size)
        {
            string fileName = GetRandomName();
            byte[] data = new byte[size];
            rnd.NextBytes(data);

            File.WriteAllBytes(fileName, data);

            return fileName;
        }

        // Generate a random string
        public static string GetRandomName()
        {
            var characters = "0123456789abcdefghijklmnopqrstuvwxyz";
            var result = new StringBuilder(5);
            for (int i = 0; i < 5; i++)
            {
                result.Append(characters[rnd.Next(characters.Length)]);
            }
            return "minio-dotnet-example-" + result.ToString();
        }


        public static string GetLocalIPAddress()
        {
            var localhost = "127.0.0.1";
            bool isNetwork = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (isNetwork)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return localhost;
            }
            else
            {
                return localhost;
            }
        }

        public static bool File_Is_Exist(string file_name)
        {
            return File.Exists(file_name);
        }

        public static void ShowDlgWarning(string message)
        {
            using (DialogMessage dialogMessage = new DialogMessage(MyDefine.API_Warning, message))
            {
                dialogMessage.ShowDialog();
            }
        }

        public static void ShowDlgListÌnfor(List<string> message)
        {
            using (DialogMessageList dialogMessageList = new DialogMessageList(MyDefine.API_OK, message))
            {
                dialogMessageList.ShowDialog();
            }
        }

        public static void ShowDlgListÌnfor(List<int> message)
        {
            using (DialogMessageList dialogMessageList = new DialogMessageList(MyDefine.API_OK, message))
            {
                dialogMessageList.ShowDialog();
            }
        }



        public static void ShowDlgError(string message)
        {
            using (DialogMessage dialogMessage = new DialogMessage(MyDefine.API_NG, message))
            {
                dialogMessage.ShowDialog();
            }
        }

        public static void ShowDlgInfor(string message)
        {
            using (DialogMessage dialogMessage = new DialogMessage(MyDefine.API_OK, message))
            {
                dialogMessage.ShowDialog();
            }
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        public extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        public extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public static void DragWindow(IntPtr hwnd)
        {
            ReleaseCapture();
            SendMessage(hwnd, 0x112, 0xf012, 0);
        }


        public static string SelectFolderDialog(string rootFolder = null)
        {
            string folder_name = null;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (rootFolder != null)
                folderBrowserDialog.SelectedPath = rootFolder;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                folder_name = folderBrowserDialog.SelectedPath;
            }

            return folder_name;
        }

        public static string OpenFileDialog(eTypeFile type_file, string initDirectory = null)
        {
            string file_name = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ReadOnlyChecked = true;
            openFileDialog.ShowReadOnly = true;

            if (!String.IsNullOrEmpty(initDirectory))
                openFileDialog.InitialDirectory = initDirectory;

            switch (type_file)
            {
                case eTypeFile.File_ToolBlock:
                    openFileDialog.Title = "Browse ToolBlock Files";
                    openFileDialog.DefaultExt = "vpp";
                    openFileDialog.Filter = "vpp files (*.vpp)|*.vpp|All files (*.*)|*.*";
                    break;
                case eTypeFile.File_Config:
                    openFileDialog.Title = "Browse Camera Config Files";
                    openFileDialog.DefaultExt = "pfs";
                    openFileDialog.Filter = "pfs files (*.pfs)|*.pfs|All files (*.*)|*.*";
                    break;
                
                case eTypeFile.File_Job:
                    openFileDialog.Title = "Browse Job Files";
                    openFileDialog.DefaultExt = "job";
                    openFileDialog.Filter = "job files (*.job)|*.job|All files (*.*)|*.*";
                    break;
                case eTypeFile.File_Json:
                    openFileDialog.Title = "Browse Json Files";
                    openFileDialog.DefaultExt = "json";
                    openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                    break;
                case eTypeFile.File_Image:
                    openFileDialog.Title = "Browse Image Files";
                    openFileDialog.DefaultExt = "bmp";
                    openFileDialog.Filter = "image files (*.bmp)|*.bmp|All files (*.*)|*.*";
                    break;

                case eTypeFile.File_Excel:
                    openFileDialog.Title = "Browse Excel Files";
                    openFileDialog.DefaultExt = "xlsx";
                    openFileDialog.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    break;
                case eTypeFile.File_SerialNumber:
                    openFileDialog.Title = "Browse File Serial Number";
                    openFileDialog.DefaultExt = "xlsx";
                    openFileDialog.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    break;
            }

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file_name = openFileDialog.FileName;
            }

            return file_name;
        }

        public static List<string> Get_List_Printer()
        {
            List<string> list_printer = System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            return list_printer;
        }



        public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var cnt = new Dictionary<T, int>();
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string Encrypt(string decrypted)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(decrypted);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();

            tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(MyDefine.hash_key));
            tripDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripDES.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(result);
        }

        public static string Decrypt(string encrypted)
        {
            byte[] data = Convert.FromBase64String(encrypted);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();

            tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(MyDefine.hash_key));
            tripDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripDES.CreateDecryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
            return UTF8Encoding.UTF8.GetString(result);
        }


        public static DateTime Timestamp_To_Datetime(UInt64 timestamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
            return dtDateTime;
        }

        public static UInt64 Datetime_To_TimeStamp(DateTime datetime)
        {
            return (UInt64)(TimeZoneInfo.ConvertTimeToUtc(datetime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }

        public static Image Download_Image(string fromUrl)
        {
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    using (Stream stream = webClient.OpenRead(fromUrl))
                    {
                        return Image.FromStream(stream);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Download {fromUrl} -> exception: {e.Message}");
                    return null;
                }

            }

            //Image img = null;
            //try
            //{
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fromUrl);
            //    request.Timeout = 3000;
            //    request.ReadWriteTimeout = 3000;

            //    var wresp = (HttpWebResponse)request.GetResponse();

            //    using (Stream stream = File.OpenRead(fromUrl))
            //    {
            //        img = Image.FromStream(stream);
            //    }

            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"Download exception: {e.Message}");

            //}

            //return img;
        }

        static public bool Download_Image(string url, string path2save)
        {
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    webClient.DownloadFileAsync(new Uri(url), path2save);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Download exception: {e.Message}");
                    return false;
                }

            }
            return true;
        }

        public static void Upload_Image(string dest_address, string file_name, string fpt_username, string pass_word)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(fpt_username, pass_word);
                    client.UploadFile(dest_address, WebRequestMethods.Ftp.UploadFile, file_name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Upload exception: {e.Message}");
            }

        }



        static readonly string s1 = @"ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚÝàáâãèéêìíòóôõùúýĂăĐđĨĩŨũƠơƯưẠạẢảẤấẦầẨẩẪẫẬậẮắẰằẲẳẴẵẶặẸẹẺẻẼẽẾếỀềỂểỄễỆệỈỉỊịỌọỎỏỐốỒồỔổỖỗỘộỚớỜờỞởỠỡỢợỤụỦủỨứỪừỬửỮữỰựỲỳỴỵỶỷỸỹ";
        static readonly string s0 = @"AAAAEEEIIOOOOUUYaaaaeeeiioooouuyAaDdIiUuOoUuAaAaAaAaAaAaAaAaAaAaAaAaEeEeEeEeEeEeEeEeIiIiOoOoOoOoOoOoOoOoOoOoOoOoUuUuUuUuUuUuUuYyYyYyYy";
        public static string RemoveDiacritics(string accentedStr)
        {
            List<char> list_char = new List<char>();
            foreach (var c in accentedStr)
            {
                var pos = s1.IndexOf(c);
                if (pos >= 0)
                {
                    list_char.Add(s0[pos]);
                }
                else
                {
                    list_char.Add(c);
                }
            }
            return new string(list_char.ToArray());
        }


        public static byte[] Serialize(Object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFomatter = new BinaryFormatter();
            binaryFomatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }

        public static Object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter.Deserialize(stream);
        }

        public static bool CreateFile(string filename)
        {
            bool bResult = true;
            if (File.Exists(filename))
            {
                return bResult;
            }

            try
            {
                //create file
                using (FileStream f = File.Create(filename))
                {
                    f.Close();
                    Console.WriteLine($"Create file {filename}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                bResult = false;
            }

            return bResult;

        }
        public static bool CreateFolder(string path_folder)
        {
            bool result = Directory.Exists(path_folder);
            if (!result)
            {
                Directory.CreateDirectory(path_folder);
                result = Directory.Exists(path_folder);
            }
            return result;
        }
        public static string GenerateNameImage()
        {
            CreateFolder(MyDefine.image_folder);
            return String.Format("{0}\\{1}.bmp", MyDefine.image_folder, DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        }
        //public static string GenerateNameImage(string model_folder, int step, bool result)
        //{
        //    CreateFolder(model_folder);
        //    return String.Format("{0}\\{1}_{2}_{3}.jpg", model_folder, result==true?"OK":"NG", DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        //}

        public static string GenerateNameImage(string cam_name)
        {
            CreateFolder(MyDefine.path_save_images);
            return String.Format("{0}\\{1}_{2}.jpg", MyDefine.path_save_images, cam_name, DateTime.Now.ToString("yyyyMMdd_hhmmss"));
        }


        public static void Save_BitMap(Bitmap bm)
        {
            try
            {
                if (bm == null)
                {
                    MyLib.ShowDlgWarning("Don't save file = null");
                    return;
                }
                string file_name = GenerateNameImage();
                Bitmap m = new Bitmap(bm);
                m.Save(file_name, ImageFormat.Bmp);
                //using (MemoryStream memory = new MemoryStream())
                //{
                //    using (FileStream fs = new FileStream(file_name, FileMode.Create, FileAccess.ReadWrite))
                //    {
                //        m.Save(memory, ImageFormat.Jpeg);
                //        byte[] bytes = memory.ToArray();
                //        fs.Write(bytes, 0, bytes.Length);
                //    }
                //}
                m.Dispose();
                bm.Dispose();
                Console.WriteLine("Saved file {0}", file_name);
            }
            catch (Exception ex)
            {
                MyLib.ShowDlgWarning(ex.Message);
            }

        }


        

        public static bool IsJobFileOK(string path)
        {
            bool isOK = File_Is_Exist(path);
            if (isOK)
            {
                //check size of job file
                long length = new System.IO.FileInfo(path).Length;
                Console.WriteLine($"Length of file = {length} bytes");
                MyLib.SequenceLog($"Length of file {path} = {length} bytes");
                if (length == 0)
                    return false;
            }
            return isOK;
        }
        public static List<string> Get_All_File_In_Folder(string path, string file_type = "*.bmp", bool debug = false)
        {
            List<string> list_files = null;
            try
            {
                string[] files_xxx = Directory.GetFiles(path, file_type, SearchOption.AllDirectories);
                list_files = new List<string>(files_xxx);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception {e.Message}");
            }


            if (debug)
            {
                foreach (var file in list_files)
                {
                    Console.WriteLine(file);
                }
            }
            return list_files;
        }

        public static bool IsImagePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            Regex regex = new Regex(MyDefine.regex_get_image_file);
            Match match = regex.Match(path);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        public static bool IsIPAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;
            Regex regex = new Regex(MyDefine.regex_get_ip);
            Match match = regex.Match(ip);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        public static List<string> Filter_Software_Type(List<string> list_files, string type, bool debug = false)
        {
            List<string> list_files_filter = new List<string>();
            Regex regex = new Regex(type);

            foreach (var file in list_files)
            {
                Match match = regex.Match(file);
                if (match.Success)
                {
                    list_files_filter.Add(file);
                }
            }

            if (debug)
            {
                foreach (var file in list_files_filter)
                {
                    Console.WriteLine(file);
                }
            }
            return list_files_filter;
        }


        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        static int MaxShowLogCount = 100;
        public static void SequenceLog(string log, LogType type = LogType.DEBUG, System.Windows.Forms.ListBox lb_ProcessingLog = null)
        {
            switch (type)
            {
                case LogType.SEQUENCE:
                    SvLogger.Log.Sequence(log);
                    break;
                case LogType.DEBUG:
                    SvLogger.Log.Debug(log);
                    break;
                case LogType.ERROR:
                    SvLogger.Log.Error(log);
                    break;
                case LogType.DATA:
                    SvLogger.Log.Data(log, DateTime.Now);
                    break;
                case LogType.RECIPE:
                    SvLogger.Log.Recipe(log);
                    break;
                default:
                    SvLogger.Log.Sequence(log);
                    break;
            }

            log = DateTime.Now.ToString("HH:mm:ss.fff: ") + log;
            try
            {
                if (lb_ProcessingLog != null)
                {
                    lb_ProcessingLog.BeginInvoke(new Action(delegate
                                    {
                                        if (lb_ProcessingLog.Items.Count >= MaxShowLogCount)
                                            lb_ProcessingLog.Items.RemoveAt(0);
                                        lb_ProcessingLog.Items.Add(log);
                                        lb_ProcessingLog.SelectedIndex = lb_ProcessingLog.Items.Count - 1;
                                    }));
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                MyLib.ShowDlgError(ex.Message);
            }
        }



    }
}
