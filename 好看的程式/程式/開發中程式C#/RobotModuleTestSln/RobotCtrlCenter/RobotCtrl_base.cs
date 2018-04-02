using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

using IRobotMotion;
using FileOperation;

//private static object _thisLock = new object();

namespace RobotCtrlCenter
{

   // public enum Joints { J1, J2, J3, J4, J5, J6 };
  //  public enum MovementsTypes { Abs, Rel };
  //  public enum SpeedTypes { Use, NoUse };
  //  public enum UnitTypes { Degree, Radian };
  //  public enum WaitTypes { Wait, NoWait };


    public partial class RobotCtrl
    {
        //public List<Type> pluginTypes = null;
        public List<Type> m_listRobotPluginTypes = null;
        public List<IRobotMotion.IRobotMotion> m_listRobots = null;
       
        private int m_nRobots = 0;

        public virtual int RobotsNumber { get { return m_nRobots; } }

        /// <summary>
        /// 初始化此模組
        /// </summary>
        /// <param name="strSettingFile">設定擋路徑</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public void Init(string strSettingFile)
        {
            FileOperation.IniFile iniFile;
            iniFile = new IniFile(strSettingFile);

            //手臂總數
            m_nRobots = iniFile.ReadInteger("Robots", "total_num", 0);

            //    Type pluginType = typeof(IRobotMotion.IRobotMotion);

            m_listRobots = new List<IRobotMotion.IRobotMotion>();

            for (int n=0; n<m_nRobots; n++)
            {
                string strSection = "Robot" + Convert.ToString(n);
                string strType = iniFile.ReadString(strSection, "type", "");
                Type pluginType = typeof(IRobotMotion.IRobotMotion);

                switch (strType)
                {
                    case "Toshiba6":
                        string strToshiba6DllName = System.IO.Directory.GetCurrentDirectory() + "\\Toshiba6Robot.dll";
                        int nToshiba6Id = iniFile.ReadInteger(strSection, "id", 0);
                        string strToshiba6IP = iniFile.ReadString(strSection, "robot_ip", "");
                        int nToshiba6Port0 = iniFile.ReadInteger(strSection, "robot_port0", 0);
                        int nToshiba6Port1 = iniFile.ReadInteger(strSection, "robot_port1", 0);
                        string strToshiba6Program = iniFile.ReadString(strSection, "robot_program", "");
                        int nToshiba6OVRD = iniFile.ReadInteger(strSection, "robot_init_ovrd", 10);
                        AssemblyName anToshiba6 = AssemblyName.GetAssemblyName(strToshiba6DllName);
                        Assembly assemblyToshiba6 = Assembly.Load(anToshiba6);
                        //Type type = assemblyToshiba6.GetType();
                        // IRobotMotion.IRobotMotion IRobot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);

                        //Type tt = type.GetInterface(pluginType.FullName);
                       // IRobotMotion.IRobotMotion IRobot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);

                        Type[] typesToshiba6 = assemblyToshiba6.GetTypes();
                        foreach (Type type in typesToshiba6)
                        {
                            //如果為純interface 或 抽象型別, 則跳過此dll
                            if (type.IsInterface || type.IsAbstract)
                                continue;
                            else if (type.GetInterface(pluginType.FullName) != null)
                            {
                                IRobotMotion.IRobotMotion IToshiba6Robot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);
                                ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_nId = nToshiba6Id;
                                ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_strRobotIP = strToshiba6IP;
                                ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_nRobotPort0 = nToshiba6Port0;
                                ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_nRobotPort1 = nToshiba6Port1;
                                ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_strRobotProgram = strToshiba6Program;
                                ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_nRobotInitOVRD = nToshiba6OVRD;
                                m_listRobots.Add(IToshiba6Robot);
                            }
                        }
                        
                      //  Type type = assemblyToshiba6.GetType();
                      //  IRobotMotion.IRobotMotion IToshiba6Robot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);
                      //  ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_nId = nToshiba6Id;
                      //  ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_strRobotIP = strToshiba6IP;
                      ////  ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_nRobotPort = nToshiba6Port;
                      //  m_listRobots.Add(IToshiba6Robot);
                        break;
                    case "Toshiba4":
                        string strToshiba4DllName = System.IO.Directory.GetCurrentDirectory() + "\\Toshiba4Robot.dll";
                        int nToshiba4Id = iniFile.ReadInteger(strSection, "id", 0);
                        string strToshiba4IP = iniFile.ReadString(strSection, "robot_ip", "");
                        int nToshiba4Port0 = iniFile.ReadInteger(strSection, "robot_port0", 0);
                        int nToshiba4Port1 = iniFile.ReadInteger(strSection, "robot_port1", 0);
                        string strToshiba4Program = iniFile.ReadString(strSection, "robot_program", "");
                        int nToshiba4OVRD = iniFile.ReadInteger(strSection, "robot_init_ovrd", 10);
                        AssemblyName anToshiba4 = AssemblyName.GetAssemblyName(strToshiba4DllName);
                        Assembly assemblyToshiba4 = Assembly.Load(anToshiba4);
                        
                        Type[] typesToshiba4 = assemblyToshiba4.GetTypes();
                        foreach (Type type in typesToshiba4)
                        {
                            //如果為純interface 或 抽象型別, 則跳過此dll
                            if (type.IsInterface || type.IsAbstract)
                                continue;
                            else if (type.GetInterface(pluginType.FullName) != null)
                            {
                                IRobotMotion.IRobotMotion IToshiba4Robot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);
                                ((Toshiba4Robot.Toshiba4)IToshiba4Robot).m_nId = nToshiba4Id;
                                ((Toshiba4Robot.Toshiba4)IToshiba4Robot).m_strRobotIP = strToshiba4IP;
                                ((Toshiba4Robot.Toshiba4)IToshiba4Robot).m_nRobotPort0 = nToshiba4Port0;
                                ((Toshiba4Robot.Toshiba4)IToshiba4Robot).m_nRobotPort1 = nToshiba4Port1;
                                ((Toshiba4Robot.Toshiba4)IToshiba4Robot).m_strRobotProgram = strToshiba4Program;
                                ((Toshiba4Robot.Toshiba4)IToshiba4Robot).m_nRobotInitOVRD = nToshiba4OVRD;
                                m_listRobots.Add(IToshiba4Robot);
                            }
                        }

                     
                        break;
                    case "Epson6":
                        string strEpson6DllName = System.IO.Directory.GetCurrentDirectory() + "\\Epson6Robot.dll";
                        int nEpson6Id = iniFile.ReadInteger(strSection, "id", 0);
                        string strEpson6GetInfoIP = iniFile.ReadString(strSection, "get_info_ip", "");
                        int nEpson6GetInfoPort = iniFile.ReadInteger(strSection, "get_info_port", 0);
                        string strEpson6SendCmdIP = iniFile.ReadString(strSection, "send_cmd_ip", "");
                        int nEpson6SendCmdPort = iniFile.ReadInteger(strSection, "send_cmd_port", 0);

                        AssemblyName anEpson6 = AssemblyName.GetAssemblyName(strEpson6DllName);
                        Assembly assemblyEpson6 = Assembly.Load(anEpson6);
                        //Type type = assemblyToshiba6.GetType();
                        // IRobotMotion.IRobotMotion IRobot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);

                        //Type tt = type.GetInterface(pluginType.FullName);
                        // IRobotMotion.IRobotMotion IRobot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);

                        Type[] typesEpson6 = assemblyEpson6.GetTypes();
                        foreach (Type type in typesEpson6)
                        {
                            //如果為純interface 或 抽象型別, 則跳過此dll
                            if (type.IsInterface || type.IsAbstract)
                                continue;
                            else if (type.GetInterface(pluginType.FullName) != null)
                            {
                                IRobotMotion.IRobotMotion IEpson6Robot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);
                                ((Epson6Robot.Epson6)IEpson6Robot).m_nId = nEpson6Id;
                                ((Epson6Robot.Epson6)IEpson6Robot).m_strGetInfoIP = strEpson6GetInfoIP;
                                ((Epson6Robot.Epson6)IEpson6Robot).m_nGetInfoPort = nEpson6GetInfoPort;
                                ((Epson6Robot.Epson6)IEpson6Robot).m_strSendCmdIP = strEpson6SendCmdIP;
                                ((Epson6Robot.Epson6)IEpson6Robot).m_nSendCmdPort = nEpson6SendCmdPort;
                                m_listRobots.Add(IEpson6Robot);
                            }
                        }

