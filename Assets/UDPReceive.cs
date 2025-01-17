using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{

    Thread receiveThread;
    public UdpClient client; 
    public int port = 5052;
    public bool startRecieving = false;
    public bool printToConsole = false;
    public string data;


    public void Start()
    {
        receiveThread = new Thread(
        new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }


    // receive thread
    private void ReceiveData()
    {
        portOlustur();

        while (startRecieving)
        {

            try
            {
                
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                if (printToConsole) { print(data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    private void portOlustur()
    {
        try
        {
            client = new UdpClient(port);
        }
        catch (SocketException err)
        {
            print(err.ToString());
        }
    }
}
