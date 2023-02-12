using System;
using System.Drawing;
using System.Windows.Forms;
using System.ServiceProcess;

namespace SystemCare
{
    public partial class service : UserControl
    {
        public service()
        {
            InitializeComponent();
        }

        private void ServiceList()
        {
            try
            {
                var services = ServiceController.GetServices();

                Listv.BeginUpdate();

                foreach (ServiceController service in services)
                {
                    ListViewItem lvi = new ListViewItem(service.Status.ToString());
                    lvi.SubItems.Add(service.DisplayName.ToString());
                    lvi.SubItems.Add(service.ServiceName.ToString());

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

        private void ServiceListReset()
        {
            try
            {
                Listv.Items.Clear();

                var services = ServiceController.GetServices();

                Listv.BeginUpdate();

                foreach (ServiceController service in services)
                {
                    ListViewItem lvi = new ListViewItem(service.Status.ToString());
                    lvi.SubItems.Add(service.DisplayName.ToString());
                    lvi.SubItems.Add(service.ServiceName.ToString());

                    Listv.Items.Add(lvi);
                }

                SetAlternatingRowColors(Listv, Color.FromArgb(255, 255, 255), Color.FromArgb(242, 242, 242));

                for (int i = 0; i < Listv.Columns.Count; i++)
                {
                    Listv.Columns[i].Width = -2;
                }

                Listv.EndUpdate();
            }catch
            {

            }
        }

        private void StartService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            TimeSpan timeout = new TimeSpan(0, 0, 5);

            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            ServiceListReset();
        }

        private void PausedService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            TimeSpan timeout = new TimeSpan(0, 0, 5);

            service.Pause();
            service.WaitForStatus(ServiceControllerStatus.Paused, timeout);
            ServiceListReset();
        }

        private void StopService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            TimeSpan timeout = new TimeSpan(0, 0, 5);

            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            ServiceListReset();
        }

        private int sortColumn = -1;
        private String[] listview_columnTitle = { "사용 여부", "프로그램", "서비스" };
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
                    string selectedname = Listv.FocusedItem.SubItems[2].ToString().Replace("ListViewSubItem: {", "").Replace("}", "");

                    Console.WriteLine(selectedname);

                    ContextMenu m = new ContextMenu();

                    MenuItem m1 = new MenuItem();
                    MenuItem m2 = new MenuItem();
                    MenuItem m3 = new MenuItem();
                    m1.Text = "Stop";
                    m2.Text = "Pause";
                    m3.Text = "Start";

                    m1.Click += (senders, es) =>
                    {
                        try
                        {
                            StopService(selectedname);
                        }
                        catch
                        {
                            MessageBox.Show("권한이 부족하여 중지 할 수 없습니다.", "SYSTEM CARE");
                        }
                    };
                    m2.Click += (senders, es) =>
                    {
                        try
                        {
                            PausedService(selectedname);
                        }
                        catch
                        {
                            MessageBox.Show("권한이 부족하여 일시정지 할 수 없습니다.", "SYSTEM CARE");
                        }
                    };
                    m3.Click += (senders, es) =>
                    {
                        try
                        {
                            StartService(selectedname);
                        }
                        catch
                        {
                            MessageBox.Show("권한이 부족하여 시작 할 수 없습니다.", "SYSTEM CARE");
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

        private void Service_Load(object sender, EventArgs e)
        {
            ServiceList();
        }
    }
}
