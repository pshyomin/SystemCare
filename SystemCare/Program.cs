using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace SystemCare
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
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

            if (ver == "1.03")
            {
                
            }
            else
            {
                if (MessageBox.Show("최신버전이 아닙니다, 업데이트를 하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStartInfo sInfo = new ProcessStartInfo("http://www.systemcare.kr/");
                    Process.Start(sInfo);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
