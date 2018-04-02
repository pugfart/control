using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using IRobotMotion;
using RobotCtrlCenter;

using ROS;

using System.Net.Sockets;

namespace RobotModuleTest
{

    //手臂位置資訊
    public struct RobotPosition
    {
        public double dbJ1;
        public double dbJ2;
        public double dbJ3;
        public double dbJ4;
        public double dbJ5;
        public double dbJ6;
        public double dbWorldX;
        public double dbWorldY;
        public double dbWorldZ;
        public double dbWorldRx;
        public double dbWorldRy;
        public double dbWorldRz;
    }


    public partial class Form1 : Form
    {
        TcpClient tcpclnt = new TcpClient();
        ASCIIEncoding asen = new ASCIIEncoding();
        String sendString;
        NetworkStream stream;
        byte[] ba;//傳Script用
        double v, a;//速度.加速度
        bool free;//是否有工作在執行
        FileInfo f=new FileInfo("D:\\task_script.txt");//一串工作順序
        bool mousedown;
        int DOcount;


        //URRobot.URRobot urRobot;
        RobotCtrlCenter.RobotCtrl g_robotCtrl;

        public int nCount = 0;

        public Jog_Speed m_nCurrJogSpeed = Jog_Speed.Mid;

        private bool m_bExitGetInfoThread = false;
     
        private delegate void myUICallBack(string myStr, Control ctl);
        private delegate string UIGridCetCellCallBack(int nRow, int nCol, DataGridView GridView);
        private delegate int UIGridGetRowColCallBack(int nRowCol, DataGridView GridView);
        private delegate void UIGridHighLightRowCallBack(int nRowCol, DataGridView GridView);
        private delegate void UIGridEnableCallBack(bool bEnable, DataGridView GridView);
        private delegate int UIRunTimesEditCallBack(Control ctrl);

        double m_dbJ1Degree = 0, m_dbJ2Degree = 0, m_dbJ3Degree = 0, m_dbJ4Degree = 0, m_dbJ5Degree = 0, m_dbJ6Degree = 0;
        double m_dbJ1Radian = 0, m_dbJ2Radian = 0, m_dbJ3Radian = 0, m_dbJ4Radian = 0, m_dbJ5Radian = 0, m_dbJ6Radian = 0;
        double m_dbWorldX = 0, m_dbWorldY = 0, m_dbWorldZ = 0, m_dbWorldRx = 0, m_dbWorldRy = 0, m_dbWorldRz = 0;
        double m_dbJ1Current = 0, m_dbJ2Current = 0, m_dbJ3Current = 0, m_dbJ4Current = 0, m_dbJ5Current = 0, m_dbJ6Current = 0;
        double m_dbJ1Voltage = 0, m_dbJ2Voltage = 0, m_dbJ3Voltage = 0, m_dbJ4Voltage = 0, m_dbJ5Voltage = 0, m_dbJ6Voltage = 0;
        double m_dbJ1Torque = 0, m_dbJ2Torque = 0, m_dbJ3Torque = 0, m_dbJ4Torque = 0, m_dbJ5Torque = 0, m_dbJ6Torque = 0;
        double m_dbJ1Temperature = 0, m_dbJ2Temperature = 0, m_dbJ3Temperature = 0, m_dbJ4Temperature = 0, m_dbJ5Temperature = 0, m_dbJ6Temperature = 0;
        double m_dbJ1Speed = 0, m_dbJ2Speed = 0, m_dbJ3Speed = 0, m_dbJ4Speed = 0, m_dbJ5Speed = 0, m_dbJ6Speed = 0;
        double m_dbJ1Acc = 0, m_dbJ2Acc = 0, m_dbJ3Acc = 0, m_dbJ4Acc = 0, m_dbJ5Acc = 0, m_dbJ6Acc = 0;

        //RobotPosition m_CurrRobotPos;

        ROS.Publisher m_PublisherPos;
        ROS.NodeHandle nodeHandle;
        MyCallback MoveCommandCallback;
        MyCallback2 MoveCommandCallback2;
        ROS.ServiceServer MoveCommandServer;
        
        public Form1()
        {
            InitializeComponent();
            nodeHandle = new ROS.NodeHandle("PomeCrl", "UR");
            m_PublisherPos = nodeHandle.advertize("RobotPosition", 96);
            v = 0.1;
            a = 0.1;
            free = true;
            mousedown = false;
            DOcount = 0;
        }

        private void button1_Click(object sender, EventArgs e)  //Robot Center Init
        {
            //urRobot = new URRobot.URRobot();
            //urRobot.Connect("192.168.1.2", 222);

            g_robotCtrl = new RobotCtrlCenter.RobotCtrl();
            g_robotCtrl.Init("C:\\tttt.ini");
            MessageBox.Show("Init Done!");
            MoveCommandCallback2 = new MyCallback2(g_robotCtrl);
            MoveCommandServer = nodeHandle.advertizeService("MoveCommand", 804, 8, MoveCommandCallback2);

            

        }

        private void button4_Click(object sender, EventArgs e)  //connect all robots
        {
            //g_robotCtrl.ConnectAllRobots();
            //MessageBox.Show("Connect Done!");
            TcpClient tcpclnt = new TcpClient();
            tcpclnt.Connect("192.168.0.6", 30002); //script連線
            //test
            stream = tcpclnt.GetStream();
            sendString = "def myProg(): \n";//定義程式開頭字串
            string strMsg = "";
            for (int n=0; n < g_robotCtrl.RobotsNumber; n++)
            {
                int nRet = g_robotCtrl.ConnectRobot(n);
                if (nRet == 0)
                    strMsg = "id=" + Convert.ToString(n) + " Connect Successful!";
                else
                    strMsg = "id=" + Convert.ToString(n) + " Connect Failed!";

                MessageBox.Show(strMsg);   
            }
          
        }

        private void button3_Click(object sender, EventArgs e)  //disconnect all robots
        {
            //g_robotCtrl.DisconnectAllRobots();

            string strMsg = "";
            for (int n = 0; n < g_robotCtrl.RobotsNumber; n++)
            {
                int nRet = g_robotCtrl.DisconnectRobot(n);
                if (nRet == 0)
                    strMsg = "id=" + Convert.ToString(n) + " Disconnect Successful!";
                else
                    strMsg = "id=" + Convert.ToString(n) + " Disconnect Failed!";

                MessageBox.Show(strMsg);
            }
        }


        private void button2_Click(object sender, EventArgs e)  //Start Get Pos Thread
        {
            /*
            //RobotPos._tbJ1 = tbJ1;
            GetPosThread = new Thread(RobotPos.GetPos(g_robotCtrl));
            double dbBase=0;
            double dbShoulder=0;
            double dbElbow=0;
            double dbWrist1=0;
            double dbWrist2=0;
            double dbWrist3=0;

            g_robotCtrl.GetJointDegree(0, ref dbBase, ref dbShoulder, ref dbElbow, ref dbWrist1, ref dbWrist2, ref dbWrist3);
            //urRobot.GetJointDegree(ref dbBase, ref dbShoulder, ref dbElbow, ref dbWrist1, ref dbWrist2, ref dbWrist3);

            //          string strMsg = dbBase.ToString() + 
             int kkk = 0;
             */

            ThreadStart GetInfo = new ThreadStart(GetInfoThreadFunc);
            Thread GetInfoThread = new Thread(GetInfo);
            m_bExitGetInfoThread = false;
            GetInfoThread.Start(); 
        }

        private void button5_Click(object sender, EventArgs e)  //Stop Get Pos Thread
        {
            m_bExitGetInfoThread = true;
        }



