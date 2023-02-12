using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;

namespace SystemCare
{
    public partial class reg : UserControl
    {
        public reg()
        {
            InitializeComponent();
        }

        string[] StartupName_1;
        string[] StartupName_2;
        string[] StartupName_4;

        private void Reg_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey registryMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey registryClassesRootKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
                RegistryKey registryUserKey = Registry.CurrentUser;

                RegistryKey Startup_1 = registryUserKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows NT").CreateSubKey("CurrentVersion").CreateSubKey("AppCompatFlags").CreateSubKey("Compatibility Assistant").CreateSubKey("Store");
                RegistryKey MuiCache_1 = registryUserKey.CreateSubKey("SOFTWARE").CreateSubKey("Classes").CreateSubKey("Local Settings").CreateSubKey("Software").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("Shell").CreateSubKey("MuiCache");
                //RegistryKey MuiCache_2 = registryMachineKey.CreateSubKey("SYSTEM").CreateSubKey("ControlSet001").CreateSubKey("Services").CreateSubKey("SharedAccess").CreateSubKey("Parameters").CreateSubKey("FirewallPolicy").CreateSubKey("FirewallRules");
                RegistryKey MuiCache_3 = registryClassesRootKey;

                StartupName_1 = Startup_1.GetValueNames();
                StartupName_2 = MuiCache_1.GetValueNames();
                //StartupName_3 = MuiCache_2.GetValueNames();
                StartupName_4 = MuiCache_3.GetSubKeyNames();

                Listv.BeginUpdate();

                foreach (string name in StartupName_1)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(name);
                    if (!fi.Exists)
                    {
                        if (!name.Contains("SIGN"))
                        {
                            ListViewItem lvi = new ListViewItem(name);
                            lvi.SubItems.Add("프로그램 경로");
                            lvi.SubItems.Add("모든 사용자(ALL)");

                            Listv.Items.Add(lvi);
                        }
                    }
                }