                        //  Type type = assemblyToshiba6.GetType();
                        //  IRobotMotion.IRobotMotion IToshiba6Robot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);
                        //  ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_nId = nToshiba6Id;
                        //  ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_strRobotIP = strToshiba6IP;
                        ////  ((Toshiba6Robot.Toshiba6)IToshiba6Robot).m_nRobotPort = nToshiba6Port;
                        //  m_listRobots.Add(IToshiba6Robot);
                        
                        break;
                    case "UR":
                        string strURDllName = System.IO.Directory.GetCurrentDirectory() + "\\URRobot.dll";
                        int nURId = iniFile.ReadInteger(strSection, "id", 0);
                        string strURIP = iniFile.ReadString(strSection, "robot_ip", "");
                        int nURRealTimePort = iniFile.ReadInteger(strSection, "robot_real_time_port", 0);
                        int nPCListenPort = iniFile.ReadInteger(strSection, "pc_listen_port", 0);

                        AssemblyName anUR = AssemblyName.GetAssemblyName(strURDllName);
                        Assembly assemblyUR = Assembly.Load(anUR);


                        Type[] typesUR = assemblyUR.GetTypes();
                        foreach (Type type in typesUR)
                        {
                            //如果為純interface 或 抽象型別, 則跳過此dll
                            if (type.IsInterface || type.IsAbstract)
                                continue;
                            else if (type.GetInterface(pluginType.FullName) != null)
                            {
                                IRobotMotion.IRobotMotion IURRobot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);
                                ((URRobot.URRobot)IURRobot).m_nId = nURId;
                                ((URRobot.URRobot)IURRobot).m_strRobotIP = strURIP;
                                ((URRobot.URRobot)IURRobot).m_nRobotRealTimePort = nURRealTimePort;
                                ((URRobot.URRobot)IURRobot).m_nPCListenPort = nPCListenPort;
                                m_listRobots.Add(IURRobot);
                            }
                        }


