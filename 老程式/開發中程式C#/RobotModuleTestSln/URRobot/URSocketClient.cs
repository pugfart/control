using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace URRobot
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct URRealTimeData
    {
        public int PackageLength;
        public double Time;

        public double Base_q_target;
        public double Shoulder_q_target;
        public double Elbow_q_target;
        public double Wrist1_q_target;
        public double Wrist2_q_target;
        public double Wrist3_q_target;

        public double Base_qd_target;
        public double Shoulder_qd_target;
        public double Elbow_qd_target;
        public double Wrist1_qd_target;
        public double Wrist2_qd_target;
        public double Wrist3_qd_target;

        public double Base_qdd_target;
        public double Shoulder_qdd_target;
        public double Elbow_qdd_target;
        public double Wrist1_qdd_target;
        public double Wrist2_qdd_target;
        public double Wrist3_qdd_target;

        public double Base_I_target;
        public double Shoulder_I_target;
        public double Elbow_I_target;
        public double Wrist1_I_target;
        public double Wrist2_I_target;
        public double Wrist3_I_target;

        public double Base_M_target;
        public double Shoulder_M_target;
        public double Elbow_M_target;
        public double Wrist1_M_target;
        public double Wrist2_M_target;
        public double Wrist3_M_target;

        public double Base_q_Actual;
        public double Shoulder_q_Actual;
        public double Elbow_q_Actual;
        public double Wrist1_q_Actual;
        public double Wrist2_q_Actual;
        public double Wrist3_q_Actual;

        public double Base_qd_Actual;
        public double Shoulder_qd_Actual;
        public double Elbow_qd_Actual;
        public double Wrist1_qd_Actual;
        public double Wrist2_qd_Actual;
        public double Wrist3_qd_Actual;

        public double Base_I_Actual;
        public double Shoulder_I_Actual;
        public double Elbow_I_Actual;
        public double Wrist1_I_Actual;
        public double Wrist2_I_Actual;
        public double Wrist3_I_Actual;

        public double Base_I_Control;
        public double Shoulder_I_Control;
        public double Elbow_I_Control;
        public double Wrist1_I_Control;
        public double Wrist2_I_Control;
        public double Wrist3_I_Control;

        public double ToolVectorActual_X;
        public double ToolVectorActual_Y;
        public double ToolVectorActual_Z;
        public double ToolVectorActual_Rx;
        public double ToolVectorActual_Ry;
        public double ToolVectorActual_Rz;

        public double TCP_Speed_Actual_X;
        public double TCP_Speed_Actual_Y;
        public double TCP_Speed_Actual_Z;
        public double TCP_Speed_Actual_Rx;
        public double TCP_Speed_Actual_Ry;
        public double TCP_Speed_Actual_Rz;

        public double TCP_Force_X;
        public double TCP_Force_Y;
        public double TCP_Force_Z;
        public double TCP_Force_Rx;
        public double TCP_Force_Ry;
        public double TCP_Force_Rz;

        public double ToolVectorTarget_X;
        public double ToolVectorTarget_Y;
        public double ToolVectorTarget_Z;
        public double ToolVectorTarget_Rx;
        public double ToolVectorTarget_Ry;
        public double ToolVectorTarget_Rz;

        public double TCP_Speed_Target_X;
        public double TCP_Speed_Target_Y;
        public double TCP_Speed_Target_Z;
        public double TCP_Speed_Target_Rx;
        public double TCP_Speed_Target_Ry;
        public double TCP_Speed_Target_Rz;

        public UInt64 DigitalInputBits;

        public double Base_MotorTemperatures;
        public double Shoulder_MotorTemperatures;
        public double Elbow_MotorTemperatures;
        public double Wrist1_MotorTemperatures;
        public double Wrist2_MotorTemperatures;
        public double Wrist3_MotorTemperatures;

        public double ControllerTimer;
        public double TestValue;
        public double RobotMode;

        public double Base_JointModes;
        public double Shoulder_JointModes;
        public double Elbow_JointModes;
        public double Wrist1_JointModes;
        public double Wrist2_JointModes;
        public double Wrist3_JointModes;

        public double SafetyMode;

        public double SofewareReserve1;
        public double SofewareReserve2;
        public double SofewareReserve3;
        public double SofewareReserve4;
        public double SofewareReserve5;
        public double SofewareReserve6;

        public double ToolAccelerometerX;
        public double ToolAccelerometerY;
        public double ToolAccelerometerZ;

        public double SofewareReserve7;
        public double SofewareReserve8;
        public double SofewareReserve9;
        public double SofewareReserve10;
        public double SofewareReserve11;
        public double SofewareReserve12;

        public double SpeedScaling;
        public double LinearMomentumNorm;

        public double SofewareReserve13;
        public double SofewareReserve14;

        public double VMain;
        public double VRobot;
        public double IRobot;

        public double Base_V_Actual;
        public double Shoulder_V_Actual;
        public double Elbow_V_Actual;
        public double Wrist1_V_Actual;
        public double Wrist2_V_Actual;
        public double Wrist3_V_Actual;

        public UInt64 DigitalOutputs;

        public double ProgramState;
    }

    //關節角度值
    public struct JointDegreeInfo
    {
        public double dbBasePosDegree;
        public double dbShoulderPosDegree;
        public double dbElbowPosDegree;
        public double dbWrist1PosDegree;
        public double dbWrist2PosDegree;
        public double dbWrist3PosDegree;
    }

    //關節徑度值
    public struct JointRadianInfo
    {
        public double dbBasePosRadian;
        public double dbShoulderPosRadian;
        public double dbElbowPosRadian;
        public double dbWrist1PosRadian;
        public double dbWrist2PosRadian;
        public double dbWrist3PosRadian;
    }

    //笛卡兒座標值
    public struct TcpPosInfo
    {
        public double dbTCP_X;
        public double dbTCP_Y;
        public double dbTCP_Z;
        public double dbTCP_Rx;
        public double dbTCP_Ry;
        public double dbTCP_Rz;
    }

    //電流值
    public struct JointCurrentInfo
    {
        public double dbBase;
        public double dbShoulder;
        public double dbElbow;
        public double dbWrist1;
        public double dbWrist2;
        public double dbWrist3;
    }

    //電壓值
    public struct JointVoltageInfo
    {
        public double dbBase;
        public double dbShoulder;
        public double dbElbow;
        public double dbWrist1;
        public double dbWrist2;
        public double dbWrist3;
    }

    //扭矩值
    public struct JointTorqueInfo
    {
        public double dbBase;
        public double dbShoulder;
        public double dbElbow;
        public double dbWrist1;
        public double dbWrist2;
        public double dbWrist3;
    }

    //溫度值
    public struct JointTemperatureInfo
    {
        public double dbBase;
        public double dbShoulder;
        public double dbElbow;
        public double dbWrist1;
        public double dbWrist2;
        public double dbWrist3;
    }

    //速度值
    public struct JointSpeedInfo
    {
        public double dbBase;
        public double dbShoulder;
        public double dbElbow;
        public double dbWrist1;
        public double dbWrist2;
        public double dbWrist3;
    }

    //加速度值
    public struct JointAccelerationInfo
    {
        public double dbBase;
        public double dbShoulder;
        public double dbElbow;
        public double dbWrist1;
        public double dbWrist2;
        public double dbWrist3;
    }


    class URSocketClient
    {
        private Socket m_sClient;     //接收資料的socket
        private int m_nURPort;      //接收圖片埠
        private byte[] m_byteRecv;
        private URRealTimeData m_realTimeData;

        public JointDegreeInfo m_jointDegreeInfo;
        public JointRadianInfo m_jointRadianInfo;
        public TcpPosInfo m_tcpPosInfo;
        public JointCurrentInfo m_jointCurrentInfo;
        public JointVoltageInfo m_jointVoltageInfo;
        public JointTorqueInfo m_jointTorqueInfo;
        public JointTemperatureInfo m_jointTemperatureInfo;
        public JointSpeedInfo m_jointSpeedInfo;
        public JointAccelerationInfo m_jointAccInfo;


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
        /// <param name="strURIP">UR手臂端的IP</param>
        /// <param name="nURPort">UR手臂端的Port</param>
        /// <returns>連線是否成功,1:成功,-1:失敗</returns>
        public int ConnectRobot(string strURIP, int nURPort)
        {
            int nRet = 0;
            try
            {
                if (m_sClient == null)
                {
                    m_nURPort = nURPort;
                    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, m_nURPort);
                    m_sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_sClient.Connect(IPAddress.Parse(strURIP), m_nURPort);

                   
                    Thread tClientSocketThread = new Thread(new ThreadStart(thread));//執行緒開始的時候要調用的方法為threadProc.thread
                    tClientSocketThread.IsBackground = true;//設置IsBackground=true,後臺執行緒會自動根據主執行緒的銷毀而銷毀
                    tClientSocketThread.Start();

                    nRet = 0;
                }
                else
                {
                    //
                    if (!m_sClient.Connected)
                    {
                        m_sClient.Connect(IPAddress.Parse(strURIP), m_nURPort);
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
                    nRet = 1;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                nRet = -1;
            }

            return nRet;
        }

        public void thread()
        {
            //一次接收4組封包(1個封包1060 bytes)
            //m_byteRecv = new byte[4240];
            m_byteRecv = new byte[8480];
            while (m_sClient.Connected)
            {
                System.Threading.Thread.Sleep(1);//每個執行緒內部的閉環裡面都要加個「短時間」睡眠，使得執行緒佔用資源得到及時釋放
                try
                {
                    //清空 m_byteRecv
                    Array.Clear(m_byteRecv, 0, 8480);
                    Receive(m_byteRecv);
                    m_realTimeData = (URRealTimeData)BytesToStruct(m_byteRecv, m_realTimeData.GetType());
                    SwapRealTimeData();

                    //  Socket sRecvPicTemp = sRecvPic.Accept();//一直在等待socket請求，並建立一個和請求相同的socket,覆蓋掉原來的socket
                    //  sRecvPicTemp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000); //設置接收資料超時
                    //  sRecvPicTemp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);//設置發送資料超時
                    //  sRecvPicTemp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 1024);//設置發送緩衝區大小--1K大小
                    //  sRecvPicTemp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1024); //設置接收緩衝區大小

                    /*
                     * 
                    #region 先取出資料頭部資訊－－－並解析頭部

                    byte[] recvHeadBytes = new byte[1024];//先取1K的資料，提取出資料的頭部
                    sRecvPicTemp.Receive(recvHeadBytes, recvHeadBytes.Length, 0);
                    string recvStr = Encoding.UTF8.GetString(recvHeadBytes);
                    string[] strHeadArray = recvStr.Split(';');//PicResponse;2;94223;69228;
                    string strHeadCmd = strHeadArray[0];//頭部命令
                    int picCounts = Convert.ToInt32(strHeadArray[1]);//資料流程中包含的圖片個數
                    int[] picLength = new int[picCounts];//每個圖片的長度
                    for (int i = 0; i < picCounts; i++)
                    {
                        picLength[i] = Convert.ToInt32(strHeadArray[i + 2]);
                    }

                    #endregion

                    */


                    /*
                    int offset = 0;//資料頭的長度
                    for (int k = 0; k < strHeadArray.Length - 1; k++)
                    {
                        offset += strHeadArray[k].Length + 1;//因為後面的分號
                    }

                    int picOffset = recvHeadBytes.Length - offset;//第一張圖片在提取資料頭的時候已經被提取了一部分了


                    if (strHeadCmd == "PicResponse")
                    {
                        #region 儲存圖片－－為了節約記憶體，可以每接收一次就保存一次圖片
                        for (int i = 0; i < picCounts; i++)
                        {
                            byte[] recvPicBytes = new byte[(picLength[i])];//每次只接收一張圖片

                            if (i == 0)//第一幅圖片有一部分在提取資料頭的時候已經提取過了。
                            {
                                byte[] recvFirstPicBuffer = new byte[picLength[i] - picOffset];
                                sRecvPicTemp.Receive(recvFirstPicBuffer, recvFirstPicBuffer.Length, 0);
                                for (int j = 0; j < picOffset; j++)
                                {
                                    recvPicBytes[j] = recvHeadBytes[offset + j];//第一幅圖片的前一部分
                                }

                                for (int j = 0; j < recvFirstPicBuffer.Length; j++)//第一張圖片的後半部分
                                {
                                    recvPicBytes[picOffset + j] = recvFirstPicBuffer[j];
                                }

                                //將圖片寫入檔
                                SavePicture(recvPicBytes, "-0");
                            }
                            else
                            {
                                sRecvPicTemp.Receive(recvPicBytes, recvPicBytes.Length, 0);//每次取一張圖片的長度
                                SavePicture(recvPicBytes, "-" + i.ToString());
                                //將圖片資料寫入檔
                            }
                        }
                        #endregion

                    }
                    */

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {

                }
            }

            int kkk = 0;
        }

        /*
        /// <summary>
        /// 保存圖片到指定路徑
        /// </summary>
        /// <param name="picBytes">圖片位元流</param>
        /// <param name="picNum">圖片編號</param>
        public void SavePicture(byte[] picBytes, string picNum)
        {
            string filename = "receivePic";

            if (!Directory.Exists("E:\\images\\"))
                Directory.CreateDirectory("E:\\images\\");
            if (File.Exists("E:\\images\\" + filename + picNum + ".jpg"))
                return;
            FileStream fs = new FileStream("E:\\images\\" + filename + picNum + ".jpg", FileMode.OpenOrCreate, FileAccess.Write);

            fs.Write(picBytes, 0, picBytes.Length);
            fs.Dispose();
            fs.Close();

        }
        */

        /// <summary>
        /// 從Server端接收資料
        /// </summary>
        /// <param name="byteRecv">資料緩衝區</param>
        public int Receive(byte[] byteRecv)
        {
            int nRecvNum = 0;
            //判斷socket 是否還是連線狀態
            if (m_sClient.Connected)
            {
                try
                {
                    // byte[] byteData = Encoding.ASCII.GetBytes(strSendData);
                    // sConnected.Receive(byteData);

                    //byte[] bytes = new byte[256];
                    nRecvNum = m_sClient.Receive(byteRecv);
 
                    // client.Send(Encoding.Unicode.GetBytes("Message from server at " + DateTime.Now.ToString()));
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }

                return nRecvNum;
            }
            else
                return -1;
        }


        /// <summary>
        /// 將位元組數組轉換為結構體
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns>object</returns>
        private object BytesToStruct(byte[] bytes, Type type)
        {
            //得到結構體大小
            int size = Marshal.SizeOf(type);
            Math.Log(size, 1);

            if (size > bytes.Length)
                return null;
            //分配結構大小的記憶體空間
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //將BYTE數組拷貝到分配好的記憶體空間
            Marshal.Copy(bytes, 0, structPtr, size);
            //將記憶體空間轉換為目標結構
            object obj = Marshal.PtrToStructure(structPtr, type);
            //釋放內容空間
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }

        /// <summary>
        /// 將int變數做Swap動作
        /// </summary>
        /// <param name="nValue">輸入int變數</param>
        /// <returns></returns>
        private int Swap(int nValue)
        {
            int nRet = 0;
            byte[] temp = BitConverter.GetBytes(nValue);
            Array.Reverse(temp);
            nRet = BitConverter.ToInt32(temp,0);
            return nRet;
        }

        /// <summary>
        /// 將double變數做Swap動作
        /// </summary>
        /// <param name="nValue">輸入double變數</param>
        /// <returns></returns>
        private double Swap(double nValue)
        {
            double nRet = 0;
            byte[] temp = BitConverter.GetBytes(nValue);
            Array.Reverse(temp);
            nRet = BitConverter.ToDouble(temp,0);
            return nRet;
        }

        /// <summary>
        /// 將UInt64變數做Swap動作
        /// </summary>
        /// <param name="nValue">輸入UInt64變數</param>
        /// <returns></returns>
        private UInt64 Swap(UInt64 nValue)
        {
            UInt64 nRet = 0;
            byte[] temp = BitConverter.GetBytes(nValue);
            Array.Reverse(temp);
            nRet = BitConverter.ToUInt64(temp, 0);
            return nRet;
        }

        /// <summary>
        /// 將所有RealTimeData的變數Swap動作
        /// </summary>
        /// <returns></returns>
        private void SwapRealTimeData()
        {
            m_realTimeData.PackageLength = Swap(m_realTimeData.PackageLength);
            m_realTimeData.Time = Swap(m_realTimeData.Time);

            m_realTimeData.Base_q_target = Swap(m_realTimeData.Base_q_target);
            m_realTimeData.Shoulder_q_target = Swap(m_realTimeData.Shoulder_q_target);
            m_realTimeData.Elbow_q_target = Swap(m_realTimeData.Elbow_q_target);
            m_realTimeData.Wrist1_q_target = Swap(m_realTimeData.Wrist1_q_target);
            m_realTimeData.Wrist2_q_target = Swap(m_realTimeData.Wrist2_q_target);
            m_realTimeData.Wrist3_q_target = Swap(m_realTimeData.Wrist3_q_target);

            m_realTimeData.Base_qd_target = Swap(m_realTimeData.Base_qd_target);
            m_realTimeData.Shoulder_qd_target = Swap(m_realTimeData.Shoulder_qd_target);
            m_realTimeData.Elbow_qd_target = Swap(m_realTimeData.Elbow_qd_target);
            m_realTimeData.Wrist1_qd_target = Swap(m_realTimeData.Wrist1_qd_target);
            m_realTimeData.Wrist2_qd_target = Swap(m_realTimeData.Wrist2_qd_target);
            m_realTimeData.Wrist3_qd_target = Swap(m_realTimeData.Wrist3_qd_target);

            m_realTimeData.Base_qdd_target = Swap(m_realTimeData.Base_qdd_target);
            m_realTimeData.Shoulder_qdd_target = Swap(m_realTimeData.Shoulder_qdd_target);
            m_realTimeData.Elbow_qdd_target = Swap(m_realTimeData.Elbow_qdd_target);
            m_realTimeData.Wrist1_qdd_target = Swap(m_realTimeData.Wrist1_qdd_target);
            m_realTimeData.Wrist2_qdd_target = Swap(m_realTimeData.Wrist2_qdd_target);
            m_realTimeData.Wrist3_qdd_target = Swap(m_realTimeData.Wrist3_qdd_target);

            m_realTimeData.Base_I_target = Swap(m_realTimeData.Base_I_target);
            m_realTimeData.Shoulder_I_target = Swap(m_realTimeData.Shoulder_I_target);
            m_realTimeData.Elbow_I_target = Swap(m_realTimeData.Elbow_I_target);
            m_realTimeData.Wrist1_I_target = Swap(m_realTimeData.Wrist1_I_target);
            m_realTimeData.Wrist2_I_target = Swap(m_realTimeData.Wrist2_I_target);
            m_realTimeData.Wrist3_I_target = Swap(m_realTimeData.Wrist3_I_target);

            m_realTimeData.Base_M_target = Swap(m_realTimeData.Base_M_target);
            m_realTimeData.Shoulder_M_target = Swap(m_realTimeData.Shoulder_M_target);
            m_realTimeData.Elbow_M_target = Swap(m_realTimeData.Elbow_M_target);
            m_realTimeData.Wrist1_M_target = Swap(m_realTimeData.Wrist1_M_target);
            m_realTimeData.Wrist2_M_target = Swap(m_realTimeData.Wrist2_M_target);
            m_realTimeData.Wrist3_M_target = Swap(m_realTimeData.Wrist3_M_target);

            m_realTimeData.Base_q_Actual = Swap(m_realTimeData.Base_q_Actual);
            m_realTimeData.Shoulder_q_Actual = Swap(m_realTimeData.Shoulder_q_Actual);
            m_realTimeData.Elbow_q_Actual = Swap(m_realTimeData.Elbow_q_Actual);
            m_realTimeData.Wrist1_q_Actual = Swap(m_realTimeData.Wrist1_q_Actual);
            m_realTimeData.Wrist2_q_Actual = Swap(m_realTimeData.Wrist2_q_Actual);
            m_realTimeData.Wrist3_q_Actual = Swap(m_realTimeData.Wrist3_q_Actual);

            m_realTimeData.Base_qd_Actual = Swap(m_realTimeData.Base_qd_Actual);
            m_realTimeData.Shoulder_qd_Actual = Swap(m_realTimeData.Shoulder_qd_Actual);
            m_realTimeData.Elbow_qd_Actual = Swap(m_realTimeData.Elbow_qd_Actual);
            m_realTimeData.Wrist1_qd_Actual = Swap(m_realTimeData.Wrist1_qd_Actual);
            m_realTimeData.Wrist2_qd_Actual = Swap(m_realTimeData.Wrist2_qd_Actual);
            m_realTimeData.Wrist3_qd_Actual = Swap(m_realTimeData.Wrist3_qd_Actual);

            m_realTimeData.Base_I_Actual = Swap(m_realTimeData.Base_I_Actual);
            m_realTimeData.Shoulder_I_Actual = Swap(m_realTimeData.Shoulder_I_Actual);
            m_realTimeData.Elbow_I_Actual = Swap(m_realTimeData.Elbow_I_Actual);
            m_realTimeData.Wrist1_I_Actual = Swap(m_realTimeData.Wrist1_I_Actual);
            m_realTimeData.Wrist2_I_Actual = Swap(m_realTimeData.Wrist2_I_Actual);
            m_realTimeData.Wrist3_I_Actual = Swap(m_realTimeData.Wrist3_I_Actual);

            m_realTimeData.Base_I_Control = Swap(m_realTimeData.Base_I_Control);
            m_realTimeData.Shoulder_I_Control = Swap(m_realTimeData.Shoulder_I_Control);
            m_realTimeData.Elbow_I_Control = Swap(m_realTimeData.Elbow_I_Control);
            m_realTimeData.Wrist1_I_Control = Swap(m_realTimeData.Wrist1_I_Control);
            m_realTimeData.Wrist2_I_Control = Swap(m_realTimeData.Wrist2_I_Control);
            m_realTimeData.Wrist3_I_Control = Swap(m_realTimeData.Wrist3_I_Control);

            m_realTimeData.ToolVectorActual_X = Swap(m_realTimeData.ToolVectorActual_X);
            m_realTimeData.ToolVectorActual_Y = Swap(m_realTimeData.ToolVectorActual_Y);
            m_realTimeData.ToolVectorActual_Z = Swap(m_realTimeData.ToolVectorActual_Z);
            m_realTimeData.ToolVectorActual_Rx = Swap(m_realTimeData.ToolVectorActual_Rx);
            m_realTimeData.ToolVectorActual_Ry = Swap(m_realTimeData.ToolVectorActual_Ry);
            m_realTimeData.ToolVectorActual_Rz = Swap(m_realTimeData.ToolVectorActual_Rz);

            m_realTimeData.TCP_Speed_Actual_X = Swap(m_realTimeData.TCP_Speed_Actual_X);
            m_realTimeData.TCP_Speed_Actual_Y = Swap(m_realTimeData.TCP_Speed_Actual_Y);
            m_realTimeData.TCP_Speed_Actual_Z = Swap(m_realTimeData.TCP_Speed_Actual_Z);
            m_realTimeData.TCP_Speed_Actual_Rx = Swap(m_realTimeData.TCP_Speed_Actual_Rx);
            m_realTimeData.TCP_Speed_Actual_Ry = Swap(m_realTimeData.TCP_Speed_Actual_Ry);
            m_realTimeData.TCP_Speed_Actual_Rz = Swap(m_realTimeData.TCP_Speed_Actual_Rz);

            m_realTimeData.TCP_Force_X = Swap(m_realTimeData.TCP_Force_X);
            m_realTimeData.TCP_Force_Y = Swap(m_realTimeData.TCP_Force_Y);
            m_realTimeData.TCP_Force_Z = Swap(m_realTimeData.TCP_Force_Z);
            m_realTimeData.TCP_Force_Rx = Swap(m_realTimeData.TCP_Force_Rx);
            m_realTimeData.TCP_Force_Ry = Swap(m_realTimeData.TCP_Force_Ry);
            m_realTimeData.TCP_Force_Rz = Swap(m_realTimeData.TCP_Force_Rz);

            m_realTimeData.ToolVectorTarget_X = Swap(m_realTimeData.ToolVectorTarget_X);
            m_realTimeData.ToolVectorTarget_Y = Swap(m_realTimeData.ToolVectorTarget_Y);
            m_realTimeData.ToolVectorTarget_Z = Swap(m_realTimeData.ToolVectorTarget_Z);
            m_realTimeData.ToolVectorTarget_Rx = Swap(m_realTimeData.ToolVectorTarget_Rx);
            m_realTimeData.ToolVectorTarget_Ry = Swap(m_realTimeData.ToolVectorTarget_Ry);
            m_realTimeData.ToolVectorTarget_Rz = Swap(m_realTimeData.ToolVectorTarget_Rz);

            m_realTimeData.TCP_Speed_Target_X = Swap(m_realTimeData.TCP_Speed_Target_X);
            m_realTimeData.TCP_Speed_Target_Y = Swap(m_realTimeData.TCP_Speed_Target_Y);
            m_realTimeData.TCP_Speed_Target_Z = Swap(m_realTimeData.TCP_Speed_Target_Z);
            m_realTimeData.TCP_Speed_Target_Rx = Swap(m_realTimeData.TCP_Speed_Target_Rx);
            m_realTimeData.TCP_Speed_Target_Ry = Swap(m_realTimeData.TCP_Speed_Target_Ry);
            m_realTimeData.TCP_Speed_Target_Rz = Swap(m_realTimeData.TCP_Speed_Target_Rz);

            m_realTimeData.DigitalInputBits = Swap(m_realTimeData.DigitalInputBits);

            m_realTimeData.Base_MotorTemperatures = Swap(m_realTimeData.Base_MotorTemperatures);
            m_realTimeData.Shoulder_MotorTemperatures = Swap(m_realTimeData.Shoulder_MotorTemperatures);
            m_realTimeData.Elbow_MotorTemperatures = Swap(m_realTimeData.Elbow_MotorTemperatures);
            m_realTimeData.Wrist1_MotorTemperatures = Swap(m_realTimeData.Wrist1_MotorTemperatures);
            m_realTimeData.Wrist2_MotorTemperatures = Swap(m_realTimeData.Wrist2_MotorTemperatures);
            m_realTimeData.Wrist3_MotorTemperatures = Swap(m_realTimeData.Wrist3_MotorTemperatures);

            m_realTimeData.ControllerTimer = Swap(m_realTimeData.ControllerTimer);
            m_realTimeData.TestValue = Swap(m_realTimeData.TestValue);
            m_realTimeData.RobotMode = Swap(m_realTimeData.RobotMode);

            m_realTimeData.Base_JointModes = Swap(m_realTimeData.Base_JointModes);
            m_realTimeData.Shoulder_JointModes = Swap(m_realTimeData.Shoulder_JointModes);
            m_realTimeData.Elbow_JointModes = Swap(m_realTimeData.Elbow_JointModes);
            m_realTimeData.Wrist1_JointModes = Swap(m_realTimeData.Wrist1_JointModes);
            m_realTimeData.Wrist2_JointModes = Swap(m_realTimeData.Wrist2_JointModes);
            m_realTimeData.Wrist3_JointModes = Swap(m_realTimeData.Wrist3_JointModes);

            m_realTimeData.SafetyMode = Swap(m_realTimeData.SafetyMode);
            m_realTimeData.SofewareReserve1 = Swap(m_realTimeData.SofewareReserve1);
            m_realTimeData.SofewareReserve2 = Swap(m_realTimeData.SofewareReserve2);
            m_realTimeData.SofewareReserve3 = Swap(m_realTimeData.SofewareReserve3);
            m_realTimeData.SofewareReserve4 = Swap(m_realTimeData.SofewareReserve4);
            m_realTimeData.SofewareReserve5 = Swap(m_realTimeData.SofewareReserve5);
            m_realTimeData.SofewareReserve6 = Swap(m_realTimeData.SofewareReserve6);
            m_realTimeData.ToolAccelerometerX = Swap(m_realTimeData.ToolAccelerometerX);
            m_realTimeData.ToolAccelerometerY = Swap(m_realTimeData.ToolAccelerometerY);
            m_realTimeData.ToolAccelerometerZ = Swap(m_realTimeData.ToolAccelerometerZ);
            m_realTimeData.SofewareReserve7 = Swap(m_realTimeData.SofewareReserve7);
            m_realTimeData.SofewareReserve8 = Swap(m_realTimeData.SofewareReserve8);
            m_realTimeData.SofewareReserve9 = Swap(m_realTimeData.SofewareReserve9);
            m_realTimeData.SofewareReserve10 = Swap(m_realTimeData.SofewareReserve10);
            m_realTimeData.SofewareReserve11 = Swap(m_realTimeData.SofewareReserve11);
            m_realTimeData.SofewareReserve12 = Swap(m_realTimeData.SofewareReserve12);
            m_realTimeData.SpeedScaling = Swap(m_realTimeData.SpeedScaling);
            m_realTimeData.LinearMomentumNorm = Swap(m_realTimeData.LinearMomentumNorm);
            m_realTimeData.SofewareReserve13 = Swap(m_realTimeData.SofewareReserve13);
            m_realTimeData.SofewareReserve14 = Swap(m_realTimeData.SofewareReserve14);
            m_realTimeData.VMain = Swap(m_realTimeData.VMain);
            m_realTimeData.VRobot = Swap(m_realTimeData.VRobot);
            m_realTimeData.IRobot = Swap(m_realTimeData.IRobot);

            m_realTimeData.Base_V_Actual = Swap(m_realTimeData.Base_V_Actual);
            m_realTimeData.Shoulder_V_Actual = Swap(m_realTimeData.Shoulder_V_Actual);
            m_realTimeData.Elbow_V_Actual = Swap(m_realTimeData.Elbow_V_Actual);
            m_realTimeData.Wrist1_V_Actual = Swap(m_realTimeData.Wrist1_V_Actual);
            m_realTimeData.Wrist2_V_Actual = Swap(m_realTimeData.Wrist2_V_Actual);
            m_realTimeData.Wrist3_V_Actual = Swap(m_realTimeData.Wrist3_V_Actual);

            m_realTimeData.DigitalOutputs = Swap(m_realTimeData.DigitalOutputs);
            m_realTimeData.ProgramState = Swap(m_realTimeData.ProgramState);


            //解讀各關節位置(角度)
            m_jointDegreeInfo.dbBasePosDegree = m_realTimeData.Base_q_Actual / 3.1415926 * 180.0;
            m_jointDegreeInfo.dbShoulderPosDegree = m_realTimeData.Shoulder_q_Actual / 3.1415926 * 180.0;
            m_jointDegreeInfo.dbElbowPosDegree = m_realTimeData.Elbow_q_Actual / 3.1415926 * 180.0;
            m_jointDegreeInfo.dbWrist1PosDegree = m_realTimeData.Wrist1_q_Actual / 3.1415926 * 180.0;
            m_jointDegreeInfo.dbWrist2PosDegree = m_realTimeData.Wrist2_q_Actual / 3.1415926 * 180.0;
            m_jointDegreeInfo.dbWrist3PosDegree = m_realTimeData.Wrist3_q_Actual / 3.1415926 * 180.0;

            //解讀各關節位置(徑度)
            m_jointRadianInfo.dbBasePosRadian = m_realTimeData.Base_q_Actual;
            m_jointRadianInfo.dbShoulderPosRadian = m_realTimeData.Shoulder_q_Actual;
            m_jointRadianInfo.dbElbowPosRadian = m_realTimeData.Elbow_q_Actual;
            m_jointRadianInfo.dbWrist1PosRadian = m_realTimeData.Wrist1_q_Actual;
            m_jointRadianInfo.dbWrist2PosRadian = m_realTimeData.Wrist2_q_Actual;
            m_jointRadianInfo.dbWrist3PosRadian = m_realTimeData.Wrist3_q_Actual;

            //解讀TCP位置
            m_tcpPosInfo.dbTCP_X = m_realTimeData.ToolVectorActual_X*1000;  //轉為 mm單位
            m_tcpPosInfo.dbTCP_Y = m_realTimeData.ToolVectorActual_Y*1000;  //轉為 mm單位
            m_tcpPosInfo.dbTCP_Z = m_realTimeData.ToolVectorActual_Z*1000;  //轉為 mm單位
            m_tcpPosInfo.dbTCP_Rx = m_realTimeData.ToolVectorActual_Rx;
            m_tcpPosInfo.dbTCP_Ry = m_realTimeData.ToolVectorActual_Ry;
            m_tcpPosInfo.dbTCP_Rz = m_realTimeData.ToolVectorActual_Rz;
            /*
            m_tcpPosInfo.dbTCP_X = m_realTimeData.ToolVectorTarget_X * 1000;  //轉為 mm單位
            m_tcpPosInfo.dbTCP_Y = m_realTimeData.ToolVectorTarget_Y * 1000;  //轉為 mm單位
            m_tcpPosInfo.dbTCP_Z = m_realTimeData.ToolVectorTarget_Z * 1000;  //轉為 mm單位
            m_tcpPosInfo.dbTCP_Rx = m_realTimeData.ToolVectorTarget_Rx;
            m_tcpPosInfo.dbTCP_Ry = m_realTimeData.ToolVectorTarget_Ry;
            m_tcpPosInfo.dbTCP_Rz = m_realTimeData.ToolVectorTarget_Rz;
            */
          
            //解讀各關節電流
            m_jointCurrentInfo.dbBase = m_realTimeData.Base_I_Actual;
            m_jointCurrentInfo.dbShoulder = m_realTimeData.Shoulder_I_Actual;
            m_jointCurrentInfo.dbElbow = m_realTimeData.Elbow_I_Actual;
            m_jointCurrentInfo.dbWrist1 = m_realTimeData.Wrist1_I_Actual;
            m_jointCurrentInfo.dbWrist2 = m_realTimeData.Wrist2_I_Actual;
            m_jointCurrentInfo.dbWrist3 = m_realTimeData.Wrist3_I_Actual;

            //解讀各關節電壓
            m_jointVoltageInfo.dbBase = m_realTimeData.Base_V_Actual;
            m_jointVoltageInfo.dbShoulder = m_realTimeData.Shoulder_V_Actual;
            m_jointVoltageInfo.dbElbow = m_realTimeData.Elbow_V_Actual;
            m_jointVoltageInfo.dbWrist1 = m_realTimeData.Wrist1_V_Actual;
            m_jointVoltageInfo.dbWrist2 = m_realTimeData.Wrist2_V_Actual;
            m_jointVoltageInfo.dbWrist3 = m_realTimeData.Wrist3_V_Actual;

            //解讀各關節扭矩
            m_jointTorqueInfo.dbBase = m_realTimeData.Base_M_target;
            m_jointTorqueInfo.dbShoulder = m_realTimeData.Shoulder_M_target;
            m_jointTorqueInfo.dbElbow = m_realTimeData.Elbow_M_target;
            m_jointTorqueInfo.dbWrist1 = m_realTimeData.Wrist1_M_target;
            m_jointTorqueInfo.dbWrist2 = m_realTimeData.Wrist2_M_target;
            m_jointTorqueInfo.dbWrist3 = m_realTimeData.Wrist3_M_target;

            //解讀各關節溫度
            m_jointTemperatureInfo.dbBase = m_realTimeData.Base_MotorTemperatures;
            m_jointTemperatureInfo.dbShoulder = m_realTimeData.Shoulder_MotorTemperatures;
            m_jointTemperatureInfo.dbElbow = m_realTimeData.Elbow_MotorTemperatures;
            m_jointTemperatureInfo.dbWrist1 = m_realTimeData.Wrist1_MotorTemperatures;
            m_jointTemperatureInfo.dbWrist2 = m_realTimeData.Wrist2_MotorTemperatures;
            m_jointTemperatureInfo.dbWrist3 = m_realTimeData.Wrist3_MotorTemperatures;

            //解讀各關節速度
            m_jointSpeedInfo.dbBase = m_realTimeData.Base_qd_target;
            m_jointSpeedInfo.dbShoulder = m_realTimeData.Shoulder_qd_target;
            m_jointSpeedInfo.dbElbow = m_realTimeData.Elbow_qd_target;
            m_jointSpeedInfo.dbWrist1 = m_realTimeData.Wrist1_qd_target;
            m_jointSpeedInfo.dbWrist2 = m_realTimeData.Wrist2_qd_target;
            m_jointSpeedInfo.dbWrist3 = m_realTimeData.Wrist3_qd_target;

            //解讀各關節加速度
            m_jointAccInfo.dbBase = m_realTimeData.Base_qdd_target;
            m_jointAccInfo.dbShoulder = m_realTimeData.Shoulder_qdd_target;
            m_jointAccInfo.dbElbow = m_realTimeData.Elbow_qdd_target;
            m_jointAccInfo.dbWrist1 = m_realTimeData.Wrist1_qdd_target;
            m_jointAccInfo.dbWrist2 = m_realTimeData.Wrist2_qdd_target;
            m_jointAccInfo.dbWrist3 = m_realTimeData.Wrist3_qdd_target;

        }
        

    }
}
