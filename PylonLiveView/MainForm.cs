using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using PylonLiveView;
using Basler.Pylon;
using OpenCvSharp;
using System.IO;
using System.Threading.Tasks;
using OpenCvSharp.Extensions;
using CTTV_VisionInspection.Common;

namespace PylonLiveView
{
    // The main window.
    public partial class MainForm : Form
    {
        
        private PixelDataConverter converter = new PixelDataConverter();
        Stopwatch stopWatch = new Stopwatch();

        // Set up the controls and events to be used and update the device list.
        public MainForm()
        {
            InitializeComponent();


            MyParam.common_param = SaveLoadParameter.Load_Parameter(MyParam.common_param, MyDefine.file_config) as CommonParam;
            // Set the default names for the controls.
            testImageControl.DefaultName = "Test Image Selector";
            pixelFormatControl.DefaultName = "Pixel Format";
            widthSliderControl.DefaultName = "Width";
            heightSliderControl.DefaultName = "Height";
            gainSliderControl.DefaultName = "Gain";
            exposureTimeSliderControl.DefaultName = "Exposure Time";

            // Update the list of available MyParam.camera devices in the upper left area.
            UpdateDeviceList();

            // Disable all buttons.
            EnableButtons( false, false );

            

            num_queue_nbup.Value = MyParam.common_param.num_frame;
        }


        // Occurs when the single frame acquisition button is clicked.
        private void toolStripButtonOneShot_Click( object sender, EventArgs e )
        {
            OneShot(); // Start the grabbing of one image.
        }


        // Occurs when the continuous frame acquisition button is clicked.
        private void toolStripButtonContinuousShot_Click( object sender, EventArgs e )
        {
            ContinuousShot(); // Start the grabbing of images until grabbing is stopped.
        }


        // Occurs when the stop frame acquisition button is clicked.
        private void toolStripButtonStop_Click( object sender, EventArgs e )
        {
            Stop(); // Stop the grabbing of images.
        }


        // Occurs when a device with an opened connection is removed.
        private void OnConnectionLost( Object sender, EventArgs e )
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke( new EventHandler<EventArgs>( OnConnectionLost ), sender, e );
                return;
            }

