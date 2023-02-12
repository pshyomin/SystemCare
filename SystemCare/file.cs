using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SystemCare
{
    public partial class file : UserControl
    {
        public file()
        {
            InitializeComponent();
        }

        void ListV_add(string a, string b, string c)
        {
            try
            {
                Listv.BeginUpdate();

                ListViewItem item = Listv.FindItemWithText((string)a);
                if (item == null)
                {
                    ListViewItem lvi = new ListViewItem(a);
                    lvi.SubItems.Add(b);
                    lvi.SubItems.Add(c);

                    Listv.Items.Add(lvi);
                }
                SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                for (int i = 0; i < Listv.Columns.Count; i++)
                {
                    Listv.Columns[i].Width = -2;
                }

                Listv.EndUpdate();
            }
            catch
            {

            }
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

                    MenuItem m1 = new MenuItem();
                    MenuItem m2 = new MenuItem();
                    MenuItem m3 = new MenuItem();
                    m1.Text = "삭제";
                    m2.Text = "목록 제거";
                    m3.Text = "전체 목록 제거";

                    m1.Click += (senders, es) =>
                    {
                        try
                        {
                            string path = selectedname.SubItems[2].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                            string filename = selectedname.SubItems[0].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                            if (File.Exists(path + @"\" + filename))
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                FileInfo f = new FileInfo(path + @"\" + filename);
                                f.Delete();
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
        private String[] listview_columnTitle = { "프로그램", "크기", "파일" };
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
            try
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

                            string path = selectedname.SubItems[2].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
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
            catch
            {

            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlgOpen = new OpenFileDialog())
                {
                    dlgOpen.Filter = "All File(*.*)|*.*";
                    dlgOpen.Title = "Select Delete File (삭제할 파일을 선택해주세요)";
                    dlgOpen.Multiselect = true;
                    if (dlgOpen.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < dlgOpen.FileNames.Length; i++)
                        {
                            try
                            {
                                if (File.Exists(dlgOpen.FileNames[i]))
                                {
                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                    FileInfo f = new FileInfo(dlgOpen.FileNames[i]);
                                    ListV_add(f.Name, ConvertSizeToString(f.Length), f.DirectoryName);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch
            {

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

        private void Label3_MouseEnter(object sender, EventArgs e)
        {
            label3.BackColor = Color.FromArgb(75, 75, 75);
        }

        private void Label3_MouseLeave(object sender, EventArgs e)
        {
            label3.BackColor = Color.FromArgb(55, 55, 55);
        }

        private void Label2_MouseEnter(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(75, 75, 75);
        }

        private void Label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = Color.FromArgb(55, 55, 55);
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("목록에 있는 모든 파일을 삭제하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < Listv.Items.Count; i++)
                    {
                        string path = Listv.Items[i].SubItems[2].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                        string filename = Listv.Items[i].SubItems[0].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");
                        Console.WriteLine(path + @"\" + filename + "   " + i);
                        if (File.Exists(path + @"\" + filename))
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            FileInfo f = new FileInfo(path + @"\" + filename);
                            f.Delete();

                        }
                    }
                    Listv.Items.Clear();

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
}
