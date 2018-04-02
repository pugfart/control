using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRobotMotion;

namespace RobotCtrlCenter
{
    public partial class RobotCtrl
    {
        /// <summary>
        /// 單一關節Jog移動
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="nDir">移動方向</param>
        /// <param name="dbSpeedPercentage">速度百分比</param>
        /// <returns>回傳是否成功,0:成功, 其他數字:失敗</returns>
        public int JogJoint(int nRobotIndex, Joints nJoint, Jog_Directions nDir, Jog_Speed nSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.JogJoint(nJoint, nDir, nSpeed);
            return nRet;

            /*
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            double dbAcc = 0;
            double dbDec = 0;
            double dbSpeed = 0;
            if (nSpeed == Jog_Speed.High)
            {
                dbAcc = 10000;dbDec = 10000; dbSpeed = 30;
            }
                
            else if (nSpeed == Jog_Speed.Mid)
            { dbAcc = 10000; dbDec = 10000; dbSpeed = 20; }
                
            else if (nSpeed == Jog_Speed.Mid)
            { dbAcc = 10000; dbDec = 10000; dbSpeed = 10; }
                

            double dbValue = 0;
            if (nDir == Jog_Directions.Plus)
                dbValue = 50;
            else if (nDir == Jog_Directions.Minus)
                dbValue = -50;

            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.NoWait, dbValue, dbAcc, dbDec, dbSpeed);
           */
          
            return nRet;
        }

        /// <summary>
        /// 單一關節Jog移動停止
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <returns>回傳是否成功,0:成功, 其他數字:失敗</returns>
        public int JogJointStop(int nRobotIndex, Joints nJoint)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.JogJointStop(nJoint);
            return nRet;
        }