                foreach (string name in StartupName_2)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(name.Replace(".ApplicationCompany", "").Replace(".FriendlyAppName", ""));
                    if (!fi.Exists)
                    {
                        if (!name.Contains(@"WINDOWS\") && !name.Contains("LangID"))
                        {
                            ListViewItem lvi = new ListViewItem(name.Replace(".ApplicationCompany", "").Replace(".FriendlyAppName", ""));
                            lvi.SubItems.Add("존재하지 않는 MUI 참조");
                            lvi.SubItems.Add(Environment.UserName);

                            Listv.Items.Add(lvi);
                        }
                    }
                }

                //foreach (string name in StartupName_3)
                //{
                //    string one = "|App=";
                //    string two = "|";
                //
                //    int one_1 = MuiCache_2.GetValue(name).ToString().IndexOf(one);
                //    int two_1 = MuiCache_2.GetValue(name).ToString().IndexOf(two);
                //    string path_check = MuiCache_2.GetValue(name).ToString().Substring(one_1 + one.Length);
                //    string path = path_check.Substring(0, path_check.IndexOf('|'));// = path_check.Substring(two_1 + two.Length);
                //    Console.WriteLine(path);
                //    System.IO.FileInfo fi = new System.IO.FileInfo(path);
                //    if (!fi.Exists)
                //    {
                //        if (path.Length > 6 || path.Contains("%SystemRoot%"))
                //        {
                //            ListViewItem lvi = new ListViewItem(name);
                //            lvi.SubItems.Add("잘못된 방화벽 정책");
                //            lvi.SubItems.Add(Environment.UserName);
                //
                //            Listv.Items.Add(lvi);
                //        }
                //    }
                //}

                foreach (string name in StartupName_4)
                {
                    try
                    {
                        if (MuiCache_3.CreateSubKey(name).SubKeyCount < 1)
                        {
                            if (MuiCache_3.CreateSubKey(name).ValueCount == 0)
                            {
                                ListViewItem lvi = new ListViewItem(name);
                                lvi.SubItems.Add("사용되지 않는 확장자");
                                lvi.SubItems.Add(Environment.UserName);

                                Listv.Items.Add(lvi);
                            }
                        }
                    }
                    catch
                    {

                    }
                }

                for (int i = 0; i < Listv.Items.Count - 1; i++)
                {
                    if (Listv.Items[i].Tag == Listv.Items[i + 1].Tag)
                    {
                        Listv.Items[i + 1].Remove();
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
                MessageBox.Show("레지스트리 목록을 불러오는데 문제가 발생하였습니다.", "SYSTEM CARE");
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

        private int GetNames(string namea)
        {
            foreach (string name in StartupName_1)
            {
                if (name == namea)
                {
                    return 1;
                }
            }

            foreach (string name in StartupName_2)
            {
                if (name == namea + ".ApplicationCompany" || name == namea + ".FriendlyAppName")
                {
                    return 2;
                }
            }

            foreach (string name in StartupName_4)
            {
                if (name == namea)
                {
                    return 4;
                }
            }

            return 0;
        }

        private int sortColumn = -1;
        private String[] listview_columnTitle = { "데이터", "문제", "사용자" };
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
                    try
                    {
                        string strItem;
                        int Result = 0;

                        RegistryKey registryMachineKey = Registry.LocalMachine;
                        RegistryKey registryUserKey = Registry.CurrentUser;
                        RegistryKey registryClassesRootKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);

                        RegistryKey Startup_1 = registryUserKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows NT").CreateSubKey("CurrentVersion").CreateSubKey("AppCompatFlags").CreateSubKey("Compatibility Assistant").CreateSubKey("Store");
                        RegistryKey MuiCache_1 = registryUserKey.CreateSubKey("SOFTWARE").CreateSubKey("Classes").CreateSubKey("Local Settings").CreateSubKey("Software").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("Shell").CreateSubKey("MuiCache");
                        RegistryKey MuiCache_3 = registryClassesRootKey;

                        foreach (Object selecteditem in Listv.SelectedItems)
                        {
                            strItem = selecteditem.ToString();
                            string mae = strItem.Replace("ListViewItem: {", "").Replace("}", "");
                            Result = GetNames(mae);

                            if (Result == 1)
                            {
                                Startup_1.DeleteValue(mae);
                            }

                            if (Result == 2)
                            {
                                if (MuiCache_1.GetValue(mae + ".ApplicationCompany") != null) MuiCache_1.DeleteValue(mae+ ".ApplicationCompany");
                                if (MuiCache_1.GetValue(mae + ".FriendlyAppName") != null) MuiCache_1.DeleteValue(mae+ ".FriendlyAppName");
                            }

                            if (Result == 4)
                            {
                                MuiCache_3.DeleteSubKey(mae);
                            }
                        }

                        if (Result == 0)
                        {
                            MessageBox.Show("존재하지 않거나 삭제에 실패하였습니다.", "SYSTEM CARE");
                        }
                        else
                        {
                            MessageBox.Show("성공적으로 삭제되었습니다.", "SYSTEM CARE");

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

                            Listv.Refresh();
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("전체 삭제하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string strItem;
                int Result = 0;

                RegistryKey registryMachineKey = Registry.LocalMachine;
                RegistryKey registryUserKey = Registry.CurrentUser;

                RegistryKey Startup_1 = registryUserKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows NT").CreateSubKey("CurrentVersion").CreateSubKey("AppCompatFlags").CreateSubKey("Compatibility Assistant").CreateSubKey("Store");

                foreach (Object selecteditem in Listv.Items)
                {
                    strItem = selecteditem.ToString();
                    string mae = strItem.Replace("ListViewItem: {", "").Replace("}", "");
                    Result = GetNames(mae);

                    if (Result == 1)
                    {
                        Startup_1.DeleteValue(mae);
                    }
                }

                Listv.Items.Clear();

                SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                for (int i = 0; i < Listv.Columns.Count; i++)
                {
                    Listv.Columns[i].Width = -2;
                }

                Listv.Refresh();

                if (Result == 0)
                {
                    MessageBox.Show("삭제에 실패하였습니다.", "SYSTEM CARE");
                }
                else
                {
                    MessageBox.Show("성공적으로 삭제되었습니다.", "SYSTEM CARE");
                }
            }
        }

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
    }

    class MyListViewComparer2 : IComparer
    {
        private int col;
        private SortOrder order;
        public MyListViewComparer2() { col = 0; order = SortOrder.Ascending; }
        public MyListViewComparer2(int column, SortOrder order) { col = column; this.order = order; }
        public int Compare(object x, object y)
        {
            int returnVal = -1; returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text); // Determine whether the sort order is descending. 
            if (order == SortOrder.Descending) // Invert the value returned by String.Compare. 
                returnVal *= -1; return returnVal;
        }
    }
}
