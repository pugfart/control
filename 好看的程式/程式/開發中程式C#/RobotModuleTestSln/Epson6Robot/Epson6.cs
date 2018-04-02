using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using IRobotMotion;

namespace Epson6Robot
{
    //關節角度值
    public struct JointPosInfo
    {
        public double dbJ1;
        public double dbJ2;
        public double dbJ3;
        public double dbJ4;
        public double dbJ5;
        public double dbJ6;
    }

    //世界座標值
    public struct WorldPosInfo
    {
        public double dbWorldX;
        public double dbWorldY;
        public double dbWorldZ;
        public double dbWorldRx;
        public double dbWorldRy;
        public double dbWorldRz;
    }

    //關節Torque值
    public struct JointTorqueInfo
    {
        public double dbJ1;
        public double dbJ2;
        public double dbJ3;
        public double dbJ4;
        public double dbJ5;
        public double dbJ6;
    }



    public class Epson6 : IRobotMotion.IRobotMotion
    {
        private Object thisLock = new Object();

        EpsonSocketClient EpsonClientGetInfo;
        EpsonSocketClient EpsonClientSendCmd;

        public const int JOINT_DEGREE = 1;
        public const int GLOBAL_POS = 2;
        public const int JOINT_TORQUE = 3;

        public int m_nId = 0;
        public string m_strGetInfoIP = "";
        public int m_nGetInfoPort = 0;
        public string m_strSendCmdIP = "";
        public int m_nSendCmdPort = 0;

      //  public double m_dbMaxAccel = 0;
     //   public double m_dbMinAccel = 0;
      //  public double m_dbMaxAccelS = 0;
     //   public double m_dbMinAccelS = 0;

      

        public JointPosInfo m_CurrJointPos;
        public WorldPosInfo m_CurrWorldPos;
        public JointTorqueInfo m_CurrJointTorque;

        public byte[] m_byteSendOK = { 0x53, 0x65, 0x6E, 0x64, 0x5F, 0x4F, 0x4B, 0x0D, 0x0A };    // Send_OK
        public byte[] m_byteStopDone = { 0x41, 0x43, 0x54, 0x5F, 0x53, 0x54, 0x4F, 0x50, 0x5F, 0x44, 0x4F,0x4E, 0x45, 0x0D, 0x0A };//ACT_STOP_DONE

        public byte[] m_byteWaitDone = { 0x4D, 0x4F, 0x56, 0x45, 0x5F, 0x57, 0x41, 0x49, 0x54, 0x5F, 0x44, 0x4F, 0x4E,0x45, 0x0D, 0x0A };//MOVE_WAIT_DONE

        //這是傳給手臂端的字串, 不用 0x0D 0x0A
        public byte[] m_byteRecvOK = { 0x52, 0x65, 0x63, 0x76, 0x5F, 0x4F, 0x4B };    // Recv_OK

        //動作代號
        const int P_TO_P = 6001;
        const int LINE = 6002;

       


        public virtual string Name { get { return "Epson6"; } }


        public void GetStatusThread()
        {
            byte[] byteRecv = new byte[256];
            byte[] byteReqPosCmd = { 0x52, 0x45, 0x51, 0x5F, 0x50, 0x4F, 0x53 }; //REQ_POS
            int nRet = 0;

            while (EpsonClientGetInfo.IsConnected())
            {
                System.Threading.Thread.Sleep(30);//每個執行緒內部的閉環裡面都要加個「短時間」睡眠，使得執行緒佔用資源得到及時釋放

                lock (thisLock)
                {
                    EpsonClientGetInfo.SendData(byteReqPosCmd);
                    while (true)
                    {
                        nRet = EpsonClientGetInfo.Receive(ref byteRecv);
                        if (nRet == 0)
                        {
                            string strRecv = Encoding.ASCII.GetString(byteRecv);

                            int nIndex = strRecv.IndexOf(" ");
                            if (nIndex != 0 && nIndex != -1)
                            {
                                //parsing Joint Degree
                                nIndex = strRecv.IndexOf("JointDeg");
                                if (nIndex != -1)
                                {
                                    ParsingJointValue(ref strRecv);
                                    EpsonClientGetInfo.SendData(m_byteRecvOK); //再回傳給 控制器 Recv_OK, 即完成交握
                                }

                                //parsing Global Pos
                                nIndex = strRecv.IndexOf("GlobalPos");
                                if (nIndex != -1)
                                {
                                    ParsingWorldValue(ref strRecv);
                                    EpsonClientGetInfo.SendData(m_byteRecvOK); //再回傳給 控制器 Recv_OK, 即完成交握
                                }

                                //parsing Joint torque
                                nIndex = strRecv.IndexOf("Torque");
                                if (nIndex != -1)
                                {
                                    ParsingTorqueValue(ref strRecv);
                                    EpsonClientGetInfo.SendData(m_byteRecvOK); //再回傳給 控制器 Recv_OK, 即完成交握
                                    break;
                                }

                            }

                        }
                    }

                }
            }

        }

