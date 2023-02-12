using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace SystemCare
{
    public partial class videofiles : UserControl
    {
        public videofiles()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        void ListV_add(string a, string b, string c, string d)
        {
            //Listv.BeginUpdate();

            ListViewItem item = Listv.FindItemWithText((string)a);
            if (item == null)
            {
                ListViewItem lvi = new ListViewItem(a);
                lvi.SubItems.Add(b);
                lvi.SubItems.Add(c);
                lvi.SubItems.Add(d);

                Listv.Items.Add(lvi);
            }

            //Listv.EndUpdate();
        }

        public void SetAlternatingRowColors(ListView lst, Color color1, Color color2)
        {
            //loop through each ListViewItem in the ListView control
            foreach (ListViewItem item in lst.Items)
            {
                if ((item.Index % 2) == 0)
                    item.BackColor = color1;
                else
                    item.BackColor = color2;
            }
        }

        private void Listv_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button.Equals(MouseButtons.Right))
                {
                    ListViewItem selectedname = Listv.FocusedItem;

                    ContextMenu m = new ContextMenu();

                    MenuItem m5 = new MenuItem();
                    MenuItem m4 = new MenuItem();
                    MenuItem m1 = new MenuItem();
                    MenuItem m2 = new MenuItem();
                    MenuItem m3 = new MenuItem();

                    m5.Text = "경로 복사 하기";
                    m4.Text = "파일 위치 열기";
                    m1.Text = "삭제";
                    m2.Text = "목록 제거";
                    m3.Text = "전체 목록 제거";


                    m5.Click += (senders, es) =>
                    {
                        string path = selectedname.SubItems[3].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Clipboard.SetText(path);
                    };
                    m4.Click += (senders, es) =>
                    {
                        try
                        {
                            string path = selectedname.SubItems[3].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            Process.Start(path);
                        }
                        catch
                        {
                            MessageBox.Show("폴더 열기에 실패하였습니다.", "SYSTEM CARE");
                        }
                    };
                    m1.Click += (senders, es) =>
                    {
                        try
                        {
                            if (MessageBox.Show("선택하신 파일을 삭제하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                string path = selectedname.SubItems[3].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                                string filename = selectedname.SubItems[0].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                                if (File.Exists(path + @"\" + filename))
                                {
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                    FileInfo f = new FileInfo(path + @"\" + filename);
                                    f.Delete();
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("삭제에 실패하였습니다.", "SYSTEM CARE");
                        }
                    };
                    m2.Click += (senders, es) =>
                    {
                        try
                        {
                            Listv.Items.Remove(selectedname);

                            SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                            for (int i = 0; i < Listv.Columns.Count; i++)
                            {
                                Listv.Columns[i].Width = -2;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("문제가 발생하여 목록에서 제거하지 못하였습니다.", "SYSTEM CARE");
                        }
                    };
                    m3.Click += (senders, es) =>
                    {
                        try
                        {
                            Listv.Items.Clear();

                            SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                            for (int i = 0; i < Listv.Columns.Count; i++)
                            {
                                Listv.Columns[i].Width = -2;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("문제가 발생하여 목록에서 제거하지 못하였습니다.", "SYSTEM CARE");
                        }
                    };

                    m.MenuItems.Add(m5);
                    m.MenuItems.Add(m4);
                    m.MenuItems.Add(m1);
                    m.MenuItems.Add(m2);
                    m.MenuItems.Add(m3);
                    m.Show(Listv, new Point(e.X, e.Y));
                }
            }
            catch
            {

            }
        }

        private int sortColumn = -1;
        private String[] listview_columnTitle = { "프로그램", "크기", "마지막 사용 날짜", "파일" };
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked. 
            if (e.Column != sortColumn)
            { // Set the sort column to the new column. 
                sortColumn = e.Column; // Set the sort order to ascending by default. 
                Listv.Sorting = SortOrder.Ascending;
                Listv.Columns[sortColumn].Text = listview_columnTitle[sortColumn];
            }
            else
            { // Determine what the last sort order was and change it. 
                if (Listv.Sorting == SortOrder.Ascending)
                {
                    Listv.Sorting = SortOrder.Descending;
                    Listv.Columns[sortColumn].Text = listview_columnTitle[sortColumn];
                }
                else
                {
                    Listv.Sorting = SortOrder.Ascending;
                    Listv.Columns[sortColumn].Text = listview_columnTitle[sortColumn];
                }
            } // Call the sort method to manually sort. 
            Listv.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object. 
            this.Listv.ListViewItemSorter = new MyListViewComparer(e.Column, Listv.Sorting);
        }

        private void Startup_Load(object sender, EventArgs e)
        {
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
            if (Listv.FocusedItem == null)
            {
                MessageBox.Show("선택된 항목이 없습니다.", "SYSTEM CARE");
            }
            else
            {
                try
                {
                    if (MessageBox.Show("선택하신 파일을 삭제하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ListViewItem selectedname = Listv.FocusedItem;

                        string path = selectedname.SubItems[3].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                        string filename = selectedname.SubItems[0].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                        if (File.Exists(path + @"\" + filename))
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            FileInfo f = new FileInfo(path + @"\" + filename);
                            f.Delete();
                        }

                        Listv.Items.Remove(selectedname);

                        SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                        for (int i = 0; i < Listv.Columns.Count; i++)
                        {
                            Listv.Columns[i].Width = -2;
                        }

                        MessageBox.Show("성공적으로 삭제되었습니다.", "SYSTEM CARE");
                    }
                }
                catch
                {
                    MessageBox.Show("삭제에 실패하였습니다.", "SYSTEM CARE");
                }
            }
        }

        Thread a;
        bool isScan = false;
        private void Label2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isScan)
                {
                    isScan = true;

                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;

                    label2.Text = "스캔 중지";
                    a = new Thread(new ThreadStart(TraverseTree));
                        a.Start();
                }
                else if (isScan)
                {
                    isScan = false;

                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    radioButton3.Enabled = true;

                    label2.Text = "스캔";
                }
            }
            catch
            {
                MessageBox.Show("작업 중 문제가 발생하여 스캔을 중지하였습니다.", "SYSTEM CARE");

                isScan = false;

                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;

                label2.Text = "스캔";
            }
        }

        private void TraverseTree()
        {
            Listv.Items.Clear();

            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            DirectoryInfo folder = new DirectoryInfo(driveInfo[0].Name);

            Stack<string> dirs = new Stack<string>();

            if (!folder.Exists)
                throw new ArgumentException();

            dirs.Push(folder.FullName);

            while (dirs.Count > 0 & isScan)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
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

                        if (radioButton1.Checked)
                        {
                            if (fi.Extension == ".avi"
                                || fi.Extension == ".bik"
                                || fi.Extension == ".flv"
                                || fi.Extension == ".mkv"
                                || fi.Extension == ".mov"
                                || fi.Extension == ".mp4"
                                || fi.Extension == ".mpeg"
                                || fi.Extension == ".webm"
                                || fi.Extension == ".wmv"
                                || fi.Extension == ".asf"
                                || fi.Extension == ".mpeg1"
                                || fi.Extension == ".mpeg2"
                                || fi.Extension == ".mpeg4"
                                || fi.Extension == ".mpg"
                                || fi.Extension == ".mpe"
                                || fi.Extension == ".rm")
                            {
                                ListV_add(fi.Name, ConvertSizeToString(fi.Length), fi.CreationTime.ToString(), fi.DirectoryName);
                            }
                        }
                        else if (radioButton2.Checked)
                        {
                            if (fi.Extension == ".bmp"
                                || fi.Extension == ".rle"
                                || fi.Extension == ".dib"
                                || fi.Extension == ".jpg"
                                || fi.Extension == ".jpeg"
                                || fi.Extension == ".gif"
                                || fi.Extension == ".png"
                                || fi.Extension == ".tif"
                                || fi.Extension == ".tiff"
                                || fi.Extension == ".raw")
                            {
                                ListV_add(fi.Name, ConvertSizeToString(fi.Length), fi.CreationTime.ToString(), fi.DirectoryName);
                            }
                        }
                        else if (radioButton3.Checked)
                        {
                            if (fi.Extension == ".avi"
                                || fi.Extension == ".bik"
                                || fi.Extension == ".flv"
                                || fi.Extension == ".mkv"
                                || fi.Extension == ".mov"
                                || fi.Extension == ".mp4"
                                || fi.Extension == ".mpeg"
                                || fi.Extension == ".webm"
                                || fi.Extension == ".wmv"
                                || fi.Extension == ".asf"
                                || fi.Extension == ".mpeg1"
                                || fi.Extension == ".mpeg2"
                                || fi.Extension == ".mpeg4"
                                || fi.Extension == ".mpg"
                                || fi.Extension == ".mpe"
                                || fi.Extension == ".rm"
                                || fi.Extension == ".bmp"
                                || fi.Extension == ".rle"
                                || fi.Extension == ".dib"
                                || fi.Extension == ".jpg"
                                || fi.Extension == ".jpeg"
                                || fi.Extension == ".gif"
                                || fi.Extension == ".png"
                                || fi.Extension == ".tif"
                                || fi.Extension == ".tiff"
                                || fi.Extension == ".raw")
                            {
                                ListV_add(fi.Name, ConvertSizeToString(fi.Length), fi.CreationTime.ToString(), fi.DirectoryName);
                            }
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

                SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                for (int i = 0; i < Listv.Columns.Count; i++)
                {
                    Listv.Columns[i].Width = -2;
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }
        }

        private static IEnumerable<string> Traverse(string rootDirectory)
        {
            IEnumerable<string> files = Enumerable.Empty<string>();
            IEnumerable<string> directories = Enumerable.Empty<string>();
            try
            {
                // The test for UnauthorizedAccessException.
                var permission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, rootDirectory);
                permission.Demand();

                files = Directory.GetFiles(rootDirectory);
                directories = Directory.GetDirectories(rootDirectory);
            }
            catch
            {
                // Ignore folder (access denied).
                rootDirectory = null;
            }

            if (rootDirectory != null)
                yield return rootDirectory;

            foreach (var file in files)
            {
                yield return file;
            }

            // Recursive call for SelectMany.
            var subdirectoryItems = directories.SelectMany(Traverse);
            foreach (var result in subdirectoryItems)
            {
                yield return result;
            }
        }

        string ConvertSizeToString(long length, bool shortFormat = true)
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



        //private void Label3_MouseEnter(object sender, EventArgs e)
        //{
        //    label3.BackColor = Color.FromArgb(75, 75, 75);
        //}

        //private void Label3_MouseLeave(object sender, EventArgs e)
        //{
        //    label3.BackColor = Color.FromArgb(55, 55, 55);
        //}

        private void Label2_MouseEnter(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(75, 75, 75);
        }

        private void Label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(55, 55, 55);
        }

        //private void Label3_Click(object sender, EventArgs e)
        //{
            //try
            //{
            //    if (MessageBox.Show("목록에 있는 모든 파일을 삭제하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //    {
            //        for (int i = 0; i < Listv.Items.Count; i++)
            //        {
            //            string path = Listv.Items[i].SubItems[2].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
            //            string filename = Listv.Items[i].SubItems[0].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
            //            Console.WriteLine(path + @"\" + filename + "   " + i);
            //            if (File.Exists(path + @"\" + filename))
            //            {
            //                GC.Collect();
            //                GC.WaitForPendingFinalizers();
            //                FileInfo f = new FileInfo(path + @"\" + filename);
            //                f.Delete();
            //
            //            }
            //        }
            //        Listv.Items.Clear();
            //
            //        SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));
            //
            //        for (int i = 0; i < Listv.Columns.Count; i++)
            //        {
            //            Listv.Columns[i].Width = -2;
            //        }
            //
            //        MessageBox.Show("성공적으로 삭제되었습니다.", "SYSTEM CARE");
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("삭제에 실패하였습니다.", "SYSTEM CARE");
            //}
        //}

        private void Listv_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