        /// <summary>
        /// PtoP(絕對位置)(無輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbX">J1絕對移動到角度值(絕對位置)</param>
        /// <param name="dbY">J2絕對移動到角度值(絕對位置)</param>
        /// <param name="dbZ">J3絕對移動到角度值(絕對位置)</param>
        /// <param name="dbRx">J4絕對移動到角度值(絕對位置)</param>
        /// <param name="dbRy">J5絕對移動到角度值(絕對位置)</param>
        /// <param name="dbRz">J6絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MovePtoPAbs(int nRobotIndex, double dbX, double dbY, double dbZ, double dbRx, double dbRy, double dbRz)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MovePtoP(SpeedTypes.NoUse, WaitTypes.Wait,
                dbX, dbY, dbZ, dbRx, dbRy, dbRz, 0, 0, 0);
            return nRet;
        }

        /// <summary>
        /// PtoP(絕對位置)(輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbX">J1絕對移動到角度值(絕對位置)</param>
        /// <param name="dbY">J2絕對移動到角度值(絕對位置)</param>
        /// <param name="dbZ">J3絕對移動到角度值(絕對位置)</param>
        /// <param name="dbRx">J4絕對移動到角度值(絕對位置)</param>
        /// <param name="dbRy">J5絕對移動到角度值(絕對位置)</param>
        /// <param name="dbRz">J6絕對移動到角度值(絕對位置)</param>
        /// <param name="dbAcc">加速度比例(0-100)</param>
        /// <param name="dbDec">減速度比例(0-100)</param>
        /// <param name="dbSpeed">最大速比例(0-100)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MovePtoPAbs(int nRobotIndex, double dbX, double dbY, double dbZ, double dbRx, double dbRy, double dbRz, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MovePtoP(SpeedTypes.Use, WaitTypes.Wait,
                dbX, dbY, dbZ, dbRx, dbRy, dbRz, dbAcc, dbDec, dbSpeed);
            return nRet;
        }

        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveAbs(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Abs, SpeedTypes.NoUse, WaitTypes.Wait, dbDegree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <param name="dbAcc">加速度比例(0-100)</param>
        /// <param name="dbDec">減速度比例(0-100)</param>
        /// <param name="dbSpeed">最大速比例(0-100)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveAbs(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Abs, SpeedTypes.Use, WaitTypes.Wait, dbDegree, dbAcc, dbDec, dbSpeed);
            return 1;
        }


        /// <summary>
        /// 單一關節移動(相對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveRel(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.NoUse, WaitTypes.Wait, dbDegree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(角度)(輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <param name="dbAcc">加速度比例(0-100)</param>
        /// <param name="dbDec">減速度比例(0-100)</param>
        /// <param name="dbSpeed">最大速比例(0-100)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveRel(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.Use, WaitTypes.Wait, dbDegree, dbAcc, dbDec, dbSpeed);
            return 1;
        }



















        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int JointAbs(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Abs, SpeedTypes.NoUse, WaitTypes.NoWait, dbDegree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(有輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <param name="dbAcc">加速度比例(0-100)</param>
        /// <param name="dbDec">減速度比例(0-100)</param>
        /// <param name="dbSpeed">最大速比例(0-100)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int JointAbs(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Abs, SpeedTypes.Use, WaitTypes.NoWait, dbDegree, dbAcc, dbDec, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 全關節移動(絕對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">J1絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ2Degree">J2絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ3Degree">J3絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ4Degree">J4絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ5Degree">J5絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ6Degree">J6絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int JointAbs(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint( MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.NoWait,
                dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 全關節移動(絕對位置)(角度)(有輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">J1絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ2Degree">J2絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ3Degree">J3絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ4Degree">J4絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ5Degree">J5絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ6Degree">J6絕對移動到角度值(絕對位置)</param>
        /// <param name="dbAcc">加速度比例(0-100)</param>
        /// <param name="dbDec">減速度比例(0-100)</param>
        /// <param name="dbSpeed">最大速比例(0-100)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int JointAbs(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.NoWait,
                dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, dbAcc, dbDec, dbSpeed);
            return 1;
        }


        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int JointAbsWait(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Abs, SpeedTypes.NoUse, WaitTypes.Wait, dbDegree, 0, 0, 0);
            return 1;
        }
  

        /*

        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointAbsDegree(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.NoWait, dbDegree, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(無輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointAbsDegreeWait(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.Wait, dbDegree, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(有輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointAbsDegreeSpeed(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.NoWait, dbDegree, dbAcc, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(有輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointAbsDegreeSpeedWait(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.Wait, dbDegree, dbAcc, dbSpeed);
            return 1;
        }
        */


        /*  
      /// <summary>
      /// 單一關節移動(絕對位置)(徑度)(無輸入速度)(不等待繼續)
      /// </summary>
      /// <param name="nRobotIndex">手臂代號</param>
      /// <param name="nJoint">關節代號</param>
      /// <param name="dbDegree">絕對移動到徑度值(絕對位置)</param>
      /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
      public int MoveJointAbsRadian(int nRobotIndex, Joints nJoint, double dbRadian)
      {
          IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
          int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Radian, SpeedTypes.NoUse, WaitTypes.NoWait, dbRadian, 0, 0);
          return 1;
      }

      /// <summary>
      /// 單一關節移動(絕對位置)(徑度)(無輸入速度)(等待繼續)
      /// </summary>
      /// <param name="nRobotIndex">手臂代號</param>
      /// <param name="nJoint">關節代號</param>
      /// <param name="dbDegree">絕對移動到徑度值(絕對位置)</param>
      /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
      public int MoveJointAbsRadianWait(int nRobotIndex, Joints nJoint, double dbRadian)
      {
          IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
          int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Radian, SpeedTypes.NoUse, WaitTypes.Wait, dbRadian, 0, 0);
          return 1;
      }

      /// <summary>
      /// 單一關節移動(絕對位置)(徑度)(有輸入速度)(不等待繼續)
      /// </summary>
      /// <param name="nRobotIndex">手臂代號</param>
      /// <param name="nJoint">關節代號</param>
      /// <param name="dbDegree">絕對移動到徑度值(絕對位置)</param>
      /// <param name="dbAcc">移動時加速度</param>
      /// <param name="dbSpeed">移動時最大速度</param>
      /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
      public int MoveJointAbsRadianSpeed(int nRobotIndex, Joints nJoint, double dbRadian, double dbAcc, double dbSpeed)
      {
          IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
          int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Radian, SpeedTypes.Use, WaitTypes.NoWait, dbRadian, dbAcc, dbSpeed);
          return 1;
      }

      /// <summary>
      /// 單一關節移動(絕對位置)(徑度)(有輸入速度)(等待繼續)
      /// </summary>
      /// <param name="nRobotIndex">手臂代號</param>
      /// <param name="nJoint">關節代號</param>
      /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
      /// <param name="dbAcc">移動時加速度</param>
      /// <param name="dbSpeed">移動時最大速度</param>
      /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
      public int MoveJointAbsRadianSpeedWait(int nRobotIndex, Joints nJoint, double dbRadian, double dbAcc, double dbSpeed)
      {
          IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
          int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Radian, SpeedTypes.Use, WaitTypes.Wait, dbRadian, dbAcc, dbSpeed);
          return 1;
      }
      */



        /// <summary>
        /// 單一關節移動(相對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelDegree(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.NoUse, WaitTypes.NoWait, dbDegree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(角度)(無輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelDegreeWait(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.NoUse, WaitTypes.Wait, dbDegree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(角度)(有輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時均速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelDegreeSpeed(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.Use, WaitTypes.NoWait, dbDegree, dbAcc, dbDec, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(角度)(有輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時均速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelDegreeSpeedWait(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.Use, WaitTypes.Wait, dbDegree, dbAcc, dbDec, dbSpeed);
            return 1;
        }


        /// <summary>
        /// 單一關節移動(相對位置)(徑度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動徑度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelRadian(int nRobotIndex, Joints nJoint, double dbRadian)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.NoUse, WaitTypes.NoWait, dbRadian, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(徑度)(無輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelRadianWait(int nRobotIndex, Joints nJoint, double dbRadian)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.NoUse, WaitTypes.Wait, dbRadian, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(徑度)(有輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動徑度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時均速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelRadianSpeed(int nRobotIndex, Joints nJoint, double dbRadian, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.Use, WaitTypes.NoWait, dbRadian, dbAcc, dbDec, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(徑度)(有輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動徑度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時均速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelRadianSpeedWait(int nRobotIndex, Joints nJoint, double dbRadian, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveJoint(nJoint, MovementsTypes.Rel, SpeedTypes.Use, WaitTypes.Wait, dbRadian, dbAcc, dbDec, dbSpeed);
            return 1;
        }



        /// <summary>
        /// 全部關節移動(絕對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">關節1 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ2Degree">關節2 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ3Degree">關節3 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ4Degree">關節4 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ5Degree">關節5 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ6Degree">關節6 絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointAbsDegree(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.NoWait,
                    dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(絕對位置)(角度)(無輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">關節1 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ2Degree">關節2 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ3Degree">關節3 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ4Degree">關節4 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ5Degree">關節5 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ6Degree">關節6 絕對移動到角度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointAbsDegreeWait(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.Wait,
                dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(絕對位置)(角度)(有輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">關節1 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ2Degree">關節2 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ3Degree">關節3 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ4Degree">關節4 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ5Degree">關節5 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ6Degree">關節6 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointAbsDegreeSpeed(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.NoWait,
                dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, dbAcc, dbDec, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(絕對位置)(角度)(有輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">關節1 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ2Degree">關節2 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ3Degree">關節3 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ4Degree">關節4 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ5Degree">關節5 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbJ6Degree">關節6 絕對移動到角度值(絕對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointAbsDegreeSpeedWait(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.Wait,
                dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, dbAcc, dbDec, dbSpeed);
            return 1;
        }



        /// <summary>
        /// 全部關節移動(絕對位置)(徑度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Radian">關節1 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ2Radian">關節2 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ3Radian">關節3 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ4Radian">關節4 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ5Radian">關節5 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ6Radian">關節6 絕對移動到徑度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointAbsRadian(int nRobotIndex, double dbJ1Radian, double dbJ2Radian, double dbJ3Radian, double dbJ4Radian, double dbJ5Radian, double dbJ6Radian)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Radian, SpeedTypes.NoUse, WaitTypes.NoWait,
                    dbJ1Radian, dbJ2Radian, dbJ3Radian, dbJ4Radian, dbJ5Radian, dbJ6Radian, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(絕對位置)(徑度)(無輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Radian">關節1 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ2Radian">關節2 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ3Radian">關節3 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ4Radian">關節4 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ5Radian">關節5 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ6Radian">關節6 絕對移動到徑度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointAbsRadianWait(int nRobotIndex, double dbJ1Radian, double dbJ2Radian, double dbJ3Radian, double dbJ4Radian, double dbJ5Radian, double dbJ6Radian)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Radian, SpeedTypes.NoUse, WaitTypes.Wait,
                dbJ1Radian, dbJ2Radian, dbJ3Radian, dbJ4Radian, dbJ5Radian, dbJ6Radian, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(絕對位置)(徑度)(有輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Radian">關節1 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ2Radian">關節2 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ3Radian">關節3 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ4Radian">關節4 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ5Radian">關節5 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ6Radian">關節6 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointAbsRadianSpeed(int nRobotIndex, double dbJ1Radian, double dbJ2Radian, double dbJ3Radian, double dbJ4Radian, double dbJ5Radian, double dbJ6Radian, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Radian, SpeedTypes.Use, WaitTypes.NoWait,
                dbJ1Radian, dbJ2Radian, dbJ3Radian, dbJ4Radian, dbJ5Radian, dbJ6Radian, dbAcc, dbDec, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(絕對位置)(徑度)(有輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Radian">關節1 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ2Radian">關節2 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ3Radian">關節3 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ4Radian">關節4 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ5Radian">關節5 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbJ6Radian">關節6 絕對移動到徑度值(絕對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointAbsRadianSpeedWait(int nRobotIndex, double dbJ1Radian, double dbJ2Radian, double dbJ3Radian, double dbJ4Radian, double dbJ5Radian, double dbJ6Radian, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Radian, SpeedTypes.Use, WaitTypes.Wait,
                dbJ1Radian, dbJ2Radian, dbJ3Radian, dbJ4Radian, dbJ5Radian, dbJ6Radian, dbAcc, dbDec, dbSpeed);
            return 1;
        }





        /// <summary>
        /// 全部關節移動(相對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">關節1 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ2Degree">關節2 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ3Degree">關節3 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ4Degree">關節4 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ5Degree">關節5 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ6Degree">關節6 相對移動到角度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointRelDegree(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Rel, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.NoWait,
                    dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(相對位置)(角度)(無輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">關節1 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ2Degree">關節2 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ3Degree">關節3 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ4Degree">關節4 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ5Degree">關節5 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ6Degree">關節6 相對移動到角度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointRelDegreeWait(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Rel, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.Wait,
                dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(相對位置)(角度)(有輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">關節1 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ2Degree">關節2 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ3Degree">關節3 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ4Degree">關節4 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ5Degree">關節5 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ6Degree">關節6 相對移動到角度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointRelDegreeSpeed(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Rel, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.NoWait,
                dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, dbAcc, dbDec, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(相對位置)(角度)(有輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Degree">關節1 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ2Degree">關節2 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ3Degree">關節3 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ4Degree">關節4 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ5Degree">關節5 相對移動到角度值(相對位置)</param>
        /// <param name="dbJ6Degree">關節6 相對移動到角度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointRelDegreeSpeedWait(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Rel, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.Wait,
                dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, dbAcc, dbDec, dbSpeed);
            return 1;
        }



        /// <summary>
        /// 全部關節移動(絕對位置)(徑度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Radian">關節1 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ2Radian">關節2 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ3Radian">關節3 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ4Radian">關節4 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ5Radian">關節5 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ6Radian">關節6 相對移動到徑度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointRelRadian(int nRobotIndex, double dbJ1Radian, double dbJ2Radian, double dbJ3Radian, double dbJ4Radian, double dbJ5Radian, double dbJ6Radian)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Rel, UnitTypes.Radian, SpeedTypes.NoUse, WaitTypes.NoWait,
                    dbJ1Radian, dbJ2Radian, dbJ3Radian, dbJ4Radian, dbJ5Radian, dbJ6Radian, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(相對位置)(徑度)(無輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Radian">關節1 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ2Radian">關節2 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ3Radian">關節3 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ4Radian">關節4 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ5Radian">關節5 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ6Radian">關節6 相對移動到徑度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointRelRadianWait(int nRobotIndex, double dbJ1Radian, double dbJ2Radian, double dbJ3Radian, double dbJ4Radian, double dbJ5Radian, double dbJ6Radian)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Rel, UnitTypes.Radian, SpeedTypes.NoUse, WaitTypes.Wait,
                dbJ1Radian, dbJ2Radian, dbJ3Radian, dbJ4Radian, dbJ5Radian, dbJ6Radian, 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(相對位置)(徑度)(有輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Radian">關節1 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ2Radian">關節2 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ3Radian">關節3 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ4Radian">關節4 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ5Radian">關節5 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ6Radian">關節6 相對移動到徑度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointRelRadianSpeed(int nRobotIndex, double dbJ1Radian, double dbJ2Radian, double dbJ3Radian, double dbJ4Radian, double dbJ5Radian, double dbJ6Radian, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Rel, UnitTypes.Radian, SpeedTypes.Use, WaitTypes.NoWait,
                dbJ1Radian, dbJ2Radian, dbJ3Radian, dbJ4Radian, dbJ5Radian, dbJ6Radian, dbAcc, dbDec, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 全部關節移動(相對位置)(徑度)(有輸入速度)(等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1Radian">關節1 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ2Radian">關節2 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ3Radian">關節3 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ4Radian">關節4 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ5Radian">關節5 相對移動到徑度值(相對位置)</param>
        /// <param name="dbJ6Radian">關節6 相對移動到徑度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時最大速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveAllJointRelRadianSpeedWait(int nRobotIndex, double dbJ1Radian, double dbJ2Radian, double dbJ3Radian, double dbJ4Radian, double dbJ5Radian, double dbJ6Radian, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveAllJoint(MovementsTypes.Rel, UnitTypes.Radian, SpeedTypes.Use, WaitTypes.Wait,
                dbJ1Radian, dbJ2Radian, dbJ3Radian, dbJ4Radian, dbJ5Radian, dbJ6Radian, dbAcc, dbDec, dbSpeed);
            return 1;
        }








    }
}
