using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SystemCare
{
    public partial class system : UserControl
    {
        public system()
        {
            InitializeComponent();

            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
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

        private void Label1_Click(object sender, EventArgs e)
        {
            try
            {
                indexNum = 0;
                indexSize = 0;
                label6.Text = indexNum + " 개";
                label7.Text = ConvertSizeToString(indexSize);

                string RootPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToLower().Replace("documents", "");
                if (GetExists(RootPath, @"AppData\Local\Temporary Internet Files"))                     TraverseTree(RootPath, @"AppData\Local\Temporary Internet Files");
                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.Cookies)))        TraverseTree("", Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.InternetCache)))  TraverseTree("", Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.History)))        TraverseTree("", Environment.GetFolderPath(Environment.SpecialFolder.History));
                if (GetExists(RootPath, @"AppData\Local\Microsoft\Windows\Wer"))                        TraverseTree(RootPath, @"AppData\Local\Microsoft\Windows\Wer");
                if (GetExists(RootPath, @"AppData\Local\Microsoft\Windows\Caches"))                     TraverseTree(RootPath, @"AppData\Local\Microsoft\Windows\Caches");
                if (GetExists(RootPath, @"AppData\Local\Temp"))                                         TraverseTree(RootPath, @"AppData\Local\Temp");
                if (GetExists(RootPath, @"AppData\LocalLow\Microsoft\CryptnetUrlCache"))                TraverseTree(RootPath, @"AppData\LocalLow\Microsoft\CryptnetUrlCache");

                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Temp")) TraverseTree("", Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Temp");
                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\LiveKernelReports")) SystemTree("", Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\LiveKernelReports");

                label6.Text = indexNum + " 개";
                label7.Text = ConvertSizeToString(indexSize);
            }
            catch
            {
                MessageBox.Show("분석 정보를 가져오는데 문제가 발생하였습니다.", "SYSTEM CARE");
            }
        }

        bool GetExists(string root, string path)
        {
            string FullPath = root + path + "\\";
            GrantAccess(FullPath);
            DirectoryInfo folder = new DirectoryInfo(FullPath);

            return folder.Exists;
        }

        private void TraverseTree(string RootPath, string Path)
        {
            string FullPath = RootPath + Path + "\\";
            GrantAccess(FullPath);
            DirectoryInfo folder = new DirectoryInfo(FullPath);

            Stack<string> dirs = new Stack<string>();

            if (!folder.Exists)
                throw new ArgumentException();

            dirs.Push(folder.FullName);
            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    GrantAccess(currentDir);
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                // Perform the required action on each file here.
                // Modify this block to perform your required task.
                foreach (string file in files)
                {
                    try
                    {
                        // Perform whatever action is required in your scenario.
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        //Console.WriteLine("{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime);

                        // Delete old files
                        indexNum++;
                        indexSize += fi.Length;
                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        // If file was deleted by a separate application
                        //  or thread since the call to TraverseTree()
                        // then just continue.
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }
        }

        private void SystemTree(string RootPath, string Path)
        {
            string FullPath = RootPath + Path + "\\";
            GrantAccess(FullPath);
            DirectoryInfo folder = new DirectoryInfo(FullPath);

            Stack<string> dirs = new Stack<string>();

            if (!folder.Exists)
                throw new ArgumentException();

            dirs.Push(folder.FullName);
            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    GrantAccess(currentDir);
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                // Perform the required action on each file here.
                // Modify this block to perform your required task.
                foreach (string file in files)
                {
                    try
                    {
                        // Perform whatever action is required in your scenario.
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        //Console.WriteLine("{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime);
                        // Delete old files
                        if (fi.Extension == ".dmp")
                        {
                            indexNum++;
                            indexSize += fi.Length;
                        }
                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        // If file was deleted by a separate application
                        //  or thread since the call to TraverseTree()
                        // then just continue.
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }
        }

        private void TraverseDTree(string RootPath, string Path)
        {
            string FullPath = RootPath + Path + "\\";
            GrantAccess(FullPath);
            DirectoryInfo folder = new DirectoryInfo(FullPath);

            Stack<string> dirs = new Stack<string>();

            if (!folder.Exists)
                throw new ArgumentException();

            dirs.Push(folder.FullName);
            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    GrantAccess(currentDir);
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                // Perform the required action on each file here.
                // Modify this block to perform your required task.
                foreach (string file in files)
                {
                    try
                    {
                        // Delete old files
                        RemoveFile(file);
                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        // If file was deleted by a separate application
                        //  or thread since the call to TraverseTree()
                        // then just continue.
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }
        }

        private void SystemDTree(string RootPath, string Path)
        {
            string FullPath = RootPath + Path + "\\";
            GrantAccess(FullPath);
            DirectoryInfo folder = new DirectoryInfo(FullPath);

            Stack<string> dirs = new Stack<string>();

            if (!folder.Exists)
                throw new ArgumentException();

            dirs.Push(folder.FullName);
            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    GrantAccess(currentDir);
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                // Perform the required action on each file here.
                // Modify this block to perform your required task.
                foreach (string file in files)
                {
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        if (fi.Extension == ".dmp")
                        {
                            RemoveFile(file);
                        }
                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        // If file was deleted by a separate application
                        //  or thread since the call to TraverseTree()
                        // then just continue.
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }
        }


        //private static void RemoveFiles(string RootPath, string Path, bool Recursive)
        //{
        //    string FullPath = RootPath + Path + "\\";
        //    if (Directory.Exists(FullPath))
        //    {
        //        DirectoryInfo DInfo = new DirectoryInfo(FullPath);
        //        FileAttributes Attr = DInfo.Attributes;
        //        DInfo.Attributes = FileAttributes.Normal;
        //        foreach (string FileName in Directory.GetFiles(FullPath))
        //        {
        //            RemoveFile(FileName);
        //        }
        //        if (Recursive)
        //        {
        //            foreach (string DirName in Directory.GetDirectories(FullPath))
        //            {
        //                RemoveFiles("", DirName, true);
        //                try { Directory.Delete(DirName); } catch { }
        //            }
        //        }
        //        DInfo.Attributes = Attr;
        //    }
        //}
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

        int indexNum = 0;
        long indexSize = 0;

        public bool GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
            return true;
        }

        private static void RemoveFile(string FileName)
        {
            if (File.Exists(FileName))
            {
                try { File.Delete(FileName); } catch { }//Locked by something and you can forget trying to delete index.dat files this way
            }
        }

        //private void DeleteFireFoxFiles(string FireFoxPath)
        //{
        //    RemoveFile(FireFoxPath + "cookies.sqlite");
        //    RemoveFile(FireFoxPath + "content-prefs.sqlite");
        //    RemoveFile(FireFoxPath + "downloads.sqlite");
        //    RemoveFile(FireFoxPath + "formhistory.sqlite");
        //    RemoveFile(FireFoxPath + "search.sqlite");
        //    RemoveFile(FireFoxPath + "signons.sqlite");
        //    RemoveFile(FireFoxPath + "search.json");
        //    RemoveFile(FireFoxPath + "permissions.sqlite");
        //}

        //public void RunCleanup()
        //{
        //    try { KillProcess("iexplore"); }
        //    catch { }//Need to stop incase they have locked the files we want to delete
        //    try { KillProcess("FireFox"); }
        //    catch { }//Need to stop incase they have locked the files we want to delete
        //    string RootPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToLower().Replace("documents", "");
        //    RemoveFiles(RootPath, @"AppData\Roaming\Macromedia\Flash Player\#SharedObjects", false);
        //    RemoveFiles(RootPath, @"AppData\Roaming\Macromedia\Flash Player\macromedia.com\support\flashplayer\sys\#local", false);
        //    RemoveFiles(RootPath, @"AppData\Local\Temporary Internet Files", false);//Not working
        //    RemoveFiles("", Environment.GetFolderPath(Environment.SpecialFolder.Cookies), true);
        //    RemoveFiles("", Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), true);
        //    RemoveFiles("", Environment.GetFolderPath(Environment.SpecialFolder.History), true);
        //    RemoveFiles(RootPath, @"AppData\Local\Microsoft\Windows\Wer", true);
        //    RemoveFiles(RootPath, @"AppData\Local\Microsoft\Windows\Caches", false);
        //    RemoveFiles(RootPath, @"AppData\Local\Microsoft\WebsiteCache", false);
        //    RemoveFiles(RootPath, @"AppData\Local\Temp", false);
        //    RemoveFiles(RootPath, @"AppData\LocalLow\Microsoft\CryptnetUrlCache", false);
        //    RemoveFiles(RootPath, @"AppData\LocalLow\Apple Computer\QuickTime\downloads", false);
        //    RemoveFiles(RootPath, @"AppData\Local\Mozilla\Firefox\Profiles", false);
        //    RemoveFiles(RootPath, @"AppData\Roaming\Microsoft\Office\Recent", false);
        //    RemoveFiles(RootPath, @"AppData\Roaming\Adobe\Flash Player\AssetCache", false);
        //    if (Directory.Exists(RootPath + @"\AppData\Roaming\Mozilla\Firefox\Profiles"))
        //    {
        //        string FireFoxPath = RootPath + @"AppData\Roaming\Mozilla\Firefox\Profiles\";
        //        DeleteFireFoxFiles(FireFoxPath);
        //        foreach (string SubPath in Directory.GetDirectories(FireFoxPath))
        //        {
        //            DeleteFireFoxFiles(SubPath + "\\");
        //        }
        //    }
        //}

        //private void KillProcess(string ProcessName)
        //{//We ned to kill Internet explorer and Firefox to stop them locking files
        //    ProcessName = ProcessName.ToLower();
        //    foreach (Process P in Process.GetProcesses())
        //    {
        //        if (P.ProcessName.ToLower().StartsWith(ProcessName))
        //            P.Kill();
        //    }
        //}

        private void Label2_MouseEnter(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(75, 75, 75);
        }

        private void Label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(55, 55, 55);
        }

        private void Label1_MouseEnter(object sender, EventArgs e)
        {
            label1.BackColor = Color.FromArgb(75, 75, 75);
        }

        private void Label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = Color.FromArgb(55, 55, 55);
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정리 하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string RootPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToLower().Replace("documents", "");
                if (GetExists(RootPath, @"AppData\Local\Temporary Internet Files"))                     TraverseDTree(RootPath, @"AppData\Local\Temporary Internet Files");
                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.Cookies)))        TraverseDTree("", Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.InternetCache)))  TraverseDTree("", Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.History)))        TraverseDTree("", Environment.GetFolderPath(Environment.SpecialFolder.History));
                if (GetExists(RootPath, @"AppData\Local\Microsoft\Windows\Wer"))                        TraverseDTree(RootPath, @"AppData\Local\Microsoft\Windows\Wer");
                if (GetExists(RootPath, @"AppData\Local\Microsoft\Windows\Caches"))                     TraverseDTree(RootPath, @"AppData\Local\Microsoft\Windows\Caches");
                if (GetExists(RootPath, @"AppData\Local\Temp"))                                         TraverseDTree(RootPath, @"AppData\Local\Temp");
                if (GetExists(RootPath, @"AppData\LocalLow\Microsoft\CryptnetUrlCache")) TraverseDTree(RootPath, @"AppData\LocalLow\Microsoft\CryptnetUrlCache");

                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Temp")) TraverseTree("", Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Temp");
                if (GetExists("", Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\LiveKernelReports")) SystemDTree("", Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\LiveKernelReports");
            }
        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }
    }
}
