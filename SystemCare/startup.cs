using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SystemCare
{
    public partial class startup : UserControl
    {
        string[] StartupName_1;
        string[] StartupName_2;
        string[] StartupName_3;
        public startup()
        {
            InitializeComponent();
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
                if (name == namea)
                {
                    return 2;
                }
            }

            foreach (string name in StartupName_3)
            {
                if (name == namea)
                {
                    return 3;
                }
            }
            return 0;
        }

        private int sortColumn = -1;
        private String[] listview_columnTitle = { "프로그램", "키", "사용자", "파일" };
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
            try
            {
                RegistryKey registryMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey registryUserKey = Registry.CurrentUser;

                RegistryKey Startup_1 = registryMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Run");
                RegistryKey Startup_2 = registryUserKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Run");
                RegistryKey Startup_3 = registryMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("WOW6432Node").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Run");

                StartupName_1 = Startup_1.GetValueNames();
                StartupName_2 = Startup_2.GetValueNames();
                StartupName_3 = Startup_3.GetValueNames();

                Listv.BeginUpdate();

                foreach (string name in StartupName_1)
                {
                    ListViewItem lvi = new ListViewItem(name);
                    lvi.SubItems.Add("HKLM:Run");
                    lvi.SubItems.Add("모든 사용자(ALL)");
                    lvi.SubItems.Add((string)Startup_1.GetValue(name));

                    Listv.Items.Add(lvi);
                }

                foreach (string name in StartupName_2)
                {
                    ListViewItem lvi = new ListViewItem(name);
                    lvi.SubItems.Add("HKCU:Run");
                    lvi.SubItems.Add(Environment.UserName);
                    lvi.SubItems.Add((string)Startup_2.GetValue(name));

                    Listv.Items.Add(lvi);
                }

                foreach (string name in StartupName_3)
                {
                    ListViewItem lvi = new ListViewItem(name);
                    lvi.SubItems.Add("HKLM:Run");
                    lvi.SubItems.Add("모든 사용자(ALL)");
                    lvi.SubItems.Add((string)Startup_3.GetValue(name));

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
                MessageBox.Show("시작 프로그램 목록을 불러오는데 문제가 발생하였습니다.", "SYSTEM CARE");
            }
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
                    if (MessageBox.Show("선택하신 시작프로그램을 삭제하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string strItem;
                        int Result = 0;

                        RegistryKey registryMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        RegistryKey registryUserKey = Registry.CurrentUser;

                        RegistryKey Startup_1 = registryMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Run");
                        RegistryKey Startup_2 = registryUserKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Run");
                        RegistryKey Startup_3 = registryMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("WOW6432Node").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Run");

                        foreach (Object selecteditem in Listv.SelectedItems)
                        {
                            strItem = selecteditem.ToString();
                            string mae = strItem.Replace("ListViewItem: {", "").Replace("}", "");
                            Result = GetNames(mae);

                            if (Result == 1)
                            {
                                Startup_1.DeleteValue(mae);
                            }
                            else if (Result == 2)
                            {
                                Startup_2.DeleteValue(mae);
                            }
                            else if (Result == 3)
                            {
                                Startup_3.DeleteValue(mae);
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
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }

    class MyListViewComparer : IComparer
    {
        private int col;
        private SortOrder order;
        public MyListViewComparer() { col = 0; order = SortOrder.Ascending; }
        public MyListViewComparer(int column, SortOrder order) { col = column; this.order = order; }
        public int Compare(object x, object y)
        {
            int returnVal = -1; returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text); // Determine whether the sort order is descending. 
            if (order == SortOrder.Descending) // Invert the value returned by String.Compare. 
                returnVal *= -1; return returnVal;
        }
    }
}
