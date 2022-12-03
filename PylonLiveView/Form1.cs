using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PylonLiveView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            InitComport();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Connect_DMR280();
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisConnect_DMR280();
        }

        private void btnTrigger_Click(object sender, EventArgs e)
        {
            Trigger_DMR280();
        }

        //Ham minh tu viet

        void InitComport()
        {
            serialPort1.PortName = "COM4";
            serialPort1.BaudRate = 115200;
            serialPort1.DtrEnable = true;

            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler); ;
        }

        private static void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.WriteLine("Data Received:");
            Console.Write(indata);
        }

        private void DisConnect_DMR280()
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();
            Console.WriteLine(serialPort1.IsOpen);
        }


        private void Connect_DMR280()
        {

            if(!serialPort1.IsOpen)
                serialPort1.Open();

            Console.WriteLine(serialPort1.IsOpen);
        }


        void Trigger_DMR280()
        {
            serialPort1.WriteLine("+");

            //delay
            Thread.Sleep(1000);


            serialPort1.WriteLine("-");
        }
    }
}
