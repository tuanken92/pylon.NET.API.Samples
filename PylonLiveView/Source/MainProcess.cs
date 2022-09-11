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
            MyParam.taskLoops[(int)eTaskLoop.Task_MergeImage].RunLoop(MyParam.common_param.time_merge_image, UpdatePLCRegister).ContinueWith((a) =>
            {
                //MyLib.ShowDlgInfor($"Done task merge image!");
                Console.WriteLine("Done task merge image!");
            }); ;

        }


        
        static void UpdatePLCRegister()
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
            

            if (MyParam.common_param.processed_frame == MyParam.common_param.num_frame)
            {
                StopMergeImage();
                Console.WriteLine($"Done processing = {MyParam.common_param.processed_frame}");



                Console.WriteLine("Mat SizeX: {0}", MyParam.mat.Size().Width);
                Console.WriteLine("Mat SizeY: {0}", MyParam.mat.Size().Height);
                Console.WriteLine("Mat Channels: {0}", MyParam.mat.Channels());
                var image_file = MyLib.GenerateNameImage();
                bool bWriteOK = Cv2.ImWrite(image_file, MyParam.mat);
                Console.WriteLine("save file {1} = {0}", image_file, bWriteOK);
                if(bWriteOK)
                {
                    //Process.Start(image_file);
                    CogImage8Grey cogImage8Grey = new CogImage8Grey(MyParam.mat.ToBitmap());
                    //MyLib.Display(cogImage8Grey, MyParam.cogDisplay, true);


                    //process
                    MyParam.toolBlockProcess.Inputs["Image"].Value = cogImage8Grey;
                    MyParam.toolBlockProcess.Run();
                }
            }    

        }

        #endregion
    }
}