        private int ParsingJointValue(ref string strData)
        {
            int nIndex = 0;
            string strSub = "";

            nIndex = strData.IndexOf(" ");
            strData = strData.Remove(0, nIndex + 1); //先刪除命令
            for (int i = 0; i < 6; i++)
            {
                nIndex = strData.IndexOf(" ");
                strSub = strData.Substring(0, nIndex);
                strData = strData.Remove(0, nIndex + 1);
                if (i == 0)
                    m_CurrJointPos.dbJ1 = Convert.ToDouble(strSub);
                else if (i == 1)
                    m_CurrJointPos.dbJ2 = Convert.ToDouble(strSub);
                else if (i == 2)
                    m_CurrJointPos.dbJ3 = Convert.ToDouble(strSub);
                else if (i == 3)
                    m_CurrJointPos.dbJ4 = Convert.ToDouble(strSub);
                else if (i == 4)
                    m_CurrJointPos.dbJ5 = Convert.ToDouble(strSub);
                else if (i == 5)
                    m_CurrJointPos.dbJ6 = Convert.ToDouble(strSub);
            }

            return 0;
        }

        private int ParsingWorldValue(ref string strData)
        {
            int nIndex = 0;
            string strSub = "";

            nIndex = strData.IndexOf(" ");
            strData = strData.Remove(0, nIndex + 1); //先刪除命令
            for (int i = 0; i < 6; i++)
            {
                nIndex = strData.IndexOf(" ");
                strSub = strData.Substring(0, nIndex);
                strData = strData.Remove(0, nIndex + 1);
                if (i == 0)
                    m_CurrWorldPos.dbWorldX = Convert.ToDouble(strSub);
                else if (i == 1)
                    m_CurrWorldPos.dbWorldY = Convert.ToDouble(strSub);
                else if (i == 2)
                    m_CurrWorldPos.dbWorldZ = Convert.ToDouble(strSub);
                else if (i == 3)
                    m_CurrWorldPos.dbWorldRz = Convert.ToDouble(strSub);
                else if (i == 4)
                    m_CurrWorldPos.dbWorldRy = Convert.ToDouble(strSub);
                else if (i == 5)
                    m_CurrWorldPos.dbWorldRx = Convert.ToDouble(strSub);
            }

            return 0;
        }

        private int ParsingTorqueValue(ref string strData)
        {
            int nIndex = 0;
            string strSub = "";

            nIndex = strData.IndexOf(" ");
            strData = strData.Remove(0, nIndex + 1); //先刪除命令
            for (int i = 0; i < 6; i++)
            {
                nIndex = strData.IndexOf(" ");
                strSub = strData.Substring(0, nIndex);
                strData = strData.Remove(0, nIndex + 1);
                if (i == 0)
                    m_CurrJointTorque.dbJ1 = Convert.ToDouble(strSub);
                else if (i == 1)
                    m_CurrJointTorque.dbJ2 = Convert.ToDouble(strSub);
                else if (i == 2)
                    m_CurrJointTorque.dbJ3 = Convert.ToDouble(strSub);
                else if (i == 3)
                    m_CurrJointTorque.dbJ4 = Convert.ToDouble(strSub);
                else if (i == 4)
                    m_CurrJointTorque.dbJ5 = Convert.ToDouble(strSub);
                else if (i == 5)
                    m_CurrJointTorque.dbJ6 = Convert.ToDouble(strSub);
            }

            return 0;
        }



        //public virtual bool Connect(string strRobotIP, int nPCListenPort)
        public virtual int Connect()
        {
            int nRet = 0;

            //Get Info Client Socket
            EpsonClientGetInfo = new EpsonSocketClient();
            nRet = EpsonClientGetInfo.ConnectRobot(m_strGetInfoIP, m_nGetInfoPort);
            if (nRet < 0)   //連線失敗
                return -10;     //IP0 socket 連線失敗

            //Send Cmd Client Socket
            EpsonClientSendCmd = new EpsonSocketClient();
            nRet = EpsonClientSendCmd.ConnectRobot(m_strSendCmdIP, m_nSendCmdPort);
            if (nRet < 0)   //連線失敗
                return -10;     //IP0 socket 連線失敗


            //開始執行 截取狀態執行緒
            Thread tGetStatusThread = new Thread(new ThreadStart(GetStatusThread));//執行緒開始的時候要調用的方法為threadProc.thread
            tGetStatusThread.IsBackground = true;//設置IsBackground=true,後臺執行緒會自動根據主執行緒的銷毀而銷毀
            tGetStatusThread.Start();

            return 0;
        }

