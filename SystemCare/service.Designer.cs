namespace SystemCare
{
    partial class service
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Listv = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // Listv
            // 
            this.Listv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.Listv.FullRowSelect = true;
            this.Listv.HideSelection = false;
            this.Listv.LabelWrap = false;
            this.Listv.Location = new System.Drawing.Point(3, 3);
            this.Listv.MultiSelect = false;
            this.Listv.Name = "Listv";
            this.Listv.Size = new System.Drawing.Size(542, 524);
            this.Listv.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.Listv.TabIndex = 1;
            this.Listv.TabStop = false;
            this.Listv.UseCompatibleStateImageBehavior = false;
            this.Listv.View = System.Windows.Forms.View.Details;
            this.Listv.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.Listv.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Listv_MouseClick);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "사용 여부";
            this.ColumnHeader1.Width = 129;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "프로그램";
            this.columnHeader2.Width = 59;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "서비스";
            this.columnHeader3.Width = 73;
            // 
            // service
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.Listv);
            this.MaximumSize = new System.Drawing.Size(548, 530);
            this.MinimumSize = new System.Drawing.Size(548, 530);
            this.Name = "service";
            this.Size = new System.Drawing.Size(548, 530);
            this.Load += new System.EventHandler(this.Service_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView Listv;
        private System.Windows.Forms.ColumnHeader ColumnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
