using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Toshiba4Robot
{
    class ToshibaSocketClient
    {
        private Socket m_sClient;     //接收資料的socket
        private int m_nToshibaPort;      //接收圖片埠
        private byte[] m_byteRecv;
       // private URRealTimeData m_realTimeData;

       // public JointDegreeInfo m_jointDegreeInfo;
       // public JointRadianInfo m_jointRadianInfo;
       // public TcpPosInfo m_tcpPosInfo;
       // public JointCurrentInfo m_jointCurrentInfo;
       // public JointVoltageInfo m_jointVoltageInfo;
      //  public JointTorqueInfo m_jointTorqueInfo;
      //  public JointTemperatureInfo m_jointTemperatureInfo;
      //  public JointSpeedInfo m_jointSpeedInfo;
      //  public JointAccelerationInfo m_jointAccInfo;


        //   public URSocketClient()
        //   {
        //new 一個 Socket
        //       m_sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //   }

        /*
        public URSocketClient(string strURIP, int nURPort)
        {
            m_nURPort = nURPort;
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, m_nURPort);
            m_sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //sRecvPic.Bind(localEndPoint);
            //sRecvPic.Listen(100);

            m_sClient.Connect(IPAddress.Parse(strURIP), nURPort);
          //  if (_sender.Connected)
         //   {
          //      byte[] sends = Encoding.Unicode.GetBytes(this.textBox1.Text);
          //      _sender.Send(sends);
          //  }
          //  IPAddress.Parse("192.168.0.53")
        }
        */

        /// <summary>
        /// 連線Robot
        /// </summary>
        /// <param name="strToshibaIP">手臂端的IP</param>
        /// <param name="nToshibaPort">手臂端的Port</param>
        /// <returns>連線是否成功,1:成功,-1:失敗</returns>
        public int ConnectRobot(string strToshibaIP, int nToshibaPort)
        {
            int nRet = -1;
            try
            {
                if (m_sClient == null)
                {
                    m_nToshibaPort = nToshibaPort;
                    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, m_nToshibaPort);
                    m_sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_sClient.Connect(IPAddress.Parse(strToshibaIP), m_nToshibaPort);

                    //    Thread tClientSocketThread = new Thread(new ThreadStart(thread));//執行緒開始的時候要調用的方法為threadProc.thread
                    //   tClientSocketThread.IsBackground = true;//設置IsBackground=true,後臺執行緒會自動根據主執行緒的銷毀而銷毀
                    //  tClientSocketThread.Start();

                    nRet = 0;
                }
                else
                {
                    //
                    if (!m_sClient.Connected)
                    {
                        m_sClient.Connect(IPAddress.Parse(strToshibaIP), nToshibaPort);
                        nRet = 0;
                    }
                    else    //已經連線了
                        nRet = 1;
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                nRet = -1;
            }

            return nRet;
        }

        /// <summary>
        /// 斷線Robot
        /// </summary>
        /// <returns>斷線是否成功,1:成功,-1:失敗</returns>
        public int DisconnectRobot()
        {
            int nRet = -1;
            try
            {
                if (m_sClient != null)
                {
                    if (m_sClient.Connected)
                    {
                        // Release the socket.
                        m_sClient.Shutdown(SocketShutdown.Both);
                        m_sClient.Disconnect(true);

                        if (m_sClient.Connected)
                            nRet = -1;
                        else
                            nRet = 0;
                    }
                    else
                        nRet = 0;
                }
                else
                    nRet = -1;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                nRet = -1;
            }

            return nRet;
        }

        public bool IsConnected()
        {
            return m_sClient.Connected;
        }

        /*
        public void thread()
        {
            //一次接收4組封包(1個封包1060 bytes)
            m_byteRecv = new byte[4240];

            while (m_sClient.Connected)
            {
                System.Threading.Thread.Sleep(1);//每個執行緒內部的閉環裡面都要加個「短時間」睡眠，使得執行緒佔用資源得到及時釋放
                try
                {
                    //清空 m_byteRecv
                    Array.Clear(m_byteRecv, 0, 4240);
                    Receive(m_byteRecv);
                 //   m_realTimeData = (URRealTimeData)BytesToStruct(m_byteRecv, m_realTimeData.GetType());
                 //   SwapRealTimeData();

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {

                }
            }
        }
        */

        /// <summary>
        /// 向遠端用戶端發送資料(string)
        /// </summary>
        /// <param name="strSendData">字串資料</param>
        public int SendData(string strSendData)
        {
           // int nSendNum = -1;
            //int nRet = -1;
            //判斷socket 是否還是連線狀態
            if (m_sClient.Connected)
            {
                try
                {
                    int nSend = 0;
                    byte[] byteData = Encoding.ASCII.GetBytes(strSendData);
                    nSend = m_sClient.Send(byteData);

                    // client.Send(Encoding.Unicode.GetBytes("Message from server at " + DateTime.Now.ToString()));
                }
                catch (SocketException ex)
                {
                    System.Console.WriteLine(ex.Message);
                    return Convert.ToInt32(ex.ErrorCode);
                }
                return 0;
            }
            else
                return -1;
        }

        /// <summary>
        /// 向遠端用戶端發送資料(byte)
        /// </summary>
        /// <param name="strSendData">字串資料</param>
        public int SendData(byte[] byteSendData)
        {
            //int nSendNum = -1;
            //int nRet = -1;
            //判斷socket 是否還是連線狀態
            if (m_sClient.Connected)
            {
                try
                {
                    int nSend = 0;
                    //byte[] byteData = Encoding.ASCII.GetBytes(strSendData);
                    nSend = m_sClient.Send(byteSendData);

                  //  byte[] kkk = { };
                  //  int nRecvNum = m_sClient.Receive(;

                    // client.Send(Encoding.Unicode.GetBytes("Message from server at " + DateTime.Now.ToString()));
                }
                catch (SocketException ex)
                {
                    System.Console.WriteLine(ex.Message);
                    return Convert.ToInt32(ex.ErrorCode);
                }
                return 0;
            }
            else
                return -1;
        }


        /// <summary>
        /// 從Server端接收資料
        /// </summary>
        /// <param name="byteRecv">資料緩衝區</param>
        public int Receive(ref byte[] byteRecv)
        {
            //int nRecvNum = -1;
            //int nRet = -1;
            //判斷socket 是否還是連線狀態
            if (m_sClient.Connected)
            {
                try
                {
                    // byte[] byteData = Encoding.ASCII.GetBytes(strSendData);
                    // sConnected.Receive(byteData);

                    //byte[] bytes = new byte[256];
                    int nRecv = 0;
                    nRecv = m_sClient.Receive(byteRecv);
 
                    // client.Send(Encoding.Unicode.GetBytes("Message from server at " + DateTime.Now.ToString()));
                }
                catch (SocketException ex)
                {
                    System.Console.WriteLine(ex.Message);
                    return Convert.ToInt32(ex.ErrorCode);
                }

                return 0;
            }
            else
                return -1;
        }


    }
}
