using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Net;

namespace SystemCare
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect
                                              , int nTopRect
                                              , int nRightRect
                                              , int nBottomRect
                                              , int nWidthEllipse
                                              , int nHeightEllipse);
        public Form1()
        {
            InitializeComponent();
        }
        game game = new game();
        system system = new system();
        registry registry = new registry();
        tool tool = new tool();
        info info = new info();
        private void Form1_Load(object sender, EventArgs e)
        {
            Links();
            //시스템 정보
            label7.Text = Environment.UserName + ", " + GetSystemInfo("OS").Replace("Microsoft ", "") + GetOSbit() + "\n" + GetSystemInfo("CPU") + ", " + GetSystemInfo("Graphic") + ", " + GetSystemInfo("Ram") + "\n" + GetSystemInfo("MainBoard");

            panel3.BackColor = Color.FromArgb(47, 132, 133);
            panel8.Controls.Add(game);
            panel8.Controls.Add(system);
            panel8.Controls.Add(registry);
            panel8.Controls.Add(tool);
            panel8.Controls.Add(info);

            game.Show();
            system.Hide();
            registry.Hide();
            tool.Hide();
            info.Hide();

        }
        public static string GetSystemInfo(string infotoGet)
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
            else
            {
                size = length / (decimal)0x40000000;
                unit = shortFormat ? " GB" : " Gigabytes";
            }


            sizeFormatted = size.ToString("0.00");

            return sizeFormatted + unit;
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        enum RecycleFlags : uint
        {
            SHRB_NOCONFIRMATION = 0x00000001, // Don't ask confirmation
            SHRB_NOPROGRESSUI = 0x00000002, // Don't show any windows dialog
            SHRB_NOSOUND = 0x00000004 // Don't make sound, ninja mode enabled :v
        }

        public long GetDirectorySize(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            FileSystemInfo[] fileSystemInfoArray = directoryInfo.GetFileSystemInfos();

            long directorySize = 0L;

            for (int i = 0; i < fileSystemInfoArray.Length; i++)
            {
                FileInfo fileInfo = fileSystemInfoArray[i] as FileInfo;

                if (fileInfo != null)
                {
                    directorySize += fileInfo.Length;
                }
            }
            return directorySize;
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://www.systemcare.kr/NOTICE/138#0");
            Process.Start(sInfo);
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string ver = "";
            string urlAddress = "http://www.systemcare.kr/ver";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                int start = data.IndexOf("<title>");
                if (start != -1)
                {
                    int end = data.IndexOf("</title>", start + "<title>".Length);
                    if (end != -1)
                    {
                        ver = data.Substring(start + "<title>".Length, end);
                    }
                }

                ver = ver.Substring(0, 5);

                response.Close();
                readStream.Close();
            }
            Console.WriteLine(ver);
            if (ver == "1.04b")
            {
                MessageBox.Show("최신버전 입니다.", "SYSTEM CARE");
            }
            else
            {
                if (MessageBox.Show("최신버전이 아닙니다, 업데이트를 하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStartInfo sInfo = new ProcessStartInfo("http://www.systemcare.kr/");
                    Process.Start(sInfo);
                }
            }
        }

        private void Links()
        {
            string urlAddress = "http://www.systemcare.kr/textad";
            string textAd = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                int index1 = data.IndexOf("TextAddon");
                string index1_1 = data.Substring(index1, 13);
                string index1_2 = index1_1.Substring(9, 4);
                int intdex1_3 = int.Parse(index1_2);
                Addurl = data.Substring(index1 + 14, intdex1_3);

                int index2 = data.IndexOf("TextValue");
                string index2_1 = data.Substring(index2, 13);
                string index2_2 = index2_1.Substring(9, 4);
                int intdex2_3 = int.Parse(index2_2);
                string index2_4 = data.Substring(index2 + 14, intdex2_3);
                textAd = index2_4;

                response.Close();
                readStream.Close();
            }


            if (textAd != "") linkLabel3.Text = textAd;
            else linkLabel3.Text = "광고 모집 ( 배너 / 텍스트 링크 )";
        }

        private string getSubString(string str, string open, string close)
        {
            if (str == null || open == null || close == null)
            {
                return null;
            }
            int start = str.IndexOf(open);
            if (start != -1)
            {
                int end = str.IndexOf(close, start + open.Length);
                if (end != -1)
                {
                    return str.Substring(start + open.Length, end);
                }
            }
            return null;
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel6_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(150, 228, 228, 228);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen2, 0, 0, 119, 0);
            g.Dispose();
        }

        private void Panel8_Paint(object sender, PaintEventArgs e)
        {
        }

        string GetOSbit()
        {
            String sProcess;
            if(Environment.Is64BitOperatingSystem == true)
            {
                sProcess = " 64 Bit";

            }
            else
            {
                sProcess = " 32 Bit";
            }
            return sProcess;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            game.Refresh();
        }

        private void WebBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        string Addurl = "http://www.systemcare.kr/";
        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(Addurl);
            Process.Start(sInfo);
        }

        private void Panel3_MouseEnter(object sender, EventArgs e)
        {
            if (!panel3_button) panel3.BackColor = Color.FromArgb(47, 132, 133);
        }

        private void Panel3_MouseLeave(object sender, EventArgs e)
        {
            if (!panel3_button) panel3.BackColor = Color.FromArgb(95, 95, 95);
        }

        private void Panel3_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(150, 228, 228, 228);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen2, 0, 0, 119, 0);
            g.DrawLine(blackpen2, 0, 101, 119, 101);
            g.Dispose();
        }

        private void Panel4_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(150, 228, 228, 228);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen2, 0, 0, 119, 0);
            g.DrawLine(blackpen2, 0, 101, 119, 101);
            g.Dispose();
        }

        private void Panel5_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(150, 228, 228, 228);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen2, 0, 0, 119, 0);
            g.DrawLine(blackpen2, 0, 101, 119, 101);
            g.Dispose();
        }

        private void Panel7_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(150, 228, 228, 228);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen2, 0, 0, 119, 0);
            g.DrawLine(blackpen2, 0, 101, 119, 101);
            g.Dispose();
        }

        private void Panel4_MouseEnter(object sender, EventArgs e)
        {
            if (!panel4_button) panel4.BackColor = Color.FromArgb(47, 132, 133);
        }

        private void Panel4_MouseLeave(object sender, EventArgs e)
        {
            if (!panel4_button) panel4.BackColor = Color.FromArgb(95, 95, 95);
        }

        private void Panel5_MouseEnter(object sender, EventArgs e)
        {
            if (!panel5_button) panel5.BackColor = Color.FromArgb(47, 132, 133);
        }

        private void Panel5_MouseLeave(object sender, EventArgs e)
        {
            if (!panel5_button) panel5.BackColor = Color.FromArgb(95, 95, 95);
        }

        private void Panel6_MouseEnter(object sender, EventArgs e)
        {
            if (!panel6_button) panel6.BackColor = Color.FromArgb(47, 132, 133);
        }

        private void Panel6_MouseLeave(object sender, EventArgs e)
        {
            if (!panel6_button) panel6.BackColor = Color.FromArgb(95, 95, 95);
        }

        private void Panel7_MouseEnter(object sender, EventArgs e)
        {
            if (!panel7_button) panel7.BackColor = Color.FromArgb(47, 132, 133);
        }

        private void Panel7_MouseLeave(object sender, EventArgs e)
        {
            if (!panel7_button) panel7.BackColor = Color.FromArgb(95, 95, 95);
        }

        bool panel3_button = true;
        bool panel4_button = false;
        bool panel5_button = false;
        bool panel6_button = false;
        bool panel7_button = false;

        private void Panel3_Click(object sender, EventArgs e)
        {
            if (panel3_button == false)
            {
                panel3_button = true;

                game.Show();
                system.Hide();
                registry.Hide();
                tool.Hide();
                info.Hide();

                panel3.BackColor = Color.FromArgb(47, 132, 133);

                panel4_button = false;
                panel4.BackColor = Color.FromArgb(95, 95, 95);
                panel5_button = false;
                panel5.BackColor = Color.FromArgb(95, 95, 95);
                panel6_button = false;
                panel6.BackColor = Color.FromArgb(95, 95, 95);
                panel7_button = false;
                panel7.BackColor = Color.FromArgb(95, 95, 95);
            }
        }

        private void Panel4_Click(object sender, EventArgs e)
        {
            if (panel4_button == false)
            {
                panel4_button = true;

                system.Show();
                game.Hide();
                registry.Hide();
                tool.Hide();
                info.Hide();

                panel4.BackColor = Color.FromArgb(47, 132, 133);

                panel3_button = false;
                panel3.BackColor = Color.FromArgb(95, 95, 95);
                panel5_button = false;
                panel5.BackColor = Color.FromArgb(95, 95, 95);
                panel6_button = false;
                panel6.BackColor = Color.FromArgb(95, 95, 95);
                panel7_button = false;
                panel7.BackColor = Color.FromArgb(95, 95, 95);
            }
        }

        private void Panel5_Click(object sender, EventArgs e)
        {
            if (panel5_button == false)
            {
                panel5_button = true;

                system.Hide();
                game.Hide();
                registry.Show();
                tool.Hide();
                info.Hide();

                panel5.BackColor = Color.FromArgb(47, 132, 133);

                panel4_button = false;
                panel4.BackColor = Color.FromArgb(95, 95, 95);
                panel3_button = false;
                panel3.BackColor = Color.FromArgb(95, 95, 95);
                panel6_button = false;
                panel6.BackColor = Color.FromArgb(95, 95, 95);
                panel7_button = false;
                panel7.BackColor = Color.FromArgb(95, 95, 95);
            }
        }

        private void Panel6_Click(object sender, EventArgs e)
        {
            if (panel6_button == false)
            {
                panel6_button = true;

                system.Hide();
                game.Hide();
                registry.Hide();
                tool.Show();
                info.Hide();

                panel6.BackColor = Color.FromArgb(47, 132, 133);

                panel4_button = false;
                panel4.BackColor = Color.FromArgb(95, 95, 95);
                panel5_button = false;
                panel5.BackColor = Color.FromArgb(95, 95, 95);
                panel3_button = false;
                panel3.BackColor = Color.FromArgb(95, 95, 95);
                panel7_button = false;
                panel7.BackColor = Color.FromArgb(95, 95, 95);
            }
        }

        private void Panel7_Click(object sender, EventArgs e)
        {
            if(panel7_button == false)
            {
                panel7_button = true;

                system.Hide();
                game.Hide();
                registry.Hide();
                tool.Hide();
                info.Show();
                if(info.isOn == false) info.On();

                panel7.BackColor = Color.FromArgb(47, 132, 133);

                panel4_button = false;
                panel4.BackColor = Color.FromArgb(95, 95, 95);
                panel5_button = false;
                panel5.BackColor = Color.FromArgb(95, 95, 95);
                panel3_button = false;
                panel3.BackColor = Color.FromArgb(95, 95, 95);
                panel6_button = false;
                panel6.BackColor = Color.FromArgb(95, 95, 95);
            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }
    }
}