                        //IRobotMotion.IRobotMotion IURRobot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(assemblyUR.GetType());
                       // ((URRobot.URRobot)IURRobot).m_nId = nURId;
                        //((URRobot.URRobot)IURRobot).m_strRobotIP = strURIP;
                        //((URRobot.URRobot)IURRobot).m_nRobotRealTimePort = nURRealTimePort;
                       // ((URRobot.URRobot)IURRobot).m_nPCListenPort = nPCListenPort;
                       // m_listRobots.Add(IURRobot);
                        break;
                }
                
            }

            

            /*
            //1.取得dll檔案名稱
            string[] dllFileNames = null;
           // dllFileNames = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            dllFileNames = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), "*.dll");
            string str = this.GetType().Assembly.Location;
            //2.取得Assembly
            List<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }

            //3.取得插件型別
            //3.取得所有Dll內的指定型別 (IRobotMotion)
            Type pluginType = typeof(IRobotMotion.IRobotMotion);
            m_listRobotPluginTypes = new List<Type>();

            m_listRobots = new List<IRobotMotion.IRobotMotion>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly == null)
                    continue;
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                  //  Type tt = type.GetInterface(pluginType.FullName); 

                    //如果為純interface 或 抽象型別, 則跳過此dll
                    if (type.IsInterface || type.IsAbstract)
                        continue;
                    else if (type.GetInterface(pluginType.FullName) != null)
                    {
                        m_listRobotPluginTypes.Add(type);

                        IRobotMotion.IRobotMotion IRobot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(type);
                        m_listRobots.Add(IRobot);
                        
                    }
                }
            } 
            */
        }

        /// <summary>
        /// 對所有手臂連線
        /// </summary>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int ConnectAllRobots()
        {
            foreach (IRobotMotion.IRobotMotion Robot in m_listRobots)
            {
                //Robot.Connect("192.168.0.2", 40000);
                int nRet = Robot.Connect();
               // if (nRet != 0)
                   
            }
            return 1 ;
        }

        /// <summary>
        /// 對所有手臂斷線
        /// </summary>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int DisconnectAllRobots()
        {
            foreach (IRobotMotion.IRobotMotion Robot in m_listRobots)
            {
                Robot.Disconnect();
            }
            return 1;
        }


        /// <summary>
        /// 對指定手臂連線
        /// </summary>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        //public int ConnectRobot(int nRobotIndex, string strIP, int nPort)
        public int ConnectRobot(int nRobotIndex)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.Connect();
            return nRet;
        }

        /// <summary>
        /// 對指定手臂斷線
        /// </summary>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int DisconnectRobot(int nRobotIndex)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.Disconnect();     
            return nRet;
        }


        /// <summary>
        /// 擷取6關節目前角度值
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1">第1個關節角度回傳值</param>
        /// <param name="dbJ2">第2個關節角度回傳值</param>
        /// <param name="dbJ3">第3個關節角度回傳值</param>
        /// <param name="dbJ4">第4個關節角度回傳值</param>
        /// <param name="dbJ5">第5個關節角度回傳值</param>
        /// <param name="dbJ6">第6個關節角度回傳值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int GetJointDegree(int nRobotIndex, ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            //Type robot = m_listRobotPluginTypes[nRobotIndex];
            //IRobotMotion.IRobotMotion IRobot = (IRobotMotion.IRobotMotion)Activator.CreateInstance(robot);

            IRobot.GetJointDegree(ref dbJ1, ref dbJ2, ref dbJ3, ref dbJ4, ref dbJ5, ref dbJ6);

            //  foreach (Type type in pluginTypes)
            //  {
            //      IPlugin.IPlugin plugin = (IPlugin.IPlugin)Activator.CreateInstance(type);
            //      MessageBox.Show(plugin.Name);
            //  }

            return 1;
        }

        /// <summary>
        /// 擷取6關節目前徑度值
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1">第1個關節徑度回傳值</param>
        /// <param name="dbJ2">第2個關節徑度回傳值</param>
        /// <param name="dbJ3">第3個關節徑度回傳值</param>
        /// <param name="dbJ4">第4個關節徑度回傳值</param>
        /// <param name="dbJ5">第5個關節徑度回傳值</param>
        /// <param name="dbJ6">第6個關節徑度回傳值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int GetJointRadian(int nRobotIndex, ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.GetJointRadian(ref dbJ1, ref dbJ2, ref dbJ3, ref dbJ4, ref dbJ5, ref dbJ6);
            return 1;
        }

        /// <summary>
        /// 擷取 笛卡爾座標系位置值
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbX">x方向回傳值(mm)</param>
        /// <param name="dbY">y方向回傳值(mm)</param>
        /// <param name="dbZ">z方向回傳值(mm)</param>
        /// <param name="dbRx">Rx方向回傳值(mm)</param>
        /// <param name="dbRy">Ry方向回傳值(mm)</param>
        /// <param name="dbRz">Rz方向回傳值(mm)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int GetWorldPos(int nRobotIndex, ref double dbX, ref double dbY, ref double dbZ, ref double dbRx, ref double dbRy, ref double dbRz)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.GetWorldPos(ref dbX, ref dbY, ref dbZ, ref dbRx, ref dbRy, ref dbRz);
            return 1;
        }

        /// <summary>
        /// 擷取6關節目前電流值
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1">第1個關節電流回傳值</param>
        /// <param name="dbJ2">第2個關節電流回傳值</param>
        /// <param name="dbJ3">第3個關節電流回傳值</param>
        /// <param name="dbJ4">第4個關節電流回傳值</param>
        /// <param name="dbJ5">第5個關節電流回傳值</param>
        /// <param name="dbJ6">第6個關節電流回傳值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int GetJointCurrentValue(int nRobotIndex, ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.GetJointCurrentValue(ref dbJ1, ref dbJ2, ref dbJ3, ref dbJ4, ref dbJ5, ref dbJ6);
            return 1;
        }

        /// <summary>
        /// 擷取6關節目前電壓值
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1">第1個關節電壓回傳值</param>
        /// <param name="dbJ2">第2個關節電壓回傳值</param>
        /// <param name="dbJ3">第3個關節電壓回傳值</param>
        /// <param name="dbJ4">第4個關節電壓回傳值</param>
        /// <param name="dbJ5">第5個關節電壓回傳值</param>
        /// <param name="dbJ6">第6個關節電壓回傳值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int GetJointVoltageValue(int nRobotIndex, ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.GetJointVoltageValue(ref dbJ1, ref dbJ2, ref dbJ3, ref dbJ4, ref dbJ5, ref dbJ6);
            return 1;
        }

        /// <summary>
        /// 擷取6關節目前扭矩值
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1">第1個關節扭矩回傳值</param>
        /// <param name="dbJ2">第2個關節扭矩回傳值</param>
        /// <param name="dbJ3">第3個關節扭矩回傳值</param>
        /// <param name="dbJ4">第4個關節扭矩回傳值</param>
        /// <param name="dbJ5">第5個關節扭矩回傳值</param>
        /// <param name="dbJ6">第6個關節扭矩回傳值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int GetJointTorqueValue(int nRobotIndex, ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.GetJointTorqueValue(ref dbJ1, ref dbJ2, ref dbJ3, ref dbJ4, ref dbJ5, ref dbJ6);
            return 1;
        }

        /// <summary>
        /// 擷取6關節目前溫度值
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1">第1個關節溫度回傳值</param>
        /// <param name="dbJ2">第2個關節溫度回傳值</param>
        /// <param name="dbJ3">第3個關節溫度回傳值</param>
        /// <param name="dbJ4">第4個關節溫度回傳值</param>
        /// <param name="dbJ5">第5個關節溫度回傳值</param>
        /// <param name="dbJ6">第6個關節溫度回傳值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int GetJointTemperatureValue(int nRobotIndex, ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.GetJointTemperatureValue(ref dbJ1, ref dbJ2, ref dbJ3, ref dbJ4, ref dbJ5, ref dbJ6);
            return 1;
        }

        /// <summary>
        /// 擷取6關節目前速度值
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1">第1個關節速度回傳值</param>
        /// <param name="dbJ2">第2個關節速度回傳值</param>
        /// <param name="dbJ3">第3個關節速度回傳值</param>
        /// <param name="dbJ4">第4個關節速度回傳值</param>
        /// <param name="dbJ5">第5個關節速度回傳值</param>
        /// <param name="dbJ6">第6個關節速度回傳值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int GetJointSpeedValue(int nRobotIndex, ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.GetJointSpeedValue(ref dbJ1, ref dbJ2, ref dbJ3, ref dbJ4, ref dbJ5, ref dbJ6);
            return 1;
        }

        /// <summary>
        /// 擷取6關節目前加速度值
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbJ1">第1個關節加速度回傳值</param>
        /// <param name="dbJ2">第2個關節加速度回傳值</param>
        /// <param name="dbJ3">第3個關節加速度回傳值</param>
        /// <param name="dbJ4">第4個關節加速度回傳值</param>
        /// <param name="dbJ5">第5個關節加速度回傳值</param>
        /// <param name="dbJ6">第6個關節加速度回傳值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int GetJointAccValue(int nRobotIndex, ref double dbJ1, ref double dbJ2, ref double dbJ3, ref double dbJ4, ref double dbJ5, ref double dbJ6)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.GetJointAccValue(ref dbJ1, ref dbJ2, ref dbJ3, ref dbJ4, ref dbJ5, ref dbJ6);
            return 1;
        }

        /// <summary>
        /// 減速停止所有關節運動(ptp時使用)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbDec">減速度值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int StopMotionForPtoP(int nRobotIndex, double dbDec)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.StopMotionForPtoP(dbDec);
            return 1;
        }

        /// <summary>
        /// 減速停止所有關節運動(補間運動時使用)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="dbDec">減速度值</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int StopMotionForLine(int nRobotIndex, double dbDec)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            IRobot.StopMotionForLine(dbDec);
            return 1;
        }


        /*
        
        /// <summary>
        /// 單一關節移動(絕對位置)(角度)(不等待繼續)(無輸入速度)
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
        /// 單一關節移動(絕對位置)(角度)(等待繼續)(無輸入速度)
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
        /// 單一關節移動(絕對位置)(角度)(不等待繼續)(有輸入速度)
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
        /// 單一關節移動(絕對位置)(角度)(等待繼續)(有輸入速度)
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



        /// <summary>
        /// 單一關節移動(絕對位置)(徑度)(不等待繼續)(無輸入速度)
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
        /// 單一關節移動(絕對位置)(徑度)(等待繼續)(無輸入速度)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">絕對移動到徑度值(絕對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointAbsRadianWait(int nRobotIndex, Joints nJoint, double dbRadian)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Radian, SpeedTypes.NoUse, WaitTypes.Wait, dbDegree, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(絕對位置)(徑度)(不等待繼續)(有輸入速度)
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
        /// 單一關節移動(絕對位置)(徑度)(等待繼續)(有輸入速度)
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







        /// <summary>
        /// 單一關節移動(相對位置)(角度)(不等待繼續)(無輸入速度)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelDegree(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Rel, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.NoWait, dbDegree, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(角度)(等待繼續)(無輸入速度)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelDegreeWait(int nRobotIndex, Joints nJoint, double dbDegree)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Rel, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.Wait, dbDegree, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(角度)(不等待繼續)(有輸入速度)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時均速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelDegreeSpeed(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Rel, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.NoWait, dbDegree, dbAcc, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(角度)(等待繼續)(有輸入速度)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時均速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveJointRelDegreeSpeedWait(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Rel, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.Wait, dbDegree, dbAcc, dbSpeed);
            return 1;
        }


        /// <summary>
        /// 單一關節移動(相對位置)(徑度)(不等待繼續)(無輸入速度)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動徑度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelRadian(int nRobotIndex, Joints nJoint, double dbRadian)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Rel, UnitTypes.Radian, SpeedTypes.NoUse, WaitTypes.NoWait, dbRadian, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(徑度)(等待繼續)(無輸入速度)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動角度值(相對位置)</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelRadianWait(int nRobotIndex, Joints nJoint, double dbRadian)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Rel, UnitTypes.Radian, SpeedTypes.NoUse, WaitTypes.Wait, dbRadian, 0, 0);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(徑度)(不等待繼續)(有輸入速度)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動徑度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時均速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public int MoveJointRelRadianSpeed(int nRobotIndex, Joints nJoint, double dbRadian, double dbAcc, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Rel, UnitTypes.Radian, SpeedTypes.Use, WaitTypes.NoWait, dbRadian, dbAcc, dbSpeed);
            return 1;
        }

        /// <summary>
        /// 單一關節移動(相對位置)(徑度)(等待繼續)(有輸入速度)
        /// </summary>
        /// <param name="nRobotIndex">手臂代號</param>
        /// <param name="nJoint">關節代號</param>
        /// <param name="dbDegree">相對移動徑度值(相對位置)</param>
        /// <param name="dbAcc">移動時加速度</param>
        /// <param name="dbSpeed">移動時均速度</param>
        /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
        public virtual int MoveJointRelRadianSpeedWait(int nRobotIndex, Joints nJoint, double dbRadian, double dbAcc, double dbSpeed)
        {
            IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
            int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Rel, UnitTypes.Radian, SpeedTypes.Use, WaitTypes.Wait, dbRadian, dbAcc, dbSpeed);
            return 1;
        }

        */





        /*
    /// <summary>
    /// 全部關節移動(絕對位置)(不等待繼續)(無輸入速度)
    /// </summary>
    /// <param name="nRobotIndex">手臂代號</param>
    /// <param name="dbJ1Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ2Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ3Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ4Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ5Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ6Degree">絕對移動到角度值(絕對位置)</param>
    /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
    public virtual int MoveAllJointAbsDegree(int nRobotIndex, double dbJ1Value, double dbJ2Value, double dbJ3Value, double dbJ4Value, double dbJ5Value, double dbJ6Value)
    {
        IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
        int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.NoWait, 
                dbJ1Value, dbJ2Value, dbJ3Value, dbJ4Value, dbJ5Value, dbJ6Value, 0, 0);
        return 1;
    }

    /// <summary>
    /// 全部關節移動(絕對位置)(等待繼續)(無輸入速度)
    /// </summary>
    /// <param name="nRobotIndex">手臂代號</param>
    /// <param name="dbJ1Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ2Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ3Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ4Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ5Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ6Degree">絕對移動到角度值(絕對位置)</param>
    /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
    public virtual int MoveAllJointAbsDegreeWait(int nRobotIndex, double dbJ1Degree, double dbJ2Degree, double dbJ3Degree, double dbJ4Degree, double dbJ5Degree, double dbJ6Degree)
    {
        IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
        int nRet = IRobot.MoveAllJoint(MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.NoUse, WaitTypes.Wait, 
            dbJ1Degree, dbJ2Degree, dbJ3Degree, dbJ4Degree, dbJ5Degree, dbJ6Degree, 0, 0);
        return 1;
    }


    /// <summary>
    /// 全部關節移動(絕對位置)(不等待繼續)(有輸入速度)
    /// </summary>
    /// <param name="nRobotIndex">手臂代號</param>
    /// <param name="dbJ1Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ2Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ3Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ4Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ5Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbJ6Degree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbAcc">移動時加速度</param>
    /// <param name="dbSpeed">移動時最大速度</param>
    /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
    public virtual int MoveAllJointAbsDegreeSpeed(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbSpeed)
    {
        IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
        int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.NoWait, dbDegree, dbAcc, dbSpeed);
        return 1;
    }

    /// <summary>
    /// 單一關節移動(絕對位置)(等待繼續)(有輸入速度)
    /// </summary>
    /// <param name="nRobotIndex">手臂代號</param>
    /// <param name="nJoint">關節代號</param>
    /// <param name="dbDegree">絕對移動到角度值(絕對位置)</param>
    /// <param name="dbAcc">移動時加速度</param>
    /// <param name="dbSpeed">移動時最大速度</param>
    /// <returns>回傳是否成功,1:成功,-1:失敗</returns>
    public virtual int MoveJointAbsDegreeSpeedWait(int nRobotIndex, Joints nJoint, double dbDegree, double dbAcc, double dbSpeed)
    {
        IRobotMotion.IRobotMotion IRobot = m_listRobots[nRobotIndex];
        int nRet = IRobot.MoveSingleJoint(nJoint, MovementsTypes.Abs, UnitTypes.Degree, SpeedTypes.Use, WaitTypes.Wait, dbDegree, dbAcc, dbSpeed);
        return 1;
    }
    */


    }
}
