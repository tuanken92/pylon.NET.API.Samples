using Basler.Pylon;
using Cognex.VisionPro;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CTTV_VisionInspection.Common.SvLogger;

namespace CTTV_VisionInspection.Common
{
    public enum eMCState
    {
        eStop,
        eStart
    }
    public static class MainProcess
    {
        #region task loop -> Merge Image
        
        

        public static void StopMergeImage()
        {
            MyParam.taskLoops[(int)eTaskLoop.Task_MergeImage].StopLoop();
        }
        public static void RunLoopMergeImage()
        {
            if (MyParam.camera == null)
            {
                Console.WriteLine($"Camera not yet connected!");
                return;
            }
            MyParam.common_param.frame_width = (int)MyParam.camera.Parameters[PLCamera.Width].GetValue();
            MyParam.common_param.frame_height = (int)MyParam.camera.Parameters[PLCamera.Height].GetValue();

            MyParam.taskLoops[(int)eTaskLoop.Task_MergeImage].ResetToken();
            MyParam.taskLoops[(int)eTaskLoop.Task_MergeImage].RunLoop(MyParam.common_param.time_merge_image, ProcessMergeFrame).ContinueWith((a) =>
            {
                //MyLib.ShowDlgInfor($"Done task merge image!");
                Console.WriteLine("Done task merge image!");
            }); ;

        }


        
        static void ProcessMergeFrame()
        {
            if (MyParam.camera == null)
            {
                Console.WriteLine($"Camera not yet connected!");
                MyParam.taskLoops[(int)eTaskLoop.Task_MergeImage].StopLoop();
                return;
            }


            byte[] data; 
            lock (MyParam.lockObject)
            {
                //Console.WriteLine($"MyParam.common_param.queue_data.Count = {MyParam.common_param.queue_data.Count}");
                if (MyParam.common_param.queue_data.Count == 0)
                {
                    return;
                }
                MyParam.common_param.processed_frame++;
                data = MyParam.common_param.queue_data.Dequeue();
            }

            //decode 2
            var src2 = new Mat(MyParam.common_param.frame_height, MyParam.common_param.frame_width, MatType.CV_8UC1);
            src2.SetArray(data);
            if (!src2.Empty())
            {
                //Console.WriteLine("Mat SizeX: {0}", src2.Size().Width);
                //Console.WriteLine("Mat SizeY: {0}", src2.Size().Height);
                //Console.WriteLine("Mat Channels: {0}", src2.Channels());

                if (!MyParam.bIsFirstImg)
                {
                    MyParam.bIsFirstImg = true;
                    MyParam.mat = src2.Clone();
                }
                else
                {
                    Cv2.VConcat(MyParam.mat, src2, MyParam.mat);
                }
                src2.Release();
            }
            else
            {
                Console.WriteLine("src2 = empty");
            } 
            
            if(!MyParam.common_param.auto_sensor_trigger)
            {
                if (MyParam.common_param.processed_frame == MyParam.common_param.num_frame)
                {
                    StopMergeImage();
                    Console.WriteLine($"Done processing = {MyParam.common_param.processed_frame}");



                    Console.WriteLine("Mat SizeX: {0}", MyParam.mat.Size().Width);
                    Console.WriteLine("Mat SizeY: {0}", MyParam.mat.Size().Height);
                    Console.WriteLine("Mat Channels: {0}", MyParam.mat.Channels());
                    //var image_file = MyLib.GenerateNameImage();
                    //bool bWriteOK = Cv2.ImWrite(image_file, MyParam.mat);
                    //Console.WriteLine("save file {1} = {0}", image_file, bWriteOK);
                    //if(bWriteOK)
                    {
                        //Process.Start(image_file);
                        CogImage8Grey cogImage8Grey = new CogImage8Grey(MyParam.mat.ToBitmap());
                        //MyLib.Display(cogImage8Grey, MyParam.cogDisplay, true);


                        //process
                        MyParam.toolBlockProcess.Inputs["Image"].Value = cogImage8Grey;
                        MyParam.toolBlockProcess.Run();
                        cogImage8Grey.Dispose();
                        MyParam.mat.Dispose();
                    }
                }    
            }
            else
            {
                if(MainProcess.trigger_status == false)
                {
                    StopMergeImage();
                    Console.WriteLine($"Done processing = {MyParam.common_param.processed_frame}");



                    Console.WriteLine("Mat SizeX: {0}", MyParam.mat.Size().Width);
                    Console.WriteLine("Mat SizeY: {0}", MyParam.mat.Size().Height);
                    Console.WriteLine("Mat Channels: {0}", MyParam.mat.Channels());
                    //var image_file = MyLib.GenerateNameImage();
                    //bool bWriteOK = Cv2.ImWrite(image_file, MyParam.mat);
                    //Console.WriteLine("save file {1} = {0}", image_file, bWriteOK);
                    //if(bWriteOK)
                    {
                        //Process.Start(image_file);
                        CogImage8Grey cogImage8Grey = new CogImage8Grey(MyParam.mat.ToBitmap());
                        //MyLib.Display(cogImage8Grey, MyParam.cogDisplay, true);


                        //process
                        MyParam.toolBlockProcess.Inputs["Image"].Value = cogImage8Grey;
                        MyParam.toolBlockProcess.Run();
                        cogImage8Grey.Dispose();
                        MyParam.mat.Dispose();
                    }

                }
            }

        }

