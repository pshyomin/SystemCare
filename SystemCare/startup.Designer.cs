namespace SystemCare
{
    partial class startup
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
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Listv
            // 
            this.Listv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader5});
            this.Listv.FullRowSelect = true;
            this.Listv.HideSelection = false;
            this.Listv.LabelWrap = false;
            this.Listv.Location = new System.Drawing.Point(3, 3);
            this.Listv.Name = "Listv";
            this.Listv.Size = new System.Drawing.Size(542, 485);
            this.Listv.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.Listv.TabIndex = 1;
            this.Listv.TabStop = false;
            this.Listv.UseCompatibleStateImageBehavior = false;
            this.Listv.View = System.Windows.Forms.View.Details;
            this.Listv.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "프로그램";
            this.ColumnHeader1.Width = 129;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "키";
            this.columnHeader2.Width = 59;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "사용자";
            this.columnHeader4.Width = 73;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "파일";
            this.columnHeader5.Width = 383;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(410, 491);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "삭 제";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            this.label1.MouseEnter += new System.EventHandler(this.Label1_MouseEnter);
            this.label1.MouseLeave += new System.EventHandler(this.Label1_MouseLeave);
            // 
            // startup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Listv);
            this.MaximumSize = new System.Drawing.Size(548, 530);
            this.MinimumSize = new System.Drawing.Size(548, 530);
            this.Name = "startup";
            this.Size = new System.Drawing.Size(548, 530);
            this.Load += new System.EventHandler(this.Startup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView Listv;
        private System.Windows.Forms.ColumnHeader ColumnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label label1;
    }
}
