using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using IRobotMotion;

namespace Toshiba4Robot
{
    //關節角度值
    public struct JointPosInfo
    {
        public double dbJ1;
        public double dbJ2;
        public double dbJ3;
        public double dbJ4;
    }

    //世界座標值
    public struct WorldPosInfo
    {
        public double dbWorldX;
        public double dbWorldY;
        public double dbWorldZ;
        public double dbWorldC;
    }

    //工件座標值
    public struct WorkPosInfo
    {
        public double dbWorkX;
        public double dbWorkY;
        public double dbWorkZ;
        public double dbWorkC;
    }

    public class Toshiba4 : IRobotMotion.IRobotMotion
    {
        private Object thisLock = new Object();

        ToshibaSocketClient ToshibaClientIP0;
        //ToshibaSocketClient ToshibaClientIP1;

        public int m_nId = 0;
        public string m_strRobotIP = "";
        public int m_nRobotPort0 = 0;
        public int m_nRobotPort1 = 0;
        public string m_strRobotProgram = "";
        public int m_nRobotInitOVRD = 0;

        public virtual string Name { get { return "Toshiba4"; } }

        public JointPosInfo m_CurrJointPos;
        public WorldPosInfo m_CurrWorldPos;
        public WorkPosInfo m_CurrWorkPos;

        public bool m_bIP0Connected = false;

        private double m_dbCurrAcc = 0;
        private double m_dbCurrDec = 0;
        private double m_dbCurrSpeed = 0;

        private int m_nRobotStatus = 0;

        public byte[] m_byteRecvOK = { 0x02, 0x4F, 0x4B, 0x0D, 0x03 };    //OK,
        public byte[] m_byteSendOK = { 0x02, 0x4F, 0x4B, 0x0D, 0x03 };    //OK,

        public void GetStatusThread()
        {
            byte[] byteReqStatus = { 0x02, 0x53, 0x46, 0x0D, 0x03 };    //SF
            byte[] byteSendOK = { 0x02, 0x4F, 0x4B, 0x0D, 0x03 };    //OK,
            byte[] byteReqStatusRecv = new byte[300];

            byte[] byteJ1 = new byte[4];
            byte[] byteJ2 = new byte[4];
            byte[] byteJ3 = new byte[4];
            byte[] byteJ4 = new byte[4];
           

            byte[] byteWorldX = new byte[4];
            byte[] byteWorldY = new byte[4];
            byte[] byteWorldZ = new byte[4];
            byte[] byteWorldC = new byte[4];

            byte[] byteWorkX = new byte[4];
            byte[] byteWorkY = new byte[4];
            byte[] byteWorkZ = new byte[4];
            byte[] byteWorkC = new byte[4];

            int nRet = 0;
            while (ToshibaClientIP0.IsConnected())
            {
                System.Threading.Thread.Sleep(1);//每個執行緒內部的閉環裡面都要加個「短時間」睡眠，使得執行緒佔用資源得到及時釋放

                lock (thisLock)
                {
                    nRet = ToshibaClientIP0.SendData(byteReqStatus);
                    if (nRet == 0)
                    {
                        nRet = ToshibaClientIP0.Receive(ref byteReqStatusRecv);

                        System.Threading.Thread.Sleep(60);   //手冊上說 最少要延遲50ms

                        //回傳OK 給控制器
                        nRet = ToshibaClientIP0.SendData(byteSendOK);

                        //目前手臂狀態
                        m_nRobotStatus = (int)byteReqStatusRecv[2 + 4];

                        //關節角度值
                        /////////////////////////////////////////////////////
                        int nJointDataStart = 0;
                        byteJ1[3] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ1[2] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ1[1] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ1[0] = byteReqStatusRecv[120 + nJointDataStart++];
                        m_CurrJointPos.dbJ1 = (double)(BitConverter.ToSingle(byteJ1, 0));

                        byteJ2[3] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ2[2] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ2[1] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ2[0] = byteReqStatusRecv[120 + nJointDataStart++];
                        m_CurrJointPos.dbJ2 = (double)(BitConverter.ToSingle(byteJ2, 0));

                        byteJ3[3] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ3[2] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ3[1] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ3[0] = byteReqStatusRecv[120 + nJointDataStart++];
                        m_CurrJointPos.dbJ3 = (double)(BitConverter.ToSingle(byteJ3, 0));

                        byteJ4[3] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ4[2] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ4[1] = byteReqStatusRecv[120 + nJointDataStart++];
                        byteJ4[0] = byteReqStatusRecv[120 + nJointDataStart++];
                        m_CurrJointPos.dbJ4 = (double)(BitConverter.ToSingle(byteJ4, 0)); 
                        /////////////////////////////////////////////////////

                        //世界座標值
                        /////////////////////////////////////////////////////
                        int nWorldDataStart = 24;
                        byteWorldX[3] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldX[2] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldX[1] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldX[0] = byteReqStatusRecv[120 + nWorldDataStart++];
                        m_CurrWorldPos.dbWorldX = (double)(BitConverter.ToSingle(byteWorldX, 0));

                        byteWorldY[3] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldY[2] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldY[1] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldY[0] = byteReqStatusRecv[120 + nWorldDataStart++];
                        m_CurrWorldPos.dbWorldY = (double)(BitConverter.ToSingle(byteWorldY, 0));

                        byteWorldZ[3] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldZ[2] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldZ[1] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldZ[0] = byteReqStatusRecv[120 + nWorldDataStart++];
                        m_CurrWorldPos.dbWorldZ = (double)(BitConverter.ToSingle(byteWorldZ, 0));

                        byteWorldC[3] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldC[2] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldC[1] = byteReqStatusRecv[120 + nWorldDataStart++];
                        byteWorldC[0] = byteReqStatusRecv[120 + nWorldDataStart++];
                        m_CurrWorldPos.dbWorldC = (double)(BitConverter.ToSingle(byteWorldC, 0));
                        /////////////////////////////////////////////////////

                        //工作座標值
                        /////////////////////////////////////////////////////
                        int nWorkDataStart = 48;
                        byteWorkX[3] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkX[2] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkX[1] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkX[0] = byteReqStatusRecv[120 + nWorkDataStart++];
                        m_CurrWorkPos.dbWorkX = (double)(BitConverter.ToSingle(byteWorkX, 0));

                        byteWorkY[3] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkY[2] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkY[1] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkY[0] = byteReqStatusRecv[120 + nWorkDataStart++];
                        m_CurrWorkPos.dbWorkY = (double)(BitConverter.ToSingle(byteWorkY, 0));

                        byteWorkZ[3] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkZ[2] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkZ[1] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkZ[0] = byteReqStatusRecv[120 + nWorkDataStart++];
                        m_CurrWorkPos.dbWorkZ = (double)(BitConverter.ToSingle(byteWorkZ, 0));

                        byteWorkC[3] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkC[2] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkC[1] = byteReqStatusRecv[120 + nWorkDataStart++];
                        byteWorkX[0] = byteReqStatusRecv[120 + nWorkDataStart++];
                        m_CurrWorkPos.dbWorkC = (double)(BitConverter.ToSingle(byteWorkC, 0));

                    }
                }
            }

        }