        private void RunBtn_Click(object sender, EventArgs e)  //執行
        {
            /*
             ThreadStart RunListStart = new ThreadStart(RunListThreadFunc);
             Thread RunListThread = new Thread(RunListStart);
             RunListThread.Start();
             */
            //test
            StreamWriter write = f.CreateText();
            int i;
            for (i = 0; i <= dataGridViewList.RowCount - 1; i++)
            {
                char m = 'a';
                string move;
                move = dataGridViewList.Rows[i].Cells[1].FormattedValue.ToString();
                if (move == "MovePtoPIns") m = 'j';
                else if (move == "LinePtoPIns") m = 'l';

                double j1 = Convert.ToDouble(dataGridViewList.Rows[i].Cells[3].Value) * 3.14 / 180,//表格各軸單位轉換 度轉徑
                    j2 = Convert.ToDouble(dataGridViewList.Rows[i].Cells[4].Value) * 3.14 / 180,
                    j3 = Convert.ToDouble(dataGridViewList.Rows[i].Cells[5].Value) * 3.14 / 180,
                    j4 = Convert.ToDouble(dataGridViewList.Rows[i].Cells[6].Value) * 3.14 / 180,
                    j5 = Convert.ToDouble(dataGridViewList.Rows[i].Cells[7].Value) * 3.14 / 180,
                    j6 = Convert.ToDouble(dataGridViewList.Rows[i].Cells[8].Value) * 3.14 / 180;
                switch (m)
                {
                    case 'j'://movej寫入
                        write.Write("movej([" + j1.ToString() + "," +
                            j2.ToString() + "," +
                            j3.ToString() + "," +
                            j4.ToString() + "," +
                            j5.ToString() + "," +
                            j6.ToString() + "],a=" +
                            a.ToString() + ",v=" +
                            v.ToString() + ")\r\n");
                        break;
                    case 'l'://movel寫入
                        write.Write("movel([" + j1.ToString() + "," +
                            j2.ToString() + "," +
                            j3.ToString() + "," +
                            j4.ToString() + "," +
                            j5.ToString() + "," +
                            j6.ToString() + "],a=" +
                            a.ToString() + ",v=" +
                            v.ToString() + ")\r\n");
                        break;
                    default:
                        break;
                }
            }
            write.Close();//寫入結束

            StreamReader read = f.OpenText();//讀檔執行
            string task;
            ba = asen.GetBytes(sendString);//送開頭
            stream.Write(ba, 0, ba.Length);
            while ((task = read.ReadLine()) != null)//送各行動作
            {
                ba = asen.GetBytes(task + "\n");
                stream.Write(ba, 0, ba.Length);
            }
            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);			     //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }

        private void StopBtn_Click(object sender, EventArgs e)  //停止
        {
           // g_robotCtrl.StopMotionForLine(0, 0);
           //test
            ba = asen.GetBytes(sendString);//送開頭
            stream.Write(ba, 0, ba.Length);
            ba = asen.GetBytes("stopj("+a.ToString()+")\n");
            stream.Write(ba, 0, ba.Length);
            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);			     //接收到 end 指令, UR即開始執行剛剛接收的檔案
            free = true;
        }

        private void ClearListBtn_Click(object sender, EventArgs e)  //清空列表
        {
            dataGridViewList.Rows.Clear();
        }

