using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using IRobotMotion;

namespace URRobot
{
    class URSocketServer
    {
        //Socket sRecvCmd;            //監聽Socket
        Socket m_sListen;            //監聽Socket
    //    public Socket m_sConnected;    //成功連接到的socket

        int m_nListenPort;    //接收圖片請求命令  //電腦端這邊開出來的Port
                              //int sendPicPort;    //發送圖片命令

        public bool m_bCloseListenSocket = true;
        public bool m_bConnectedSocket = false;

        public TcpClient tcpclnt = new TcpClient();
        //下命令讓UR移動的socket (在電腦這端為Server端)
        //需為非阻塞式socket

        /*
    public URSocketServer(int nListenPort)
    {
        m_nListenPort = nListenPort; //監聽Port
                                //sendPicPort = sendPort;

        //建立本地socket，一直對4000埠進行偵聽
        //  IPEndPoint recvCmdLocalEndPoint = new IPEndPoint(IPAddress.Any, recvCmdPort);
        //  sRecvCmd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //  sRecvCmd.Bind(recvCmdLocalEndPoint);
        //  sRecvCmd.Listen(100);

        IPEndPoint recvCmdLocalEndPoint = new IPEndPoint(IPAddress.Any, m_nListenPort);
        sListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sListen.Bind(recvCmdLocalEndPoint);
        sListen.Listen(100);

    }
    */

        /// <summary>
        /// 連線Robot
        /// </summary>
        /// <param name="nListenPort">監聽Port, 從手臂端連線到PC端</param>
        /// <returns>連線是否成功,1:成功,-1:失敗</returns>
        public int ConnectRobot(string URIP,int nPCListenPort)
        {
            int nRet = -1;
            try
            {
                m_nListenPort = nPCListenPort; //監聽Port
                                       
                //建立本地socket，一直對40000埠進行偵聽
               // IPEndPoint  recvCmdLocalEndPoint = new IPEndPoint(IPAddress.Any, m_nListenPort);
                //IPEndPoint recvCmdLocalEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.5"), m_nListenPort);
                tcpclnt.Connect("192.168.0.6", 30002);
                //script連線 用同ip不同port
                m_sListen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
               // m_sListen = new Socket( SocketType.Stream, ProtocolType.Tcp);
                //m_sListen.Bind(recvCmdLocalEndPoint);
                m_sListen.Listen(0);
                m_bCloseListenSocket = false;

                nRet = 0;
                //m_sListen.BeginAccept(new AsyncCallback(Accept), m_sListen);
                //  m_sConnected = m_sListen.Accept();
                //  int kkk = 0;

                  Thread tListenSocketThread = new Thread(new ThreadStart(thread));
                  tListenSocketThread.IsBackground = true;
                  tListenSocketThread.Start();
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                nRet = -1;
            }

            return nRet;
        }

        /*
        void Accept(IAsyncResult iar)
        {
            //还原传入的原始套接字
            Socket MyServer = (Socket)iar.AsyncState;
            //在原始套接字上调用EndAccept方法，返回新的套接字
           // Socket service = MyServer.EndAccept(iar);

            m_sConnected = MyServer.EndAccept(iar);
        }
        */

        /// <summary>
        /// 斷線Robot
        /// </summary>
        /// <returns>斷線是否成功,1:成功,-1:失敗</returns>
        public int DisconnectRobot()
        {
           
            int nRet = -1;
            try
            {
              /*  if (m_sConnected != null)
                {
                    if (m_sConnected.Connected)
                    {
                        // Release the socket.
                        m_sConnected.Shutdown(SocketShutdown.Both);
                        m_sConnected.Disconnect(false);
                        m_sConnected.Close();

                        if (m_sConnected.Connected)
                            nRet = -1;
                        else
                            nRet = 0;
                    }
                }
                else
                    nRet = 0;*/


                if (m_sListen != null)
                {
                    // if (m_sListen.Connected)
                    // {

                    // Release the socket.
                    //m_sListen.Shutdown(SocketShutdown.Both);
                    // m_sListen.Disconnect(false);
                    m_sListen.Close();

                    m_bCloseListenSocket = true;

                    if (m_sListen.Connected)
                        nRet = -1;
                    else
                        nRet = 0;
                    //  }
                }
                else
                    nRet = 0;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                nRet = -1;
            }

            return nRet;
        }

