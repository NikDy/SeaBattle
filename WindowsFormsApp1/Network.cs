using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace WindowsFormsApp1
{
    public class Network
    {
        private TcpListener Listener = null;
        private TcpClient client = null;
        private NetworkStream stream = null;
        private Form1 parentForm;
        Thread receiveThread = null;
        private int id = 0;

        public Network(Form1 form1)
        {
            parentForm = form1;
            id = Environment.TickCount % 100000;
        }

        public int WaitForConnect()
        {
            client = Listener.AcceptTcpClient();
            stream = client.GetStream();
            return 1;
        }


        public void StartReciveThread()
        {
            receiveThread = new Thread(new ThreadStart(ReceiveMessageT));
            receiveThread.Start(); 
        }


        public void StopReciveThread()
        {
            try
            {
                receiveThread.Interrupt();
                receiveThread.Abort();
            }
            catch { };
        }


        private void ReceiveMessageT()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    
                    string message = builder.ToString();

                    if(message.Contains("_"+id.ToString()))
                    {
                        break;
                    }
                    else
                    {
                        message = message.Remove(message.IndexOf('_'));
                    }

                    if (message == "Start")                         parentForm.EnemyIsReady = true;
                    
                    int.TryParse(message, out int res);
                    if (res > 0 && res < 101 && parentForm.Turn)    parentForm.StrikeEnemy(res - 1);
                    if (res > 0 && res < 101 && !parentForm.Turn)   parentForm.StrikePlayer(res - 1);
                    if (res < 0 && parentForm.Turn)                 parentForm.ConfirmEnemyStrike(res);
                }
                catch
                {
                }
            }
        }


        public void TransmitMessage(string message)
        {
            string markedMessage = message + "_"+ id.ToString();
            byte[] data = System.Text.Encoding.Unicode.GetBytes(markedMessage);
            stream.Write(data, 0, data.Length);
        }

        
        public void Connect(string adress)
        {
            client = new TcpClient(adress, 25565);
            stream = client.GetStream();
            StartReciveThread();
        }


        public string StartToBeHost()
        {
            IPAddress localAddr = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            Listener = new TcpListener(25565);
            Listener.Start();
            StartReciveThread();
            return localAddr.MapToIPv4().ToString();
        }
    }
}
