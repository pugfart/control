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

        public int Arc(int nRobotIndex, double dbMidX, double dbMidY, double dbMidZ, double dbMidRx, double dbMidRy, double dbMidRz,
                                        double dbEndX, double dbEndY, double dbEndZ, double dbEndRx, double dbEndRy, double dbEndRz)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.Arc(SpeedTypes.NoUse, WaitTypes.Wait, dbMidX, dbMidY, dbMidZ, dbMidRx, dbMidRy, dbMidRz,
                                                                        dbEndX, dbEndY, dbEndZ, dbEndRx, dbEndRy, dbEndRz, 0, 0, 0);
            return nRet;
        }

        public int Arc(int nRobotIndex, double dbMidX, double dbMidY, double dbMidZ, double dbMidRx, double dbMidRy, double dbMidRz,
                                        double dbEndX, double dbEndY, double dbEndZ, double dbEndRx, double dbEndRy, double dbEndRz,
                                        double dbAcc, double dbDec, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.Arc(SpeedTypes.Use, WaitTypes.Wait, dbMidX, dbMidY, dbMidZ, dbMidRx, dbMidRy, dbMidRz,
                                                                        dbEndX, dbEndY, dbEndZ, dbEndRx, dbEndRy, dbEndRz, dbAcc, dbDec, dbSpeed);
            return nRet;
        }

    }
}
