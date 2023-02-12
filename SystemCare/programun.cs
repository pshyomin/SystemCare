using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace SystemCare
{
    public partial class programun : UserControl
    {
        public programun()
        {
            InitializeComponent();
        }

        string[] StartupName_1;
        string[] StartupName_2;
        string[] StartupName_3;

        private void Programun_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey registryMachineKey = Registry.LocalMachine;
                RegistryKey registryMachineKey64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey registryUserKey = Registry.CurrentUser;

                RegistryKey mProgram_1 = registryMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Uninstall");
                RegistryKey mProgram_2 = registryMachineKey64.CreateSubKey("SOFTWARE").CreateSubKey("WOW6432Node").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Uninstall");
                RegistryKey uProgram_1 = registryUserKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Uninstall");

                StartupName_1 = mProgram_1.GetSubKeyNames();
                StartupName_2 = mProgram_2.GetSubKeyNames();
                StartupName_3 = uProgram_1.GetSubKeyNames();

                Listv.BeginUpdate();

                foreach (string name in StartupName_1)
                {
                    if (mProgram_1.CreateSubKey(name).GetValue("DisplayName") != null)
                    {
                        if (mProgram_1.CreateSubKey(name).GetValue("SystemComponent") == null)
                        {
                            if (mProgram_1.CreateSubKey(name).GetValue("UninstallString") != null)
                            {
                                ListViewItem item = Listv.FindItemWithText((string)mProgram_1.CreateSubKey(name).GetValue("DisplayName"));
                                if (item == null)
                                {
                                    ListViewItem lvi = new ListViewItem((string)mProgram_1.CreateSubKey(name).GetValue("DisplayName"));
                                    lvi.SubItems.Add((string)mProgram_1.CreateSubKey(name).GetValue("Publisher"));
                                    lvi.SubItems.Add((string)ConvertSizeToString(Convert.ToInt32(mProgram_1.CreateSubKey(name).GetValue("EstimatedSize"))));
                                    lvi.SubItems.Add((string)mProgram_1.CreateSubKey(name).GetValue("DisplayVersion"));
                                    lvi.SubItems.Add((string)mProgram_1.CreateSubKey(name).GetValue("UninstallString"));

                                    Listv.Items.Add(lvi);
                                }
                            }
                        }
                    }
                }

                foreach (string name in StartupName_2)
                {
                    if (mProgram_2.CreateSubKey(name).GetValue("DisplayName") != null)
                    {
                        if (mProgram_2.CreateSubKey(name).GetValue("SystemComponent") == null)
                        {
                            if (mProgram_2.CreateSubKey(name).GetValue("UninstallString") != null)
                            {
                                ListViewItem item = Listv.FindItemWithText((string)mProgram_2.CreateSubKey(name).GetValue("DisplayName"));
                                if (item == null)
                                {
                                    ListViewItem lvi = new ListViewItem((string)mProgram_2.CreateSubKey(name).GetValue("DisplayName"));
                                    lvi.SubItems.Add((string)mProgram_2.CreateSubKey(name).GetValue("Publisher"));
                                    lvi.SubItems.Add((string)ConvertSizeToString(Convert.ToInt32(mProgram_2.CreateSubKey(name).GetValue("EstimatedSize"))));
                                    lvi.SubItems.Add((string)mProgram_2.CreateSubKey(name).GetValue("DisplayVersion"));
                                    lvi.SubItems.Add((string)mProgram_2.CreateSubKey(name).GetValue("UninstallString"));

                                    Listv.Items.Add(lvi);
                                }
                            }
                        }
                    }
                }

                foreach (string name in StartupName_3)
                {
                    if (uProgram_1.CreateSubKey(name).GetValue("DisplayName") != null)
                    {
                        if (uProgram_1.CreateSubKey(name).GetValue("SystemComponent") == null)
                        {
                            if (uProgram_1.CreateSubKey(name).GetValue("UninstallString") != null)
                            {
                                ListViewItem item = Listv.FindItemWithText((string)uProgram_1.CreateSubKey(name).GetValue("DisplayName"));
                                if (item == null)
                                {
                                    ListViewItem lvi = new ListViewItem((string)uProgram_1.CreateSubKey(name).GetValue("DisplayName"));
                                    lvi.SubItems.Add((string)uProgram_1.CreateSubKey(name).GetValue("Publisher"));
                                    lvi.SubItems.Add((string)ConvertSizeToString(Convert.ToInt32(uProgram_1.CreateSubKey(name).GetValue("EstimatedSize"))));
                                    lvi.SubItems.Add((string)uProgram_1.CreateSubKey(name).GetValue("DisplayVersion"));
                                    lvi.SubItems.Add((string)uProgram_1.CreateSubKey(name).GetValue("UninstallString"));

                                    Listv.Items.Add(lvi);
                                }
                            }
                        }
                    }
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
                MessageBox.Show("설치된 프로그램 목록을 불러오는데 문제가 발생하였습니다.", "SYSTEM CARE");
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

        string ConvertSizeToString(long length, bool shortFormat = true)
        {
            length *= 1000;

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

        private int sortColumn = -1;
        private String[] listview_columnTitle = { "프로그램", "게시자", "크기", "버전", "삭제 경로" };
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
            this.Listv.ListViewItemSorter = new MyListViewComparer2(e.Column, Listv.Sorting);
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            if (Listv.FocusedItem == null)
            {
                MessageBox.Show("선택된 항목이 없습니다.", "SYSTEM CARE");
            }
            else
            {
                if (MessageBox.Show("삭제하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SelectUnistall(Listv.FocusedItem.SubItems[4].Text);
                }
            }
        }

        void SelectUnistall(string name)
        {
            try
            {
                if (name.Contains("MsiExec.exe"))
                {
                    ProcessStartInfo cmd = new ProcessStartInfo();
                    Process process = new Process();
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
                    process.StandardInput.Write(@name + Environment.NewLine);
                    process.StandardInput.Close();
                    process.WaitForExit();
                    process.Close();

                    int cnt = Listv.SelectedItems.Count;
                    for (int i = cnt - 1; i >= 0; i--)
                    {
                        Listv.Items.Remove(Listv.SelectedItems[i]);
                    }

                    SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                    for (int i = 0; i < Listv.Columns.Count; i++)
                    {
                        Listv.Columns[i].Width = -2;
                    }
                }
                else
                {
                    if (name.Contains('"'))
                    {
                        ProcessStartInfo cmd = new ProcessStartInfo();
                        Process process = new Process();
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
                        process.StandardInput.Write(@name + Environment.NewLine);
                        process.StandardInput.Close();
                        process.WaitForExit();
                        process.Close();

                        int cnt = Listv.SelectedItems.Count;
                        for (int i = cnt - 1; i >= 0; i--)
                        {
                            Listv.Items.Remove(Listv.SelectedItems[i]);
                        }

                        SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                        for (int i = 0; i < Listv.Columns.Count; i++)
                        {
                            Listv.Columns[i].Width = -2;
                        }
                    }
                    else
                    {
                        ProcessStartInfo cmd = new ProcessStartInfo();
                        Process process = new Process();
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
                        process.StandardInput.Write(@"" + '"' + name + '"' + Environment.NewLine);
                        process.StandardInput.Close();
                        process.WaitForExit();
                        process.Close();

                        int cnt = Listv.SelectedItems.Count;
                        for (int i = cnt - 1; i >= 0; i--)
                        {
                            Listv.Items.Remove(Listv.SelectedItems[i]);
                        }

                        SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                        for (int i = 0; i < Listv.Columns.Count; i++)
                        {
                            Listv.Columns[i].Width = -2;
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
