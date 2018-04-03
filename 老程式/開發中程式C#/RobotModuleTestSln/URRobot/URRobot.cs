using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using IRobotMotion;

namespace URRobot
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

    public class URRobot : IRobotMotion.IRobotMotion
    {
        private Object thisLock = new Object();

        // public URSocketClient urClient;
        URSocketClient URClient;
        URSocketServer URServer;

        public int m_nId = 0;
        public string m_strRobotIP = "";
        public int m_nRobotRealTimePort = 0;
        public int m_nPCListenPort = 0;

        public double m_dbMaxJointSpeed = 3.14159;
        public double m_dbMaxJointAcc = 3.14159;
        public double m_dbMaxLineSpeed = 0.3;   // m/s
        public double m_dbMaxLineAcc = 1; // m/s^2

        public JointPosInfo m_CurrJointPos;
        public WorldPosInfo m_CurrWorldPos;

        public virtual string Name { get { return "URRobot"; } }

        //public virtual bool Connect(string strRobotIP, int nPCListenPort)
        public virtual int Connect()
        {
            //Server Socket 用於操作手臂動作的socket
           // URSocketServer URServer = new URSocketServer(40000);
            URServer = new URSocketServer();
            URServer.ConnectRobot(m_strRobotIP,m_nPCListenPort);

          

            //Client Socket 用於擷取手臂RealTime Data
            // URClient = new URSocketClient(strIP, 30003);
            URClient = new URSocketClient();
            URClient.ConnectRobot(m_strRobotIP, m_nRobotRealTimePort);   //30003為UR手臂端固定的RealTimeData Port

            
            //開始執行 截取狀態執行緒
           // Thread tGetStatusThread = new Thread(new ThreadStart(GetStatusThread));//執行緒開始的時候要調用的方法為threadProc.thread
           // tGetStatusThread.IsBackground = true;//設置IsBackground=true,後臺執行緒會自動根據主執行緒的銷毀而銷毀
           // tGetStatusThread.Start();

            return 0;
        }

        public virtual int Disconnect()
        {
            //Server Socket 用於操作手臂動作的socket
          //   URSocketServer URServer = new URSocketServer(40000, 0);
          //    Thread tSocketServerThread = new Thread(new ThreadStart(URServer.thread));//執行緒開始的時候要調用的方法為threadProc.thread
          //    tSocketServerThread.IsBackground = true;//設置IsBackground=true,後臺執行緒會自動根據主執行緒的銷毀而銷毀
           //   tSocketServerThread.Start();

            int nRet1 = URClient.DisconnectRobot();

            //先通知UR 斷線
            int nRet2 = NoticeURDisconnect();
            Thread.Sleep(500);

            int nRet3 = URServer.DisconnectRobot();

            return nRet1;
        }


        private int ParsingJointValue(ref string strData)
        {
            int nIndex = 0;
            string strSub = "";

            nIndex = strData.IndexOf("[");
            strData = strData.Remove(0, nIndex + 1); //先刪除[

            nIndex = strData.IndexOf("]");
            strData = strData.Remove(nIndex, 1); //再刪除]

            for (int i = 0; i < 5; i++)
            {
                nIndex = strData.IndexOf(",");  //找每個數值中間的間隔符號,
                strSub = strData.Substring(0, nIndex);
                strData = strData.Remove(0, nIndex + 1);
                if (i == 0)
                    m_CurrJointPos.dbJ1 = Convert.ToDouble(strSub) / 3.1415926 * 180.0; 
                else if (i == 1)
                    m_CurrJointPos.dbJ2 = Convert.ToDouble(strSub) / 3.1415926 * 180.0; 
                else if (i == 2)
                    m_CurrJointPos.dbJ3 = Convert.ToDouble(strSub) / 3.1415926 * 180.0; 
                else if (i == 3)
                    m_CurrJointPos.dbJ4 = Convert.ToDouble(strSub) / 3.1415926 * 180.0;
                else if (i == 4)
                    m_CurrJointPos.dbJ5 = Convert.ToDouble(strSub) / 3.1415926 * 180.0;
                //else if (i == 5)
                //     m_CurrJointPos.dbJ6 = Convert.ToDouble(strSub);
            }

            nIndex = strData.IndexOf("\n");  //找最後一個數值的間隔符號,
            strSub = strData.Substring(0, nIndex);
            strData = strData.Remove(0, nIndex + 1);
            m_CurrJointPos.dbJ6 = Convert.ToDouble(strSub) / 3.1415926 * 180.0;

            return 0;
        }


        private int ParsingWorldValue(ref string strData)
        {
            int nIndex = 0;
            string strSub = "";

            nIndex = strData.IndexOf("[");
            strData = strData.Remove(0, nIndex + 1); //先刪除[

            nIndex = strData.IndexOf("]");
            strData = strData.Remove(nIndex, 1); //再刪除]

            for (int i = 0; i < 5; i++)
            {
                nIndex = strData.IndexOf(",");  //找每個數值中間的間隔符號,
                strSub = strData.Substring(0, nIndex);
                strData = strData.Remove(0, nIndex + 1);
                if (i == 0)
                    m_CurrWorldPos.dbWorldX = Convert.ToDouble(strSub) * 1000;
                else if (i == 1)
                    m_CurrWorldPos.dbWorldY = Convert.ToDouble(strSub) * 1000;
                else if (i == 2)
                    m_CurrWorldPos.dbWorldZ = Convert.ToDouble(strSub) * 1000;
                else if (i == 3)
                    m_CurrWorldPos.dbWorldRx = Convert.ToDouble(strSub);
                else if (i == 4)
                    m_CurrWorldPos.dbWorldRy = Convert.ToDouble(strSub);
                //else if (i == 5)
                //     m_CurrJointPos.dbJ6 = Convert.ToDouble(strSub);
            }

            nIndex = strData.IndexOf("\n");  //找最後一個數值的間隔符號,
            strSub = strData.Substring(0, nIndex);
            strData = strData.Remove(0, nIndex + 1);
            m_CurrWorldPos.dbWorldRz = Convert.ToDouble(strSub);

            return 0;
        }


        public void GetStatusThread()
        {
            // byte[] byteRecv = new byte[256];
            // byte[] byteReqPosCmd = { 0x52, 0x45, 0x51, 0x5F, 0x50, 0x4F, 0x53 }; //REQ_POS
            // int nRet = 0;

            byte[] byteRecv;
            bool bRetSend = false;
            string strCmd = "";
            byteRecv = new byte[64];

            while (URServer.m_bConnectedSocket == false)
            {
                Thread.Sleep(30);
            }


            while ( URServer.m_bConnectedSocket == true )
            {
                System.Threading.Thread.Sleep(30);//每個執行緒內部的閉環裡面都要加個「短時間」睡眠，使得執行緒佔用資源得到及時釋放

                //擷取各關節角度
                lock (thisLock)
                {
                    strCmd = string.Format("(10)");
                    bRetSend = URServer.SendData(strCmd);
                    if (bRetSend == true)
                    { 
                        Thread.Sleep(30);

                        int nLen = URServer.Receive(byteRecv);
                        if (nLen > 0)    // 讀取到資料 
                        {
                            string result = System.Text.Encoding.UTF8.GetString(byteRecv);
                            ParsingJointValue(ref result); 
                        }
                        
                    }
                }

                //擷取TCP座標
                lock (thisLock)
                {
                    strCmd = string.Format("(11)");
                    bRetSend = URServer.SendData(strCmd);
                    if (bRetSend == true)
                    {
                  
                        Thread.Sleep(30);

                        int nLen = URServer.Receive(byteRecv);
                        if (nLen > 0)    // 讀取到資料 
                        {
                            string result = System.Text.Encoding.UTF8.GetString(byteRecv);
                            ParsingWorldValue(ref result);
                        }

                    }
                }
            }

        }

        public virtual int GetJointDegree(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            dbJ1 = URClient.m_jointDegreeInfo.dbBasePosDegree;
            dbJ2 = URClient.m_jointDegreeInfo.dbShoulderPosDegree;
            dbJ3 = URClient.m_jointDegreeInfo.dbElbowPosDegree;
            dbJ4 = URClient.m_jointDegreeInfo.dbWrist1PosDegree;
            dbJ5 = URClient.m_jointDegreeInfo.dbWrist2PosDegree;
            dbJ6 = URClient.m_jointDegreeInfo.dbWrist3PosDegree;

          //  dbJ1 = m_CurrJointPos.dbJ1;
          //  dbJ2 = m_CurrJointPos.dbJ2;
          //  dbJ3 = m_CurrJointPos.dbJ3;
          //  dbJ4 = m_CurrJointPos.dbJ4;
          //  dbJ5 = m_CurrJointPos.dbJ5;
          //  dbJ6 = m_CurrJointPos.dbJ6;



            /*
            bool bRetSend = false;
            string strCmd1 = string.Format("(10)");
            bRetSend = URServer.SendData(strCmd1);


            bRetSend = URServer.SendData(strCmd1);
            sprintf_s(szCmd, 16, "%s", "(10)");
            len = strlen(szCmd);
            bRet = m_TcpServer.SendData(m_TcpServer.m_ConnectSocket, szCmd, len);

            Sleep(30);
            */
            return 0;
        }

        public virtual int GetJointRadian(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            /*
            dbJ1 = URClient.m_jointRadianInfo.dbBasePosRadian;
            dbJ2 = URClient.m_jointRadianInfo.dbShoulderPosRadian;
            dbJ3 = URClient.m_jointRadianInfo.dbElbowPosRadian;
            dbJ4 = URClient.m_jointRadianInfo.dbWrist1PosRadian;
            dbJ5 = URClient.m_jointRadianInfo.dbWrist2PosRadian;
            dbJ6 = URClient.m_jointRadianInfo.dbWrist3PosRadian;
            */
            return 0;
        }

        public virtual int GetWorldPos(ref double dbX, ref double dbY, ref double dbZ, ref double dbRx, ref double dbRy, ref double dbRz)
        {
            
            dbX = URClient.m_tcpPosInfo.dbTCP_X;
            dbY = URClient.m_tcpPosInfo.dbTCP_Y;
            dbZ = URClient.m_tcpPosInfo.dbTCP_Z;
            dbRx = URClient.m_tcpPosInfo.dbTCP_Rx;
            dbRy = URClient.m_tcpPosInfo.dbTCP_Ry;
            dbRz = URClient.m_tcpPosInfo.dbTCP_Rz;
            
            /*
            dbX = m_CurrWorldPos.dbWorldX ;
            dbY = m_CurrWorldPos.dbWorldY;
            dbZ = m_CurrWorldPos.dbWorldZ;
            dbRx = m_CurrWorldPos.dbWorldRx;
            dbRy = m_CurrWorldPos.dbWorldRy;
            dbRz = m_CurrWorldPos.dbWorldRz;
            */

            return 0;
        }

        //擷取 6關節電流
        public virtual int GetJointCurrentValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            /*
            dbJ1 = URClient.m_jointCurrentInfo.dbBase;
            dbJ2 = URClient.m_jointCurrentInfo.dbShoulder;
            dbJ3 = URClient.m_jointCurrentInfo.dbElbow;
            dbJ4 = URClient.m_jointCurrentInfo.dbWrist1;
            dbJ5 = URClient.m_jointCurrentInfo.dbWrist2;
            dbJ6 = URClient.m_jointCurrentInfo.dbWrist3;
            */
            return 0;
        }

        //擷取 6關節電壓
        public virtual int GetJointVoltageValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            /*
            dbJ1 = URClient.m_jointVoltageInfo.dbBase;
            dbJ2 = URClient.m_jointVoltageInfo.dbShoulder;
            dbJ3 = URClient.m_jointVoltageInfo.dbElbow;
            dbJ4 = URClient.m_jointVoltageInfo.dbWrist1;
            dbJ5 = URClient.m_jointVoltageInfo.dbWrist2;
            dbJ6 = URClient.m_jointVoltageInfo.dbWrist3;
            */
            return 0;
        }

        //擷取 6關節扭矩
        public virtual int GetJointTorqueValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            /*
            dbJ1 = URClient.m_jointTorqueInfo.dbBase;
            dbJ2 = URClient.m_jointTorqueInfo.dbShoulder;
            dbJ3 = URClient.m_jointTorqueInfo.dbElbow;
            dbJ4 = URClient.m_jointTorqueInfo.dbWrist1;
            dbJ5 = URClient.m_jointTorqueInfo.dbWrist2;
            dbJ6 = URClient.m_jointTorqueInfo.dbWrist3;
            */
            return 0;
        }

        //擷取 6關節溫度
        public virtual int GetJointTemperatureValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            /*
            dbJ1 = URClient.m_jointTemperatureInfo.dbBase;
            dbJ2 = URClient.m_jointTemperatureInfo.dbShoulder;
            dbJ3 = URClient.m_jointTemperatureInfo.dbElbow;
            dbJ4 = URClient.m_jointTemperatureInfo.dbWrist1;
            dbJ5 = URClient.m_jointTemperatureInfo.dbWrist2;
            dbJ6 = URClient.m_jointTemperatureInfo.dbWrist3;
            */
            return 0;
        }

        //擷取 6關節速度
        public virtual int GetJointSpeedValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            /*
            dbJ1 = URClient.m_jointSpeedInfo.dbBase;
            dbJ2 = URClient.m_jointSpeedInfo.dbShoulder;
            dbJ3 = URClient.m_jointSpeedInfo.dbElbow;
            dbJ4 = URClient.m_jointSpeedInfo.dbWrist1;
            dbJ5 = URClient.m_jointSpeedInfo.dbWrist2;
            dbJ6 = URClient.m_jointSpeedInfo.dbWrist3;
            */
            return 0;
        }

        //擷取 6關節加速度
        public virtual int GetJointAccValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            /*
            dbJ1 = URClient.m_jointAccInfo.dbBase;
            dbJ2 = URClient.m_jointAccInfo.dbShoulder;
            dbJ3 = URClient.m_jointAccInfo.dbElbow;
            dbJ4 = URClient.m_jointAccInfo.dbWrist1;
            dbJ5 = URClient.m_jointAccInfo.dbWrist2;
            dbJ6 = URClient.m_jointAccInfo.dbWrist3;
            */
            return 0;
        }

        //關節Jog 移動
        public virtual int JogJoint(Joints nJoint, Jog_Directions nDir, Jog_Speed nSpeed)
        {
            double dbValue = 0;
            lock (thisLock)
            {
                double dbSpeed = 0, dbAcc = 0, dbDec = 0;
                if (nSpeed == Jog_Speed.High)
                {
                    dbSpeed = 15;
                    dbAcc = 15;
                    dbDec = 15;
                }
                else if (nSpeed == Jog_Speed.Mid)
                {
                    dbSpeed =8;
                    dbAcc =8;
                    dbDec = 8;
                }
                else if (nSpeed == Jog_Speed.Low)
                {
                    dbSpeed = 3;
                    dbAcc = 3;
                    dbDec = 3;
                }

                if (nDir == Jog_Directions.Plus)
                {
                    if (nJoint == Joints.J1) dbValue = 359;
                    else if (nJoint == Joints.J2) dbValue = 150;
                    else if (nJoint == Joints.J3) dbValue = -30;
                    else if (nJoint == Joints.J4) dbValue = 0;
                    else if (nJoint == Joints.J5) dbValue = 359;
                    else if (nJoint == Joints.J6) dbValue = 359;
                }
                else
                {
                    if (nJoint == Joints.J1) dbValue = -359;
                    else if (nJoint == Joints.J2) dbValue = -150;
                    else if (nJoint == Joints.J3) dbValue = -138;
                    else if (nJoint == Joints.J4) dbValue = -174;
                    else if (nJoint == Joints.J5) dbValue = -359;
                    else if (nJoint == Joints.J6) dbValue = -359;
                }

                
                int nRet = 0;
                nRet = MoveJoint(nJoint, MovementsTypes.Abs, SpeedTypes.Use, WaitTypes.NoWait,
                    dbValue, dbAcc, dbDec, dbSpeed);

            }

          
            return 0;
        }

        //關節Jog 移動停止
        public virtual int JogJointStop(Joints nJoint)
        {
            int nRet = 0;
            lock (thisLock)
            {
                nRet = StopMotionForPtoP(60);
            }
            return nRet;
        }

        //座標Jog 移動
        public virtual int JogCoordnate(Coordnates nCoordnate, Jog_Directions nDir, Jog_Speed nSpeed)
        {
            double dbValue = 0;
            lock (thisLock)
            {
                double dbSpeed = 0, dbAcc = 0, dbDec = 0;
                if (nSpeed == Jog_Speed.High)
                {
                    dbSpeed = 15;
                    dbAcc = 15;
                    dbDec = 15;
                }
                else if (nSpeed == Jog_Speed.Mid)
                {
                    dbSpeed = 8;
                    dbAcc = 8;
                    dbDec = 8;
                }
                else if (nSpeed == Jog_Speed.Low)
                {
                    dbSpeed = 3;
                    dbAcc = 3;
                    dbDec = 3;
                }

                if (nDir == Jog_Directions.Plus)
                {
                    if (nCoordnate == Coordnates.X) dbValue = 50;
                    else if (nCoordnate == Coordnates.Y) dbValue = 50;
                    else if (nCoordnate == Coordnates.Z) dbValue = 50;
                    //else if (nCoordnate == Coordnates.Rx) dbValue = 20;
                    else if (nCoordnate == Coordnates.Rx) dbValue = 1.8;
                    else if (nCoordnate == Coordnates.Ry) dbValue = 20;
                    else if (nCoordnate == Coordnates.Rz) dbValue = 20;
                }
                else
                {
                    if (nCoordnate == Coordnates.X) dbValue = -50;
                    else if (nCoordnate == Coordnates.Y) dbValue = -50;
                    else if (nCoordnate == Coordnates.Z) dbValue = -50;
                    //else if (nCoordnate == Coordnates.Rx) dbValue = -20;
                    else if (nCoordnate == Coordnates.Rx) dbValue = -1.97;
                    else if (nCoordnate == Coordnates.Ry) dbValue = -20;
                    else if (nCoordnate == Coordnates.Rz) dbValue = -20;
                }

                int nRet = 0;
                nRet = LineCoordnate(nCoordnate, MovementsTypes.Rel, SpeedTypes.Use, WaitTypes.NoWait,
                    dbValue, dbAcc, dbDec, dbSpeed);

            }
   
            return 0;
        }

        //座標Jog 移動停止
        public virtual int JogCoordnateStop(Coordnates nJoint)
        {
            int nRet = 0;
            lock (thisLock)
            {
                nRet = StopMotionForLine(60);
            }
            return nRet;
        }


        //在直角坐標系上 點對點 同動 (非直線)
        public virtual int MovePtoP(SpeedTypes nSpeedType, WaitTypes nWaitType,
                    double dbXValue, double dbYValue, double dbZValue, double dbRxValue, double dbRyValue, double dbRzValue, double dbAcc, double dbDec, double dbSpeed)
        {
            return 0;
        }

        //在直角坐標系上 點對點 直線運動
        public virtual int LinePtoP(SpeedTypes nSpeedType, WaitTypes nWaitType,
                    double dbXValue, double dbYValue, double dbZValue, double dbRxValue, double dbRyValue, double dbRzValue, double dbAcc, double dbDec, double dbSpeed)
        {
            return 0;
        }

        //在直角坐標系上 圓弧運動
        public virtual int Arc(SpeedTypes nSpeedType, WaitTypes nWaitType,
                     double dbMidXValue, double dbMidYValue, double dbMidZValue, double dbMidRxValue, double dbMidRyValue, double dbMidRzValue,
                     double dbEndXValue, double dbEndYValue, double dbEndZValue, double dbEndRxValue, double dbEndRyValue, double dbEndRzValue,
                     double dbAcc, double dbDec, double dbSpeed)
        {
            return 0;
        }

        //單一關節 移動角度
        public virtual int MoveJoint(Joints nJoint, MovementsTypes nMovementType, SpeedTypes nSpeedUsed, WaitTypes nWaitType, 
                                    double dbValue, double dbAcc, double dbDec, double dbSpeed)
        {
            int nRetResult = 0;

            lock (thisLock)
            {
                bool bRetSend = false;

                //先記錄目前各關節位置(徑度)
                double dbJ1 = 0, dbJ2 = 0, dbJ3 = 0, dbJ4 = 0, dbJ5 = 0, dbJ6 = 0;
                dbJ1 = URClient.m_jointRadianInfo.dbBasePosRadian;
                dbJ2 = URClient.m_jointRadianInfo.dbShoulderPosRadian;
                dbJ3 = URClient.m_jointRadianInfo.dbElbowPosRadian;
                dbJ4 = URClient.m_jointRadianInfo.dbWrist1PosRadian;
                dbJ5 = URClient.m_jointRadianInfo.dbWrist2PosRadian;
                dbJ6 = URClient.m_jointRadianInfo.dbWrist3PosRadian;

                dbValue = dbValue * 3.1415926 / 180.0;  //將輸入的變數轉為徑度

                /*
                //1.單位選擇
                switch (nUnit)
                {
                    case UnitTypes.Degree:
                        dbValue = dbValue * 3.1415926 / 180.0;  //將輸入的變數轉為徑度
                      //  dbJ1 = URClient.m_jointRadianInfo.dbBasePosRadian * 3.1415926 / 180.0;
                     //   dbJ2 = URClient.m_jointRadianInfo.dbShoulderPosRadian * 3.1415926 / 180.0;
                      //  dbJ3 = URClient.m_jointRadianInfo.dbElbowPosRadian * 3.1415926 / 180.0;
                      //  dbJ4 = URClient.m_jointRadianInfo.dbWrist1PosRadian * 3.1415926 / 180.0;
                      //  dbJ5 = URClient.m_jointRadianInfo.dbWrist2PosRadian * 3.1415926 / 180.0;
                      //  dbJ6 = URClient.m_jointRadianInfo.dbWrist3PosRadian * 3.1415926 / 180.0;
                        break;
                    case UnitTypes.Radian:
                      //  dbValue = dbValue ;
                      //  dbJ1 = URClient.m_jointRadianInfo.dbBasePosRadian;
                      //  dbJ2 = URClient.m_jointRadianInfo.dbShoulderPosRadian;
                      //  dbJ3 = URClient.m_jointRadianInfo.dbElbowPosRadian;
                      //  dbJ4 = URClient.m_jointRadianInfo.dbWrist1PosRadian;
                      //  dbJ5 = URClient.m_jointRadianInfo.dbWrist2PosRadian;
                     //   dbJ6 = URClient.m_jointRadianInfo.dbWrist3PosRadian;
                       
                        break;
                }
                */
                //2.移動量模式選擇
                switch (nMovementType)
                {
                    case MovementsTypes.Abs:
                        //3.關節選擇
                        switch (nJoint)
                        {
                            case Joints.J1: dbJ1 = dbValue; break;
                            case Joints.J2: dbJ2 = dbValue; break;
                            case Joints.J3: dbJ3 = dbValue; break;
                            case Joints.J4: dbJ4 = dbValue; break;
                            case Joints.J5: dbJ5 = dbValue; break;
                            case Joints.J6: dbJ6 = dbValue; break;
                        }
                        break;
                    case MovementsTypes.Rel:
                        switch (nJoint)
                        {
                            case Joints.J1: dbJ1 = dbJ1 + dbValue; break;
                            case Joints.J2: dbJ2 = dbJ2 + dbValue; break;
                            case Joints.J3: dbJ3 = dbJ3 + dbValue; break;
                            case Joints.J4: dbJ4 = dbJ4 + dbValue; break;
                            case Joints.J5: dbJ5 = dbJ5 + dbValue; break;
                            case Joints.J6: dbJ6 = dbJ6 + dbValue; break;
                        }
                        break;
                }

                //4.速度型態選擇
                //送給UR的速度單位為 rad/s  加速度 rad/s^2
                double dbTempSpeed = (dbSpeed / 100.0) * m_dbMaxJointSpeed;
                double dbTempAcc = (dbAcc / 100.0) * m_dbMaxJointAcc;
                switch (nSpeedUsed)
                {
                    case SpeedTypes.Use:
                        string strCmd1 = string.Format("(25,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
                                                    dbJ1, dbJ2, dbJ3, dbJ4, dbJ5, dbJ6, dbTempAcc, dbTempSpeed, 0, 0);
                        bRetSend = URServer.SendData(strCmd1);

                        break;
                    case SpeedTypes.NoUse:
                        string strCmd2 = string.Format("(20,{0},{1},{2},{3},{4},{5})",
                                                    dbJ1, dbJ2, dbJ3, dbJ4, dbJ5, dbJ6);
                        bRetSend = URServer.SendData(strCmd2);
                        break;
                }

                if (bRetSend == true)
                {
                    //5.等待型態選擇
                    if (nWaitType == WaitTypes.Wait)
                    {
                        byte[] byteRecv;
                        byteRecv = new byte[32];

                        while (true)
                        {
                            Thread.Sleep(30);

                            int nLen = URServer.Receive(byteRecv);
                            if (nLen > 0)    // 讀取到資料 
                            {
                                string result = System.Text.Encoding.UTF8.GetString(byteRecv);
                                int nRetCompare = string.Compare(result, "MoveJ Done");
                                if (nRetCompare == 0)
                                    nRetResult = 0;
                                else
                                    nRetResult = -10;

                                break;
                            }
                            else    //沒有讀取到資料 
                            {
                                nRetResult = -10;
                                //if (nLen == SOCKET_ERROR)
                                //{
                                //   int nErr = WSAGetLastError();
                                //   if (nErr != WSAEWOULDBLOCK)
                                //   {
                                //       bRet = FALSE;
                                //       break;
                                //   }
                                // }

                                break;
                            }
                        }
                    }
                    else
                    {
                        nRetResult = 0;
                    }
                }
                else
                {
                    nRetResult = -10;
                }

            }

            return nRetResult;

        }


        //單一直角坐標系方向 移動
        public virtual int LineCoordnate(Coordnates nCoordnate, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
                        double dbValue, double dbAcc, double dbDec, double dbSpeed)
        {

            int nRetResult = 0;

            lock (thisLock)
            {
                bool bRetSend = false;

                //先記錄目前各世界座標位置()
                double dbX = 0, dbY = 0, dbZ = 0, dbRx = 0, dbRy = 0, dbRz = 0;
                dbX = URClient.m_tcpPosInfo.dbTCP_X / 1000.0;   //轉為 m 單位
                dbY = URClient.m_tcpPosInfo.dbTCP_Y / 1000.0;   //轉為 m 單位
                dbZ = URClient.m_tcpPosInfo.dbTCP_Z / 1000.0;   //轉為 m 單位
                dbRx = URClient.m_tcpPosInfo.dbTCP_Rx;
                dbRy = URClient.m_tcpPosInfo.dbTCP_Ry;
                dbRz = URClient.m_tcpPosInfo.dbTCP_Rz;

              

                /*
                dbX = URClient.m_jointRadianInfo.dbBasePosRadian;
                dbY = URClient.m_jointRadianInfo.dbShoulderPosRadian;
                dbZ = URClient.m_jointRadianInfo.dbElbowPosRadian;
                dbRx = URClient.m_jointRadianInfo.dbWrist1PosRadian;
                dbRy = URClient.m_jointRadianInfo.dbWrist2PosRadian;
                dbRz = URClient.m_jointRadianInfo.dbWrist3PosRadian;
                dbValue = dbValue * 3.1415926 / 180.0;  //將輸入的變數轉為徑度
                */

                /*
                //1.單位選擇
                switch (nUnit)
                {
                    case UnitTypes.Degree:
                        dbValue = dbValue * 3.1415926 / 180.0;  //將輸入的變數轉為徑度
                      //  dbJ1 = URClient.m_jointRadianInfo.dbBasePosRadian * 3.1415926 / 180.0;
                     //   dbJ2 = URClient.m_jointRadianInfo.dbShoulderPosRadian * 3.1415926 / 180.0;
                      //  dbJ3 = URClient.m_jointRadianInfo.dbElbowPosRadian * 3.1415926 / 180.0;
                      //  dbJ4 = URClient.m_jointRadianInfo.dbWrist1PosRadian * 3.1415926 / 180.0;
                      //  dbJ5 = URClient.m_jointRadianInfo.dbWrist2PosRadian * 3.1415926 / 180.0;
                      //  dbJ6 = URClient.m_jointRadianInfo.dbWrist3PosRadian * 3.1415926 / 180.0;
                        break;
                    case UnitTypes.Radian:
                      //  dbValue = dbValue ;
                      //  dbJ1 = URClient.m_jointRadianInfo.dbBasePosRadian;
                      //  dbJ2 = URClient.m_jointRadianInfo.dbShoulderPosRadian;
                      //  dbJ3 = URClient.m_jointRadianInfo.dbElbowPosRadian;
                      //  dbJ4 = URClient.m_jointRadianInfo.dbWrist1PosRadian;
                      //  dbJ5 = URClient.m_jointRadianInfo.dbWrist2PosRadian;
                     //   dbJ6 = URClient.m_jointRadianInfo.dbWrist3PosRadian;
                       
                        break;
                }
                */

                /*
                //2.移動量模式選擇
                switch (nMovementType)
                {
                    case MovementsTypes.Abs:
                        //3.座標選擇
                        switch (nCoordnate)
                        {
                            
                            case Coordnates.X: dbX = dbValue / 1000.0; break;
                            case Coordnates.Y: dbY = dbValue / 1000.0; break;
                            case Coordnates.Z: dbZ = dbValue / 1000.0; break;
                           // case Coordnates.Rx: dbRx = dbValue; break;
                            //case Coordnates.Ry: dbRy = dbValue; break;
                           // case Coordnates.Rz: dbRz = dbValue; break;
                            case Coordnates.Rx: dbRx = dbValue * 3.1415926 / 180.0; break;
                            case Coordnates.Ry: dbRy = dbValue * 3.1415926 / 180.0; break;
                            case Coordnates.Rz: dbRz = dbValue * 3.1415926 / 180.0; break;
                            
                        }
                        break;
                    case MovementsTypes.Rel:
                        switch (nCoordnate)
                        {
                            
                            case Coordnates.X: dbX = dbX + (dbValue / 1000.0); break;
                            case Coordnates.Y: dbY = dbY + (dbValue / 1000.0); break;
                            case Coordnates.Z: dbZ = dbZ + (dbValue / 1000.0); break;
                        //    case Coordnates.Rx: dbRx = dbRx + dbValue; break;
                         //   case Coordnates.Ry: dbRy = dbRy + dbValue; break;
                         //   case Coordnates.Rz: dbRz = dbRz + dbValue; break;
                            case Coordnates.Rx: dbRx = dbValue; break;
                          //  case Coordnates.Rx: dbRx = dbRx + (dbValue * 3.1415926 / 180.0); break;
                            case Coordnates.Ry: dbRy = dbRy + (dbValue * 3.1415926 / 180.0); break;
                            case Coordnates.Rz: dbRz = dbRz + (dbValue * 3.1415926 / 180.0); break;

                                
                               // case Coordnates.X: dbX = dbX + dbValue; break;
                               // case Coordnates.Y: dbY = dbY + dbValue; break;
                               // case Coordnates.Z: dbZ = dbZ + dbValue; break;
                               // case Coordnates.Rx: dbRx = dbRx + dbValue; break;
                               // case Coordnates.Ry: dbRy = dbRy + dbValue; break;
                               // case Coordnates.Rz: dbRz = dbRz + dbValue; break;
                                
                        }
                        break;
                }
                */

                //4.速度型態選擇
                //送給UR的速度單位為 rad/s  加速度 rad/s^2
              //  double dbTempSpeed = (dbSpeed / 100.0) * m_dbMaxLineSpeed;
               // double dbTempAcc = (dbAcc / 100.0) * m_dbMaxLineAcc;
                double dbTempSpeed = 0.05;
                double dbTempAcc = 0.05;
                switch (nSpeedType)
                {
                    case SpeedTypes.Use:
                        string strCmd1 = string.Format("(26,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
                                                    dbX, dbY, dbZ, dbRx, dbRy, dbRz, dbTempAcc, dbTempSpeed, 0, 0);
                        bRetSend = URServer.SendData(strCmd1);

                        break;
                    case SpeedTypes.NoUse:
                        string strCmd2 = string.Format("(21,{0},{1},{2},{3},{4},{5})",
                                                     dbX, dbY, dbZ, dbRx, dbRy, dbRz);
                        bRetSend = URServer.SendData(strCmd2);
                        break;
                }

                if (bRetSend == true)
                {
                    //5.等待型態選擇
                    if (nWaitType == WaitTypes.Wait)
                    {
                        byte[] byteRecv;
                        byteRecv = new byte[32];

                        while (true)
                        {
                            Thread.Sleep(30);

                            int nLen = URServer.Receive(byteRecv);
                            if (nLen > 0)    // 讀取到資料 
                            {
                                string result = System.Text.Encoding.UTF8.GetString(byteRecv);
                                int nRetCompare = string.Compare(result, "MoveL Done");
                                if (nRetCompare == 0)
                                    nRetResult = 0;
                                else
                                    nRetResult = -10;

                                break;
                            }
                            else    //沒有讀取到資料 
                            {
                                nRetResult = -10;
                                break;
                            }
                        }
                    }
                    else
                    {
                        nRetResult = 0;
                    }
                }
                else
                {
                    nRetResult = -10;
                }

            }

            return nRetResult;
        }









        public virtual int MoveAllJoint(MovementsTypes nMovementType, UnitTypes nUnit, SpeedTypes nSpeedUsed, WaitTypes nWaitType,
                                double dbJ1Value, double dbJ2Value, double dbJ3Value, double dbJ4Value, double dbJ5Value, double dbJ6Value, double dbAcc, double dbDec, double dbSpeed)
        {
            int nRetResult = 0;

            lock (thisLock)
            {
                bool bRetSend = false;

                //先記錄目前各關節位置
                double dbJ1 = 0, dbJ2 = 0, dbJ3 = 0, dbJ4 = 0, dbJ5 = 0, dbJ6 = 0;
              
                //1.單位選擇
                switch (nUnit)
                {
                    case UnitTypes.Degree:
                        dbJ1 = dbJ1Value * 3.1415926 / 180.0; ;
                        dbJ2 = dbJ2Value * 3.1415926 / 180.0; ;
                        dbJ3 = dbJ3Value * 3.1415926 / 180.0; ;
                        dbJ4 = dbJ4Value * 3.1415926 / 180.0; ;
                        dbJ5 = dbJ5Value * 3.1415926 / 180.0; ;
                        dbJ6 = dbJ6Value * 3.1415926 / 180.0; ;
                        break;
                    case UnitTypes.Radian:
                        dbJ1 = dbJ1Value;
                        dbJ2 = dbJ2Value;
                        dbJ3 = dbJ3Value;
                        dbJ4 = dbJ4Value;
                        dbJ5 = dbJ5Value;
                        dbJ6 = dbJ6Value;
                        break;
                }

                //2.移動量模式選擇
                switch (nMovementType)
                {
                    case MovementsTypes.Abs:
                        break;
                    case MovementsTypes.Rel:
                        dbJ1 = URClient.m_jointRadianInfo.dbBasePosRadian + dbJ1;
                        dbJ2 = URClient.m_jointRadianInfo.dbShoulderPosRadian + dbJ2;
                        dbJ3 = URClient.m_jointRadianInfo.dbElbowPosRadian + dbJ3;
                        dbJ4 = URClient.m_jointRadianInfo.dbWrist1PosRadian + dbJ4;
                        dbJ5 = URClient.m_jointRadianInfo.dbWrist2PosRadian + dbJ5;
                        dbJ6 = URClient.m_jointRadianInfo.dbWrist3PosRadian + dbJ6;
                        break;
                }

                //4.速度型態選擇
                switch (nSpeedUsed)
                {
                    case SpeedTypes.Use:
                        string strCmd1 = string.Format("(25,{0},{1},{2},{3},{4},{5},{6},{7},{8},{9})",
                                                    dbJ1, dbJ2, dbJ3, dbJ4, dbJ5, dbJ6, dbAcc, dbSpeed, 0, 0);
                        bRetSend = URServer.SendData(strCmd1);

                        break;
                    case SpeedTypes.NoUse:
                        string strCmd2 = string.Format("(20,{0},{1},{2},{3},{4},{5})",
                                                    dbJ1, dbJ2, dbJ3, dbJ4, dbJ5, dbJ6);
                        bRetSend = URServer.SendData(strCmd2);
                        break;
                }

                if (bRetSend == true)
                {
                    //5.等待型態選擇
                    if (nWaitType == WaitTypes.Wait)
                    {
                        byte[] byteRecv;
                        byteRecv = new byte[32];

                        while (true)
                        {
                            Thread.Sleep(30);

                            int nLen = URServer.Receive(byteRecv);
                            if (nLen > 0)    // 讀取到資料 
                            {
                                string result = System.Text.Encoding.UTF8.GetString(byteRecv);
                                int nRetCompare = string.Compare(result, "MoveJ Done");
                                if (nRetCompare == 0)
                                    nRetResult = 1;
                                else
                                    nRetResult = 0;

                                break;
                            }
                            else    //沒有讀取到資料 
                            {
                                nRetResult = 0;
                                //if (nLen == SOCKET_ERROR)
                                //{
                                //   int nErr = WSAGetLastError();
                                //   if (nErr != WSAEWOULDBLOCK)
                                //   {
                                //       bRet = FALSE;
                                //       break;
                                //   }
                                // }

                                break;
                            }
                        }
                    }
                    else
                    {
                        nRetResult = 1;
                    }
                }
                else
                {
                    nRetResult = 0;
                }

            }

            return nRetResult;
        }

        public virtual int LineSingleCoordnate(Coordnates nCoordnate, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
                       double dbValue, double dbAccPercentage, double dbDecPercentage, double dbSpeedPercentage)
        {
            return 0;
        }


        private int NoticeURDisconnect()
        {
            bool bRetSend = false;
            int nRetResult = 0;

            string strCmd = string.Format("(999)");
            bRetSend = URServer.SendData(strCmd);

            if (bRetSend == true)
                nRetResult = 1;
            else
                nRetResult = -1;

            return nRetResult;
        }


        public virtual int StopMotionForPtoP(double dbDec)
        {
            bool bRetSend = false;
            int nRetResult = 0;

            double dbTempDec = (dbDec / 100.0) * m_dbMaxJointAcc;

            string strCmd = string.Format("(100,{0})", dbTempDec);
            bRetSend = URServer.SendData(strCmd);
            if (bRetSend == true)
                nRetResult = 0;
            else
                nRetResult = -1;

            return nRetResult;
        }

        public virtual int StopMotionForLine(double dbDec)
        {
            bool bRetSend = false;
            int nRetResult = 0;

            double dbTempDec = (dbDec / 100.0) * m_dbMaxLineAcc;

            string strCmd = string.Format("(101,{0})", dbTempDec);
            bRetSend = URServer.SendData(strCmd);

            if (bRetSend == true)
                nRetResult = 0;
            else
                nRetResult = -1;

            return nRetResult;
        }

    }



}
