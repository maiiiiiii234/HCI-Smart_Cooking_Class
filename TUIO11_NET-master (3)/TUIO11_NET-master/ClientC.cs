// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SocketClient;
using System.Diagnostics.Metrics;

namespace SocketClient
{
    public class ClientC
    {
        int port;
        string message;
        int byteCount;
        NetworkStream stream;
        byte[] sendData;
        TcpClient tcpClient;

        public bool connectToSocket(String host, int portNumber)
        {
            try
            {
                tcpClient = new TcpClient(host, portNumber);
                stream = tcpClient.GetStream();
                Console.WriteLine("Connection Made ! with " + host);
                return true;
            }
            catch (System.Net.Sockets.SocketException e)
            {
                Console.WriteLine("Connection Failed: " + e);
                return false;
            }
        }

        public bool sendMessage(String msg)
        {
            try
            {
                byteCount = Encoding.ASCII.GetByteCount(msg);
                sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(msg);
                stream.Write(sendData, 0, sendData.Length);
                Console.WriteLine("Data sent" + msg);
                return true;
            }
            catch (System.NullReferenceException e)
            {
                Console.WriteLine("Connection not initialized : " + e);
                return false;
            }
        }

        public bool recieveMessage()
        {
            try
            {
                Console.WriteLine("recieving");
                // Receive some data from the peer.
                byte[] receiveBuffer = new byte[1024];
                int bytesReceived = stream.Read(receiveBuffer);
                string data = Encoding.UTF8.GetString(receiveBuffer.AsSpan(0, bytesReceived));

                Console.WriteLine($"This is what the server sent to you: {data}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection not initialized : " + e);
                return false;
            }
        }
        public bool closeConnection()
        {
            stream.Close();
            tcpClient.Close();
            Console.WriteLine("Connection terminated : ");
            return true;
        }

        static void Main()
        {
            ClientC c = new ClientC();
            c.connectToSocket("127.0.0.1", 65435);
            c.recieveMessage();
            c.closeConnection();

        }
    }
}