        public virtual int Connect()
        {
            // int nErrorCode = 0;
            //byte[] byteRecvOK = { 0x02, 0x4F, 0x4B, 0x0D, 0x03 };    //OK,


            //簡易順序通信
            int nRet = 0;
            ToshibaClientIP0 = new ToshibaSocketClient();
            nRet = ToshibaClientIP0.ConnectRobot(m_strRobotIP, m_nRobotPort0);
            if (nRet < 0)   //連線失敗
                return -10;     //IP0 socket 連線失敗

            //   ToshibaClientIP1 = new ToshibaSocketClient();
            //   nRet = ToshibaClientIP1.ConnectRobot(m_strRobotIP, m_nRobotPort1);
            //   if (nRet < 0)   //連線失敗
            //      return -11;     //IP1 socket 連線失敗
            //


            //////////////////////////////////////////////////////
            //Servo On
            byte[] byteServoOn = { 0x02, 0x53, 0x4F, 0x0D, 0x03 };    //SO
            byte[] byteServoOnRecv = new byte[5];

            ToshibaClientIP0.SendData(byteServoOn);
            nRet = ToshibaClientIP0.Receive(ref byteServoOnRecv);
            if (nRet == 0)
            {
                if (BitConverter.ToString(byteServoOnRecv) != BitConverter.ToString(m_byteRecvOK))
                    return -100;    //手臂Servo ON 失敗
            }
            else
                return -10;

            //////////////////////////////////////////////////////

            /*
            //////////////////////////////////////////////////////
            //程式選擇
            //SL,PARSING
            byte[] byteProgram = Encoding.ASCII.GetBytes(m_strRobotProgram);
            byte[] byteProgramSel = new byte[5 + byteProgram.Length];
            byteProgramSel[0] = 0x02;
            byteProgramSel[1] = 0x53;
            byteProgramSel[2] = 0x4C;
            byteProgramSel[3] = 0x2C;
            Array.Copy(byteProgram, 0, byteProgramSel, 4, byteProgram.Length );
            byteProgramSel[4+ byteProgram.Length] = 0x03;

            nRet = ToshibaClientIP0.SendData(byteProgramSel);
            if (nRet == 0)
            {
                byte[] byteProgramSelRecv = new byte[5];
                ToshibaClientIP0.Receive(ref byteProgramSelRecv);
                if (BitConverter.ToString(byteProgramSelRecv) != BitConverter.ToString(m_byteRecvOK))
                    return -101;     //手臂選擇程式檔案失敗
            }
            //////////////////////////////////////////////////////
            */

            //Thread.Sleep(500);

            /*
            //////////////////////////////////////////////////////
            //執行程式
            byte[] byteRunProgram = { 0x02, 0x52, 0x4E, 0x03 };    //RN
            nRet = ToshibaClientIP0.SendData(byteRunProgram);
            if (nRet == byteRunProgram.Length)
            {
                byte[] byteRunProgramRecv = new byte[5];
                ToshibaClientIP0.Receive(ref byteRunProgramRecv);
                if (BitConverter.ToString(byteRunProgramRecv) != BitConverter.ToString(byteRecvOK))
                    return -102;      //手臂執行程式失敗
            }
            //////////////////////////////////////////////////////
            */

            // Thread.Sleep(2000);

            /*
            //////////////////////////////////////////////////////
            //寫入OVRD(速度百分比)
            string strOVRD = Convert.ToString(m_nRobotInitOVRD);
            byte[] byteOVRDValue = Encoding.ASCII.GetBytes(strOVRD);
            byte[] byteOVRD = new byte[10 + byteOVRDValue.Length];
            byteOVRD[0] = 0x02;     //STX
            byteOVRD[1] = 0x45;     //E
            byteOVRD[2] = 0x43;     //C
            byteOVRD[3] = 0x2C;     //,
            byteOVRD[4] = 0x4F;     //O
            byteOVRD[5] = 0x56;     //V
            byteOVRD[6] = 0x52;     //R
            byteOVRD[7] = 0x44;     //D
            byteOVRD[8] = 0x20;     //Space
            Array.Copy(byteOVRDValue, 0, byteOVRD, 9, byteOVRDValue.Length);
            byteOVRD[9 + byteOVRDValue.Length] = 0x03;    ///ETX

            //byte[] byteOVRD = { 0x02, 0x45, 0x43, 0x2C, 0x4F, 0x56, 0x52, 0x44, 0x20, 0x32, 0x30, 0x03 };    //BR

            byte[] byteOVRDRecv = new byte[5];

            ToshibaClientIP0.SendData(byteOVRD);
            nRet = ToshibaClientIP0.Receive(ref byteOVRDRecv);
            if (nRet == 0)
            {
                if (BitConverter.ToString(byteOVRDRecv) != BitConverter.ToString(m_byteRecvOK))
                    return -103;    //手臂設定OVRD失敗
            }
            else
                return -10;

            //////////////////////////////////////////////////////
            */

            //////////////////////////////////////////////////////
            //寫入手臂預設速度
            m_dbCurrSpeed = 60;
            m_dbCurrAcc = 60;
            m_dbCurrDec = 60;
            // string strAccCmd = string.Format("ACCEL = {0}", m_dbCurrAcc);
            // string strDecCmd = string.Format("DECEL = {0}", m_dbCurrDec);
            // string strSpeedCmd = string.Format("SPEED = {0}", m_dbCurrSpeed);

            // nRet = SendDoCmd(strAccCmd);       //傳送Acc
            // if (nRet != 0) return nRet;

            // nRet = SendDoCmd(strDecCmd);        //傳送Dec
            // if (nRet != 0) return nRet;

            // nRet = SendDoCmd(strSpeedCmd);      //傳送Speed
            // if (nRet != 0) return nRet;
            //////////////////////////////////////////////////////



            //開始執行 截取狀態執行緒
            Thread tGetStatusThread = new Thread(new ThreadStart(GetStatusThread));//執行緒開始的時候要調用的方法為threadProc.thread
            tGetStatusThread.IsBackground = true;//設置IsBackground=true,後臺執行緒會自動根據主執行緒的銷毀而銷毀
            tGetStatusThread.Start();


            /*
            //////////////////////////////////////////////////////
            //要求系統綜合狀態
            byte[] byteReqStatus = { 0x02, 0x53, 0x46, 0x0D, 0x03 };    //SF
            nRet = ToshibaClientIP0.SendData(byteReqStatus);
            if (nRet == byteServoOn.Length)
            {
                byte[] byteReqStatusRecv = new byte[360];
                ToshibaClientIP0.Receive(ref byteReqStatusRecv);

                Thread.Sleep(50);

                byte[] byteSendOK = { 0x02, 0x4F, 0x4B, 0x0D, 0x03 };    //OK,
                nRet = ToshibaClientIP0.SendData(byteSendOK);

                //120
                byte[] byteXPos = new byte[4];
                byte[] byteYPos = new byte[4];
                byte[] byteZPos = new byte[4];
                byte[] byteAPos = new byte[4];
                byte[] byteBPos = new byte[4];
                byte[] byteCPos = new byte[4];

                float fff = 39.333f;
                byte[] bytefff = BitConverter.GetBytes(fff);

                for (int i=0; i<4; i++)
                    byteXPos[3-i] = byteReqStatusRecv[120+i];

                float fJ1 = BitConverter.ToSingle(byteXPos, 0);

                for (int i = 0; i < 4; i++)
                    byteYPos[i] = byteReqStatusRecv[124 + i];

                float fJ2 = BitConverter.ToSingle(byteYPos, 0);

                for (int i = 0; i < 4; i++)
                    byteXPos[i] = byteReqStatusRecv[128 + i];

                for (int i = 0; i < 4; i++)
                    byteAPos[i] = byteReqStatusRecv[132 + i];

                for (int i = 0; i < 4; i++)
                    byteBPos[i] = byteReqStatusRecv[136 + i];

                for (int i = 0; i < 4; i++)
                    byteCPos[i] = byteReqStatusRecv[140 + i];

            }
            //////////////////////////////////////////////////////
            */

            /*
            int nErrorCode = 0;
            try
            {
                if (m_Robot == null)
                {
                    m_Robot = new TsRemoteV();
                    m_Robot.SetIPaddr(0, m_strRobotIP, m_nRobotPort0);
                }

                m_bIP0Connected = m_Robot.Connect(1);
                if (m_bIP0Connected)
                {
                    m_Robot.WatchDogStart(100, 300, 0, new TsRemoteV.TSStatusEvent(testTSStatusEvent));
                    m_Robot.ServoOn();
                    m_Robot.SetOVRD(m_nRobotInitOVRD);
                    m_dbCurrSpeed = 60 ;
                    m_dbCurrAcc = 100 ;
                    m_dbCurrDec = 100 ;
                    m_Robot.MvSpeed = m_dbCurrSpeed;
                    m_Robot.MvAccel = m_dbCurrAcc * 1000;
                    m_Robot.MvDecel = m_dbCurrDec * 1000;

                    //m_Robot.ProgramSelect(m_strRobotProgram);
                    //m_Robot.ProgramRun();

                }
                else
                {
                    nErrorCode = -1;
                }
            }
            catch (TsRemoteSException ex)
            {
                //TsRemoteSException.ErrorCode.TCPError
                //string str = ex.errorCode.ToString();
                //return Convert.ToInt32(ex.errorCode);
                // return -1;
                nErrorCode = Convert.ToInt32(ex.errorCode);
                System.Console.WriteLine("Connect 例外 : ErrCode = {0} , {1} !", nErrorCode, ex.errorCode.ToString());
            }
            
            return nErrorCode;
            */

            return 0;
        }

