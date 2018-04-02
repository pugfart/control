using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Toshiba6Robot
{
    /*
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
    */

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