            // Close the MyParam.camera object.
            DestroyCamera();
            // Because one device is gone, the list needs to be updated.
            UpdateDeviceList();
        }


        // Occurs when the connection to a MyParam.camera device is opened.
        private void OnCameraOpened( Object sender, EventArgs e )
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke( new EventHandler<EventArgs>( OnCameraOpened ), sender, e );
                return;
            }

            // The image provider is ready to grab. Enable the grab buttons.
            EnableButtons( true, false );
        }


        // Occurs when the connection to a MyParam.camera device is closed.
        private void OnCameraClosed( Object sender, EventArgs e )
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke( new EventHandler<EventArgs>( OnCameraClosed ), sender, e );
                return;
            }

            // The MyParam.camera connection is closed. Disable all buttons.
            EnableButtons( false, false );
        }


        // Occurs when a MyParam.camera starts grabbing.
        private void OnGrabStarted( Object sender, EventArgs e )
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke( new EventHandler<EventArgs>( OnGrabStarted ), sender, e );
                return;
            }

            // Reset the stopwatch used to reduce the amount of displayed images. The MyParam.camera may acquire images faster than the images can be displayed.

            stopWatch.Reset();

            // Do not update the device list while grabbing to reduce jitter. Jitter may occur because the GUI thread is blocked for a short time when enumerating.
            updateDeviceListTimer.Stop();

            // The MyParam.camera is grabbing. Disable the grab buttons. Enable the stop button.
            EnableButtons( false, true );
        }

        
        // Occurs when an image has been acquired and is ready to be processed.
        private void OnImageGrabbed( Object sender, ImageGrabbedEventArgs e )
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
                // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
                BeginInvoke( new EventHandler<ImageGrabbedEventArgs>( OnImageGrabbed ), sender, e.Clone() );
                return;
            }

            try
            {
                // Acquire the image from the MyParam.camera. Only show the latest image. The MyParam.camera may acquire images faster than the images can be displayed.

                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.IsValid)
                {
                    
                    //push to queue
                    lock(MyParam.lockObject)
                    {
                        byte[] buffer = grabResult.PixelData as byte[];
                        MyParam.common_param.queue_data.Enqueue(buffer);
                        MyParam.common_param.cur_frame++;
                    }    

                    // Reduce the number of displayed images to a reasonable amount if the MyParam.camera is acquiring images very fast.
                    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                    {
                        stopWatch.Restart();

                        Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                        // Lock the bits of the bitmap.
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        // Place the pointer to the buffer of the bitmap.
                        converter.OutputPixelFormat = PixelType.BGRA8packed;
                        IntPtr ptrBmp = bmpData.Scan0;
                        converter.Convert( ptrBmp, bmpData.Stride * bitmap.Height, grabResult );
                        bitmap.UnlockBits( bmpData );

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
                    else
                    {
                        Console.WriteLine("Skip display, time process to long = {0}", stopWatch.ElapsedMilliseconds);
                    }

                    if(MyParam.common_param.cur_frame == MyParam.common_param.num_frame)
                    {
                        Stop();
                        
                        //_ =  Async2();
                        Console.WriteLine("Stop Grab = {0}", MyParam.common_param.num_frame);
                    }
                }
                else
                {
                    Console.WriteLine("Error: {0} {1}", grabResult.ErrorCode, grabResult.ErrorDescription);
                }
            }
            catch (Exception exception)
            {
                ShowException( exception );
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
        }


        public async Task Async2()
        {

            Action myaction = () => {
                MergeImage();
            };
            Task task = new Task(myaction);
            task.Start();

            await task;

            Console.WriteLine("Get Image done!");

        }


        void MergeImage()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            Mat mat = new Mat();
            bool bIsFirstImg = false;
            while (MyParam.common_param.queue_data.Count > 0)
            {
                var data = MyParam.common_param.queue_data.Dequeue();


                //decode 2
                var src2 = new Mat(MyParam.common_param.frame_height, MyParam.common_param.frame_height, MatType.CV_8UC1);
                src2.SetArray(data);
                if (!src2.Empty())
                {
                    //Console.WriteLine("Mat SizeX: {0}", src2.Size().Width);
                    //Console.WriteLine("Mat SizeY: {0}", src2.Size().Height);
                    //Console.WriteLine("Mat Channels: {0}", src2.Channels());

                    if (!bIsFirstImg)
                    {
                        bIsFirstImg = true;
                        mat = src2.Clone();
                    }
                    else
                    {
                        Cv2.VConcat(mat, src2, mat);
                    }
                    src2.Release();
                }
            }

            watch.Stop();
            var time_im_decode2 = watch.Elapsed.TotalMilliseconds;
            Console.WriteLine($"time_im_decode2 grab = {time_im_decode2}");

            Console.WriteLine("Mat SizeX: {0}", mat.Size().Width);
            Console.WriteLine("Mat SizeY: {0}", mat.Size().Height);
            Console.WriteLine("Mat Channels: {0}", mat.Channels());
            var image_file = GenerateNameImage();
            bool b = Cv2.ImWrite(image_file, mat);
            Console.WriteLine("save file {1} = {0}", image_file, b);
            MyLib.Display(mat, pictureBoxMergeImage);

        }


        public static string GenerateNameImage()
        {
            CreateFolder(MyDefine.path_save_images);
            return String.Format("{0}\\{1}.tiff", MyDefine.path_save_images, DateTime.Now.ToString("yyyyMMdd_hhmmss"));
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

        // Occurs when a MyParam.camera has stopped grabbing.
        private void OnGrabStopped( Object sender, GrabStopEventArgs e )
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke( new EventHandler<GrabStopEventArgs>( OnGrabStopped ), sender, e );
                return;
            }

            // Reset the stopwatch.
            stopWatch.Reset();

            // Re-enable the updating of the device list.
            updateDeviceListTimer.Start();

            // The MyParam.camera stopped grabbing. Enable the grab buttons. Disable the stop button.
            EnableButtons( true, false );

            // If the grabbed stop due to an error, display the error message.
            if (e.Reason != GrabStopReason.UserRequest)
            {
                MessageBox.Show( "A grab error occured:\n" + e.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }


        // Checks if single shot is supported by the MyParam.camera.
        public bool IsSingleShotSupported()
        {
            // Camera can be null if not yet opened
            if (MyParam.camera == null)
            {
                return false;
            }

            // Camera can be closed
            if (!MyParam.camera.IsOpen)
            {
                return false;
            }

            bool canSet = MyParam.camera.Parameters[PLCamera.AcquisitionMode].CanSetValue( "SingleFrame" );
            return canSet;
        }


        // Helps to set the states of all buttons.
        private void EnableButtons( bool canGrab, bool canStop )
        {
            toolStripButtonContinuousShot.Enabled = canGrab;
            toolStripButtonOneShot.Enabled = canGrab && IsSingleShotSupported();
            toolStripButtonStop.Enabled = canStop;
        }


        // Stops the grabbing of images and handles exceptions.
        private void Stop()
        {
            // Stop the grabbing.
            try
            {
                MyParam.camera.StreamGrabber.Stop();
            }
            catch (Exception exception)
            {
                ShowException( exception );
            }
        }


        // Closes the MyParam.camera object and handles exceptions.
        private void DestroyCamera()
        {
            // Disable all parameter controls.
            try
            {
                if (MyParam.camera != null)
                {

                    testImageControl.Parameter = null;
                    pixelFormatControl.Parameter = null;
                    widthSliderControl.Parameter = null;
                    heightSliderControl.Parameter = null;
                    gainSliderControl.Parameter = null;
                    exposureTimeSliderControl.Parameter = null;
                }
            }
            catch (Exception exception)
            {
                ShowException( exception );
            }

            // Destroy the MyParam.camera object.
            try
            {
                if (MyParam.camera != null)
                {
                    MyParam.camera.Close();
                    MyParam.camera.Dispose();
                    MyParam.camera = null;
                }
            }
            catch (Exception exception)
            {
                ShowException( exception );
            }
        }


        // Starts the grabbing of a single image and handles exceptions.
        private void OneShot()
        {
            try
            {
                // Starts the grabbing of one image.
                Configuration.AcquireSingleFrame( MyParam.camera, null );
                MyParam.camera.StreamGrabber.Start( 1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber );
            }
            catch (Exception exception)
            {
                ShowException( exception );
            }
        }


        // Starts the continuous grabbing of images and handles exceptions.
        private void ContinuousShot()
        {
            try
            {
                // Start the grabbing of images until grabbing is stopped.
                //Configuration.AcquireContinuous( MyParam.camera, null );
                MyParam.camera.StreamGrabber.Start( GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber );
            }
            catch (Exception exception)
            {
                ShowException( exception );
            }
        }


        // Updates the list of available MyParam.camera devices.
        private void UpdateDeviceList()
        {
            try
            {
                // Ask the MyParam.camera finder for a list of MyParam.camera devices.
                List<ICameraInfo> allCameras = CameraFinder.Enumerate();

                ListView.ListViewItemCollection items = deviceListView.Items;

                // Loop over all cameras found.
                foreach (ICameraInfo cameraInfo in allCameras)
                {
                    // Loop over all cameras in the list of cameras.
                    bool newitem = true;
                    foreach (ListViewItem item in items)
                    {
                        ICameraInfo tag = item.Tag as ICameraInfo;

                        // Is the MyParam.camera found already in the list of cameras?
                        if (tag[CameraInfoKey.FullName] == cameraInfo[CameraInfoKey.FullName])
                        {
                            tag = cameraInfo;
                            newitem = false;
                            break;
                        }
                    }

                    // If the MyParam.camera is not in the list, add it to the list.
                    if (newitem)
                    {
                        // Create the item to display.
                        ListViewItem item = new ListViewItem(cameraInfo[CameraInfoKey.FriendlyName]);

                        // Create the tool tip text.
                        string toolTipText = "";
                        foreach (KeyValuePair<string, string> kvp in cameraInfo)
                        {
                            toolTipText += kvp.Key + ": " + kvp.Value + "\n";
                        }
                        item.ToolTipText = toolTipText;

                        // Store the MyParam.camera info in the displayed item.
                        item.Tag = cameraInfo;

                        // Attach the device data.
                        deviceListView.Items.Add( item );
                    }
                }



                // Remove old MyParam.camera devices that have been disconnected.
                foreach (ListViewItem item in items)
                {
                    bool exists = false;

                    // For each MyParam.camera in the list, check whether it can be found by enumeration.
                    foreach (ICameraInfo cameraInfo in allCameras)
                    {
                        if (((ICameraInfo)item.Tag)[CameraInfoKey.FullName] == cameraInfo[CameraInfoKey.FullName])
                        {
                            exists = true;
                            break;
                        }
                    }
                    // If the MyParam.camera has not been found, remove it from the list view.
                    if (!exists)
                    {
                        deviceListView.Items.Remove( item );
                    }
                }
            }
            catch (Exception exception)
            {
                ShowException( exception );
            }
        }


        // Shows exceptions in a message box.
        private void ShowException( Exception exception )
        {
            MessageBox.Show( "Exception caught:\n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
        }


        // Closes the MyParam.camera object when the window is closed.
        private void MainForm_FormClosing( object sender, FormClosingEventArgs ev )
        {
            // Close the MyParam.camera object.
            DestroyCamera();
            UpdateParam();
            SaveLoadParameter.Save_Parameter(MyParam.common_param, MyDefine.file_config);
        }


        // Occurs when a new MyParam.camera has been selected in the list. Destroys the object of the currently opened MyParam.camera device and
        // creates a new object for the selected MyParam.camera device. After that, the connection to the selected MyParam.camera device is opened.
        private void deviceListView_SelectedIndexChanged( object sender, EventArgs ev )
        {
            // Destroy the old MyParam.camera object.
            if (MyParam.camera != null)
            {
                DestroyCamera();
            }


            // Open the connection to the selected MyParam.camera device.
            if (deviceListView.SelectedItems.Count > 0)
            {
                // Get the first selected item.
                ListViewItem item = deviceListView.SelectedItems[0];
                // Get the attached device data.
                ICameraInfo selectedCamera = item.Tag as ICameraInfo;
                try
                {
                    // Create a new MyParam.camera object.
                    MyParam.camera = new Camera( selectedCamera );

                    //MyParam.camera.CameraOpened += Configuration.AcquireContinuous;

                    // Register for the events of the image provider needed for proper operation.
                    MyParam.camera.ConnectionLost += OnConnectionLost;
                    MyParam.camera.CameraOpened += OnCameraOpened;
                    MyParam.camera.CameraClosed += OnCameraClosed;
                    MyParam.camera.StreamGrabber.GrabStarted += OnGrabStarted;
                    MyParam.camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                    MyParam.camera.StreamGrabber.GrabStopped += OnGrabStopped;

                    // Open the connection to the MyParam.camera device.
                    MyParam.camera.Open();

                    // Set the parameter for the controls.
                    if (MyParam.camera.Parameters[PLCamera.TestImageSelector].IsWritable)
                    {
                        testImageControl.Parameter = MyParam.camera.Parameters[PLCamera.TestImageSelector];
                    }
                    else
                    {
                        testImageControl.Parameter = MyParam.camera.Parameters[PLCamera.TestPattern];
                    }
                    pixelFormatControl.Parameter = MyParam.camera.Parameters[PLCamera.PixelFormat];
                    widthSliderControl.Parameter = MyParam.camera.Parameters[PLCamera.Width];
                    heightSliderControl.Parameter = MyParam.camera.Parameters[PLCamera.Height];


                   

                    if (MyParam.camera.Parameters.Contains( PLCamera.GainAbs ))
                    {
                        gainSliderControl.Parameter = MyParam.camera.Parameters[PLCamera.GainAbs];
                    }
                    else
                    {
                        gainSliderControl.Parameter = MyParam.camera.Parameters[PLCamera.Gain];
                    }
                    if (MyParam.camera.Parameters.Contains( PLCamera.ExposureTimeAbs ))
                    {
                        exposureTimeSliderControl.Parameter = MyParam.camera.Parameters[PLCamera.ExposureTimeAbs];
                    }
                    else
                    {
                        exposureTimeSliderControl.Parameter = MyParam.camera.Parameters[PLCamera.ExposureTime];
                    }
                }
                catch (Exception exception)
                {
                    ShowException( exception );
                }
            }
        }


        // If the F5 key has been pressed, update the list of devices.
        private void deviceListView_KeyDown( object sender, KeyEventArgs ev )
        {
            if (ev.KeyCode == Keys.F5)
            {
                ev.Handled = true;
                // Update the list of available MyParam.camera devices.
                UpdateDeviceList();
            }
        }


        // Timer callback used to periodically check whether displayed MyParam.camera devices are still attached to the PC.
        private void updateDeviceListTimer_Tick( object sender, EventArgs e )
        {
            UpdateDeviceList();
        }

        private void num_queue_nbup_ValueChanged(object sender, EventArgs e)
        {
            
        }


        void UpdateParam()
        {
            //prepare queue data
            
            MyParam.common_param.num_frame = (int)num_queue_nbup.Value;
        }
        private void GetFrame_Btn_Click(object sender, EventArgs e)
        {
            if (MyParam.camera == null)
                return;

            // We also increase the number of memory buffers to be used while grabbing.
            long max_buffer = MyParam.camera.Parameters[PLCameraInstance.MaxNumBuffer].GetMaximum();

            MyParam.camera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(MyParam.common_param.num_frame);

            UpdateParam();
            MyParam.common_param.cur_frame = 0;
            MyParam.common_param.processed_frame = 0;
            MyParam.bIsFirstImg = false;
            MyParam.mat.Release();
            MyParam.common_param.queue_data.Clear();
            MyParam.common_param.queue_data = new Queue<byte[]>(MyParam.common_param.num_frame);
            Console.WriteLine("num_queue = {0}, real = {1}, max buffer = {2}", MyParam.common_param.num_frame, num_queue_nbup.Value, max_buffer);
            Console.WriteLine("width = {0}, height = {1}", MyParam.common_param.frame_width, MyParam.common_param.frame_height);
            MainProcess.RunLoopMergeImage();
            //start
            ContinuousShot();
        }


        public class MyDefine
        {

            public static uint WM_LBUTTONDOWN = 0x201;
            public static uint WM_LBUTTONUP = 0x202;

            public static readonly string workingDirectory = Environment.CurrentDirectory;
            public static readonly string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            public static readonly string workspaceDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            public static readonly string resources_folder = String.Format($"{workingDirectory}\\resources");

            public static readonly string regex_get_image_file = @"[^\s]+(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$";
            public static readonly string regex_get_ip = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";


            #region Path file json
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
            #endregion


        }

        private void CloseCamera_Btn_Click(object sender, EventArgs e)
        {
            if (MyParam.camera == null)
                return;

            if (MyParam.camera.IsOpen)
                MyParam.camera.Close();
        }

        private void SettingBtn_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm();
            settingForm.ShowDialog();

        }
    }
}