        public virtual int Disconnect()
        {
            int nErrorCode = 0;
            //byte[] byteRecvOK = { 0x02, 0x4F, 0x4B, 0x0D, 0x03 };    //OK,

            lock (thisLock)
            {

                if (ToshibaClientIP0.IsConnected())
                {

                    int nRet = 0;
                    //////////////////////////////////////////////////////
                    //程式停止
                    // byte[] byteProgramStop = { 0x02, 0x53, 0x50, 0x0D, 0x03 };    //SP
                    // nRet = ToshibaClientIP0.SendData(byteProgramStop);
                    // if (nRet == byteProgramStop.Length)
                    // {
                    //     byte[] byteProgramStopRecv = new byte[5];
                    //     ToshibaClientIP0.Receive(ref byteProgramStopRecv);
                    //     if (BitConverter.ToString(byteProgramStopRecv) != BitConverter.ToString(byteRecvOK))
                    //         return -105;    //手臂停止程式 失敗
                    // }
                    //////////////////////////////////////////////////////



                    //////////////////////////////////////////////////////
                    //Servo Off
                    byte[] byteServoOff = { 0x02, 0x42, 0x52, 0x0D, 0x03 };    //BR
                    byte[] byteServoOffRecv = new byte[5];

                    ToshibaClientIP0.SendData(byteServoOff);
                    nRet = ToshibaClientIP0.Receive(ref byteServoOffRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteServoOffRecv) != BitConverter.ToString(m_byteRecvOK))
                            return -104;    //手臂Servo OFF 失敗
                    }
                    else
                        return -10;

                    //////////////////////////////////////////////////////

                    //斷線IP0
                    nRet = ToshibaClientIP0.DisconnectRobot();
                    if (nRet < 0)
                        return -12;     //IP0 socket 斷線失敗  

                }
            }

