using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Epson6Robot
{
   
    class EpsonSocketClient
    {
        private Socket m_sClient;     //接收資料的socket
        private int m_nEpsonPort;      //接收圖片埠
        private byte[] m_byteRecv;
      


        /// <summary>
        /// 連線Robot
        /// </summary>
        /// <param name="strEpsonIP">手臂端的IP</param>
        /// <param name="nEpsonPort">手臂端的Port</param>
        /// <returns>連線是否成功,1:成功,-1:失敗</returns>
        public int ConnectRobot(string strEpsonIP, int nEpsonPort)
        {
            int nRet = -1;
            byte[] byteRecv = new byte[12];

            //這是從手臂端接收回來的字串,要加 0x0D 0x0A
            byte[] byteConnectOK = { 0x43, 0x6F, 0x6E, 0x6E, 0x65, 0x63, 0x74, 0x20, 0x4F, 0x4B, 0x0D, 0x0A }; //"Connect OK"   

            //這是傳給手臂端的字串, 不用 0x0D 0x0A
            byte[] m_byteRecvOK = { 0x52, 0x65, 0x63, 0x76, 0x5F, 0x4F, 0x4B };    // Recv_OK
      
            try
            {

                if (m_sClient == null)
                {
                    m_nEpsonPort = nEpsonPort;
                    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, m_nEpsonPort);
                    m_sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_sClient.Connect(IPAddress.Parse(strEpsonIP), m_nEpsonPort);

                    nRet = Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(byteConnectOK))
                        {
                            nRet = -100;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(100);
                            //再回傳給 控制器 Recv_OK, 即完成連線交握
                            SendData(m_byteRecvOK);
                            nRet = 0;
                        }
                           
                    }
                    else
                        nRet = - 10; //通訊發生錯誤  


                    //    Thread tClientSocketThread = new Thread(new ThreadStart(thread));//執行緒開始的時候要調用的方法為threadProc.thread
                    //   tClientSocketThread.IsBackground = true;//設置IsBackground=true,後臺執行緒會自動根據主執行緒的銷毀而銷毀
                    //  tClientSocketThread.Start();

                }
                else
                {
                    //
                    if (!m_sClient.Connected)
                    {
                        m_sClient.Connect(IPAddress.Parse(strEpsonIP), nEpsonPort);
                        nRet = Receive(ref byteRecv);
                        if (nRet == 0)
                        {
                            if (BitConverter.ToString(byteRecv) != BitConverter.ToString(byteConnectOK))
                                nRet = -100;
                            else
                                nRet = 0;
                        }
                        else
                            nRet = -10; //通訊發生錯誤  
                    }
                    else    //已經連線了
                        nRet = 0;
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