        private void RunListThreadFunc()
        {
            int nRowCount=0;
            int nColCount=0;
            int nRunTimes = 0;
            UIGridEnable(false, this.dataGridViewList);

            nRowCount = UIGridGetRowCol( 1, this.dataGridViewList);
            nColCount = UIGridGetRowCol(2, this.dataGridViewList);
            nRunTimes = UIRunTimesEdit(this.RunTimesEdit);

            string strTemp = "";
            int nRobotId = 0;
            string strIns = "";
            double dbJ1 = 0;
            double dbJ2 = 0;
            double dbJ3 = 0;
            double dbJ4 = 0;
            double dbJ5 = 0;
            double dbJ6 = 0;
            double dbX = 0;
            double dbY = 0;
            double dbZ = 0;
            double dbRx = 0;
            double dbRy = 0;
            double dbRz = 0;
            double dbAcc = 0;
            double dbDec = 0;
            double dbSpeed = 0;
            double dbArcMidX = 0;
            double dbArcMidY = 0;
            double dbArcMidZ = 0;
            double dbArcMidRx = 0;
            double dbArcMidRy = 0;
            double dbArcMidRz = 0;
            double dbPar1 = 0;
            double dbPar2 = 0;

            bool bBreak = false;

            for (int nRun = 0; nRun < nRunTimes; nRun++)
            {
                for (int nRow = 0; nRow < nRowCount - 1; nRow++)
                {
                    strIns = UIGridCetCell(nRow, 1, this.dataGridViewList);
                    strTemp = UIGridCetCell(nRow, 2, this.dataGridViewList);
                    nRobotId = Convert.ToInt32(strTemp);

                    strTemp = UIGridCetCell(nRow, 3, this.dataGridViewList);
                    dbJ1 = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 4, this.dataGridViewList);
                    dbJ2 = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 5, this.dataGridViewList);
                    dbJ3 = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 6, this.dataGridViewList);
                    dbJ4 = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 7, this.dataGridViewList);
                    dbJ5 = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 8, this.dataGridViewList);
                    dbJ6 = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 9, this.dataGridViewList);
                    dbX = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 10, this.dataGridViewList);
                    dbY = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 11, this.dataGridViewList);
                    dbZ = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 12, this.dataGridViewList);
                    dbRx = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 13, this.dataGridViewList);
                    dbRy = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 14, this.dataGridViewList);
                    dbRz = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 15, this.dataGridViewList);
                    dbAcc = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 16, this.dataGridViewList);
                    dbDec = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 17, this.dataGridViewList);
                    dbSpeed = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 18, this.dataGridViewList);
                    dbPar1 = Convert.ToDouble(strTemp);

                    strTemp = UIGridCetCell(nRow, 19, this.dataGridViewList);
                    dbPar2 = Convert.ToDouble(strTemp);

                    UIGridHighLightRow(nRow, this.dataGridViewList);

                    int nRet = 0;
                    if (strIns == "MovePtoPIns")
                    {
                        if (dbAcc == 0 && dbDec == 0 && dbSpeed == 0)
                            nRet = g_robotCtrl.MovePtoPAbs(nRobotId, dbX, dbY, dbZ, dbRx, dbRy, dbRz);
                        else
                            nRet = g_robotCtrl.MovePtoPAbs(nRobotId, dbX, dbY, dbZ, dbRx, dbRy, dbRz, dbAcc, dbDec, dbSpeed);
                    }
                    else if (strIns == "LinePtoPIns")
                    {
                        if (dbAcc == 0 && dbDec == 0 && dbSpeed == 0)
                            nRet = g_robotCtrl.LinePtoPAbs(nRobotId, dbX, dbY, dbZ, dbRx, dbRy, dbRz);
                        else
                            nRet = g_robotCtrl.LinePtoPAbs(nRobotId, dbX, dbY, dbZ, dbRx, dbRy, dbRz, dbAcc, dbDec, dbSpeed);
                    }
                    else if (strIns == "ArcMidIns")
                    {
                        dbArcMidX = dbX;
                        dbArcMidY = dbY;
                        dbArcMidZ = dbZ;
                        dbArcMidRx = dbRx;
                        dbArcMidRy = dbRy;
                        dbArcMidRz = dbRz;
                    }
                    else if (strIns == "ArcEndIns")
                    {
                        if (dbAcc == 0 && dbDec == 0 && dbSpeed == 0)
                            nRet = g_robotCtrl.Arc(nRobotId, dbArcMidX, dbArcMidY, dbArcMidZ, dbArcMidRx, dbArcMidRy, dbArcMidRz,
                                                    dbX, dbY, dbZ, dbRx, dbRy, dbRz);
                        else
                            nRet = g_robotCtrl.Arc(nRobotId, dbArcMidX, dbArcMidY, dbArcMidZ, dbArcMidRx, dbArcMidRy, dbArcMidRz,
                                                    dbX, dbY, dbZ, dbRx, dbRy, dbRz, dbAcc, dbDec, dbSpeed);
                    }
                    else if (strIns == "MoveAbsIns")
                    {
                        if (dbAcc == 0 && dbDec == 0 && dbSpeed == 0)
                            nRet = g_robotCtrl.MoveAbs(nRobotId, (Joints)dbPar1, dbPar2);
                        else
                            nRet = g_robotCtrl.MoveAbs(nRobotId, (Joints)dbPar1, dbPar2, dbAcc, dbDec, dbSpeed);
                    }
                    else if (strIns == "LineAbsIns")
                    {
                        if (dbAcc == 0 && dbDec == 0 && dbSpeed == 0)
                            nRet = g_robotCtrl.LineAbs(nRobotId, (Coordnates)dbPar1, dbPar2);
                        else
                            nRet = g_robotCtrl.LineAbs(nRobotId, (Coordnates)dbPar1, dbPar2, dbAcc, dbDec, dbSpeed);
                    }
                    else if (strIns == "MoveRelIns")
                    {
                        if (dbAcc == 0 && dbDec == 0 && dbSpeed == 0)
                            nRet = g_robotCtrl.MoveRel(nRobotId, (Joints)dbPar1, dbPar2);
                        else
                            nRet = g_robotCtrl.MoveRel(nRobotId, (Joints)dbPar1, dbPar2, dbAcc, dbDec, dbSpeed);
                    }
                    else if (strIns == "LineRelIns")
                    {
                        if (dbAcc == 0 && dbDec == 0 && dbSpeed == 0)
                            nRet = g_robotCtrl.LineRel(nRobotId, (Coordnates)dbPar1, dbPar2);
                        else
                            nRet = g_robotCtrl.LineRel(nRobotId, (Coordnates)dbPar1, dbPar2, dbAcc, dbDec, dbSpeed);
                    }

                    if (nRet == 27) //break 被按下
                    {
                        bBreak = true;
                        break;
                    }
                    //  g_robotCtrl.JointAbs(nRobotId, dbJ1, dbJ2, dbJ3, dbJ4, dbJ5, dbJ6);
                }

                if (bBreak == true)
                    break;
            }


            

            UIGridEnable(true, this.dataGridViewList);
        }

        private void UIGridEnable(bool bEnable, DataGridView GridView)
        {
        //判斷這個Grid的物件是否在同一個執行緒上
            if (this.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                UIGridEnableCallBack GridEnable = new UIGridEnableCallBack(UIGridEnable);
                this.Invoke(GridEnable, bEnable, GridView);
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                if (bEnable)
                    GridView.Enabled = true;
                else
                    GridView.Enabled = false;
                
            }
        }

        private void UIGridHighLightRow(int nRowCol, DataGridView GridView)
        {
            //判斷這個Grid的物件是否在同一個執行緒上
            if (this.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                UIGridHighLightRowCallBack GridHighLightRow = new UIGridHighLightRowCallBack(UIGridHighLightRow);
                this.Invoke(GridHighLightRow, nRowCol, GridView);
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                if ((nRowCol -1) >= 0)
                    GridView.Rows[nRowCol - 1].Selected = false;

                GridView.Rows[nRowCol].Selected = true;
            }
        }

        // private delegate string UIGridCetCellCallBack(int nRow, int nCol, DataGridView GridView);
        // private delegate int UIGridGetRowColCallBack(int nRowCol, DataGridView GridView);
        private int UIGridGetRowCol(int nRowCol, DataGridView GridView)
        {
            //判斷這個Grid的物件是否在同一個執行緒上
            if (this.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                // myUICallBack myUpdate = new myUICallBack(UIPrint);
                // this.Invoke(myUpdate, text, ctrl);

                UIGridGetRowColCallBack GridGetRowCol = new UIGridGetRowColCallBack(UIGridGetRowCol);
                return (int) (this.Invoke(GridGetRowCol, nRowCol, GridView));
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                // ctrl.Text = text;
                if (nRowCol == 1)
                    return GridView.RowCount;
                else
                    return GridView.ColumnCount;
            }
        }

       // private void UIGridCetCell(ref string strValue, int nRow, int nCol, DataGridView GridView)
        private string UIGridCetCell(int nRow, int nCol, DataGridView GridView)
        {
            //判斷這個Grid的物件是否在同一個執行緒上
            if (this.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                // myUICallBack myUpdate = new myUICallBack(UIPrint);
                // this.Invoke(myUpdate, text, ctrl);

               // UIGridCetCellCallBack GridGetCell = new UIGridCetCellCallBack(UIGridCetCell);
               // this.Invoke(GridGetCell, strValue, nRow, nCol, GridView);

                UIGridCetCellCallBack GridGetCell = new UIGridCetCellCallBack(UIGridCetCell);
                return (string)(this.Invoke(GridGetCell, nRow, nCol, GridView));
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                // ctrl.Text = text;
                return GridView.Rows[nRow].Cells[nCol].Value.ToString();
            }
        }

       
        private int UIRunTimesEdit(Control ctrl)
        {
            //判斷這個Grid的物件是否在同一個執行緒上
            if (this.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                // myUICallBack myUpdate = new myUICallBack(UIPrint);
                // this.Invoke(myUpdate, text, ctrl);
                UIRunTimesEditCallBack RunTimesEdit = new UIRunTimesEditCallBack(UIRunTimesEdit);
                return (int)(this.Invoke(RunTimesEdit, ctrl));
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                return Convert.ToInt32(ctrl.Text);
            }
        }


        private void GetInfoThreadFunc()
        {
            while (!m_bExitGetInfoThread)
            { 
                g_robotCtrl.GetJointDegree(0, ref m_dbJ1Degree, ref m_dbJ2Degree, ref m_dbJ3Degree, ref m_dbJ4Degree, ref m_dbJ5Degree, ref m_dbJ6Degree);
                g_robotCtrl.GetJointRadian(0, ref m_dbJ1Radian, ref m_dbJ2Radian, ref m_dbJ3Radian, ref m_dbJ4Radian, ref m_dbJ5Radian, ref m_dbJ6Radian);
                g_robotCtrl.GetWorldPos(0, ref m_dbWorldX, ref m_dbWorldY, ref m_dbWorldZ, ref m_dbWorldRx, ref m_dbWorldRy, ref m_dbWorldRz);
                g_robotCtrl.GetJointCurrentValue(0, ref m_dbJ1Current, ref m_dbJ2Current, ref m_dbJ3Current, ref m_dbJ4Current, ref m_dbJ5Current, ref m_dbJ6Current);
                g_robotCtrl.GetJointVoltageValue(0, ref m_dbJ1Voltage, ref m_dbJ2Voltage, ref m_dbJ3Voltage, ref m_dbJ4Voltage, ref m_dbJ5Voltage, ref m_dbJ6Voltage);
                g_robotCtrl.GetJointTorqueValue(0, ref m_dbJ1Torque, ref m_dbJ2Torque, ref m_dbJ3Torque, ref m_dbJ4Torque, ref m_dbJ5Torque, ref m_dbJ6Torque);
                g_robotCtrl.GetJointTemperatureValue(0, ref m_dbJ1Temperature, ref m_dbJ2Temperature, ref m_dbJ3Temperature, ref m_dbJ4Temperature, ref m_dbJ5Temperature, ref m_dbJ6Temperature);
                g_robotCtrl.GetJointSpeedValue(0, ref m_dbJ1Speed, ref m_dbJ2Speed, ref m_dbJ3Speed, ref m_dbJ4Speed, ref m_dbJ5Speed, ref m_dbJ6Speed);
                g_robotCtrl.GetJointAccValue(0, ref m_dbJ1Acc, ref m_dbJ2Acc, ref m_dbJ3Acc, ref m_dbJ4Acc, ref m_dbJ5Acc, ref m_dbJ6Acc);

                UIPrint(m_dbJ1Degree.ToString(), this.tbJ1);
                UIPrint(m_dbJ2Degree.ToString(), this.tbJ2);
                UIPrint(m_dbJ3Degree.ToString(), this.tbJ3);
                UIPrint(m_dbJ4Degree.ToString(), this.tbJ4);
                UIPrint(m_dbJ5Degree.ToString(), this.tbJ5);
                UIPrint(m_dbJ6Degree.ToString(), this.tbJ6);

                UIPrint(m_dbJ1Radian.ToString(), this.tbJ1Radian);
                UIPrint(m_dbJ2Radian.ToString(), this.tbJ2Radian);
                UIPrint(m_dbJ3Radian.ToString(), this.tbJ3Radian);
                UIPrint(m_dbJ4Radian.ToString(), this.tbJ4Radian);
                UIPrint(m_dbJ5Radian.ToString(), this.tbJ5Radian);
                UIPrint(m_dbJ6Radian.ToString(), this.tbJ6Radian);

                UIPrint(m_dbWorldX.ToString(), this.tbX);
                UIPrint(m_dbWorldY.ToString(), this.tbY);
                UIPrint(m_dbWorldZ.ToString(), this.tbZ);
                UIPrint(m_dbWorldRx.ToString(), this.tbRx);
                UIPrint(m_dbWorldRy.ToString(), this.tbRy);
                UIPrint(m_dbWorldRz.ToString(), this.tbRz);

                UIPrint(m_dbJ1Current.ToString(), this.tbJ1Current);
                UIPrint(m_dbJ2Current.ToString(), this.tbJ2Current);
                UIPrint(m_dbJ3Current.ToString(), this.tbJ3Current);
                UIPrint(m_dbJ4Current.ToString(), this.tbJ4Current);
                UIPrint(m_dbJ5Current.ToString(), this.tbJ5Current);
                UIPrint(m_dbJ6Current.ToString(), this.tbJ6Current);

                UIPrint(m_dbJ1Voltage.ToString(), this.tbJ1Voltage);
                UIPrint(m_dbJ2Voltage.ToString(), this.tbJ2Voltage);
                UIPrint(m_dbJ3Voltage.ToString(), this.tbJ3Voltage);
                UIPrint(m_dbJ4Voltage.ToString(), this.tbJ4Voltage);
                UIPrint(m_dbJ5Voltage.ToString(), this.tbJ5Voltage);
                UIPrint(m_dbJ6Voltage.ToString(), this.tbJ6Voltage);

                UIPrint(m_dbJ1Torque.ToString(), this.tbJ1Torque);
                UIPrint(m_dbJ2Torque.ToString(), this.tbJ2Torque);
                UIPrint(m_dbJ3Torque.ToString(), this.tbJ3Torque);
                UIPrint(m_dbJ4Torque.ToString(), this.tbJ4Torque);
                UIPrint(m_dbJ5Torque.ToString(), this.tbJ5Torque);
                UIPrint(m_dbJ6Torque.ToString(), this.tbJ6Torque);

                UIPrint(m_dbJ1Temperature.ToString(), this.tbJ1Temperature);
                UIPrint(m_dbJ2Temperature.ToString(), this.tbJ2Temperature);
                UIPrint(m_dbJ3Temperature.ToString(), this.tbJ3Temperature);
                UIPrint(m_dbJ4Temperature.ToString(), this.tbJ4Temperature);
                UIPrint(m_dbJ5Temperature.ToString(), this.tbJ5Temperature);
                UIPrint(m_dbJ6Temperature.ToString(), this.tbJ6Temperature);

                UIPrint(m_dbJ1Speed.ToString(), this.tbJ1Speed);
                UIPrint(m_dbJ2Speed.ToString(), this.tbJ2Speed);
                UIPrint(m_dbJ3Speed.ToString(), this.tbJ3Speed);
                UIPrint(m_dbJ4Speed.ToString(), this.tbJ4Speed);
                UIPrint(m_dbJ5Speed.ToString(), this.tbJ5Speed);
                UIPrint(m_dbJ6Speed.ToString(), this.tbJ6Speed);

                UIPrint(m_dbJ1Acc.ToString(), this.tbJ1Acc);
                UIPrint(m_dbJ2Acc.ToString(), this.tbJ2Acc);
                UIPrint(m_dbJ3Acc.ToString(), this.tbJ3Acc);
                UIPrint(m_dbJ4Acc.ToString(), this.tbJ4Acc);
                UIPrint(m_dbJ5Acc.ToString(), this.tbJ5Acc);
                UIPrint(m_dbJ6Acc.ToString(), this.tbJ6Acc);

                double[] dbRobotPosition;
                dbRobotPosition = new double[12];
                dbRobotPosition[0] = m_dbJ1Degree;
                dbRobotPosition[1] = m_dbJ2Degree;
                dbRobotPosition[2] = m_dbJ3Degree;
                dbRobotPosition[3] = m_dbJ4Degree;
                dbRobotPosition[4] = m_dbJ5Degree;
                dbRobotPosition[5] = m_dbJ6Degree;
                dbRobotPosition[6] = m_dbWorldX;
                dbRobotPosition[7] = m_dbWorldY;
                dbRobotPosition[8] = m_dbWorldZ;
                dbRobotPosition[9] = m_dbWorldRx;
                dbRobotPosition[10] = m_dbWorldRy;
                dbRobotPosition[11] = m_dbWorldRz;
                //test
                m_dbJ1Radian = m_dbJ1Degree / 180 * 3.14;
                m_dbJ2Radian = m_dbJ2Degree / 180 * 3.14;
                m_dbJ3Radian = m_dbJ3Degree / 180 * 3.14;
                m_dbJ4Radian = m_dbJ4Degree / 180 * 3.14;
                m_dbJ5Radian = m_dbJ5Degree / 180 * 3.14;
                m_dbJ6Radian = m_dbJ6Degree / 180 * 3.14;
                if (m_PublisherPos != null)
                    m_PublisherPos.publish(SerializePosition(dbRobotPosition));



                System.Threading.Thread.Sleep(100);
            }
        }

        static byte[] SerializePosition(double[] data)
        {
            byte[] sData = new byte[8 * data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                byte[] lData = BitConverter.GetBytes(data[i]);
                for (int j = 0; j < 8; j++)
                    sData[i * 8 + j] = lData[j];
            }
            return sData;
        }

        private void UIPrint(string text, Control ctrl)
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (this.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                myUICallBack myUpdate = new myUICallBack(UIPrint);
                this.Invoke(myUpdate, text, ctrl);
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                ctrl.Text = text;
            }
        }

        private void dataGridViewList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
         /*
            UIGridEnable(false, this.dataGridViewList);

            string strTemp = "";
            int nRobotId = 0;
            string strIns = "";
            double dbJ1 = 0;
            double dbJ2 = 0;
            double dbJ3 = 0;
            double dbJ4 = 0;
            double dbJ5 = 0;
            double dbJ6 = 0;
            double dbX = 0;
            double dbY = 0;
            double dbZ = 0;
            double dbRx = 0;
            double dbRy = 0;
            double dbRz = 0;
            double dbAcc = 0;
            double dbDec = 0;
            double dbSpeed = 0;
          
           
            strIns = UIGridCetCell(e.RowIndex, 1, this.dataGridViewList);
            strTemp = UIGridCetCell(e.RowIndex, 2, this.dataGridViewList);
            nRobotId = Convert.ToInt32(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 3, this.dataGridViewList);
            dbJ1 = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 4, this.dataGridViewList);
            dbJ2 = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 5, this.dataGridViewList);
            dbJ3 = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 6, this.dataGridViewList);
            dbJ4 = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 7, this.dataGridViewList);
            dbJ5 = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 8, this.dataGridViewList);
            dbJ6 = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 9, this.dataGridViewList);
            dbX = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 10, this.dataGridViewList);
            dbY = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 11, this.dataGridViewList);
            dbZ = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 12, this.dataGridViewList);
            dbRx = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 13, this.dataGridViewList);
            dbRy = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 14, this.dataGridViewList);
            dbRz = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 15, this.dataGridViewList);
            dbAcc = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 16, this.dataGridViewList);
            dbDec = Convert.ToDouble(strTemp);

            strTemp = UIGridCetCell(e.RowIndex, 17, this.dataGridViewList);
            dbSpeed = Convert.ToDouble(strTemp);

            UIGridHighLightRow(e.RowIndex, this.dataGridViewList);

            int nRet = 0;
            nRet = g_robotCtrl.MoveAbs(nRobotId, dbX, dbY, dbZ, dbRx, dbRy, dbRz);

            UIGridEnable(true, this.dataGridViewList);
            */
        }


        private void J1PositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            // g_robotCtrl.MoveJointAbsDegreeSpeed(0, Joints.J1, 350, 0.5, 0.1);
            // g_robotCtrl.MoveJointAbsDegreeSpeed(0, Joints.J1, 50, 0.5, 0.1);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J1, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J1 Positive JogJoint Error! code = {0}", nRet));
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([6.2831852," + m_dbJ2Radian.ToString() + " , " + m_dbJ3Radian.ToString() + ", " + m_dbJ4Radian.ToString() + "," + m_dbJ5Radian.ToString() + ", " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J1PositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J1);
            if (nRet != 0)
                MessageBox.Show(string.Format("J1 JogJointStop Error! code = {0}", nRet));
            // g_robotCtrl.StopMotionForPtoP(0, 2.0);
            */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J1NegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            // g_robotCtrl.MoveJointAbsDegreeSpeed(0, Joints.J1, -350, 0.5, 0.1);
            // g_robotCtrl.MoveJointAbsDegreeSpeed(0, Joints.J1, -50, 0.5, 0.1);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J1, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J1 Negative JogJoint Error! code = {0}", nRet));*/
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([-6.2831852," + m_dbJ2Radian.ToString() + " , " + m_dbJ3Radian.ToString() + ", " + m_dbJ4Radian.ToString() + "," + m_dbJ5Radian.ToString() + ", " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J1NegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J1);
            if (nRet != 0)
                MessageBox.Show(string.Format("J1 JogJointStop Error! code = {0}", nRet));
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }



        private void J2PositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            // g_robotCtrl.MoveJointAbsDegree(0, Joints.J2, 350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J2, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J2 Positive JogJoint Error! code = {0}", nRet));*/
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + ",6.2831852 , " + m_dbJ3Radian.ToString() + ", " + m_dbJ4Radian.ToString() + "," + m_dbJ5Radian.ToString() + ", " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J2PositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J2);
            if (nRet != 0)
                MessageBox.Show(string.Format("J2 JogJointStop Error! code = {0}", nRet));
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }

        }

        private void J2NegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.MoveJointAbsDegree(0, Joints.J2, -350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J2, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J2 Negative Positive JogJoint Error! code = {0}", nRet));*/
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + ",-6.2831852 , " + m_dbJ3Radian.ToString() + ", " + m_dbJ4Radian.ToString() + "," + m_dbJ5Radian.ToString() + ", " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }

        }

        private void J2NegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J2);
            if (nRet != 0)
                MessageBox.Show(string.Format("J2 JogJointStop Error! code = {0}", nRet));
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案     
            }       
        }



        private void J3PositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            //g_robotCtrl.MoveJointAbsDegree(0, Joints.J3, 350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J3, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J3 Positive JogJoint Error! code = {0}", nRet));      */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + "," + m_dbJ2Radian.ToString() + " , 6.2831852, " + m_dbJ4Radian.ToString() + "," + m_dbJ5Radian.ToString() + ", " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J3PositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J3);
            if (nRet != 0)
                MessageBox.Show(string.Format("J3 JogJointStop Error! code = {0}", nRet)); 
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J3NegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            // g_robotCtrl.MoveJointAbsDegree(0, Joints.J3, -350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J3, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J3 Negative JogJoint Error! code = {0}", nRet));  */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + "," + m_dbJ2Radian.ToString() + " , -6.2831852, " + m_dbJ4Radian.ToString() + "," + m_dbJ5Radian.ToString() + ", " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J3NegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J3);
            if (nRet != 0)
                MessageBox.Show(string.Format("J3 JogJointStop Error! code = {0}", nRet));           
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        

        private void J4PositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            //g_robotCtrl.MoveJointAbsDegree(0, Joints.J4, 350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J4, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J4 Positive JogJoint Error! code = {0}", nRet));    */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + "," + m_dbJ2Radian.ToString() + " , " + m_dbJ3Radian.ToString() + ", 6.2831852," + m_dbJ5Radian.ToString() + ", " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案  
            }
        }

        

        private void J4PositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J4);
            if (nRet != 0)
                MessageBox.Show(string.Format("J4 JogJointStop Error! code = {0}", nRet));        
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案 
            }
        }

        private void J4NegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            //g_robotCtrl.MoveJointAbsDegree(0, Joints.J4, -350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J4, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J4 Negative JogJoint Error! code = {0}", nRet));    */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + "," + m_dbJ2Radian.ToString() + " , " + m_dbJ3Radian.ToString() + ", -6.2831852," + m_dbJ5Radian.ToString() + ", " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void XPositiveBtn_Click(object sender, EventArgs e)
        {

        }

        private void J1PositiveBtn_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void DeleteRowBtn_Click(object sender, EventArgs e)  //刪除列表
        {

        }

        private void J2PositiveBtn_Click(object sender, EventArgs e)
        {

        }

        private void J3PositiveBtn_Click(object sender, EventArgs e)
        {

        }

        private void J4PositiveBtn_Click(object sender, EventArgs e)
        {

        }

        private void J5PositiveBtn_Click(object sender, EventArgs e)
        {

        }

        private void J6PositiveBtn_Click(object sender, EventArgs e)
        {

        }

        private void J1NegativeBtn_Click(object sender, EventArgs e)
        {

        }

        private void J2NegativeBtn_Click(object sender, EventArgs e)
        {

        }

        private void J3NegativeBtn_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "set_standard_digital_out(0,true))\n"; //輸出on
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }else
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "set_standard_digital_out(0,false))\n"; //輸出off
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "set_standard_digital_out(0,true))\n"; //輸出on
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
           
        }

        private void J4NegativeBtn_Click(object sender, EventArgs e)
        {

        }

        private void J4NegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J4);
            if (nRet != 0)
                MessageBox.Show(string.Format("J4 JogJointStop Error! code = {0}", nRet));      
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案 
            }  
        }



        private void J5PositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            // g_robotCtrl.MoveJointAbsDegree(0, Joints.J5, 350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J5, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J5 Positive JogJoint Error! code = {0}", nRet));         */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + "," + m_dbJ2Radian.ToString() + " , " + m_dbJ3Radian.ToString() + ", " + m_dbJ4Radian.ToString() + ",6.2831852, " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案  
            }
        }

        private void J5PositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            // g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J5);
            if (nRet != 0)
                MessageBox.Show(string.Format("J5 JogJointStop Error! code = {0}", nRet));         
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J5NegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            // g_robotCtrl.MoveJointAbsDegree(0, Joints.J5, -350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J5, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J5 Negative JogJoint Error! code = {0}", nRet));     */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + "," + m_dbJ2Radian.ToString() + " , " + m_dbJ3Radian.ToString() + ", " + m_dbJ4Radian.ToString() + ",-6.2831852, " + m_dbJ6Radian.ToString() + "], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J5NegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J5);
            if (nRet != 0)
                MessageBox.Show(string.Format("J5 JogJointStop Error! code = {0}", nRet));      
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案    
            }
        }


        private void J6PositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            //g_robotCtrl.MoveJointAbsDegree(0, Joints.J6, 350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J6, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J6 Positive JogJoint Error! code = {0}", nRet));      */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + "," + m_dbJ2Radian.ToString() + " , " + m_dbJ3Radian.ToString() + ", " + m_dbJ4Radian.ToString() + "," + m_dbJ5Radian.ToString() + ", 6.2831852 ], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案   
            } 
        }

        private void J6PositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J6);
            if (nRet != 0)
                MessageBox.Show(string.Format("J6 JogJointStop Error! code = {0}", nRet));            
                */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

        private void J6NegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            //g_robotCtrl.MoveJointAbsDegree(0, Joints.J6, -350);
            int nRet = g_robotCtrl.JogJoint(0, Joints.J6, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("J6 Negative JogJoint Error! code = {0}", nRet));       */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movej([" + m_dbJ1Radian.ToString() + "," + m_dbJ2Radian.ToString() + " , " + m_dbJ3Radian.ToString() + ", " + m_dbJ4Radian.ToString() + "," + m_dbJ5Radian.ToString() + ", -6.2831852], a=" + a.ToString() + ", v=" + v.ToString() + ")\n";
                ba = asen.GetBytes(task);//movej
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案     
            }
        }

        private void J6NegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            //g_robotCtrl.StopMotionForPtoP(0, 2.0);
            int nRet = g_robotCtrl.JogJointStop(0, Joints.J6);
            if (nRet != 0)
                MessageBox.Show(string.Format("J6 JogJointStop Error! code = {0}", nRet));      */
            if (free )
            {
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "stopj(a=" + a.ToString() + ")\n"; //movej stop
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            }
        }

      

        private void button7_Click(object sender, EventArgs e)
        {
            g_robotCtrl.DisconnectRobot(0);
        }

     

        private void HiSpeedChk_CheckedChanged(object sender, EventArgs e)
        {
            if (HiSpeedChk.Checked == true)
            {
                MidSpeedChk.Checked = false;
                LowSpeedChk.Checked = false;
                m_nCurrJogSpeed = Jog_Speed.High;
                v = 0.2;
                a = 0.6;
            }
        }

        private void MidSpeedChk_CheckedChanged(object sender, EventArgs e)
        {
            if (MidSpeedChk.Checked == true)
            {
                HiSpeedChk.Checked = false;
                LowSpeedChk.Checked = false;
                m_nCurrJogSpeed = Jog_Speed.Mid;
                v = 0.15;
                a = 0.45;
            }
        }

        private void LowSpeedChk_CheckedChanged(object sender, EventArgs e)
        {
            if (LowSpeedChk.Checked == true)
            {
                HiSpeedChk.Checked = false;
                MidSpeedChk.Checked = false;
                m_nCurrJogSpeed = Jog_Speed.Low;
                v = 0.1;
                a = 0.3;
            }
        }
       
        private void XPositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.X, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("X Positive JogJoint Error! code = {0}", nRet));*/
           
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000 + 0.1).ToString() + ","
                    + (m_dbWorldY / 1000).ToString()
                    + "," + (m_dbWorldZ / 1000).ToString()
                    + "," + m_dbWorldRx.ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=0.5,v=0.05)\n"; //movel X軸正移動  單位m,rad          
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void XPositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.X);
            if (nRet != 0)
                MessageBox.Show(string.Format("x JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=0.5)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }

        private void XNegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.X, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("X Negative JogJoint Error! code = {0}", nRet));*/
           
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000 - 0.1).ToString() + ","
                    + (m_dbWorldY / 1000).ToString()
                    + "," + (m_dbWorldZ / 1000).ToString()
                    + "," + m_dbWorldRx.ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=0.5,v=0.05)\n"; //movel X軸負移動  單位m,rad          
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void XNegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.X);
            if (nRet != 0)
                MessageBox.Show(string.Format("x JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=0.5)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }



        private void YPositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Y, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Y Positive JogJoint Error! code = {0}", nRet));*/
           
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() + ","
                    + (m_dbWorldY / 1000 + 0.1).ToString()
                    + "," + (m_dbWorldZ / 1000).ToString()
                    + "," + m_dbWorldRx.ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=0.5,v=0.05)\n"; //movel Y軸正移動  單位m,rad   
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void YPositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Y);
            if (nRet != 0)
                MessageBox.Show(string.Format("Y JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=0.5)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }

        private void YNegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Y, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Y Negative JogJoint Error! code = {0}", nRet));*/
           
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() + ","
                    + (m_dbWorldY / 1000 - 0.1).ToString()
                    + "," + (m_dbWorldZ / 1000).ToString()
                    + "," + m_dbWorldRx.ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=0.5,v=0.05)\n"; //movel Y軸負移動  單位m,rad 
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void YNegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Y);
            if (nRet != 0)
                MessageBox.Show(string.Format("Y JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=0.5)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }


        private void ZPositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Z, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Z Positive JogJoint Error! code = {0}", nRet));*/
            
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() + ","
                    + (m_dbWorldY / 1000).ToString()
                    + "," + (m_dbWorldZ / 1000 + 0.1).ToString()
                    + "," + m_dbWorldRx.ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=0.5,v=0.05)\n"; //movel Z軸正移動  單位m,rad          
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void ZPositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Z);
            if (nRet != 0)
                MessageBox.Show(string.Format("Z JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=0.5)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }

        private void ZNegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Z, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Z Negative JogJoint Error! code = {0}", nRet));*/
            
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() + ","
                    + (m_dbWorldY / 1000).ToString()
                    + "," + (m_dbWorldZ / 1000 - 0.1).ToString()
                    + "," + m_dbWorldRx.ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=0.5,v=0.05)\n"; //movel Z軸負移動  單位m,rad            
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void ZNegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Z);
            if (nRet != 0)
                MessageBox.Show(string.Format("Z JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=0.5)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }


        private void RxPositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Rx, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Rx Positive JogJoint Error! code = {0}", nRet));*/
            
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() +
                    "," + (m_dbWorldY / 1000).ToString() + ","
                    + (m_dbWorldZ / 1000).ToString() + ","
                    + (m_dbWorldRx + 1).ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=2,v=0.05)\n"; //movel X軸正轉  單位m,rad          
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void RxPositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Rx);
            if (nRet != 0)
                MessageBox.Show(string.Format("Rx JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=2)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }

        private void RxNegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Rx, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Rx Negative JogJoint Error! code = {0}", nRet));*/
           
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() +
                    "," + (m_dbWorldY / 1000).ToString() + ","
                    + (m_dbWorldZ / 1000).ToString() + ","
                    + (m_dbWorldRx - 1).ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=2,v=0.05)\n"; //movel X軸負轉  單位m,rad          
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void RxNegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Rx);
            if (nRet != 0)
                MessageBox.Show(string.Format("Rx JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=2)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }


        private void RyPositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Ry, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Ry Positive JogJoint Error! code = {0}", nRet));*/
            
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() +
                    "," + (m_dbWorldY / 1000).ToString() + ","
                    + (m_dbWorldZ / 1000).ToString() + ","
                    + m_dbWorldRx.ToString() + ","
                    + (m_dbWorldRy + 1).ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=2,v=0.05)\n"; //movel Y軸正轉  單位m,rad          
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void RyPositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Ry);
            if (nRet != 0)
                MessageBox.Show(string.Format("Ry JogJointStop Error! code = {0}", nRet));*/
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=2)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }

        private void RyNegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Ry, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Ry Negative JogJoint Error! code = {0}", nRet));*/
            
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() +
                    "," + (m_dbWorldY / 1000).ToString() + ","
                    + (m_dbWorldZ / 1000).ToString() + ","
                    + m_dbWorldRx.ToString() + ","
                    + (m_dbWorldRy - 1).ToString() + ","
                    + m_dbWorldRz.ToString() + "]),a=2,v=0.05)\n"; //movel Y軸負轉  單位m,rad          
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void RyNegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Ry);
            if (nRet != 0)
                MessageBox.Show(string.Format("Ry JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=2)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }


        private void RzPositiveBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Rz, Jog_Directions.Plus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Rz Positive JogJoint Error! code = {0}", nRet));*/
            
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() +
                    "," + (m_dbWorldY / 1000).ToString() + ","
                    + (m_dbWorldZ / 1000).ToString() + ","
                    + m_dbWorldRx.ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + (m_dbWorldRz + 1).ToString() + "]),a=2,v=0.05)\n"; //movel Z軸正轉  單位m,rad          
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void RzPositiveBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Rz);
            if (nRet != 0)
                MessageBox.Show(string.Format("Rz JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=2)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }

        private void RzNegativeBtn_MouseDown(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnate(0, Coordnates.Rz, Jog_Directions.Minus, m_nCurrJogSpeed);
            if (nRet != 0)
                MessageBox.Show(string.Format("Rz Negative JogJoint Error! code = {0}", nRet));*/
            
                ba = asen.GetBytes(sendString);
                stream.Write(ba, 0, ba.Length);              //輸出程式開頭

                string task = "movel(get_inverse_kin(p[" + (m_dbWorldX / 1000).ToString() +
                    "," + (m_dbWorldY / 1000).ToString() + ","
                    + (m_dbWorldZ / 1000).ToString() + ","
                    + m_dbWorldRx.ToString() + ","
                    + m_dbWorldRy.ToString() + ","
                    + (m_dbWorldRz - 1).ToString() + "]),a=2,v=0.05)\n"; //movel Z軸負轉  單位m,rad          
                ba = asen.GetBytes(task);
                stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

                ba = asen.GetBytes("end \n");                            //程式結束開始執行
                stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
            
        }

        private void RzNegativeBtn_MouseUp(object sender, MouseEventArgs e)
        {/*
            int nRet = g_robotCtrl.JogCoordnateStop(0, Coordnates.Rz);
            if (nRet != 0)
                MessageBox.Show(string.Format("Rz JogJointStop Error! code = {0}", nRet));*/
            
            ba = asen.GetBytes(sendString);
            stream.Write(ba, 0, ba.Length);              //輸出程式開頭

            string task = "stopl(a=2)\n"; //movel stop
            ba = asen.GetBytes(task);
            stream.Write(ba, 0, ba.Length);                            //送 取得資料 指令

            ba = asen.GetBytes("end \n");                            //程式結束開始執行
            stream.Write(ba, 0, ba.Length);              //接收到 end 指令, UR即開始執行剛剛接收的檔案
        }


        private void JointsPtPInsBtn_Click(object sender, EventArgs e)  //MovePtoPAbs指令
        {
            int nRowCount = 0;
            nRowCount = dataGridViewList.RowCount;

            string strIndex = string.Format("{0}", nRowCount);
            string strIns = "MovePtoPIns";
            string strRobot = string.Format("{0}", 0);
            string strJ1 = string.Format("{0:0.###}", m_dbJ1Degree);
            string strJ2 = string.Format("{0:0.###}", m_dbJ2Degree);
            string strJ3 = string.Format("{0:0.###}", m_dbJ3Degree);
            string strJ4 = string.Format("{0:0.###}", m_dbJ4Degree);
            string strJ5 = string.Format("{0:0.###}", m_dbJ5Degree);
            string strJ6 = string.Format("{0:0.###}", m_dbJ6Degree);
            string strX = string.Format("{0:0.###}", m_dbWorldX);
            string strY = string.Format("{0:0.###}", m_dbWorldY);
            string strZ = string.Format("{0:0.###}", m_dbWorldZ);
            string strRx = string.Format("{0:0.###}", m_dbWorldRx);
            string strRy = string.Format("{0:0.###}", m_dbWorldRy);
            string strRz = string.Format("{0:0.###}", m_dbWorldRz);
            string strAcc = string.Format("{0}", 0);
            string strDec = string.Format("{0}" , 0);
            string strSpeed = string.Format("{0}", 0);
            string strPar1 = string.Format("{0}", 0);
            string strPar2 = string.Format("{0}", 0);
            DataGridViewRowCollection rows = dataGridViewList.Rows;
            rows.Add(new Object[] { strIndex, strIns, strRobot, strJ1, strJ2, strJ3, strJ4, strJ5, strJ6, strX, strY, strZ, strRx, strRy, strRz, strAcc, strDec, strSpeed, strPar1, strPar2 });    
        }

        private void JointsMoveInsBtn_Click(object sender, EventArgs e)   //LinePtoPAbs指令
        {
            int nRowCount = 0;
            nRowCount = dataGridViewList.RowCount;

            string strIndex = string.Format("{0}", nRowCount);
            string strIns = "LinePtoPIns";
            string strRobot = string.Format("{0}", 0);
            string strJ1 = string.Format("{0:0.###}", m_dbJ1Degree);
            string strJ2 = string.Format("{0:0.###}", m_dbJ2Degree);
            string strJ3 = string.Format("{0:0.###}", m_dbJ3Degree);
            string strJ4 = string.Format("{0:0.###}", m_dbJ4Degree);
            string strJ5 = string.Format("{0:0.###}", m_dbJ5Degree);
            string strJ6 = string.Format("{0:0.###}", m_dbJ6Degree);
            string strX = string.Format("{0:0.###}", m_dbWorldX);
            string strY = string.Format("{0:0.###}", m_dbWorldY);
            string strZ = string.Format("{0:0.###}", m_dbWorldZ);
            string strRx = string.Format("{0:0.###}", m_dbWorldRx);
            string strRy = string.Format("{0:0.###}", m_dbWorldRy);
            string strRz = string.Format("{0:0.###}", m_dbWorldRz);
            string strAcc = string.Format("{0}", 0);
            string strDec = string.Format("{0}", 0);
            string strSpeed = string.Format("{0}", 0);
            string strPar1 = string.Format("{0}", 0);
            string strPar2 = string.Format("{0}", 0);
            DataGridViewRowCollection rows = dataGridViewList.Rows;
            rows.Add(new Object[] { strIndex, strIns, strRobot, strJ1, strJ2, strJ3, strJ4, strJ5, strJ6, strX, strY, strZ, strRx, strRy, strRz, strAcc, strDec, strSpeed, strPar1, strPar2 });

            //rows.get
        }

        private void ArcMidPtInsBtn_Click(object sender, EventArgs e)   //LinePtoPAbs指令
        {
            int nRowCount = 0;
            nRowCount = dataGridViewList.RowCount;

            string strIndex = string.Format("{0}", nRowCount);
            string strIns = "ArcMidIns";
            string strRobot = string.Format("{0}", 0);
            string strJ1 = string.Format("{0:0.###}", m_dbJ1Degree);
            string strJ2 = string.Format("{0:0.###}", m_dbJ2Degree);
            string strJ3 = string.Format("{0:0.###}", m_dbJ3Degree);
            string strJ4 = string.Format("{0:0.###}", m_dbJ4Degree);
            string strJ5 = string.Format("{0:0.###}", m_dbJ5Degree);
            string strJ6 = string.Format("{0:0.###}", m_dbJ6Degree);
            string strX = string.Format("{0:0.###}", m_dbWorldX);
            string strY = string.Format("{0:0.###}", m_dbWorldY);
            string strZ = string.Format("{0:0.###}", m_dbWorldZ);
            string strRx = string.Format("{0:0.###}", m_dbWorldRx);
            string strRy = string.Format("{0:0.###}", m_dbWorldRy);
            string strRz = string.Format("{0:0.###}", m_dbWorldRz);
            string strAcc = string.Format("{0}", 0);
            string strDec = string.Format("{0}", 0);
            string strSpeed = string.Format("{0}", 0);
            string strPar1 = string.Format("{0}", 0);
            string strPar2 = string.Format("{0}", 0);
            DataGridViewRowCollection rows = dataGridViewList.Rows;
            rows.Add(new Object[] { strIndex, strIns, strRobot, strJ1, strJ2, strJ3, strJ4, strJ5, strJ6, strX, strY, strZ, strRx, strRy, strRz, strAcc, strDec, strSpeed, strPar1, strPar2 });
        }

        private void ArcEndPtInsBtn_Click(object sender, EventArgs e)  //ArcEnd指令
        {
            int nRowCount = 0;
            nRowCount = dataGridViewList.RowCount;

            string strIndex = string.Format("{0}", nRowCount);
            string strIns = "ArcEndIns";
            string strRobot = string.Format("{0}", 0);
            string strJ1 = string.Format("{0:0.###}", m_dbJ1Degree);
            string strJ2 = string.Format("{0:0.###}", m_dbJ2Degree);
            string strJ3 = string.Format("{0:0.###}", m_dbJ3Degree);
            string strJ4 = string.Format("{0:0.###}", m_dbJ4Degree);
            string strJ5 = string.Format("{0:0.###}", m_dbJ5Degree);
            string strJ6 = string.Format("{0:0.###}", m_dbJ6Degree);
            string strX = string.Format("{0:0.###}", m_dbWorldX);
            string strY = string.Format("{0:0.###}", m_dbWorldY);
            string strZ = string.Format("{0:0.###}", m_dbWorldZ);
            string strRx = string.Format("{0:0.###}", m_dbWorldRx);
            string strRy = string.Format("{0:0.###}", m_dbWorldRy);
            string strRz = string.Format("{0:0.###}", m_dbWorldRz);
            string strAcc = string.Format("{0}", 0);
            string strDec = string.Format("{0}", 0);
            string strSpeed = string.Format("{0}", 0);
            string strPar1 = string.Format("{0}", 0);
            string strPar2 = string.Format("{0}", 0);
            DataGridViewRowCollection rows = dataGridViewList.Rows;
            rows.Add(new Object[] { strIndex, strIns, strRobot, strJ1, strJ2, strJ3, strJ4, strJ5, strJ6, strX, strY, strZ, strRx, strRy, strRz, strAcc, strDec, strSpeed, strPar1, strPar2 });
        }

        private void MoveAbsInsBtn_Click(object sender, EventArgs e)  //MoveAbs指令
        {
            int nRowCount = 0;
            nRowCount = dataGridViewList.RowCount;

            string strIndex = string.Format("{0}", nRowCount);
            string strIns = "MoveAbsIns";
            string strRobot = string.Format("{0}", 0);
            string strJ1 = string.Format("{0:0.###}", m_dbJ1Degree);
            string strJ2 = string.Format("{0:0.###}", m_dbJ2Degree);
            string strJ3 = string.Format("{0:0.###}", m_dbJ3Degree);
            string strJ4 = string.Format("{0:0.###}", m_dbJ4Degree);
            string strJ5 = string.Format("{0:0.###}", m_dbJ5Degree);
            string strJ6 = string.Format("{0:0.###}", m_dbJ6Degree);
            string strX = string.Format("{0:0.###}", m_dbWorldX);
            string strY = string.Format("{0:0.###}", m_dbWorldY);
            string strZ = string.Format("{0:0.###}", m_dbWorldZ);
            string strRx = string.Format("{0:0.###}", m_dbWorldRx);
            string strRy = string.Format("{0:0.###}", m_dbWorldRy);
            string strRz = string.Format("{0:0.###}", m_dbWorldRz);
            string strAcc = string.Format("{0}", 0);
            string strDec = string.Format("{0}", 0);
            string strSpeed = string.Format("{0}", 0);
            string strPar1 = string.Format("{0}", 1);
            string strPar2 = string.Format("{0}", 0);
            DataGridViewRowCollection rows = dataGridViewList.Rows;
            rows.Add(new Object[] { strIndex, strIns, strRobot, strJ1, strJ2, strJ3, strJ4, strJ5, strJ6, strX, strY, strZ, strRx, strRy, strRz, strAcc, strDec, strSpeed, strPar1, strPar2 });
        }

        private void LineAbsInsBtn_Click(object sender, EventArgs e)   //LineAbs指令
        {
            int nRowCount = 0;
            nRowCount = dataGridViewList.RowCount;

            string strIndex = string.Format("{0}", nRowCount);
            string strIns = "LineAbsIns";
            string strRobot = string.Format("{0}", 0);
            string strJ1 = string.Format("{0:0.###}", m_dbJ1Degree);
            string strJ2 = string.Format("{0:0.###}", m_dbJ2Degree);
            string strJ3 = string.Format("{0:0.###}", m_dbJ3Degree);
            string strJ4 = string.Format("{0:0.###}", m_dbJ4Degree);
            string strJ5 = string.Format("{0:0.###}", m_dbJ5Degree);
            string strJ6 = string.Format("{0:0.###}", m_dbJ6Degree);
            string strX = string.Format("{0:0.###}", m_dbWorldX);
            string strY = string.Format("{0:0.###}", m_dbWorldY);
            string strZ = string.Format("{0:0.###}", m_dbWorldZ);
            string strRx = string.Format("{0:0.###}", m_dbWorldRx);
            string strRy = string.Format("{0:0.###}", m_dbWorldRy);
            string strRz = string.Format("{0:0.###}", m_dbWorldRz);
            string strAcc = string.Format("{0}", 0);
            string strDec = string.Format("{0}", 0);
            string strSpeed = string.Format("{0}", 0);
            string strPar1 = string.Format("{0}", 1);
            string strPar2 = string.Format("{0}", 0);
            DataGridViewRowCollection rows = dataGridViewList.Rows;
            rows.Add(new Object[] { strIndex, strIns, strRobot, strJ1, strJ2, strJ3, strJ4, strJ5, strJ6, strX, strY, strZ, strRx, strRy, strRz, strAcc, strDec, strSpeed, strPar1, strPar2 });
        }

        private void MoveRelInsBtn_Click(object sender, EventArgs e)  //MoveRel指令
        {
            int nRowCount = 0;
            nRowCount = dataGridViewList.RowCount;

            string strIndex = string.Format("{0}", nRowCount);
            string strIns = "MoveRelIns";
            string strRobot = string.Format("{0}", 0);
            string strJ1 = string.Format("{0:0.###}", m_dbJ1Degree);
            string strJ2 = string.Format("{0:0.###}", m_dbJ2Degree);
            string strJ3 = string.Format("{0:0.###}", m_dbJ3Degree);
            string strJ4 = string.Format("{0:0.###}", m_dbJ4Degree);
            string strJ5 = string.Format("{0:0.###}", m_dbJ5Degree);
            string strJ6 = string.Format("{0:0.###}", m_dbJ6Degree);
            string strX = string.Format("{0:0.###}", m_dbWorldX);
            string strY = string.Format("{0:0.###}", m_dbWorldY);
            string strZ = string.Format("{0:0.###}", m_dbWorldZ);
            string strRx = string.Format("{0:0.###}", m_dbWorldRx);
            string strRy = string.Format("{0:0.###}", m_dbWorldRy);
            string strRz = string.Format("{0:0.###}", m_dbWorldRz);
            string strAcc = string.Format("{0}", 0);
            string strDec = string.Format("{0}", 0);
            string strSpeed = string.Format("{0}", 0);
            string strPar1 = string.Format("{0}", 1);
            string strPar2 = string.Format("{0}", 0);
            DataGridViewRowCollection rows = dataGridViewList.Rows;
            rows.Add(new Object[] { strIndex, strIns, strRobot, strJ1, strJ2, strJ3, strJ4, strJ5, strJ6, strX, strY, strZ, strRx, strRy, strRz, strAcc, strDec, strSpeed, strPar1, strPar2 });
        }

        private void LineRelInsBtn_Click(object sender, EventArgs e)   //LineRel指令
        {
            int nRowCount = 0;
            nRowCount = dataGridViewList.RowCount;

            string strIndex = string.Format("{0}", nRowCount);
            string strIns = "LineRelIns";
            string strRobot = string.Format("{0}", 0);
            string strJ1 = string.Format("{0:0.###}", m_dbJ1Degree);
            string strJ2 = string.Format("{0:0.###}", m_dbJ2Degree);
            string strJ3 = string.Format("{0:0.###}", m_dbJ3Degree);
            string strJ4 = string.Format("{0:0.###}", m_dbJ4Degree);
            string strJ5 = string.Format("{0:0.###}", m_dbJ5Degree);
            string strJ6 = string.Format("{0:0.###}", m_dbJ6Degree);
            string strX = string.Format("{0:0.###}", m_dbWorldX);
            string strY = string.Format("{0:0.###}", m_dbWorldY);
            string strZ = string.Format("{0:0.###}", m_dbWorldZ);
            string strRx = string.Format("{0:0.###}", m_dbWorldRx);
            string strRy = string.Format("{0:0.###}", m_dbWorldRy);
            string strRz = string.Format("{0:0.###}", m_dbWorldRz);
            string strAcc = string.Format("{0}", 0);
            string strDec = string.Format("{0}", 0);
            string strSpeed = string.Format("{0}", 0);
            string strPar1 = string.Format("{0}", 1);
            string strPar2 = string.Format("{0}", 0);
            DataGridViewRowCollection rows = dataGridViewList.Rows;
            rows.Add(new Object[] { strIndex, strIns, strRobot, strJ1, strJ2, strJ3, strJ4, strJ5, strJ6, strX, strY, strZ, strRx, strRy, strRz, strAcc, strDec, strSpeed, strPar1, strPar2 });
        }

    }



    /*
    class RobotPos
    {
        public static TextBox _tbJ1;
        public static TextBox _tbJ2;
        public static TextBox _tbJ3;
        public static TextBox _tbJ4;
        public static TextBox _tbJ5;
        public static TextBox _tbJ6;

        delegate void PrintHandler(TextBox tb, string text);

        public static void GetPos(RobotCtrlCenter.RobotCtrl ctrl)
        {
            double dbBase = 0;
            double dbShoulder = 0;
            double dbElbow = 0;
            double dbWrist1 = 0;
            double dbWrist2 = 0;
            double dbWrist3 = 0;

            ctrl.GetJointDegree(0, ref dbBase, ref dbShoulder, ref dbElbow, ref dbWrist1, ref dbWrist2, ref dbWrist3);
            Print(_tbJ1, dbBase.ToString());
        }


        public static void Print(TextBox tb, string text)
        {
            //判斷這個TextBox的物件是否在同一個執行緒上
            if (tb.InvokeRequired)
            {
                //當InvokeRequired為true時，表示在不同的執行緒上，所以進行委派的動作!!
                PrintHandler ph = new PrintHandler(Print);
                tb.Invoke(ph, tb, text);
            }
            else
            {
                //表示在同一個執行緒上了，所以可以正常的呼叫到這個TextBox物件
                tb.Text = tb.Text + text + Environment.NewLine;
            }
        }
    }

    */

    public class MyCallback : ROS.ServiceCallback
    {
        private RobotCtrl Robot;
        public MyCallback(RobotCtrl robot)
        {
            Robot = robot;
        }
        public override void Call(byte[] request, ref byte[] response)
        {
            int result = Robot.MovePtoPAbs(0, BitConverter.ToDouble(request, 196), BitConverter.ToDouble(request, 204), BitConverter.ToDouble(request, 212), BitConverter.ToDouble(request, 220), BitConverter.ToDouble(request, 228), BitConverter.ToDouble(request, 236));
            byte[] returnCode = BitConverter.GetBytes(result);
            response[4] = returnCode[0];
            response[5] = returnCode[1];
            response[6] = returnCode[2];
            response[7] = returnCode[3];
        }
    }

    public class MyCallback2 : ROS.ServiceCallback
    {
        private RobotCtrl Robot;
        public MyCallback2(RobotCtrl robot)
        {
            Robot = robot;
        }
        public override void Call(byte[] request, ref byte[] response)
        {
           // int result = Robot.JogJoint(0, Joints.J1, Jog_Directions.Plus, Jog_Speed.High);
            int result = Robot.JogJoint(0, (Joints)BitConverter.ToInt32(request, 0), (Jog_Directions)BitConverter.ToInt32(request, 4), (Jog_Speed)BitConverter.ToInt32(request, 8));
            byte[] returnCode = BitConverter.GetBytes(result);
            response[4] = returnCode[0];
            response[5] = returnCode[1];
            response[6] = returnCode[2];
            response[7] = returnCode[3];
        }
    }



}