        public virtual int Disconnect()
        {
            lock (thisLock)
            {
                //1.斷線 Get Info Client Socket
                if (EpsonClientGetInfo.IsConnected())
                {
                    //先通知手臂程式,要斷線
                    // char szCmd[64] = { 0 };
                    // sprintf_s(szCmd, 64, "%s", "DISCONNECT");
                    // bRet = m_TcpGetInfoClient.SendData(szCmd, 64);


                    //byte[] byteCmd = { 0x44, 0x49, 0x53, 0x43, 0x4F, 0x4E, 0x4E, 0x45, 0x43, 0x54 }; //DISCONNECT

                    string strCmd = string.Format("DISCONNECT");
                    byte[] byteCmd = Encoding.ASCII.GetBytes(strCmd);
                    byte[] byteRecv = new byte[9];
                    int nRet = 0;

                    EpsonClientGetInfo.SendData(byteCmd);
                    nRet = EpsonClientGetInfo.Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        //判斷 控制器是否 收到命令
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteSendOK))
                            return -100;
                    }
                    else
                        return -10; //通訊發生錯誤  


                    //斷線 Socket
                    nRet = EpsonClientGetInfo.DisconnectRobot();
                    if (nRet < 0)
                        return -12;     //IP0 socket 斷線失敗  
                }

