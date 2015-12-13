//------------------------------------------------------------------------------
// University of Texas at Arlington
// CSE 4392 - Robotic Vision
// Course Project - Fall 2015
// Deepak Pokhrel: Implementation, Design and Documentation of Algorithm
// Francisco Martinez: Hardware Integration, Testing and Software Developer
// Algorithm that allows a camera mounted on the robot to detect a person and follow him.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    class IrobotMove
    {
        private SerialPort IO;
        private bool IsValid()
        {
            return IO.IsOpen;
        }

        public void backward()
        {
            if (IsValid())
            {
                Console.WriteLine("The Connection was Successfull-Moving Backward");
                byte[] byteArray = new byte[7];
                byteArray[0] = 128;
                byteArray[1] = 131;
                byteArray[2] = 137;
                byteArray[3] = 255;
                byteArray[4] = 216;
                byteArray[5] = 80;
                byteArray[6] = 00;
                if (SendCommand(byteArray))
                {
                    Console.WriteLine("Commands sent-Moving Backward");
                }
                if (IO != null)
                {
                    Console.WriteLine("Io closed");
                    IO.Close();
                }
            }
            else
            {
                Console.WriteLine("The Connection failed");
            }
        }

        public void forward()
        {
            if (IsValid())
            {
                Console.WriteLine("The Connection was Successfull-Moving Forward");
                byte[] byteArray = new byte[7];
                byteArray[0] = 128;
                byteArray[1] = 131;
                byteArray[2] = 137;
                byteArray[3] = 0;
                byteArray[4] = 40;
                byteArray[5] = 80;
                byteArray[6] = 00;
                if (SendCommand(byteArray))
                {
                    Console.WriteLine("Commands sent-Moving Forward");
                }
                if (IO != null)
                {
                    Console.WriteLine("Io closed");
                    IO.Close();
                }
            }
            else
            {
                Console.WriteLine("The Connection failed");
            }

        }

        public void stop()
        {
            if (IsValid())
            {
                Console.WriteLine("The Connection was Successfull - Stop");
                byte[] byteArray = new byte[7];
                byteArray[0] = 128;
                byteArray[1] = 131;
                byteArray[2] = 137;
                byteArray[3] = 0;
                byteArray[4] = 0;
                byteArray[5] = 0;
                byteArray[6] = 0;
                if (SendCommand(byteArray))
                {
                    Console.WriteLine("Commands sent- Stop");
                }
                if (IO != null)
                {
                    Console.WriteLine("Io closed");
                    IO.Close();
                }
            }
            else
            {
                Console.WriteLine("The Connection failed");
            }
        }

        public bool TryToConnect()
        {
            var isPortSet = SetPort("COM4");
            if (isPortSet)
            {
                return true;
            }
            return false;
        }

        private bool SetPort(string portNum)
        {
            portNum = "COM4";
            try
            {
                if (IO != null)
                {
                    IO.Close();
                }
                IO = new SerialPort(portNum, 57600, Parity.None, 8, StopBits.One);
                IO.DtrEnable = false;
                IO.Handshake = Handshake.None;
                IO.RtsEnable = false;
                IO.Open();
                return true;
            }
            catch
            {
                portNum = String.Empty;
                return false;
            }
        }

        private bool SendCommand(IEnumerable<byte> commandCollection)
        {
            try
            {
                var commandArr = commandCollection.ToArray();
                IO.Write(commandArr, 0, commandArr.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static IEnumerable<byte> DecimalToHighLowBytes(int decimalNum)
        {
            byte highByte = (byte)(decimalNum >> 8);
            byte lowByte = (byte)(decimalNum & 255);
            var commands = new List<byte>() { highByte, lowByte };
            return commands;
        }

        private static int UnsignedHighLowBytesToDecimal(byte highByte, byte lowByte)
        {
            return 256 * highByte + lowByte;
        }

        private static int SignedHighLowBytesToDecimal(byte highByte, byte lowByte)
        {
            uint u = (uint)highByte << 8 | lowByte;
            int num = (int)(u >= (1u << 15) ? u - (1u << 16) : u);
            return num;
        }

        private void SensorDataWasReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var numOfBytes = IO.BytesToRead;
            byte[] sensorsData = new byte[numOfBytes];
            IO.Read(sensorsData, 0, numOfBytes);
        }

        public void connection()
        {
            if (TryToConnect())
            {
                Console.WriteLine("The Connection Established");
            }
        }
    }
}
