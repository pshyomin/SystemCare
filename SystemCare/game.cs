using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace SystemCare
{
    public partial class game : UserControl
    {
        bool Opti = false;

        int GetValueNum = 0;
        string GetSubKey = "";
        bool isCheck = false;
        string isValue = "0";

        public game()
        {
            InitializeComponent();

            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);

            RegistryKey registrySystemCareKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            foreach(string reg in registrySystemCareKey.CreateSubKey("SOFTWARE").GetSubKeyNames())
            {
                if (reg == "SystemCare")
                {
                    isCheck = true;
                    isValue = registrySystemCareKey.CreateSubKey("SOFTWARE").CreateSubKey("SystemCare").GetValue("optimization").ToString();
                    break;
                }
            }

            if(isCheck == false)
            {
                registrySystemCareKey.CreateSubKey("SOFTWARE").CreateSubKey("SystemCare").SetValue("optimization", 0);
            }

            if(isValue == "1")
            {
                Opti = true;
                pictureBox1.Image = Properties.Resources.game;
            }
        }

        private void Game_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(255, 228, 228, 228);
            Pen blackpen = new Pen(c, 1);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;
            
            g.DrawLine(blackpen, 0, 0, 737, 0);
            g.DrawLine(blackpen2, 0, 534, 737, 534);
        }

        private void Label1_MouseEnter(object sender, EventArgs e)
        {
            label1.BackColor = Color.FromArgb(75, 75, 75);
        }

        private void Label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = Color.FromArgb(55, 55, 55);
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Opti == false)
                {
                    if (MessageBox.Show("게임최적화를 하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Opti = true;
                        pictureBox1.Image = Properties.Resources.game;

                        RegistryKey registryPingKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        RegistryKey registryKeyboardKey = Registry.CurrentUser;

                        //키보드 세팅
                        RegistryKey Keyboard_1 = registryKeyboardKey.CreateSubKey("Control Panel").CreateSubKey("Accessibility").CreateSubKey("Keyboard Response");
                        RegistryKey Keyboard_2 = registryKeyboardKey.CreateSubKey("Control Panel").CreateSubKey("Keyboard");

                        Keyboard_1.SetValue("BounceTime", 0);
                        Keyboard_1.SetValue("Last BounceKey Setting", 0x00000000);
                        Keyboard_1.SetValue("Last Valid Wait", 0x00000000);
                        Keyboard_1.SetValue("Last Valid Delay", 0x00000000);
                        Keyboard_1.SetValue("Last Valid Repeat", 0x00000050);
                        Keyboard_1.SetValue("DelayBeforeAcceptance", 0, RegistryValueKind.String);
                        Keyboard_1.SetValue("AutoRepeatRate", 100, RegistryValueKind.String);
                        Keyboard_1.SetValue("AutoRepeatDelay", 0, RegistryValueKind.String);

                        Keyboard_2.SetValue("InitialKeyboardIndicators", 2, RegistryValueKind.String);
                        Keyboard_2.SetValue("KeyboardSpeed", 60, RegistryValueKind.String);
                        Keyboard_2.SetValue("KeyboardDelay", 0, RegistryValueKind.String);

                        //네트워크 세팅
                        RegistryKey Net_1 = registryPingKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows NT").CreateSubKey("CurrentVersion").CreateSubKey("Multimedia").CreateSubKey("SystemProfile");
                        RegistryKey Net_2 = registryPingKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows NT").CreateSubKey("CurrentVersion").CreateSubKey("Multimedia").CreateSubKey("SystemProfile").CreateSubKey("Tasks").CreateSubKey("Games");

                        Net_1.SetValue("NetworkThrottlingIndex", 0xffffffff);
                        Net_1.SetValue("SystemResponsiveness", 0x00000000);

                        Net_2.SetValue("GPU Priority", 8);
                        Net_2.SetValue("Priority", 6);
                        Net_2.SetValue("Scheduling Category", "High");


                        string[] a = registryPingKey.CreateSubKey("SYSTEM").CreateSubKey("CurrentControlSet").CreateSubKey("services").CreateSubKey("Tcpip").CreateSubKey("Parameters").CreateSubKey("Interfaces").GetSubKeyNames();

                        for (int i = 0; i < a.Count(); i++)
                        {
                            if (registryPingKey.CreateSubKey("SYSTEM").CreateSubKey("CurrentControlSet").CreateSubKey("services").CreateSubKey("Tcpip").CreateSubKey("Parameters").CreateSubKey("Interfaces").CreateSubKey(a[i]).GetValueNames().Count() >= GetValueNum)
                            {
                                GetSubKey = a[i];
                                GetValueNum = registryPingKey.CreateSubKey("SYSTEM").CreateSubKey("CurrentControlSet").CreateSubKey("services").CreateSubKey("Tcpip").CreateSubKey("Parameters").CreateSubKey("Interfaces").CreateSubKey(a[i]).GetValueNames().Count();
                            }
                        }
                        RegistryKey FastPing_1 = registryPingKey.CreateSubKey("SYSTEM").CreateSubKey("CurrentControlSet").CreateSubKey("services").CreateSubKey("Tcpip").CreateSubKey("Parameters").CreateSubKey("Interfaces").CreateSubKey(GetSubKey);
                        RegistryKey FastPing_2 = registryPingKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("MSMQ").CreateSubKey("Parameter");

                        FastPing_1.SetValue("TcpAckFrequency", 0x00000001);
                        FastPing_1.SetValue("TCPNoDelay", 0x00000001);

                        FastPing_2.SetValue("TCPNoDelay", 0x00000001);

                        ////////////////////////////////////////////////////////////////////////////

                        cmdCommend(@"DISM /Online /Enable-Feature /FeatureName:MSMQ-Container");
                        cmdCommend(@"DISM /Online /Enable-Feature /FeatureName:MSMQ-Server");
                        cmdCommend(@"DISM /Online /Enable-Feature /FeatureName:MSMQ-Triggers");
                        cmdCommend(@"DISM /Online /Enable-Feature /FeatureName:MSMQ-ADIntegration");
                        cmdCommend(@"DISM /Online /Enable-Feature /FeatureName:MSMQ-HTTP");
                        cmdCommend(@"DISM /Online /Enable-Feature /FeatureName:MSMQ-Multicast");
                        cmdCommend(@"DISM /Online /Enable-Feature /FeatureName:MSMQ-DCOMProxy");
                        cmdCommend(@"bcdedit /deletevalue useplatformclock");
                        cmdCommend(@"netsh interface tcp set global autotuninglevel=highlyrestricted");

                        foreach (Process process in Process.GetProcesses())
                        {
                            foreach (int id in pProcessList)
                            {
                                if (process.Id == id)
                                {
                                    cmdCommendKill("taskkill /pid " + id + " /f");
                                }
                            }
                        }
                        pProcessList.Clear();

                        MessageBox.Show("성공적으로 게임최적화를 완료하였습니다.\n\n재부팅 후 적용됩니다.", "SYSTEM CARE");
                        RegistryKey registrySystemCareKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        registrySystemCareKey.CreateSubKey("SOFTWARE").CreateSubKey("SystemCare").SetValue("optimization", 1);
                    }
                }
                else
                {
                    if (MessageBox.Show("게임최적화를 이전 값으로 복구 하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Opti = false;
                        pictureBox1.Image = Properties.Resources.game_off;

                        RegistryKey registryPingKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        RegistryKey registryKeyboardKey = Registry.CurrentUser;

                        //키보드 세팅
                        RegistryKey Keyboard_1 = registryKeyboardKey.CreateSubKey("Control Panel").CreateSubKey("Accessibility").CreateSubKey("Keyboard Response");
                        RegistryKey Keyboard_2 = registryKeyboardKey.CreateSubKey("Control Panel").CreateSubKey("Keyboard");

                        Keyboard_1.SetValue("BounceTime", 0);
                        Keyboard_1.SetValue("Last BounceKey Setting", 0x00000000);
                        Keyboard_1.SetValue("Last Valid Wait", 0x000003e8);
                        Keyboard_1.SetValue("Last Valid Delay", 0x00000000);
                        Keyboard_1.SetValue("Last Valid Repeat", 0x00000000);
                        Keyboard_1.SetValue("DelayBeforeAcceptance", 1000, RegistryValueKind.String);
                        Keyboard_1.SetValue("AutoRepeatRate", 500, RegistryValueKind.String);
                        Keyboard_1.SetValue("AutoRepeatDelay", 1000, RegistryValueKind.String);

                        Keyboard_2.SetValue("InitialKeyboardIndicators", 2, RegistryValueKind.String);
                        Keyboard_2.SetValue("KeyboardSpeed", 31, RegistryValueKind.String);
                        Keyboard_2.SetValue("KeyboardDelay", 1, RegistryValueKind.String);

                        //네트워크 세팅
                        RegistryKey Net_1 = registryPingKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows NT").CreateSubKey("CurrentVersion").CreateSubKey("Multimedia").CreateSubKey("SystemProfile");
                        RegistryKey Net_2 = registryPingKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows NT").CreateSubKey("CurrentVersion").CreateSubKey("Multimedia").CreateSubKey("SystemProfile").CreateSubKey("Tasks").CreateSubKey("Games");

                        Net_1.SetValue("NetworkThrottlingIndex", 0xffffffff);
                        Net_1.SetValue("SystemResponsiveness", 0x00000014);

                        Net_2.SetValue("GPU Priority", 8);
                        Net_2.SetValue("Priority", 2);
                        Net_2.SetValue("Scheduling Category", "Medium");

                        string[] a = registryPingKey.CreateSubKey("SYSTEM").CreateSubKey("CurrentControlSet").CreateSubKey("services").CreateSubKey("Tcpip").CreateSubKey("Parameters").CreateSubKey("Interfaces").GetSubKeyNames();

                        for (int i = 0; i < a.Count(); i++)
                        {
                            if (registryPingKey.CreateSubKey("SYSTEM").CreateSubKey("CurrentControlSet").CreateSubKey("services").CreateSubKey("Tcpip").CreateSubKey("Parameters").CreateSubKey("Interfaces").CreateSubKey(a[i]).GetValueNames().Count() >= GetValueNum)
                            {
                                GetSubKey = a[i];
                                GetValueNum = registryPingKey.CreateSubKey("SYSTEM").CreateSubKey("CurrentControlSet").CreateSubKey("services").CreateSubKey("Tcpip").CreateSubKey("Parameters").CreateSubKey("Interfaces").CreateSubKey(a[i]).GetValueNames().Count();
                            }
                        }
                        RegistryKey FastPing_1 = registryPingKey.CreateSubKey("SYSTEM").CreateSubKey("CurrentControlSet").CreateSubKey("services").CreateSubKey("Tcpip").CreateSubKey("Parameters").CreateSubKey("Interfaces").CreateSubKey(GetSubKey);
                        RegistryKey FastPing_2 = registryPingKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("MSMQ").CreateSubKey("Parameter");

                        if (FastPing_1.GetValue("TcpAckFrequency") != null) FastPing_1.DeleteValue("TcpAckFrequency");
                        if (FastPing_1.GetValue("TCPNoDelay") != null) FastPing_1.DeleteValue("TCPNoDelay");
                        if (FastPing_1.GetValue("TCPNoDelay") != null) FastPing_2.DeleteValue("TCPNoDelay");

                        ////////////////////////////////////////////////////////////////////////////

                        cmdCommend(@"DISM /Online /Disable-Feature /FeatureName:MSMQ-Container");
                        cmdCommend(@"DISM /Online /Disable-Feature /FeatureName:MSMQ-Server");
                        cmdCommend(@"DISM /Online /Disable-Feature /FeatureName:MSMQ-Triggers");
                        cmdCommend(@"DISM /Online /Disable-Feature /FeatureName:MSMQ-ADIntegration");
                        cmdCommend(@"DISM /Online /Disable-Feature /FeatureName:MSMQ-HTTP");
                        cmdCommend(@"DISM /Online /Disable-Feature /FeatureName:MSMQ-Multicast");
                        cmdCommend(@"DISM /Online /Disable-Feature /FeatureName:MSMQ-DCOMProxy");
                        cmdCommend(@"netsh interface tcp set global autotuninglevel=normal");

                        foreach (Process process in Process.GetProcesses())
                        {
                            foreach (int id in pProcessList)
                            {
                                if (process.Id == id)
                                {
                                    cmdCommendKill("taskkill /pid " + id + " /f");
                                }
                            }
                        }
                        pProcessList.Clear();

                        MessageBox.Show("성공적으로 게임최적화를 이전 상태로 복구하였습니다.\n\n재부팅 후 적용됩니다.", "SYSTEM CARE");
                        RegistryKey registrySystemCareKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        registrySystemCareKey.CreateSubKey("SOFTWARE").CreateSubKey("SystemCare").SetValue("optimization", 0);
                    }
                }
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        List<int> pProcessList = new List<int>();

        void cmdCommend(string cmdstring)
        {
            ProcessStartInfo cmd = new ProcessStartInfo();
            Process process = new Process();
            Process processkill = new Process();
            cmd.FileName = @"cmd";
            cmd.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.CreateNoWindow = true;

            cmd.UseShellExecute = false;
            cmd.RedirectStandardOutput = true;
            cmd.RedirectStandardInput = true;
            cmd.RedirectStandardError = true;

            process.EnableRaisingEvents = false;
            process.StartInfo = cmd;
            process.Start();
            process.StandardInput.Write(cmdstring + Environment.NewLine);
            process.StandardInput.Write(@"exit" + Environment.NewLine);
            process.StandardInput.Close();

            pProcessList.Add(process.Id);
            process.WaitForExit(1);
            process.Close();
        }

        void cmdCommendKill(string cmdstring)
        {
            ProcessStartInfo cmd = new ProcessStartInfo();
            Process process = new Process();
            Process processkill = new Process();
            cmd.FileName = @"cmd";
            cmd.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.CreateNoWindow = true;

            cmd.UseShellExecute = false;
            cmd.RedirectStandardOutput = true;
            cmd.RedirectStandardInput = true;
            cmd.RedirectStandardError = true;

            process.EnableRaisingEvents = false;
            process.StartInfo = cmd;
            process.Start();
            process.StandardInput.Write(cmdstring + Environment.NewLine);
            process.StandardInput.Close();
            process.WaitForExit(1);
            process.Close();
        }
    }
}
