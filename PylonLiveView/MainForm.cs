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
using Cognex.VisionPro;

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


            MyParam.cogDisplay = this.cogDisplay1;
            MyParam.cogRecordDisplay = this.cogRecordDisplay1;

            //MyLib.InitObject((int)TYPE_OF_TOOLBLOCK.AcqFifo);
            MyLib.InitObject((int)TYPE_OF_TOOLBLOCK.ImageProcess);

            TriggerSensor_Chbx.Checked = MyParam.common_param.auto_sensor_trigger;

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
                        Console.WriteLine("Push to queue = {0}", MyParam.common_param.cur_frame);
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

                    if(!MyParam.common_param.auto_sensor_trigger)
                    {
                        if (MyParam.common_param.cur_frame == MyParam.common_param.num_frame)
                        {
                            Stop();
                            Console.WriteLine("Stop Grab = {0}", MyParam.common_param.num_frame);
                        }
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
                //MergeImage();
            };
            Task task = new Task(myaction);
            task.Start();

            await task;

            Console.WriteLine("Get Image done!");

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
            MyLib.ReleaseObject();

            MainProcess.StopScanIO();
            MainProcess.StopMergeImage();

            //save param
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


                    //ScanIO
                    if(MyParam.common_param.auto_sensor_trigger)
                        MainProcess.RunLoopScanIO();

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


        

        private void CloseCamera_Btn_Click(object sender, EventArgs e)
        {
            if (MyParam.camera == null)
                return;

            if (MyParam.camera.IsOpen)
                MyParam.camera.Close();

            //ScanIO
            MainProcess.StopScanIO();
        }

        private void SettingBtn_Click(object sender, EventArgs e)
        {
            SettingForm settingForm = new SettingForm();
            settingForm.Show();

        }


        

        private void TestBtn_Click(object sender, EventArgs e)
        {
            //Display(true);
            
            //Display(false);

            
            //CogImage8Grey cogImage8Grey = new CogImage8Grey(MyParam.mat.ToBitmap());
            //cogDisplay1.Image = cogImage8Grey;

        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("Size changed!");
            //Console.WriteLine(e.ToString());
            //if(this.WindowState == FormWindowState.Maximized)
            {
                var size = splitContainerImageView.Panel2.ClientSize;
                Console.WriteLine($"{size.Width}, {size.Height}");
                pictureBox.Location = new System.Drawing.Point(10, 10);
                pictureBox.Size= new System.Drawing.Size(size.Width/2 - 10, size.Height - 30);
                cogDisplay1.Location = new System.Drawing.Point(size.Width / 2 + 10, 10);
                cogDisplay1.Size = new System.Drawing.Size(size.Width / 2 - 10, size.Height - 30);

                cogRecordDisplay1.Location = new System.Drawing.Point(size.Width + 10, 10);
                cogRecordDisplay1.Size = new System.Drawing.Size(size.Width / 2 - 10, size.Height - 30);
            }
        }


        static bool bFistInit = false;
        private void TriggerSensor_Chbx_CheckedChanged(object sender, EventArgs e)
        {

            MyParam.common_param.auto_sensor_trigger = TriggerSensor_Chbx.Checked;
            Console.WriteLine("auto_sensor_trigger = " + MyParam.common_param.auto_sensor_trigger);

            GetFrame_Btn.Enabled = !MyParam.common_param.auto_sensor_trigger;
            

            //Ignore first init!
            if(!bFistInit)
            {
                bFistInit = true;
                return;
            }

            if(!MyParam.common_param.auto_sensor_trigger)
            {
                MainProcess.StopScanIO();
            }
            else
            {
                MainProcess.RunLoopScanIO();
            }
        }

        private void TestBtnOn_Click(object sender, EventArgs e)
        {
            if (MyParam.camera == null)
                return;

            MainProcess.trigger_status = true;
            TestOn_Btn.Enabled = false;
            TestOff_Btn.Enabled = true;

            //// We also increase the number of memory buffers to be used while grabbing.
            //long max_buffer = MyParam.camera.Parameters[PLCameraInstance.MaxNumBuffer].GetMaximum();
            //MyParam.camera.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(1000);

            //MyParam.common_param.cur_frame = 0;
            //MyParam.common_param.processed_frame = 0;
            //MyParam.bIsFirstImg = false;
            //MyParam.mat.Release();
            //MyParam.common_param.queue_data.Clear();
            //MyParam.common_param.queue_data = new Queue<byte[]>(MyParam.common_param.num_frame);
            //Console.WriteLine("num_queue = {0}, max buffer = {1}", 1000, max_buffer);
            //Console.WriteLine("width = {0}, height = {1}", MyParam.common_param.frame_width, MyParam.common_param.frame_height);
            //MainProcess.RunLoopMergeImage();
            ////start
            //ContinuousShot();
        }

        private void TestOff_Btn_Click(object sender, EventArgs e)
        {
            //Stop(); // Stop the grabbing of images.
            MainProcess.trigger_status = false;
            TestOn_Btn.Enabled = true;
            TestOff_Btn.Enabled = false;
        }
    }
}