        #endregion
          


        #region task loop -> ScanIO



        public static void StopScanIO()
        {
            MyParam.taskLoops[(int)eTaskLoop.Task_ScanIO].StopLoop();
        }
        public static void RunLoopScanIO()
        {
                        
            if (!MyLib.IsCameraConnected())
            {
                MyLib.ShowDlgWarning($"Camera not yet connected!");
                return;
            }
            Console.WriteLine("Run task scanIO!");
            MyParam.taskLoops[(int)eTaskLoop.Task_ScanIO].ResetToken();
            MyParam.taskLoops[(int)eTaskLoop.Task_ScanIO].RunLoop(MyParam.common_param.time_scan_io, ScanIO).ContinueWith((a) =>
            {
                //MyLib.ShowDlgInfor($"Done task merge image!");
                Console.WriteLine("Done task scanIO!");
            });

        }


        static bool old_state = false;
        static bool first_read_pulse = false;

        public static bool trigger_status = false;
        static void ScanIO()
        {

            
            if (!MyLib.IsCameraConnected())
            {
                MyParam.taskLoops[(int)eTaskLoop.Task_ScanIO].StopLoop();
                MyLib.ShowDlgWarning($"Camera not yet connected!");
                return;
            }

            //scanIO here
            // Select a line
            MyParam.camera.Parameters[PLCamera.LineSelector].SetValue(PLCamera.LineSelector.Line1);
            // Get the status of the line
            bool status = MyParam.camera.Parameters[PLCamera.LineStatus].GetValue();
            //Console.WriteLine("status = {0}", status);

            if(!first_read_pulse)
            {
                first_read_pulse = true;
                old_state = status;
            }

            if(old_state != status)
            {
                //have pulse
                if(status == true) //from 0->1
                {
                    trigger_status = true;
                    MyLib.StartGetFrame();
                }
                else //from 1->0
                {
                    trigger_status=false;
                    MyLib.StopGetFrame();
                }

                //update old_state
                old_state=status;
                Console.WriteLine("trigger status = {0}", trigger_status);

            }
            else
            {

            }

            //tuanna-todo: check here
            //test
            //if(trigger_status == true)
            //{
            //    MyLib.StartGetFrame();
            //}    
            //else
            //{
            //    MyLib.StopGetFrame();
            //}
        }

        #endregion
    }
}