                //2.斷線 Send Cmd Client Socket
                if (EpsonClientSendCmd.IsConnected())
                {
                    //先通知手臂程式,要斷線
                    // char szCmd[64] = { 0 };
                    // sprintf_s(szCmd, 64, "%s", "DISCONNECT");
                    // bRet = m_TcpGetInfoClient.SendData(szCmd, 64);

                    string strCmd = string.Format("DISCONNECT");
                    byte[] byteCmd = Encoding.ASCII.GetBytes(strCmd);
                    byte[] byteRecv = new byte[9];
                    int nRet = 0;

                    EpsonClientSendCmd.SendData(byteCmd);
                    nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteSendOK))
                            return -100;
                    }
                    else
                        return -10; //通訊發生錯誤  


                    //斷線 Socket
                    nRet = EpsonClientSendCmd.DisconnectRobot();
                    if (nRet < 0)
                        return -12;     //IP0 socket 斷線失敗  
                }

            }
            return 0;
        }

        //擷取 6關節角度(有支援)
        public virtual int GetJointDegree(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            dbJ1 = m_CurrJointPos.dbJ1;
            dbJ2 = m_CurrJointPos.dbJ2;
            dbJ3 = m_CurrJointPos.dbJ3;
            dbJ4 = m_CurrJointPos.dbJ4;
            dbJ5 = m_CurrJointPos.dbJ5;
            dbJ6 = m_CurrJointPos.dbJ6;
            return 0;

        }

        //擷取 6關節徑度(無支援)
        public virtual int GetJointRadian(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {

            return 0;
        }

        //擷取 世界座標位置(有支援)
        public virtual int GetWorldPos(ref double dbX, ref double dbY, ref double dbZ, ref double dbRx, ref double dbRy, ref double dbRz)
        {
            dbX = m_CurrWorldPos.dbWorldX;
            dbY = m_CurrWorldPos.dbWorldY;
            dbZ = m_CurrWorldPos.dbWorldZ;
            dbRx = m_CurrWorldPos.dbWorldRx;
            dbRy = m_CurrWorldPos.dbWorldRy;
            dbRz = m_CurrWorldPos.dbWorldRz;
            return 0;
        }

        //擷取 6關節電流(無支援)
        public virtual int GetJointCurrentValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {

            return 0;
        }

        //擷取 6關節電壓(無支援)
        public virtual int GetJointVoltageValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {

            return 0;
        }

        //擷取 6關節扭矩(有支援)
        public virtual int GetJointTorqueValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            dbJ1 = m_CurrJointTorque.dbJ1;
            dbJ2 = m_CurrJointTorque.dbJ2;
            dbJ3 = m_CurrJointTorque.dbJ3;
            dbJ4 = m_CurrJointTorque.dbJ4;
            dbJ5 = m_CurrJointTorque.dbJ5;
            dbJ6 = m_CurrJointTorque.dbJ6;
            return 0;
        }

        //擷取 6關節溫度(無支援)
        public virtual int GetJointTemperatureValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {

            return 1;
        }

        //擷取 6關節速度(無支援)
        public virtual int GetJointSpeedValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {

            return 0;
        }

        //擷取 6關節加速度(無支援)
        public virtual int GetJointAccValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {

            return 0;
        }



        //關節Jog 移動
        public virtual int JogJoint(Joints nJoint, Jog_Directions nDir, Jog_Speed nSpeed)
        {
            int nRet = 0;

            double dbValue = 0;
            lock (thisLock)
            {
                if (nSpeed == Jog_Speed.High)
                    nRet = SetAllSpeed(P_TO_P, 30, 30, 30);
                else if (nSpeed == Jog_Speed.Mid)
                    nRet = SetAllSpeed(P_TO_P, 20, 20, 20);
                else if (nSpeed == Jog_Speed.Low)
                    nRet = SetAllSpeed(P_TO_P, 10, 10, 10);

                if (nDir == Jog_Directions.Plus)
                {
                    if (nJoint == Joints.J1) dbValue = 170;
                    else if (nJoint == Joints.J2) dbValue = 65;
                    else if (nJoint == Joints.J3) dbValue = 200;
                    else if (nJoint == Joints.J4) dbValue = 150;
                    else if (nJoint == Joints.J5) dbValue = 120;
                    else if (nJoint == Joints.J6) dbValue = 350;
                }
                else
                {
                    if (nJoint == Joints.J1) dbValue = -170;
                    else if (nJoint == Joints.J2) dbValue = -90;
                    else if (nJoint == Joints.J3) dbValue = -40;
                    else if (nJoint == Joints.J4) dbValue = -150;
                    else if (nJoint == Joints.J5) dbValue = -130;
                    else if (nJoint == Joints.J6) dbValue = -350;
                }

                nRet = MoveJoint(nJoint, MovementsTypes.Abs, SpeedTypes.NoUse, WaitTypes.NoWait,
                                       dbValue, 0, 0, 0);
            }

            return 0;
        }

        //關節Jog 移動停止
        public virtual int JogJointStop(Joints nJoint)
        {
            int nRet = 0;
            lock (thisLock)
            {
                nRet = StopMotionForPtoP(0);
            }
            return nRet;
        }
       
        //座標Jog 移動
        public virtual int JogCoordnate(Coordnates nCoordnate, Jog_Directions nDir, Jog_Speed nSpeed)
        {
            int nRet = 0;

            double dbValue = 0;
            lock (thisLock)
            {
                /*
                if (nSpeed == Jog_Speed.High)
                    nRet = SetAllSpeed(LINE, 300, 2000, 2000);
                else if (nSpeed == Jog_Speed.Mid)
                    nRet = SetAllSpeed(LINE, 200, 1000, 1000);
                else if (nSpeed == Jog_Speed.Low)
                    nRet = SetAllSpeed(LINE, 100, 500, 500);
                    */
                if (nSpeed == Jog_Speed.High)
                    nRet = SetAllSpeed(LINE, 20, 20, 20);
                else if (nSpeed == Jog_Speed.Mid)
                    nRet = SetAllSpeed(LINE, 10, 10, 10);
                else if (nSpeed == Jog_Speed.Low)
                    nRet = SetAllSpeed(LINE, 5, 5, 5);

                if (nDir == Jog_Directions.Plus)
                {
                    if (nCoordnate == Coordnates.X) dbValue = 50;
                    else if (nCoordnate == Coordnates.Y) dbValue = 50;
                    else if (nCoordnate == Coordnates.Z) dbValue = 50;
                    else if (nCoordnate == Coordnates.Rx) dbValue = 50;
                    else if (nCoordnate == Coordnates.Ry) dbValue = 50;
                    else if (nCoordnate == Coordnates.Rz) dbValue = 50;
                }
                else
                {
                    if (nCoordnate == Coordnates.X) dbValue = -50;
                    else if (nCoordnate == Coordnates.Y) dbValue = -50;
                    else if (nCoordnate == Coordnates.Z) dbValue = -50;
                    else if (nCoordnate == Coordnates.Rx) dbValue = -50;
                    else if (nCoordnate == Coordnates.Ry) dbValue = -50;
                    else if (nCoordnate == Coordnates.Rz) dbValue = -50;
                }

                nRet = LineCoordnate(nCoordnate, MovementsTypes.Rel, SpeedTypes.NoUse, WaitTypes.NoWait,
                        dbValue, 0, 0, 0);

            }

            return 0; 
        }

        //座標Jog 移動停止
        public virtual int JogCoordnateStop(Coordnates nCoordate)
        {
            int nRet = 0;
            lock (thisLock)
            {
                nRet = StopMotionForPtoP(0);
            }
            return nRet;
        }



        //在直角坐標系上 點對點 同動 (非直線)
        public virtual int MovePtoP(SpeedTypes nSpeedType, WaitTypes nWaitType,
                    double dbXValue, double dbYValue, double dbZValue, double dbRxValue, double dbRyValue, double dbRzValue, double dbAcc, double dbDec, double dbSpeed)
        {
            int nErrorCode = 0;
            lock (thisLock)
            {
                try
                {
                    double dbX = 0, dbY = 0, dbZ = 0, dbRx = 0, dbRy = 0, dbRz = 0;
                    dbX = dbXValue;
                    dbY = dbYValue;
                    dbZ = dbZValue;
                    dbRx = dbRxValue;
                    dbRy = dbRyValue;
                    dbRz = dbRzValue;

                    string strCmdHead = "";
                    string strCmdPos = "";
                    string strCmdSpeed = "";
                    string strFinalCmd = "";

                    //送給控制器的位置順序為 X,Y,Z,Rz,Ry,Rx
                    strCmdPos = string.Format(",{0:0.###},{1:0.###},{2:0.###},{3:0.###},{4:0.###},{5:0.###}",
                        dbX, dbY, dbZ, dbRz, dbRy, dbRx);

                    strCmdHead = "PTP_WORLD_ALL_ABS";

                    if (nSpeedType == SpeedTypes.Use)
                    {
                        strCmdHead = strCmdHead + "_SPEED";
                        strCmdSpeed = string.Format(",{0:0.###},{1:0.###},{2:0.###}", dbSpeed, dbAcc, dbDec);
                    }

                    if (nWaitType == WaitTypes.Wait)
                        strCmdHead = strCmdHead + "_WAIT";

                    strFinalCmd = strCmdHead + strCmdPos + strCmdSpeed;

                    byte[] byteFinalCmd = Encoding.ASCII.GetBytes(strFinalCmd);
                    byte[] byteRecv = new byte[9];
                    int nRet = 0;

                    EpsonClientSendCmd.SendData(byteFinalCmd);
                    nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteSendOK))
                            return -100;
                    }
                    else
                        return -10; //通訊發生錯誤  
                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("MovePtoP 例外 : ErrCode = {0} , {1} !", ex.ToString());
                    return nErrorCode;
                }
            }

            //等待手臂停止
            if (nWaitType == WaitTypes.Wait)
            {
                string strChkCmd = string.Format("GET_IN_WAIT_FUNC");
                byte[] byteChkCmd = Encoding.ASCII.GetBytes(strChkCmd);
                byte[] byteRecv = new byte[3];
                int nRet = 0;

                while (true)
                {
                    System.Threading.Thread.Sleep(1);

                    lock (thisLock)
                    {
                        
                        EpsonClientSendCmd.SendData(byteChkCmd);
                        nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                        if (nRet == 0)
                        {
                            
                            if (byteRecv[0] == 0x31)   //手臂已經不在等待停止函式中了, 離開迴圈
                                break; 
                        }
                        else
                            return -10; //通訊發生錯誤  
                    }
                }
            }

            return nErrorCode;
        }

        //在直角坐標系上 點對點 直線運動
        public virtual int LinePtoP(SpeedTypes nSpeedType, WaitTypes nWaitType,
                double dbXValue, double dbYValue, double dbZValue, double dbRxValue, double dbRyValue, double dbRzValue, double dbAcc, double dbDec, double dbSpeed)
        {
            int nErrorCode = 0;
            lock (thisLock)
            {
                try
                {
                    double dbX = 0, dbY = 0, dbZ = 0, dbRx = 0, dbRy = 0, dbRz = 0;
                    dbX = dbXValue;
                    dbY = dbYValue;
                    dbZ = dbZValue;
                    dbRx = dbRxValue;
                    dbRy = dbRyValue;
                    dbRz = dbRzValue;
                    
                    string strCmdHead = "";
                    string strCmdPos = "";
                    string strCmdSpeed = "";
                    string strFinalCmd = "";

                    //送給控制器的位置順序為 X,Y,Z,Rz,Ry,Rx
                    strCmdPos = string.Format(",{0:0.###},{1:0.###},{2:0.###},{3:0.###},{4:0.###},{5:0.###}",
                        dbX, dbY, dbZ, dbRz, dbRy, dbRx);

                    strCmdHead = "LINE_WORLD_ALL_ABS";
             
                    if (nSpeedType == SpeedTypes.Use)
                    {
                        strCmdHead = strCmdHead + "_SPEED";
                        strCmdSpeed = string.Format(",{0:0.###},{1:0.###},{2:0.###}", dbSpeed, dbAcc, dbDec);
                    }

                    if (nWaitType == WaitTypes.Wait)
                        strCmdHead = strCmdHead + "_WAIT";

                    strFinalCmd = strCmdHead + strCmdPos + strCmdSpeed;

                    byte[] byteFinalCmd = Encoding.ASCII.GetBytes(strFinalCmd);
                    byte[] byteRecv = new byte[9];
                    int nRet = 0;

                    EpsonClientSendCmd.SendData(byteFinalCmd);
                    nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteSendOK))
                            return -100;          
                    }
                    else
                        return -10; //通訊發生錯誤  

                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("LinePtoP 例外 : ErrCode = {0} , {1} !", ex.ToString());
                    return nErrorCode;
                }
            }

            //等待手臂停止
            if (nWaitType == WaitTypes.Wait)
            {
                string strChkCmd = string.Format("GET_IN_WAIT_FUNC");
                byte[] byteChkCmd = Encoding.ASCII.GetBytes(strChkCmd);
                byte[] byteRecv = new byte[3];
                int nRet = 0;

                while (true)
                {
                    System.Threading.Thread.Sleep(1);

                    lock (thisLock)
                    {
                        EpsonClientSendCmd.SendData(byteChkCmd);
                        nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                        if (nRet == 0)
                        {

                            if (byteRecv[0] == 0x31)   //手臂已經不在等待停止函式中了, 離開迴圈
                                break;
                        }
                        else
                            return -10; //通訊發生錯誤  
                    }
                }
            }

            return nErrorCode;
        }

        //在直角坐標系上 圓弧運動
        public virtual int Arc(SpeedTypes nSpeedType, WaitTypes nWaitType,
                   double dbMidXValue, double dbMidYValue, double dbMidZValue, double dbMidRxValue, double dbMidRyValue, double dbMidRzValue,
                   double dbEndXValue, double dbEndYValue, double dbEndZValue, double dbEndRxValue, double dbEndRyValue, double dbEndRzValue,
                   double dbAcc, double dbDec, double dbSpeed)
        {
            int nErrorCode = 0;
            lock (thisLock)
            {

                string strCmdHead = "";
                string strCmdMiddlePos = "";
                string strCmdEndPos = "";
                string strCmdSpeed = "";
                string strFinalCmd = "";

                //送給控制器的位置順序為 X,Y,Z,Rz,Ry,Rx
                strCmdMiddlePos = string.Format(",{0:0.###},{1:0.###},{2:0.###},{3:0.###},{4:0.###},{5:0.###}",
                    dbMidXValue, dbMidYValue, dbMidZValue, dbMidRzValue, dbMidRyValue, dbMidRxValue);

                //送給控制器的位置順序為 X,Y,Z,Rz,Ry,Rx
                strCmdEndPos = string.Format(",{0:0.###},{1:0.###},{2:0.###},{3:0.###},{4:0.###},{5:0.###}",
                    dbEndXValue, dbEndYValue, dbEndZValue, dbEndRzValue, dbEndRyValue, dbEndRxValue);

               
                strCmdHead = "ARC_XYZ_ABS";

                if (nSpeedType == SpeedTypes.Use)
                {
                    strCmdHead = strCmdHead + "_SPEED";
                    strCmdSpeed = string.Format(",{0:0.###},{1:0.###},{2:0.###}", dbSpeed, dbAcc, dbDec);
                }

                if (nWaitType == WaitTypes.Wait)
                    strCmdHead = strCmdHead + "_WAIT";

                strFinalCmd = strCmdHead + strCmdMiddlePos + strCmdEndPos + strCmdSpeed;

                byte[] byteFinalCmd = Encoding.ASCII.GetBytes(strFinalCmd);
                byte[] byteRecv = new byte[9];
                int nRet = 0;

                EpsonClientSendCmd.SendData(byteFinalCmd);
                nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteSendOK))
                        return -100;
                }
                else
                    return -10; //通訊發生錯誤   
            }

            //等待手臂停止
            if (nWaitType == WaitTypes.Wait)
            {
                string strChkCmd = string.Format("GET_IN_WAIT_FUNC");
                byte[] byteChkCmd = Encoding.ASCII.GetBytes(strChkCmd);
                byte[] byteRecv = new byte[3];
                int nRet = 0;

                while (true)
                {
                    System.Threading.Thread.Sleep(1);

                    lock (thisLock)
                    {
                        EpsonClientSendCmd.SendData(byteChkCmd);
                        nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                        if (nRet == 0)
                        {

                            if (byteRecv[0] == 0x31)   //手臂已經不在等待停止函式中了, 離開迴圈
                                break;
                        }
                        else
                            return -10; //通訊發生錯誤  
                    }
                }
            }

            return nErrorCode;
        }


        //單一關節 移動角度
        public virtual int MoveJoint(Joints nJoint, MovementsTypes nMovementType, SpeedTypes nSpeedUsed, WaitTypes nWaitType,
                                   double dbValue, double dbAcc, double dbDec, double dbSpeed)
        {
            int nErrorCode = 0;
            lock (thisLock)
            {
                try
                {
                    double dbJ1 = 0, dbJ2 = 0, dbJ3 = 0, dbJ4 = 0, dbJ5 = 0, dbJ6 = 0;
                    dbJ1 = m_CurrJointPos.dbJ1;
                    dbJ2 = m_CurrJointPos.dbJ2;
                    dbJ3 = m_CurrJointPos.dbJ3;
                    dbJ4 = m_CurrJointPos.dbJ4;
                    dbJ5 = m_CurrJointPos.dbJ5;
                    dbJ6 = m_CurrJointPos.dbJ6;

                    //關節選擇
                    switch (nJoint)
                    {
                        case Joints.J1: dbJ1 = dbValue; break;
                        case Joints.J2: dbJ2 = dbValue; break;
                        case Joints.J3: dbJ3 = dbValue; break;
                        case Joints.J4: dbJ4 = dbValue; break;
                        case Joints.J5: dbJ5 = dbValue; break;
                        case Joints.J6: dbJ6 = dbValue; break; 
                    }

                    string strCmdHead = "";
                    string strCmdPos = "";
                    string strCmdSpeed = "";
                    string strFinalCmd = "";

                    strCmdPos = string.Format(",{0:0.###},{1:0.###},{2:0.###},{3:0.###},{4:0.###},{5:0.###}",
                        dbJ1, dbJ2, dbJ3, dbJ4, dbJ5, dbJ6);

                    if (nMovementType == MovementsTypes.Abs)
                        strCmdHead = "MOVE_JOINTS_ABS";
                    else if (nMovementType == MovementsTypes.Rel)
                        strCmdHead = "MOVE_JOINTS_REL";

                    if (nSpeedUsed == SpeedTypes.Use)
                    {
                        strCmdHead = strCmdHead + "_SPEED";
                        strCmdSpeed = string.Format(",{0:0.###},{1:0.###},{2:0.###}", dbSpeed, dbAcc, dbDec);
                    }
                   
                    if (nWaitType == WaitTypes.Wait)
                        strCmdHead = strCmdHead + "_WAIT";


                    strFinalCmd = strCmdHead + strCmdPos + strCmdSpeed;

                    
                    byte[] byteFinalCmd = Encoding.ASCII.GetBytes(strFinalCmd);
                    byte[] byteRecv = new byte[9];
                    int nRet = 0;

                    EpsonClientSendCmd.SendData(byteFinalCmd);
                    nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteSendOK))
                            return -100;
                    }
                    else
                        return -10; //通訊發生錯誤  

                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("MoveJoint 例外 : ErrCode = {0} , {1} !", ex.ToString());
                    return nErrorCode;
                }

            }

            //等待手臂停止
            if (nWaitType == WaitTypes.Wait)
            {
                string strChkCmd = string.Format("GET_IN_WAIT_FUNC");
                byte[] byteChkCmd = Encoding.ASCII.GetBytes(strChkCmd);
                byte[] byteRecv = new byte[3];
                int nRet = 0;

                while (true)
                {
                    System.Threading.Thread.Sleep(1);
                    lock (thisLock)
                    {
                        EpsonClientSendCmd.SendData(byteChkCmd);
                        nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                        if (nRet == 0)
                        {
                            if (byteRecv[0] == 0x31)   //手臂已經不在等待停止函式中了, 離開迴圈
                                break;
                        }
                        else
                            return -10; //通訊發生錯誤  
                    }
                }
            }

            return nErrorCode;
        }


        //單一直角坐標系方向 移動
        public virtual int LineCoordnate(Coordnates nCoordnate, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
                        double dbValue, double dbAcc, double dbDec, double dbSpeed)
        {
            int nErrorCode = 0;
            lock (thisLock)
            {
                try
                {
                    string strCmdHead = "";
                    string strCmdPos = "";
                    string strCmdSpeed = "";
                    string strFinalCmd = "";

                    //送給控制器的位置順序為 關節代號與位置
                    strCmdPos = string.Format(",{0:0.###},{1:0.###}", (int)nCoordnate, dbValue);

                    if (nMovementType == MovementsTypes.Abs)
                        strCmdHead = "LINE_WORLD_SINGLE_ABS";
                    else if (nMovementType == MovementsTypes.Rel)
                        strCmdHead = "LINE_WORLD_SINGLE_REL";

                    if (nSpeedType == SpeedTypes.Use)
                    {
                        strCmdHead = strCmdHead + "_SPEED";
                        strCmdSpeed = string.Format(",{0:0.###},{1:0.###},{2:0.###}", dbSpeed, dbAcc, dbDec);
                    }

                    if (nWaitType == WaitTypes.Wait)
                        strCmdHead = strCmdHead + "_WAIT";

                    strFinalCmd = strCmdHead + strCmdPos + strCmdSpeed;

                    byte[] byteFinalCmd = Encoding.ASCII.GetBytes(strFinalCmd);
                    byte[] byteRecv = new byte[9];
                    int nRet = 0;

                    EpsonClientSendCmd.SendData(byteFinalCmd);
                    nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteSendOK))
                            return -100;
                    }
                    else
                        return -10; //通訊發生錯誤  

                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("LineCoordnate 例外 : ErrCode = {0} , {1} !", ex.ToString());
                    return nErrorCode;
                }  
            }

            //等待手臂停止
            if (nWaitType == WaitTypes.Wait)
            {
                string strChkCmd = string.Format("GET_IN_WAIT_FUNC");
                byte[] byteChkCmd = Encoding.ASCII.GetBytes(strChkCmd);
                byte[] byteRecv = new byte[3];
                int nRet = 0;

                while (true)
                {
                    System.Threading.Thread.Sleep(1);
                    lock (thisLock)
                    {
                        EpsonClientSendCmd.SendData(byteChkCmd);
                        nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                        if (nRet == 0)
                        {
                            if (byteRecv[0] == 0x31)   //手臂已經不在等待停止函式中了, 離開迴圈
                                break;
                        }
                        else
                            return -10; //通訊發生錯誤  
                    }
                }
            }
            return nErrorCode;
 
        }







        public virtual int MoveAllJoint(MovementsTypes nMovementType, UnitTypes nUnit, SpeedTypes nSpeedUsed, WaitTypes nWaitType,
                                double dbJ1Value, double dbJ2Value, double dbJ3Value, double dbJ4Value, double dbJ5Value, double dbJ6Value, double dbAcc, double dbDec, double dbSpeed)
        {
            return 1;
        }

        public virtual int LineSingleCoordnate(Coordnates nCoordnate, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
                       double dbValue, double dbAccPercentage, double dbDecPercentage, double dbSpeedPercentage)
        {
            return 1;
        }

        public virtual int StopMotionForPtoP(double dbDec)
        {
            int nErrorCode = 0;
            lock (thisLock)
            {
                try
                {
                    string strCmd = string.Format("ACT_STOP");
                    byte[] byteCmd = Encoding.ASCII.GetBytes(strCmd);
                    byte[] byteRecv = new byte[15];
                    int nRet = 0;

                    EpsonClientSendCmd.SendData(byteCmd);
                    nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        //判斷 控制器是否 收到命令
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteStopDone))
                            return -100;
                    }
                    else
                        return -10; //通訊發生錯誤  
                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("StopMotionForPtoP 例外 : ErrCode = {0} , {1} !", ex.ToString());
                    return nErrorCode;
                }
            }

            return nErrorCode;
        }

        public virtual int StopMotionForLine(double dbDec)
        {

            int nErrorCode = 0;
            lock (thisLock)
            {
                try
                {
                    string strCmd = string.Format("ACT_STOP");
                    byte[] byteCmd = Encoding.ASCII.GetBytes(strCmd);
                    byte[] byteRecv = new byte[15];
                    int nRet = 0;

                    EpsonClientSendCmd.SendData(byteCmd);
                    nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        //判斷 控制器是否 收到命令
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteStopDone))
                            return -100;
                    }
                    else
                        return -10; //通訊發生錯誤  
                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("StopMotionForLine 例外 : ErrCode = {0} , {1} !", ex.ToString());
                    return nErrorCode;
                }
            }

            return nErrorCode;
        }

        private int SetAllSpeed(long lMotion, double dbSpeed, double dbAcc, double dbDec)
        {
            int nErrorCode = 0;
            lock (thisLock)
            {
                try
                {
                    string strCmd = "";
                    double dbSendSpeed = 0;
                    double dbSendAcc = 0;
                    double dbSendDec = 0;

                    if (lMotion == P_TO_P)
                        strCmd = string.Format("SET_PTP_ALL_SPEED,{0:0.###},{1:0.###},{2:0.###}", dbSpeed, dbAcc, dbDec);
                    else if (lMotion == LINE)
                        strCmd = string.Format("SET_LINE_ALL_SPEED,{0:0.###},{1:0.###},{2:0.###}", dbSpeed, dbAcc, dbDec);

                    byte[] byteCmd = Encoding.ASCII.GetBytes(strCmd);
                    byte[] byteRecv = new byte[9];
                    int nRet = 0;

                    EpsonClientSendCmd.SendData(strCmd);
                    nRet = EpsonClientSendCmd.Receive(ref byteRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteSendOK))
                            return -100;
                    }
                    else
                        return -10; //通訊發生錯誤  
                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("SetAllSpeed 例外 : ErrCode = {0} , {1} !", ex.ToString());
                    return nErrorCode;
                }
            }

            return nErrorCode;
        }


    }
}