            return nErrorCode;
        }


        public virtual int GetJointDegree(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            dbJ1 = m_CurrJointPos.dbJ1;
            dbJ2 = m_CurrJointPos.dbJ2;
            dbJ3 = m_CurrJointPos.dbJ3;
            dbJ4 = m_CurrJointPos.dbJ4;
            return 0;
        }

        public virtual int GetJointRadian(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            return 0;
        }

        public virtual int GetWorldPos(ref double dbX, ref double dbY, ref double dbZ, ref double dbRx, ref double dbRy, ref double dbRz)
        {
            dbX = m_CurrWorldPos.dbWorldX;
            dbY = m_CurrWorldPos.dbWorldY;
            dbZ = m_CurrWorldPos.dbWorldZ;   
            dbRz = m_CurrWorldPos.dbWorldC;
            return 0; 
        }

        //擷取 6關節電流
        public virtual int GetJointCurrentValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            return 0;
        }

        //擷取 6關節電壓
        public virtual int GetJointVoltageValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            return 0;
        }

        //擷取 6關節扭矩
        public virtual int GetJointTorqueValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            return 0;
        }

        //擷取 6關節溫度
        public virtual int GetJointTemperatureValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            return 0;
        }

        //擷取 6關節速度
        public virtual int GetJointSpeedValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            return 0;
        }

        //擷取 6關節加速度
        public virtual int GetJointAccValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {

            return 0;
        }

        //關節Jog 移動
        public virtual int JogJoint(Joints nJoint, Jog_Directions nDir, Jog_Speed nSpeed)
        {
            lock (thisLock)
            {
                int nRet = 0;

                //Jog 移動方式
                ////////////////////////////////////////
                byte[] byteMd = { 0x02, 0x4D, 0x44, 0x2C, 0x30, 0x0D, 0x03 };    //MD,0
                byte[] byteMdRecv = new byte[5];
                ToshibaClientIP0.SendData(byteMd);
                nRet = ToshibaClientIP0.Receive(ref byteMdRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteMdRecv) != BitConverter.ToString(m_byteRecvOK))
                        return -104;
                }
                else
                    return -10; //通訊發生錯誤  

                ////////////////////////////////////////

                //Jog 速度
                ////////////////////////////////////////
                string strJogSpeed = "";
                if (nSpeed == Jog_Speed.High)
                    strJogSpeed = "RT,2";
                else if (nSpeed == Jog_Speed.Mid)
                    strJogSpeed = "RT,1";
                else if (nSpeed == Jog_Speed.Low)
                    strJogSpeed = "RT,0";

                byte[] byteJogSpeed = Encoding.ASCII.GetBytes(strJogSpeed);
                byte[] byteSetJogSpeed = new byte[3 + byteJogSpeed.Length];
                byteSetJogSpeed[0] = 0x02;
                Array.Copy(byteJogSpeed, 0, byteSetJogSpeed, 1, byteJogSpeed.Length);
                byteSetJogSpeed[1 + byteJogSpeed.Length] = 0x0D;  //CR
                byteSetJogSpeed[2 + byteJogSpeed.Length] = 0x03;

                byte[] byteJogSpeedRecv = new byte[5];
                ToshibaClientIP0.SendData(byteSetJogSpeed);
                nRet = ToshibaClientIP0.Receive(ref byteJogSpeedRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteJogSpeedRecv) != BitConverter.ToString(m_byteRecvOK))
                        return -100;
                }
                else
                    return -10; //通訊發生錯誤  
                ////////////////////////////////////////


                //Jog 座標設定
                ////////////////////////////////////////
                byte[] byteJogCoord = { 0x02, 0x53, 0x43, 0x2C, 0x30, 0x0D, 0x03 };    //SC,0
                byte[] byteJogCoordRecv = new byte[5];

                ToshibaClientIP0.SendData(byteJogCoord);
                nRet = ToshibaClientIP0.Receive(ref byteJogCoordRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteJogCoordRecv) != BitConverter.ToString(m_byteRecvOK))
                        return -104;    //手臂Servo OFF 失敗
                }
                else
                    return -10; //通訊發生錯誤  

                ////////////////////////////////////////


                //Jog 動作
                ////////////////////////////////////////
                string strJogMove = "";
                strJogMove = string.Format("JG,{0} ", (int)nJoint);

                if (nDir == Jog_Directions.Plus)
                    strJogMove = strJogMove + "+";
                else
                    strJogMove = strJogMove + "-";


                byte[] byteJogMove = Encoding.ASCII.GetBytes(strJogMove);
                byte[] byteSetJogMove = new byte[3 + byteJogMove.Length];
                byteSetJogMove[0] = 0x02;
                Array.Copy(byteJogMove, 0, byteSetJogMove, 1, byteJogMove.Length);
                byteSetJogMove[1 + byteJogMove.Length] = 0x0D;  //CR
                byteSetJogMove[2 + byteJogMove.Length] = 0x03;

                byte[] byteJogMoveRecv = new byte[5];
                ToshibaClientIP0.SendData(byteSetJogMove);
                nRet = ToshibaClientIP0.Receive(ref byteJogMoveRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteJogMoveRecv) != BitConverter.ToString(m_byteRecvOK))
                        return -100;
                }
                else
                    return -10; //通訊發生錯誤  

                ////////////////////////////////////////

            }

            return 0;

        }

        //關節Jog 停止
        public virtual int JogJointStop(Joints nJoint)
        {
            lock (thisLock)
            {
                byte[] byteRecvOK = { 0x02, 0x4F, 0x4B, 0x0D, 0x03 };    //OK,
                int nRet = 0;
                //Jog 動作
                ////////////////////////////////////////
                string strJogMove = "";
                strJogMove = string.Format("JG,{0} !", (int)nJoint);

                byte[] byteJogMove = Encoding.ASCII.GetBytes(strJogMove);
                byte[] byteSetJogMove = new byte[3 + byteJogMove.Length];
                byteSetJogMove[0] = 0x02;
                Array.Copy(byteJogMove, 0, byteSetJogMove, 1, byteJogMove.Length);
                byteSetJogMove[1 + byteJogMove.Length] = 0x0D;  //CR
                byteSetJogMove[2 + byteJogMove.Length] = 0x03;

                byte[] byteJogMoveRecv = new byte[5];
                ToshibaClientIP0.SendData(byteSetJogMove);
                nRet = ToshibaClientIP0.Receive(ref byteJogMoveRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteJogMoveRecv) != BitConverter.ToString(byteRecvOK))
                        return -100;
                }
                else
                    return -10; //通訊發生錯誤  


                ////////////////////////////////////////

            }
            return 0;
        }

        //座標系Jog 移動
        public virtual int JogCoordnate(Coordnates nCoordnate, Jog_Directions nDir, Jog_Speed nSpeed)
        {
            lock (thisLock)
            {
                if (nCoordnate == Coordnates.Rx || nCoordnate == Coordnates.Ry)
                    return -104;

                int nRet = 0;

                //Jog 移動方式
                ////////////////////////////////////////
                byte[] byteMd = { 0x02, 0x4D, 0x44, 0x2C, 0x30, 0x0D, 0x03 };    //MD,0
                byte[] byteMdRecv = new byte[5];

                ToshibaClientIP0.SendData(byteMd);
                nRet = ToshibaClientIP0.Receive(ref byteMdRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteMdRecv) != BitConverter.ToString(m_byteRecvOK))
                        return -104;
                }
                else
                    return -10; //通訊發生錯誤  

                ////////////////////////////////////////

                //Jog 速度
                ////////////////////////////////////////
                string strJogSpeed = "";
                if (nSpeed == Jog_Speed.High)
                    strJogSpeed = "RT,2";
                else if (nSpeed == Jog_Speed.Mid)
                    strJogSpeed = "RT,1";
                else if (nSpeed == Jog_Speed.Low)
                    strJogSpeed = "RT,0";

                byte[] byteJogSpeed = Encoding.ASCII.GetBytes(strJogSpeed);
                byte[] byteSetJogSpeed = new byte[3 + byteJogSpeed.Length];
                byteSetJogSpeed[0] = 0x02;
                Array.Copy(byteJogSpeed, 0, byteSetJogSpeed, 1, byteJogSpeed.Length);
                byteSetJogSpeed[1 + byteJogSpeed.Length] = 0x0D;  //CR
                byteSetJogSpeed[2 + byteJogSpeed.Length] = 0x03;

                byte[] byteJogSpeedRecv = new byte[5];
                ToshibaClientIP0.SendData(byteSetJogSpeed);
                nRet = ToshibaClientIP0.Receive(ref byteJogSpeedRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteJogSpeedRecv) != BitConverter.ToString(m_byteRecvOK))
                        return -100;
                }
                else
                    return -10; //通訊發生錯誤  

                ////////////////////////////////////////


                //Jog 座標設定
                ////////////////////////////////////////
                byte[] byteJogCoord = { 0x02, 0x53, 0x43, 0x2C, 0x33, 0x0D, 0x03 };    //SC,3
                byte[] byteJogCoordRecv = new byte[5];
                ToshibaClientIP0.SendData(byteJogCoord);
                nRet = ToshibaClientIP0.Receive(ref byteJogCoordRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteJogCoordRecv) != BitConverter.ToString(m_byteRecvOK))
                        return -104;    //手臂Servo OFF 失敗
                }
                else
                    return -10; //通訊發生錯誤  

                ////////////////////////////////////////


                //Jog 動作
                ////////////////////////////////////////
                int nTempCoordate = (int)nCoordnate;

                if (nTempCoordate == (int)Coordnates.Rz)
                    nTempCoordate = 4;

                string strJogMove = "";
                //strJogMove = string.Format("JG,{0} ", (int)nCoordnate);
                strJogMove = string.Format("JG,{0} ", (int)nTempCoordate);

                if (nDir == Jog_Directions.Plus)
                    strJogMove = strJogMove + "+";
                else
                    strJogMove = strJogMove + "-";


                byte[] byteJogMove = Encoding.ASCII.GetBytes(strJogMove);
                byte[] byteSetJogMove = new byte[3 + byteJogMove.Length];
                byteSetJogMove[0] = 0x02;
                Array.Copy(byteJogMove, 0, byteSetJogMove, 1, byteJogMove.Length);
                byteSetJogMove[1 + byteJogMove.Length] = 0x0D;  //CR
                byteSetJogMove[2 + byteJogMove.Length] = 0x03;

                byte[] byteJogMoveRecv = new byte[5];
                ToshibaClientIP0.SendData(byteSetJogMove);
                nRet = ToshibaClientIP0.Receive(ref byteJogMoveRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteJogMoveRecv) != BitConverter.ToString(m_byteRecvOK))
                        return -100;
                }
                else
                    return -10; //通訊發生錯誤  

                ////////////////////////////////////////
            }


            return 0;
        }

        //座標系Jog 停止
        public virtual int JogCoordnateStop(Coordnates nCoordnate)
        {
            lock (thisLock)
            {
                if (nCoordnate == Coordnates.Rx || nCoordnate == Coordnates.Ry)
                    return -104;

                int nRet = 0;
                //Jog 動作
                ////////////////////////////////////////
                int nTempCoordate = (int)nCoordnate;

                if (nTempCoordate == (int)Coordnates.Rz)
                    nTempCoordate = 4;

                string strJogMove = "";
                //strJogMove = string.Format("JG,{0} !", (int)nCoordnate);
                strJogMove = string.Format("JG,{0} !", nTempCoordate);

                byte[] byteJogMove = Encoding.ASCII.GetBytes(strJogMove);
                byte[] byteSetJogMove = new byte[3 + byteJogMove.Length];
                byteSetJogMove[0] = 0x02;
                Array.Copy(byteJogMove, 0, byteSetJogMove, 1, byteJogMove.Length);
                byteSetJogMove[1 + byteJogMove.Length] = 0x0D;  //CR
                byteSetJogMove[2 + byteJogMove.Length] = 0x03;

                byte[] byteJogMoveRecv = new byte[5];
                ToshibaClientIP0.SendData(byteSetJogMove);
                nRet = ToshibaClientIP0.Receive(ref byteJogMoveRecv);
                if (nRet == 0)
                {
                    if (BitConverter.ToString(byteJogMoveRecv) != BitConverter.ToString(m_byteRecvOK))
                        return -100;
                }
                else
                    return -10; //通訊發生錯誤  

                ////////////////////////////////////////

            }
            return 0;
        }

        //在直角坐標系上 點對點 同動 (非直線)
        public virtual int MovePtoP(SpeedTypes nSpeedType, WaitTypes nWaitType,
                     double dbXValue, double dbYValue, double dbZValue, double dbRxValue, double dbRyValue, double dbRzValue, double dbAcc, double dbDec, double dbSpeed)
        {

            int nErrorCode = 0;
            int nRet = 0;
            lock (thisLock)
            {
                try
                {
                    //速度型態選擇
                    switch (nSpeedType)
                    {
                        case SpeedTypes.Use:

                            //設定新的各項速度
                            if (dbSpeed > 100) dbSpeed = 100;
                            if (dbSpeed < 1) dbSpeed = 1;
                            if (dbAcc > 100) dbAcc = 100;
                            if (dbAcc < 1) dbAcc = 1;
                            if (dbDec > 100) dbDec = 100;
                            if (dbDec < 1) dbDec = 1;

                            //MOVE 命令
                            string strMoveCmd = string.Format("MOVE POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###}) WITH ACCEL={4}, DECEL={5}, SPEED={6}",
                                                               dbXValue, dbYValue, dbZValue, dbRzValue, dbAcc, dbDec, dbSpeed);
                            //傳送MOVE
                            nRet = SendDoCmd(strMoveCmd);
                            if (nRet != 0) return nRet;


                            // System.Threading.Thread.Sleep(60);
                            // ConfigV cv1 = ConfigV.LANSS;
                            //  m_Robot.Move(dbXValue, dbYValue, dbZValue, dbRxValue, dbRyValue, dbRzValue, 0, 0, cv1);

                            break;

                        case SpeedTypes.NoUse:
                            //MOVE 命令
                            string strMoveCmd2 = string.Format("MOVE POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###}) WITH ACCEL={4}, DECEL={5}, SPEED={6}",
                                                                dbXValue, dbYValue, dbZValue, dbRzValue, m_dbCurrAcc, m_dbCurrDec, m_dbCurrSpeed);
                            //傳送 DO MOVE
                            nRet = SendDoCmd(strMoveCmd2);
                            if (nRet != 0) return nRet;

                            //System.Threading.Thread.Sleep(60);
                            //ConfigV cv2 = ConfigV.LANSS;
                            //m_Robot.Move(dbXValue, dbYValue, dbZValue, dbRxValue, dbRyValue, dbRzValue, 0, 0, cv2);
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("MovePtoP 例外 : ErrCode = {0} , {1} !", nErrorCode, ex.ToString());
                    return nErrorCode;
                }

            }

            // 等待手臂開始運動
            int nStatus = 0;
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(33);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //先確定手臂已經開始運動, 運動後離開迴圈
                    if (nStatus == 1)
                        break;
                }
            }

            //等待手臂停止
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(1);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //確定手臂已經停止, 停止後離開迴圈
                    if (nStatus != 1)
                        break;
                }
            }

            return nErrorCode;
        }

        //在直角坐標系上 點對點 直線運動
        public virtual int LinePtoP(SpeedTypes nSpeedType, WaitTypes nWaitType,
                      double dbXValue, double dbYValue, double dbZValue, double dbRxValue, double dbRyValue, double dbRzValue, double dbAcc, double dbDec, double dbSpeed)
        {
            int nErrorCode = 0;
            int nRet = 0;
            lock (thisLock)
            {
                try
                {
                    //速度型態選擇
                    switch (nSpeedType)
                    {
                        case SpeedTypes.Use:

                            //設定新的各項速度
                            if (dbSpeed > 100) dbSpeed = 100;
                            if (dbSpeed < 1) dbSpeed = 1;
                            if (dbAcc > 100) dbAcc = 100;
                            if (dbAcc < 1) dbAcc = 1;
                            if (dbDec > 100) dbDec = 100;
                            if (dbDec < 1) dbDec = 1;

                            //MOVES 命令
                            string strMovesCmd = string.Format("MOVES POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###}) WITH ACCEL={4}, DECEL={5}, SPEED={6}",
                                                               dbXValue, dbYValue, dbZValue, dbRzValue, dbAcc, dbDec, dbSpeed);

                            //傳送MOVES
                            nRet = SendDoCmd(strMovesCmd);
                            if (nRet != 0) return nRet;

                            //System.Threading.Thread.Sleep(60);
                            //ConfigV cv1 = ConfigV.LANSS;
                            //m_Robot.Moves(dbXValue, dbYValue, dbZValue, dbRxValue, dbRyValue, dbRzValue, 0, 0, cv1);

                            break;
                        case SpeedTypes.NoUse:

                            //MOVES 命令
                            string strMovesCmd2 = string.Format("MOVES POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###}) WITH ACCEL={4}, DECEL={5}, SPEED={6}",
                                                               dbXValue, dbYValue, dbZValue, dbRzValue, m_dbCurrAcc, m_dbCurrDec, m_dbCurrSpeed);
                            //傳送 DO MOVE
                            nRet = SendDoCmd(strMovesCmd2);
                            if (nRet != 0) return nRet;

                            // System.Threading.Thread.Sleep(60);
                            //ConfigV cv2 = ConfigV.LANSS;
                            //m_Robot.Moves(dbXValue, dbYValue, dbZValue, dbRxValue, dbRyValue, dbRzValue, 0, 0, cv2);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("LinePtoP 例外 : ErrCode = {0} , {1} !", nErrorCode, ex.ToString());
                    return nErrorCode;
                }
            }

            // 等待手臂開始運動
            int nStatus = 0;
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(33);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //先確定手臂已經開始運動, 運動後離開迴圈
                    if (nStatus == 1)
                        break;
                }
            }

            //等待手臂停止
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(1);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //確定手臂已經停止, 停止後離開迴圈
                    if (nStatus != 1)
                        break;
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
            int nRet = 0;
            lock (thisLock)
            {
                try
                {
                    //速度型態選擇
                    switch (nSpeedType)
                    {
                        case SpeedTypes.Use:

                            //設定新的各項速度
                            if (dbSpeed > 100) dbSpeed = 100;
                            if (dbSpeed < 1) dbSpeed = 1;
                            if (dbAcc > 100) dbAcc = 100;
                            if (dbAcc < 1) dbAcc = 1;
                            if (dbDec > 100) dbDec = 100;
                            if (dbDec < 1) dbDec = 1;

                            //弧中點位置
                            string strMidPt = string.Format(" POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###})",
                                       dbMidXValue, dbMidYValue, dbMidZValue, dbMidRzValue);
                            //弧終點位置
                            string strEndPt = string.Format(" POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###})",
                                       dbEndXValue, dbEndYValue, dbEndZValue, dbEndRzValue);

                            //圓弧命令
                            string strArcCmd = string.Format("MOVEC {0} {1} WITH ACCEL={2}, DECEL={3}, SPEED={4}",
                                                              strMidPt, strEndPt, dbAcc, dbDec, dbSpeed);
                            //傳送 ARC
                            nRet = SendDoCmd(strArcCmd);
                            if (nRet != 0) return nRet;

                            break;
                        case SpeedTypes.NoUse:

                            //弧中點位置
                            string strMidPt2 = string.Format(" POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###})",
                                        dbMidXValue, dbMidYValue, dbMidZValue, dbMidRzValue);

                            //弧終點位置
                            string strEndPt2 = string.Format(" POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###})",
                                       dbEndXValue, dbEndYValue, dbEndZValue, dbEndRzValue);

                            //圓弧命令
                            string strArcCmd2 = string.Format("MOVEC {0} {1} WITH ACCEL={2}, DECEL={3}, SPEED={4}", strMidPt2, strEndPt2, m_dbCurrAcc, m_dbCurrDec, m_dbCurrSpeed);


                            //傳送 Arc
                            nRet = SendDoCmd(strArcCmd2);
                            if (nRet != 0) return nRet;

                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("Arc 例外 : ErrCode = {0} , {1} !", nErrorCode, ex.ToString());
                    return nErrorCode;
                }
            }

            // 等待手臂開始運動
            int nStatus = 0;
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(33);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //先確定手臂已經開始運動, 運動後離開迴圈
                    if (nStatus == 1)
                        break;
                }
            }

            //等待手臂停止
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(1);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //確定手臂已經停止, 停止後離開迴圈
                    if (nStatus != 1)
                        break;
                }
            }

            return nErrorCode;
        }


        //單一關節 移動角度
        public virtual int MoveJoint(Joints nJoint, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
                                    double dbValue, double dbAcc, double dbDec, double dbSpeed)
        {
            int nErrorCode = 0;
            int nRet = 0;
            lock (thisLock)
            {
                try
                {
                    double dbDegree = 0;

                    //移動量模式選擇
                    switch (nMovementType)
                    {
                        case MovementsTypes.Abs:
                            dbDegree = dbValue;
                            break;
                        case MovementsTypes.Rel:
                            //先記錄目前各關節位置(角度)

                            double dbJ1 = 0, dbJ2 = 0, dbJ3 = 0, dbJ4 = 0 ;
                            nRet = GetRobotJointsPosCmd(ref dbJ1, ref dbJ2, ref dbJ3, ref dbJ4);
                            if (nRet != 0) return nRet;

                            switch (nJoint)
                            {
                                case Joints.J1: dbDegree = dbJ1 + dbValue; break;
                                case Joints.J2: dbDegree = dbJ2 + dbValue; break;
                                case Joints.J3: dbDegree = dbJ3 + dbValue; break;
                                case Joints.J4: dbDegree = dbJ4 + dbValue; break;
                                
                            }
                            break;
                    }

                    switch (nSpeedType)
                    {
                        case SpeedTypes.Use:
                            //設定新的各項速度
                            if (dbSpeed > 100) dbSpeed = 100;
                            if (dbSpeed < 1) dbSpeed = 1;
                            if (dbAcc > 100) dbAcc = 100;
                            if (dbAcc < 1) dbAcc = 1;
                            if (dbDec > 100) dbDec = 100;
                            if (dbDec < 1) dbDec = 1;

                            //MOVEA 命令
                            string strMoveaCmd = string.Format("MOVEA {0}, {1} WITH ACCEL={2}, DECEL={3}, SPEED={4}",
                                                               (int)nJoint, dbDegree, dbAcc, dbDec, dbSpeed);
                            //傳送MOVEA
                            nRet = SendDoCmd(strMoveaCmd);
                            if (nRet != 0) return nRet;


                            break;
                        case SpeedTypes.NoUse:

                            //MOVEA 命令
                            string strMoveaCmd2 = string.Format("MOVEA {0}, {1} WITH ACCEL={2}, DECEL={3}, SPEED={4}",
                                                              (int)nJoint, dbDegree, m_dbCurrAcc, m_dbCurrDec, m_dbCurrSpeed);
                            //傳送MOVEA
                            nRet = SendDoCmd(strMoveaCmd2);
                            if (nRet != 0) return nRet;

                            break;
                    }

                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("MoveJoint 例外 : ErrCode = {0} , {1} !", nErrorCode, ex.ToString());
                    return nErrorCode;
                }
            }

            // 等待手臂開始運動
            int nStatus = 0;
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(33);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //先確定手臂已經開始運動, 運動後離開迴圈
                    if (nStatus == 1)
                        break;
                }
            }

            //等待手臂停止
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(1);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //確定手臂已經停止, 停止後離開迴圈
                    if (nStatus != 1)
                        break;
                }
            }

            return nErrorCode;
        }


        //單一直角坐標系方向 移動
        public virtual int LineCoordnate(Coordnates nCoordnate, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
                        double dbValue, double dbAcc, double dbDec, double dbSpeed)
        {
            int nErrorCode = 0;
            int nRet = 0;
            lock (thisLock)
            {
                try
                {

                    //先記錄目前各XYZ位置
                    double dbX = 0, dbY = 0, dbZ = 0, dbRz = 0;
                    GetRobotWorldPosCmd(ref dbX, ref dbY, ref dbZ, ref dbRz);
                    if (nRet != 0) return nRet;

                    //移動量模式選擇
                    switch (nMovementType)
                    {
                        case MovementsTypes.Abs:
                            switch (nCoordnate)
                            {
                                case Coordnates.X: dbX = dbValue; break;
                                case Coordnates.Y: dbY = dbValue; break;
                                case Coordnates.Z: dbZ = dbValue; break;
                                case Coordnates.Rz: dbRz = dbValue; break;
                            }
                            break;
                        case MovementsTypes.Rel:
                            switch (nCoordnate)
                            {
                                case Coordnates.X: dbX = dbX + dbValue; break;
                                case Coordnates.Y: dbY = dbY + dbValue; break;
                                case Coordnates.Z: dbZ = dbZ + dbValue; break;
                                case Coordnates.Rz: dbRz = dbRz + dbValue; break;
                            }
                            break;
                    }

                    switch (nSpeedType)
                    {
                        case SpeedTypes.Use:
                            //設定新的各項速度
                            if (dbSpeed > 100) dbSpeed = 100;
                            if (dbSpeed < 1) dbSpeed = 1;
                            if (dbAcc > 100) dbAcc = 100;
                            if (dbAcc < 1) dbAcc = 1;
                            if (dbDec > 100) dbDec = 100;
                            if (dbDec < 1) dbDec = 1;

                            //MOVES 命令
                            string strMovesCmd = string.Format("MOVES POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###}) WITH ACCEL={4}, DECEL={5}, SPEED={6}",
                                                               dbX, dbY, dbZ, dbRz, dbAcc, dbDec, dbSpeed);
                            //傳送MOVES
                            nRet = SendDoCmd(strMovesCmd);
                            if (nRet != 0) return nRet;

                            break;
                        case SpeedTypes.NoUse:

                            //MOVES 命令
                            string strMovesCmd2 = string.Format("MOVES POINT ({0:0.###},{1:0.###},{2:0.###},{3:0.###}) WITH ACCEL={4}, DECEL={5}, SPEED={6}",
                                                               dbX, dbY, dbZ, dbRz, m_dbCurrAcc, m_dbCurrDec, m_dbCurrSpeed);

                            //傳送MOVES
                            nRet = SendDoCmd(strMovesCmd2);
                            if (nRet != 0) return nRet;

                            break;
                    }

                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("LineCoordnate 例外 : ErrCode = {0} , {1} !", nErrorCode, ex.ToString());
                    return nErrorCode;
                }
            }

            // 等待手臂開始運動
            int nStatus = 0;
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(33);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //先確定手臂已經開始運動, 運動後離開迴圈
                    if (nStatus == 1)
                        break;
                }
            }

            //等待手臂停止
            while (true)
            {
                lock (thisLock)
                {
                    System.Threading.Thread.Sleep(1);
                    nRet = GetRobotStatusCmd(ref nStatus);
                    if (nRet != 0) return nRet;

                    //確定手臂已經停止, 停止後離開迴圈
                    if (nStatus != 1)
                        break;
                }
            }

            return nErrorCode;

        }


        public virtual int MoveAllJoint(MovementsTypes nMovementType, UnitTypes nUnit, SpeedTypes nSpeedType, WaitTypes nWaitType,
                                double dbJ1Value, double dbJ2Value, double dbJ3Value, double dbJ4Value, double dbJ5Value, double dbJ6Value, double dbAcc, double dbDec, double dbSpeed)
        {
         
            return 0;
        }


        public virtual int LineSingleCoordnate(Coordnates nCoordnate, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
                       double dbValue, double dbAccPercentage, double dbDecPercentage, double dbSpeedPercentage)
        {  
            return 0;
        }


        public virtual int StopMotionForPtoP(double dbDec)
        {
            int nErrorCode = 0;
            lock (thisLock)
            {
                try
                {
                    byte[] byteBreak = { 0x02, 0x45, 0x43, 0x2C, 0x42, 0x52, 0x45, 0x41, 0x4B, 0x0D, 0x03 };    //EC,BREAK
                    byte[] byteBreakRecv = new byte[5];
                    int nRet = 0;
                    ToshibaClientIP0.SendData(byteBreak);
                    nRet = ToshibaClientIP0.Receive(ref byteBreakRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteBreakRecv) != BitConverter.ToString(m_byteRecvOK))
                            return -100;    //手臂Servo ON 失敗
                    }
                    else
                        return -10;

                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("StopMotionForPtoP 例外 : ErrCode = {0} , {1} !", nErrorCode, ex.ToString());
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
 
                    byte[] byteBreak = { 0x02, 0x45, 0x43, 0x2C, 0x42, 0x52, 0x45, 0x41, 0x4B, 0x0D, 0x03 };    //EC,BREAK
                    byte[] byteBreakRecv = new byte[5];
                    int nRet = 0;
                    ToshibaClientIP0.SendData(byteBreak);
                    nRet = ToshibaClientIP0.Receive(ref byteBreakRecv);
                    if (nRet == 0)
                    {
                        if (BitConverter.ToString(byteBreakRecv) != BitConverter.ToString(m_byteRecvOK))
                            return -100;    //手臂Servo ON 失敗
                    }
                    else
                        return -10;
                }
                catch (Exception ex)
                {
                    nErrorCode = -10;
                    System.Console.WriteLine("StopMotionForLine 例外 : ErrCode = {0} , {1} !", nErrorCode, ex.ToString());
                    return nErrorCode;  
                }

            }

            return nErrorCode;
        }


        private int SendDoCmd(string strCmd)
        {
            string strDoCmd = string.Format("DO,{0}", strCmd);

            byte[] byteDoCmd = Encoding.ASCII.GetBytes(strDoCmd);
            byte[] byteSetDo = new byte[3 + byteDoCmd.Length];
            byteSetDo[0] = 0x02;
            Array.Copy(byteDoCmd, 0, byteSetDo, 1, byteDoCmd.Length);
            byteSetDo[1 + byteDoCmd.Length] = 0x0D;  //CR
            byteSetDo[2 + byteDoCmd.Length] = 0x03;

            int nRet = 0;
            byte[] byteRecv = new byte[5];

            ToshibaClientIP0.SendData(byteSetDo);
            nRet = ToshibaClientIP0.Receive(ref byteRecv);
            if (nRet == 0)
            {
                if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteRecvOK))
                    return -100;
            }
            else
                return -10; //通訊發生錯誤  

            ////////////////////////////////////////

            return 0;
        }


        private int SendMPCmd(string strCmd)
        {
            string strMPCmd = string.Format("MP,0 {0}", strCmd);

            byte[] byteMPCmd = Encoding.ASCII.GetBytes(strMPCmd);
            byte[] byteSetMP = new byte[3 + byteMPCmd.Length];
            byteSetMP[0] = 0x02;
            Array.Copy(byteMPCmd, 0, byteSetMP, 1, byteMPCmd.Length);
            byteSetMP[1 + byteMPCmd.Length] = 0x0D;  //CR
            byteSetMP[2 + byteMPCmd.Length] = 0x03;

            int nRet = 0;
            byte[] byteRecv = new byte[5];

            ToshibaClientIP0.SendData(byteSetMP);
            nRet = ToshibaClientIP0.Receive(ref byteRecv);
            if (nRet == 0)
            {
                if (BitConverter.ToString(byteRecv) != BitConverter.ToString(m_byteRecvOK))
                    return -100;
            }
            else
                return -10; //通訊發生錯誤  

            ////////////////////////////////////////

            return 0;
        }


        private int GetRobotStatusCmd(ref int nStatus)
        {
            byte[] byteSFCmd = { 0x02, 0x53, 0x46, 0x0D, 0x03 };    //SF
            byte[] byteRecv = new byte[300];
            int nRet = 0;

            ToshibaClientIP0.SendData(byteSFCmd);
            nRet = ToshibaClientIP0.Receive(ref byteRecv);
            if (nRet == 0)
            {
                System.Threading.Thread.Sleep(60);          //手冊上說 收到回傳值後, 最少要延遲50ms, 再回傳OK 給控制器
                ToshibaClientIP0.SendData(m_byteSendOK);    //回傳OK 給控制器

                //擷取目前手臂狀態
                nStatus = (int)byteRecv[2 + 4];
            }
            else
                return -10; //通訊發生錯誤  

            ////////////////////////////////////////

            return 0;
        }


        private int GetRobotJointsPosCmd(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4)
        {
            // byte[] bytePRCmd = { 0x02, 0x50, 0x52, 0x2C, 0x30, 0x0D, 0x03 };    //PR,0
            byte[] byteSFCmd = { 0x02, 0x53, 0x46, 0x0D, 0x03 };    //SF
            byte[] byteRecv = new byte[300];
            int nRet = 0;

            byte[] byteJ1 = new byte[4];
            byte[] byteJ2 = new byte[4];
            byte[] byteJ3 = new byte[4];
            byte[] byteJ4 = new byte[4];
           

            ToshibaClientIP0.SendData(byteSFCmd);
            nRet = ToshibaClientIP0.Receive(ref byteRecv);
            if (nRet == 0)
            {
                System.Threading.Thread.Sleep(64);          //手冊上說 收到回傳值後, 最少要延遲50ms, 再回傳OK 給控制器
                ToshibaClientIP0.SendData(m_byteSendOK);    //回傳OK 給控制器

                //擷取目前手臂狀態
                int nJointDataStart = 0;
                byteJ1[3] = byteRecv[120 + nJointDataStart++];
                byteJ1[2] = byteRecv[120 + nJointDataStart++];
                byteJ1[1] = byteRecv[120 + nJointDataStart++];
                byteJ1[0] = byteRecv[120 + nJointDataStart++];
                dbJ1 = (double)(BitConverter.ToSingle(byteJ1, 0));

                byteJ2[3] = byteRecv[120 + nJointDataStart++];
                byteJ2[2] = byteRecv[120 + nJointDataStart++];
                byteJ2[1] = byteRecv[120 + nJointDataStart++];
                byteJ2[0] = byteRecv[120 + nJointDataStart++];
                dbJ2 = (double)(BitConverter.ToSingle(byteJ2, 0));

                byteJ3[3] = byteRecv[120 + nJointDataStart++];
                byteJ3[2] = byteRecv[120 + nJointDataStart++];
                byteJ3[1] = byteRecv[120 + nJointDataStart++];
                byteJ3[0] = byteRecv[120 + nJointDataStart++];
                dbJ3 = (double)(BitConverter.ToSingle(byteJ3, 0));

                byteJ4[3] = byteRecv[120 + nJointDataStart++];
                byteJ4[2] = byteRecv[120 + nJointDataStart++];
                byteJ4[1] = byteRecv[120 + nJointDataStart++];
                byteJ4[0] = byteRecv[120 + nJointDataStart++];
                dbJ4 = (double)(BitConverter.ToSingle(byteJ4, 0));
                /////////////////////////////////////////////////////
            }
            else
                return -10; //通訊發生錯誤  

            ////////////////////////////////////////

            return 0;
        }


        private int GetRobotWorldPosCmd(ref double dbX, ref double dbY, ref double dbZ, ref double dbRz)
        {
            //byte[] bytePRCmd = { 0x02, 0x50, 0x52, 0x2C, 0x31, 0x0D, 0x03 };    //PR,1
            byte[] byteSFCmd = { 0x02, 0x53, 0x46, 0x0D, 0x03 };    //SF
            byte[] byteRecv = new byte[300];
            int nRet = 0;

            byte[] byteWorldX = new byte[4];
            byte[] byteWorldY = new byte[4];
            byte[] byteWorldZ = new byte[4];
            byte[] byteWorldC = new byte[4];

            ToshibaClientIP0.SendData(byteSFCmd);
            nRet = ToshibaClientIP0.Receive(ref byteRecv);
            if (nRet == 0)
            {
                System.Threading.Thread.Sleep(64);          //手冊上說 收到回傳值後, 最少要延遲50ms, 再回傳OK 給控制器
                ToshibaClientIP0.SendData(m_byteSendOK);    //回傳OK 給控制器

                //世界座標值
                /////////////////////////////////////////////////////
                int nWorldDataStart = 24;
                byteWorldX[3] = byteRecv[120 + nWorldDataStart++];
                byteWorldX[2] = byteRecv[120 + nWorldDataStart++];
                byteWorldX[1] = byteRecv[120 + nWorldDataStart++];
                byteWorldX[0] = byteRecv[120 + nWorldDataStart++];
                dbX = (double)(BitConverter.ToSingle(byteWorldX, 0));

                byteWorldY[3] = byteRecv[120 + nWorldDataStart++];
                byteWorldY[2] = byteRecv[120 + nWorldDataStart++];
                byteWorldY[1] = byteRecv[120 + nWorldDataStart++];
                byteWorldY[0] = byteRecv[120 + nWorldDataStart++];
                dbY = (double)(BitConverter.ToSingle(byteWorldY, 0));

                byteWorldZ[3] = byteRecv[120 + nWorldDataStart++];
                byteWorldZ[2] = byteRecv[120 + nWorldDataStart++];
                byteWorldZ[1] = byteRecv[120 + nWorldDataStart++];
                byteWorldZ[0] = byteRecv[120 + nWorldDataStart++];
                dbZ = (double)(BitConverter.ToSingle(byteWorldZ, 0));

                byteWorldC[3] = byteRecv[120 + nWorldDataStart++];
                byteWorldC[2] = byteRecv[120 + nWorldDataStart++];
                byteWorldC[1] = byteRecv[120 + nWorldDataStart++];
                byteWorldC[0] = byteRecv[120 + nWorldDataStart++];
                dbRz = (double)(BitConverter.ToSingle(byteWorldC, 0));
                /////////////////////////////////////////////////////

            }
            else
                return -10; //通訊發生錯誤  

            ////////////////////////////////////////

            return 0;
        }

    }
}
