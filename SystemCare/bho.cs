using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;

namespace SystemCare
{
    public partial class bho : UserControl
    {
        public bho()
        {
            InitializeComponent();
        }

        string[] StartupName_1;
        string[] StartupName_2;
        string[] StartupName_3;

        private void Bho_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey registryMachineKey = Registry.LocalMachine;
                RegistryKey registryMachineKey2 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey registryUserKey = Registry.CurrentUser;

                RegistryKey ActiveX_1 = registryMachineKey2.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Explorer").CreateSubKey("Browser Helper Objects");
                RegistryKey ActiveX_2 = registryMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Internet Explorer").CreateSubKey("Extensions");
                RegistryKey ActiveX_3 = registryMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Internet Explorer").CreateSubKey("Toolbar");

                StartupName_1 = ActiveX_1.GetSubKeyNames();
                StartupName_2 = ActiveX_2.GetSubKeyNames();
                StartupName_3 = ActiveX_3.GetSubKeyNames();

                Listv.BeginUpdate();

                foreach (string name in StartupName_1)
                {
                    ListViewItem item = Listv.FindItemWithText(name);
                    if (item == null)
                    {
                        ListViewItem lvi = new ListViewItem(name);
                        lvi.SubItems.Add(GetFileNameOfActiveX(name));
                        lvi.SubItems.Add(GetFilePathOfActiveX(name));

                        Listv.Items.Add(lvi);
                    }
                }

                foreach (string name in StartupName_2)
                {
                    string GetID = (string)ActiveX_2.CreateSubKey(name).GetValue("ClsidExtension");
                    ListViewItem item = Listv.FindItemWithText(name);
                    if (item == null)
                    {
                        ListViewItem lvi = new ListViewItem(name);
                        lvi.SubItems.Add(GetFileNameOfActiveX(GetID));
                        lvi.SubItems.Add(GetFilePathOfActiveX(GetID));

                        Listv.Items.Add(lvi);
                    }
                }

                foreach (string name in StartupName_3)
                {
                    string GetID = (string)ActiveX_3.CreateSubKey(name).GetValue("ClsidExtension");
                    ListViewItem item = Listv.FindItemWithText(name);
                    if (item == null)
                    {
                        ListViewItem lvi = new ListViewItem(name);
                        lvi.SubItems.Add(GetFileNameOfActiveX(GetID));
                        lvi.SubItems.Add(GetFilePathOfActiveX(GetID));

                        Listv.Items.Add(lvi);
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
                MessageBox.Show("탐색기 플러그인 목록을 불러오는데 문제가 발생하였습니다.", "SYSTEM CARE");
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

        private static string GetFilePathOfActiveX(string comName)
        {
            string clsid = comName;
            RegistryKey subKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64).OpenSubKey("CLSID\\" + clsid + "\\LocalServer32");

            if (subKey == null)
            {
                subKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64).OpenSubKey("CLSID\\" + clsid + "\\InprocServer32");
            }

            if (subKey == null)
            {
                subKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64).OpenSubKey("WOW6432Node\\CLSID\\" + clsid + "\\InprocServer32");
            }

            if (subKey == null) return null;

            return (string)subKey.GetValue("");
        }

        private static string GetFileNameOfActiveX(string comName)
        {
            string clsid = comName;
            RegistryKey subKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64).OpenSubKey("CLSID\\" + clsid);

            if (subKey == null)
            {
                subKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64).OpenSubKey("CLSID\\" + clsid);
            }

            if (subKey == null)
            {
                subKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64).OpenSubKey("WOW6432Node\\CLSID\\" + clsid);
            }

            if (subKey == null) return null;

            return (string)subKey.GetValue("");
        }

        private int sortColumn = -1;
        private String[] listview_columnTitle = { "클래스ID", "프로그램", "파일" };
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
            this.Listv.ListViewItemSorter = new MyListViewComparer3(e.Column, Listv.Sorting);
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            if (Listv.FocusedItem == null)
            {
                MessageBox.Show("선택된 항목이 없습니다.", "SYSTEM CARE");
            }
            else
            {
                if (MessageBox.Show("선택하신 탐색기 플러그인을 삭제 하시겠습니까?", "SYSTEM CARE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        string strItem;
                        int Result = 0;

                        RegistryKey registryMachineKey = Registry.LocalMachine;
                        RegistryKey registryMachineKey2 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                        RegistryKey registryUserKey = Registry.CurrentUser;

                        RegistryKey ActiveX_1 = registryMachineKey2.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Explorer").CreateSubKey("Browser Helper Objects");
                        RegistryKey ActiveX_2 = registryMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Internet Explorer").CreateSubKey("Extensions");
                        RegistryKey ActiveX_3 = registryMachineKey.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Internet Explorer").CreateSubKey("Toolbar");

                        foreach (Object selecteditem in Listv.SelectedItems)
                        {
                            strItem = selecteditem.ToString();
                            string mae = strItem.Replace("ListViewItem: {", "").Replace("}", "");

                            Result = GetNames(mae + "}");

                            if (Result == 1)
                            {
                                ActiveX_1.DeleteSubKeyTree(mae + "}");
                            }
                            else if (Result == 2)
                            {
                                ActiveX_2.DeleteSubKeyTree(mae + "}");

                            }
                            else if (Result == 3)
                            {
                                ActiveX_3.DeleteSubKeyTree(mae + "}");
                            }
                        }

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

                        if (Result == 0)
                        {
                            MessageBox.Show("존재하지 않거나 삭제에 실패하였습니다.", "SYSTEM CARE");
                        }
                        else
                        {
                            MessageBox.Show("성공적으로 삭제 되었습니다.", "SYSTEM CARE");
                        }
                    }
                    catch
                    { }
                }
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

        private void Label1_MouseEnter(object sender, EventArgs e)
        {
            label1.BackColor = Color.FromArgb(75, 75, 75);
        }

        private void Label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = Color.FromArgb(55, 55, 55);
        }
    }

    class MyListViewComparer3 : IComparer
    {
        private int col;
        private SortOrder order;
        public MyListViewComparer3() { col = 0; order = SortOrder.Ascending; }
        public MyListViewComparer3(int column, SortOrder order) { col = column; this.order = order; }
        public int Compare(object x, object y)
        {
            int returnVal = -1; returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text); // Determine whether the sort order is descending. 
            if (order == SortOrder.Descending) // Invert the value returned by String.Compare. 
                returnVal *= -1; return returnVal;
        }
    }
}
