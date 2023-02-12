using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Management;
using System.IO;

namespace SystemCare
{
    public partial class info : UserControl
    {
        public bool isOn = false;
        public info()
        {
            InitializeComponent();

            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public void On()
        {
            isOn = true;
            SystemInfo();
            System.Threading.Timer timer = new System.Threading.Timer(CallBack);
            timer.Change(0, 10000);
            System.Threading.Timer timer2 = new System.Threading.Timer(CallBack2);
            timer2.Change(0, 1000);

            foreach (DriveInfo d in drv)
            {
                comboBox1.Items.Add(d.Name);
            }
            comboBox1.SelectedIndex = 0;
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

        void SystemInfo()
        {
            label8.Text = Environment.UserName;
            label9.Text = GetSystemInfo("CPU");
            label10.Text = GetSystemInfo("MainBoard");
            label11.Text = GetSystemInfo("Ram");
            label12.Text = GetSystemInfo("Graphic");
            label13.Text = GetSystemInfo("Sound");
            label14.Text = GetSystemInfo("OS").Replace("Microsoft ", "") + GetOSbit();

            label15.Text = GetSystemInfo("CPUm");
            label17.Text = GetSystemInfo("MainBoardm");

            label19.Text = GetIPAddress();
            label21.Text = GetMacAddress();
        }

        public string GetIPAddress()
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    System.Array ar;
                    ar = (System.Array)(mo.Properties["IpAddress"].Value);
                    st = ar.GetValue(0).ToString();
                    break;
                }
            }
            return st;
        }

        private void RAMCounter()
        {
            var wmiObject = new ManagementObjectSearcher("select * from Win32_OperatingSystem");

            var memoryValues = wmiObject.Get().Cast<ManagementObject>().Select(mo => new
            {
                FreePhysicalMemory = Double.Parse(mo["FreePhysicalMemory"].ToString()),
                TotalVisibleMemorySize = Double.Parse(mo["TotalVisibleMemorySize"].ToString())
            }).FirstOrDefault();

            if (memoryValues != null)
            {
                var percent = ((memoryValues.TotalVisibleMemorySize - memoryValues.FreePhysicalMemory) / memoryValues.TotalVisibleMemorySize) * 100;
                progressBar1.Value = (int)percent;
            }
            label24.Text = ConvertSizeToString(((long)memoryValues.TotalVisibleMemorySize - (long)memoryValues.FreePhysicalMemory) * 1000) + " / " + ConvertSizeToString((long)memoryValues.TotalVisibleMemorySize * 1000);
        }

        public string GetMacAddress()
        {
            string mac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    mac = mo["MacAddress"].ToString();
                    break;
                }
            }
            return mac;
        }

        string GetOSbit()
        {
            String sProcess;
            if (Environment.Is64BitOperatingSystem == true)
            {
                sProcess = " 64 Bit";

            }
            else
            {
                sProcess = " 32 Bit";
            }
            return sProcess;
        }

        private void Info_Load(object sender, EventArgs e)
        {
        }

        delegate void TimerEventFiredDelegate();

        void CallBack(Object state)
        {
            BeginInvoke(new TimerEventFiredDelegate(Work));
        }

        private void Work()
        {
            RAMCounter();
        }

        void CallBack2(Object state)
        {
            BeginInvoke(new TimerEventFiredDelegate(Work2));
        }

        DriveInfo drive;
        DriveInfo[] drv = DriveInfo.GetDrives();
        private void Work2()
        {
            DriveInfo d = drive;
            if (d.DriveType == DriveType.Fixed)
            {
                progressBar3.Maximum = Convert.ToInt32(d.TotalSize / 1024 / 1024);
                progressBar3.Value = Convert.ToInt32(d.AvailableFreeSpace / 1024 / 1024);

                label27.Text = "남은 용량 " + ConvertSizeToString(d.TotalFreeSpace);
            }
        }

        public string GetSystemInfo(string infotoGet)
        {
            switch (infotoGet)
            {
                case "Ram":
                    ulong a = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
                    string b = ConvertSizeToString((long)a);
                    return b;
                case "CPU":
                    ManagementObjectSearcher MS1 = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                    foreach (ManagementObject MO in MS1.Get())
                    {
                        return MO["Name"].ToString();
                    }
                    return "unknown";
                case "Graphic":
                    ManagementObjectSearcher MS2 = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");
                    foreach (ManagementObject MO in MS2.Get())
                    {
                        return MO["Description"].ToString();
                    }
                    return "unknown";
                case "MainBoard":
                    ManagementObjectSearcher MS3 = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                    foreach (ManagementObject MO in MS3.Get())
                    {
                        return (MO["Product"].ToString());
                    }
                    return "unknown";
                case "OS":
                    ManagementObjectSearcher MS4 = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                    foreach (ManagementObject MO in MS4.Get())
                    {
                        return MO["Caption"].ToString();
                    }
                    return "unknown";
                case "Sound":
                    ManagementObjectSearcher MS5 = new ManagementObjectSearcher("SELECT * FROM Win32_SoundDevice");
                    foreach (ManagementObject MO in MS5.Get())
                    {
                        return MO["Caption"].ToString();
                    }
                    return "unknown";
                case "CPUm":
                    ManagementObjectSearcher MS6 = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                    foreach (ManagementObject MO in MS6.Get())
                    {
                        return MO["Manufacturer"].ToString();
                    }
                    return "unknown";
                case "MainBoardm":
                    ManagementObjectSearcher MS7 = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                    foreach (ManagementObject MO in MS7.Get())
                    {
                        return (MO["Manufacturer"].ToString());
                    }
                    return "unknown";
                default:
                    return "unknown";
            }
        }

        static string ConvertSizeToString(long length, bool shortFormat = true)
        {
            if (length < 0)
                return "";

            decimal size;
            string sizeFormatted, unit;

            if (length < 1000) // 1KB
            {
                size = length;
                unit = shortFormat ? " B" : " Bytes";
            }
            else if (length < 1000000) // 1MB
            {
                size = length / (decimal)0x400;
                unit = shortFormat ? " KB" : " Kilobytes";
            }
            else if (length < 1000000000) // 1GB
            {
                size = length / (decimal)0x100000;
                unit = shortFormat ? " MB" : " Megabytes";
            }
            else if (length < 1000000000000)
            {
                size = length / (decimal)0x40000000;
                unit = shortFormat ? " GB" : " Gigabytes";
            }
            else
            {
                size = length / (decimal)0x10000000000;
                unit = shortFormat ? " TB" : " Terabytes";
            }


            sizeFormatted = size.ToString("0.00");

            return sizeFormatted + unit;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drive = drv[comboBox1.SelectedIndex];
        }
    }
}