        /// <summary>
        /// 建立 keepalive 作業所需的輸入資料 
        /// </summary>
        /// <param name="onOff">是否啟用1:on ,0:off</param>
        private byte[] GetKeepAliveSetting(int onOff, int keepAliveTime, int keepAliveInterval)
        {
            byte[] buffer = new byte[12];
            BitConverter.GetBytes(onOff).CopyTo(buffer, 0);
            BitConverter.GetBytes(keepAliveTime).CopyTo(buffer, 4);
            BitConverter.GetBytes(keepAliveInterval).CopyTo(buffer, 8);
            return buffer;
        }

        private void thread()
        {
            //Accept 以同步方式從偵聽通訊端的連接請求佇列中提取第一個掛起的連接請求，然後創建並返回新的 Socket
            //Socket sRecvCmdTemp = sRecvCmd.Accept();
            //sRecvCmdTemp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);//設置接收資料超時
            //sRecvCmdTemp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);//設置發送資料超時
            //sRecvCmdTemp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 1024); //設置發送緩衝區大小 1K
            //sRecvCmdTemp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024);//設置接收緩衝區大小1K

            while (!m_bCloseListenSocket)
            {
                //每個執行緒內部的閉環裡面都要加個「短時間」睡眠，使得執行緒佔用資源得到及時釋放
                System.Threading.Thread.Sleep(10);

                try
                {
                    //Accept 以同步方式從偵聽通訊端的連接請求佇列中提取第一個掛起的連接請求，然後創建並返回新的 Socket
                    //Socket sRecvCmdTemp = m_sListen.Accept();
                  /*  m_sConnected = m_sListen.Accept();
                    m_sConnected.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);//設置接收資料超時
                    m_sConnected.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);//設置發送資料超時
                    m_sConnected.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 1024); //設置發送緩衝區大小 1K
                    m_sConnected.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024);//設置接收緩衝區大小1K
*/
                    m_bConnectedSocket = true;

                //    m_sConnected.IOControl(IOControlCode.KeepAliveValues, GetKeepAliveSetting(1, 5000, 5000), null);
                    // if (sConnected.Connected == true)

                    /*
                    byte[] recvBytes = new byte[1024];//開啟一個緩衝區，存儲接收到的資訊
                    sRecvCmdTemp.Receive(recvBytes); //將讀得的內容放在recvBytes中
                    string strRecvCmd = Encoding.Default.GetString(recvBytes);//
                                                                              //程式運行到這個地方，已經能接收到遠端發過來的命令了

                    //*************
                    //解碼命令，並執行相應的操作----如下面的發送本機圖片
                    //*************
                    string[] strArray = strRecvCmd.Split(';');
                    if (strArray[0] == "PicRequest")
                    {
                        //遠處終端的請求端IP和埠，如：127.0.0.1：4000
                        string[] strRemoteEndPoint = sRecvCmdTemp.RemoteEndPoint.ToString().Split(':');
                        string strRemoteIP = strRemoteEndPoint[0];
                        SentPictures(strRemoteIP, sendPicPort); //發送本機圖片檔

                        recvBytes = null;
                    }
                    */

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }


        }

        /// <summary>
        /// 向遠端用戶端發送資料
        /// </summary>
        /// <param name="strSendData">字串資料</param>
        public bool SendData(string strSendData)
        {
            //判斷socket 是否還是連線狀態
         /*   if (m_sConnected.Connected)
            {
                try
                {
                    byte[] byteData = Encoding.ASCII.GetBytes(strSendData);
                    m_sConnected.Send(byteData);

                    // client.Send(Encoding.Unicode.GetBytes("Message from server at " + DateTime.Now.ToString()));
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                return true;
            }
            else*/
                return false;
        }

        /// <summary>
        /// 從Client端接收資料
        /// </summary>
        /// <param name="byteRecv">資料緩衝區</param>
        public int Receive(byte[] byteRecv)
        {
            int nRecvNum = 0;
            //判斷socket 是否還是連線狀態
       /*     if (m_sConnected.Connected)
            {
                try
                {
                   // byte[] byteData = Encoding.ASCII.GetBytes(strSendData);
                   // sConnected.Receive(byteData);

                    //byte[] bytes = new byte[256];
                    nRecvNum = m_sConnected.Receive(byteRecv);

                    // client.Send(Encoding.Unicode.GetBytes("Message from server at " + DateTime.Now.ToString()));
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }

                return nRecvNum;
            }
            else*/
                return -1;
        }

        /*
        /// <summary>
        /// 向遠端用戶端發送圖片
        /// </summary>
        /// <param name="strRemoteIP">遠端用戶端IP</param>
        /// <param name="sendPort">發送圖片的埠</param>
        private static void SentPictures(string strRemoteIP, int sendPort)
        {


            string path = "D:\\images\\";
            string strImageTag = "image";//圖片名稱中包含有image的所有圖片檔


            try
            {
                string[] picFiles = Directory.GetFiles(path, strImageTag + "*", SearchOption.TopDirectoryOnly);//滿足要求的檔個數


                if (picFiles.Length == 0)
                {
                    return;//沒有圖片，不做處理
                }

                long sendBytesTotalCounts = 0;//發送資料流程總長度

                //消息頭部：命令標識+檔數目+......檔i長度+
                string strMsgHead = "PicResponse;" + picFiles.Length + ";";



                //消息體：圖片檔流
                byte[][] msgPicBytes = new byte[picFiles.Length][];
                for (int j = 0; j < picFiles.Length; j++)
                {
                    FileStream fs = new FileStream(picFiles[j].ToString(), FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(fs);
                    msgPicBytes[j] = new byte[fs.Length];
                    strMsgHead += fs.Length.ToString() + ";";
                    sendBytesTotalCounts += fs.Length;
                    reader.Read(msgPicBytes[j], 0, msgPicBytes[j].Length);
                }

                byte[] msgHeadBytes = Encoding.Default.GetBytes(strMsgHead);//將消息頭字串轉成byte陣列
                sendBytesTotalCounts += msgHeadBytes.Length;
                //要發送的資料流程:資料頭＋資料體
                byte[] sendMsgBytes = new byte[sendBytesTotalCounts];//要發送的總數組

                for (int i = 0; i < msgHeadBytes.Length; i++)
                {
                    sendMsgBytes[i] = msgHeadBytes[i]; //資料頭
                }


                int index = msgHeadBytes.Length;
                for (int i = 0; i < picFiles.Length; i++)
                {

                    for (int j = 0; j < msgPicBytes[i].Length; j++)
                    {
                        sendMsgBytes[index + j] = msgPicBytes[i][j];
                    }
                    index += msgPicBytes[i].Length;
                }
                //程式執行到此處，帶有圖片資訊的報文已經準備好了
                //PicResponse;2;94223;69228;
                //+圖片1位元流+......圖片2位元流

                try
                {
                    #region 發送圖片
                    Socket sSendPic = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress ipAddress = IPAddress.Parse(strRemoteIP);//remoteip = "127.0.0.1"


                    try
                    {
                        sSendPic.Connect(ipAddress, sendPort);//連接無端用戶端主機
                        sSendPic.Send(sendMsgBytes, sendMsgBytes.Length, 0);//發送本地圖片
                    }
                    catch (System.Exception e)
                    {
                        System.Console.Write("SentPictures函數在建立遠端連線時出現異常：" + e.Message);
                    }
                    finally
                    {
                        sSendPic.Close();
                    }
                    #endregion
                }
                catch
                {
                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }
        */



        




    }
}




   
