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
        /*
        STDMETHOD(LineSingleRel)(LONG lDirection, DOUBLE dbPos, LONG* lRet);
        STDMETHOD(LineSingleRelWait)(LONG lDirection, DOUBLE dbPos, LONG* lRet);
        STDMETHOD(LineSingleRelSpeed)(LONG lDirection, DOUBLE dbPos, DOUBLE dbSpeed, DOUBLE dbAcc, DOUBLE dbDec, LONG* lRet);
        STDMETHOD(LineSingleRelSpeedWait)(LONG lDirection, DOUBLE dbPos, DOUBLE dbSpeed, DOUBLE dbAcc, DOUBLE dbDec, LONG* lRet);
        STDMETHOD(LineSingleAbs)(LONG lDirection, DOUBLE dbPos, LONG* lRet);
        STDMETHOD(LineSingleAbsWait)(LONG lDirection, DOUBLE dbPos, LONG* lRet);
        STDMETHOD(LineSingleAbsSpeed)(LONG lDirection, DOUBLE dbPos, DOUBLE dbSpeed, DOUBLE dbAcc, DOUBLE dbDec, LONG* lRet);
        STDMETHOD(LineSingleAbsSpeedWait)(LONG lDirection, DOUBLE dbPos, DOUBLE dbSpeed, DOUBLE dbAcc, DOUBLE dbDec, LONG* lRet);
        */

        /// <summary>
        /// 單一關節Jog移動
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="nDir">移動方向</param>
        /// <param name="dbSpeedPercentage">速度百分比</param>
        /// <returns>回傳是否成功,0:成功, 其他數字:失敗</returns>
        public int JogCoordnate(int nRobotIndex, Coordnates nCoordnate, Jog_Directions nDir, Jog_Speed nSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.JogCoordnate(nCoordnate, nDir, nSpeed);
            return nRet;
        }

        /// <summary>
        /// 單一關節Jog移動停止
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <returns>回傳是否成功,0:成功, 其他數字:失敗</returns>
        public int JogCoordnateStop(int nRobotIndex, Coordnates nCoordnate)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.JogCoordnateStop(nCoordnate);
            return nRet;
        }

        /// <summary>
        /// 笛卡兒座標系_單一方向 移動(絕對位置)(角度)(無輸入速度)(不等待繼續)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nCoordnate">座標方向</param>
        /// <param name="dbPos">指定位置</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int LinePtoPAbs(int nRobotIndex, double dbX, double dbY, double dbZ, double dbRx, double dbRy, double dbRz)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.LinePtoP(SpeedTypes.NoUse, WaitTypes.Wait, dbX, dbY, dbZ, dbRx, dbRy, dbRz, 0, 0, 0);
            return nRet;
        }

        public int LinePtoPAbs(int nRobotIndex, double dbX, double dbY, double dbZ, double dbRx, double dbRy, double dbRz, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.LinePtoP(SpeedTypes.Use, WaitTypes.Wait, dbX, dbY, dbZ, dbRx, dbRy, dbRz, dbAcc, dbDec, dbSpeed);
            return nRet;
        }

        public int LineAbs(int nRobotIndex, Coordnates nCoordnate, double dbValue)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.LineCoordnate(nCoordnate, MovementsTypes.Abs, SpeedTypes.NoUse, WaitTypes.Wait, dbValue, 0, 0, 0);
            return nRet;
        }

        public int LineAbs(int nRobotIndex, Coordnates nCoordnate, double dbValue, double dbAcc, double dbDec, double dbSpeed)
        {  
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.LineCoordnate(nCoordnate, MovementsTypes.Abs, SpeedTypes.Use, WaitTypes.Wait, dbValue, dbAcc, dbDec, dbSpeed);
            return nRet;
        }

        public int LineRel(int nRobotIndex, Coordnates nCoordnate, double dbValue)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.LineCoordnate(nCoordnate, MovementsTypes.Rel, SpeedTypes.NoUse, WaitTypes.Wait, dbValue, 0, 0, 0);
            return nRet;
        }

        public int LineRel(int nRobotIndex, Coordnates nCoordnate, double dbValue, double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.LineCoordnate(nCoordnate, MovementsTypes.Rel, SpeedTypes.Use, WaitTypes.Wait, dbValue, dbAcc, dbDec, dbSpeed);
            return nRet;
        }










        //     public int LineAbs(int nRobotIndex, Coordnates nCoordnate, double dbPos)
        //    {
        //         IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
        //        int nRet = IRobot.LineSingleCoordnate(nCoordnate, MovementsTypes.Abs, SpeedTypes.NoUse, WaitTypes.NoWait, dbPos, 0, 0, 0);
        //       return nRet;
        //    }



        //    public int LineAbs(int nRobotIndex, Coordnates nCoordnate, double dbPos, double dbAccPercentage, double dbDecPercentage, double dbSpeedPercentage)
        //   {
        //        IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
        //       int nRet = IRobot.LineSingleCoordnate(nCoordnate, MovementsTypes.Abs, SpeedTypes.Use, WaitTypes.NoWait, dbPos, dbAccPercentage, dbDecPercentage, dbSpeedPercentage);
        //       return nRet;
        //    }

        //   public int LineRel(int nRobotIndex, Coordnates nCoordnate, double dbPos)
        //   {
        //       IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
        //       int nRet = IRobot.LineSingleCoordnate(nCoordnate, MovementsTypes.Rel, SpeedTypes.NoUse, WaitTypes.NoWait, dbPos, 0, 0, 0);
        //       return nRet;
        //   }

        //   public int LineRel(int nRobotIndex, Coordnates nCoordnate, double dbPos, double dbAccPercentage, double dbDecPercentage, double dbSpeedPercentage)
        //   {
        //        IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
        //        int nRet = IRobot.LineSingleCoordnate(nCoordnate, MovementsTypes.Rel, SpeedTypes.Use, WaitTypes.NoWait, dbPos, dbAccPercentage, dbDecPercentage, dbSpeedPercentage);
        //        return nRet;
        //    }

        /*
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

    }
}
