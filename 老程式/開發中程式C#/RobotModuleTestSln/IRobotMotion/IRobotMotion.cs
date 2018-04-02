using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRobotMotion
{
    public enum Joints { J1=1, J2, J3, J4, J5, J6 };
    public enum Coordnates { X=1, Y, Z, Rx, Ry, Rz };
    public enum MovementsTypes { Abs, Rel };
    public enum SpeedTypes { Use, NoUse };
    public enum UnitTypes { Degree, Radian };
    public enum WaitTypes { Wait, NoWait };
    public enum Jog_Directions { Plus, Minus };
    public enum Jog_Speed { High, Mid, Low };

    public interface IRobotMotion
    {
        string Name { get; }

        //連線Robot
        //bool Connect(string strRobotIP, int nPCListenPort);
        int Connect();

        //斷線Robot
        int Disconnect();

        //擷取 6關節角度值
        int GetJointDegree(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6);
        
        //擷取 6關節徑度值
        int GetJointRadian(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6);
       
        //擷取 世界座標系值
        int GetWorldPos(ref double dbX, ref double dbY, ref double dbZ, ref double dbRx, ref double dbRy, ref double dbRz);

        //擷取 6關節電流
        int GetJointCurrentValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6);

        //擷取 6關節電壓
        int GetJointVoltageValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6);

        //擷取 6關節扭矩
        int GetJointTorqueValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6);

        //擷取 6關節溫度
        int GetJointTemperatureValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6);

        //擷取 6關節速度
        int GetJointSpeedValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6);

        //擷取 6關節加速度
        int GetJointAccValue(ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6);

        //關節停止移動
        int StopMotionForPtoP(double dbDec);

        //關節停止移動
        int StopMotionForLine(double dbDec);

        /*
        //擷取 DI狀態
        int GetDIStatus(int nDINum, ref bool bStatus);

        //擷取 DO狀態
        int GetDOStatus(int nDONum, ref bool bStatus);
        
        int SetSoftHomeJointDegreePos(double dbJ1, double dbJ2, double dbJ3, double dbJ4, double dbJ5, double dbJ6);
        */

        //jog 運動
        int JogJoint(Joints nJoint, Jog_Directions nDir, Jog_Speed nSpeed);
        int JogJointStop(Joints nJoint);
        int JogCoordnate(Coordnates nCoordnate, Jog_Directions nDir, Jog_Speed nSpeed);
        int JogCoordnateStop(Coordnates nCoordnate);

        //在直角坐標系上 點對點 同動 (非直線)
        int MovePtoP(SpeedTypes nSpeedType, WaitTypes nWaitType,
                      double dbXValue, double dbYValue, double dbZValue, double dbRxValue, double dbRyValue, double dbRzValue, double dbAcc, double dbDec, double dbSpeed);

        //在直角坐標系上 點對點 直線運動
        int LinePtoP(SpeedTypes nSpeedType, WaitTypes nWaitType,
                      double dbXValue, double dbYValue, double dbZValue, double dbRxValue, double dbRyValue, double dbRzValue, double dbAcc, double dbDec, double dbSpeed);


        //在直角坐標系上 圓弧運動
        int Arc(SpeedTypes nSpeedType, WaitTypes nWaitType,
                      double dbMidXValue, double dbMidYValue, double dbMidZValue, double dbMidRxValue, double dbMidRyValue, double dbMidRzValue,
                      double dbEndXValue, double dbEndYValue, double dbEndZValue, double dbEndRxValue, double dbEndRyValue, double dbEndRzValue,
                      double dbAcc, double dbDec, double dbSpeed);

        //單一關節 移動角度
        int MoveJoint(Joints nJoint, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType, 
                        double dbValue, double dbAcc, double dbDec, double dbSpeed);

        //單一直角坐標系方向 移動
        int LineCoordnate(Coordnates nCoordnate, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
                        double dbValue, double dbAcc, double dbDec, double dbSpeed);







        int MoveAllJoint(MovementsTypes nMovementType, UnitTypes nUnitType, SpeedTypes nSpeedType, WaitTypes nWaitType, 
                       double dbJ1Value, double dbJ2Value, double dbJ3Value, double dbJ4Value, double dbJ5Value, double dbJ6Value, double dbAcc, double dbDec, double dbSpeed);


        int LineSingleCoordnate(Coordnates nCoordnate, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
                        double dbValue, double dbAccPercentage, double dbDecPercentage, double dbSpeedPercentage);

     //   int LineAllDirection(Directions nDirection, MovementsTypes nMovementType, SpeedTypes nSpeedType, WaitTypes nWaitType,
       //                 double dbXValue, double dbYValue, double dbZValue, double dbRxValue, double dbRyValue, double dbRzValue, double dbAcc, double dbSpeed);


        /*
        int MoveJointAbsDegree(Joints nJoint, double dbDegree);
        int MoveJointAbsDegreeWait(Joints nJoint, double dbDegree);
        int MoveJointAbsDegreeSpeed(Joints nJoint, double dbDegree, double dbAcc, double dbSpeed);
        int MoveJointAbsDegreeSpeedWait(Joints nJoint, double dbDegree, double dbAcc, double dbSpeed);
  
        int MoveJointRelDegree(Joints nJoint, double dbDegree);
        int MoveJointRelDegreeWait(Joints nJoint, double dbDegree);
        int MoveJointRelDegreeSpeed(Joints nJoint, double dbDegree, double dbAcc, double dbSpeed);
        int MoveJointRelDegreeSpeedWait(Joints nJoint, double dbDegree, double dbAcc, double dbSpeed);
        */

    }



}
