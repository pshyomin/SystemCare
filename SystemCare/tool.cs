using System;
using System.Drawing;
using System.Windows.Forms;

namespace SystemCare
{
    public partial class tool : UserControl
    {
        public tool()
        {
            InitializeComponent();

            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(255, 228, 228, 228);
            Pen blackpen = new Pen(c, 3);
            Pen blackpen2 = new Pen(c, 1);
            Pen blackpen3 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen, 184, 0, 184, panel1.Height);
            g.DrawLine(blackpen2, 0, 0, 184, 0);
            g.DrawLine(blackpen3, 0, 534, 184, 534);
            g.Dispose();
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

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(255, 232, 232, 232);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen2, 0, 36, 183, 36);
            g.Dispose();
        }

        private void Panel3_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(255, 232, 232, 232);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen2, 0, 0, 183, 0);
            g.DrawLine(blackpen2, 0, 36, 183, 36);
            g.Dispose();
        }

        private void Panel4_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(255, 232, 232, 232);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen2, 0, 0, 183, 0);
            g.DrawLine(blackpen2, 0, 36, 183, 36);
            g.Dispose();
        }

        private void Panel5_Paint(object sender, PaintEventArgs e)
        {
            Color c = Color.FromArgb(255, 232, 232, 232);
            Pen blackpen2 = new Pen(c, 1);

            Graphics g = e.Graphics;

            g.DrawLine(blackpen2, 0, 0, 183, 0);
            g.DrawLine(blackpen2, 0, 36, 183, 36);
            g.Dispose();
        }

        private void Panel2_MouseEnter(object sender, EventArgs e)
        {
            if (!panel2_button)
            {
                label1.ForeColor = Color.White;
                pictureBox1.Image = Properties.Resources.arrow255;
                panel2.BackColor = Color.FromArgb(47, 132, 133);
            }
        }

        private void Panel2_MouseLeave(object sender, EventArgs e)
        {
            if (!panel2_button)
            {
                label1.ForeColor = Color.Black;
                pictureBox1.Image = Properties.Resources.arrow228;
                panel2.BackColor = Color.FromArgb(242, 242, 242);
            }
        }

        private void Panel3_MouseEnter(object sender, EventArgs e)
        {
            if (!panel3_button)
            {
                label2.ForeColor = Color.White;
                pictureBox2.Image = Properties.Resources.arrow255;
                panel3.BackColor = Color.FromArgb(47, 132, 133);
            }
        }

        private void Panel3_MouseLeave(object sender, EventArgs e)
        {
            if (!panel3_button)
            {
                label2.ForeColor = Color.Black;
                pictureBox2.Image = Properties.Resources.arrow228;
                panel3.BackColor = Color.FromArgb(242, 242, 242);
            }
        }

        private void Panel4_MouseEnter(object sender, EventArgs e)
        {
            if (!panel4_button)
            {
                label3.ForeColor = Color.White;
                pictureBox3.Image = Properties.Resources.arrow255;
                panel4.BackColor = Color.FromArgb(47, 132, 133);
            }
        }

        private void Panel4_MouseLeave(object sender, EventArgs e)
        {
            if (!panel4_button)
            {
                label3.ForeColor = Color.Black;
                pictureBox3.Image = Properties.Resources.arrow228;
                panel4.BackColor = Color.FromArgb(242, 242, 242);
            }
        }

        private void Panel5_MouseEnter(object sender, EventArgs e)
        {
            if (!panel5_button)
            {
                label4.ForeColor = Color.White;
                pictureBox4.Image = Properties.Resources.arrow255;
                panel5.BackColor = Color.FromArgb(47, 132, 133);
            }
        }

        private void Panel5_MouseLeave(object sender, EventArgs e)
        {
            if (!panel5_button)
            {
                label4.ForeColor = Color.Black;
                pictureBox4.Image = Properties.Resources.arrow228;
                panel5.BackColor = Color.FromArgb(242, 242, 242);
            }
        }

        programun programun = new programun();
        file file = new file();
        oldfiles oldfiles = new oldfiles();
        videofiles videofiles = new videofiles();
        private void Tool_Load(object sender, EventArgs e)
        {
            label1.ForeColor = Color.White;
            pictureBox1.Image = Properties.Resources.arrow255;
            panel2.BackColor = Color.FromArgb(47, 132, 133);

            panel6.Controls.Add(programun);
            panel6.Controls.Add(file);
            panel6.Controls.Add(oldfiles);
            panel6.Controls.Add(videofiles);

            programun.Show();
            file.Hide();
            oldfiles.Hide();
            videofiles.Hide();
        }

        bool panel2_button = true;
        bool panel3_button = false;
        bool panel4_button = false;
        bool panel5_button = false;

        private void Panel2_Click(object sender, EventArgs e)
        {
            if (panel2_button == false)
            {
                panel2_button = true;

                programun.Show();
                file.Hide();
                oldfiles.Hide();
                videofiles.Hide();

                label1.ForeColor = Color.White;
                pictureBox1.Image = Properties.Resources.arrow255;
                panel2.BackColor = Color.FromArgb(47, 132, 133);

                panel3_button = false;
                label2.ForeColor = Color.Black;
                pictureBox2.Image = Properties.Resources.arrow228;
                panel3.BackColor = Color.FromArgb(242, 242, 242);
                panel4_button = false;
                label3.ForeColor = Color.Black;
                pictureBox3.Image = Properties.Resources.arrow228;
                panel4.BackColor = Color.FromArgb(242, 242, 242);
                panel5_button = false;
                label4.ForeColor = Color.Black;
                pictureBox4.Image = Properties.Resources.arrow228;
                panel5.BackColor = Color.FromArgb(242, 242, 242);
            }
        }

        private void Panel3_Click(object sender, EventArgs e)
        {
            if (panel3_button == false)
            {
                panel3_button = true;

                programun.Hide();
                file.Show();
                oldfiles.Hide();
                videofiles.Hide();

                label2.ForeColor = Color.White;
                pictureBox2.Image = Properties.Resources.arrow255;
                panel3.BackColor = Color.FromArgb(47, 132, 133);

                panel2_button = false;
                label1.ForeColor = Color.Black;
                pictureBox1.Image = Properties.Resources.arrow228;
                panel2.BackColor = Color.FromArgb(242, 242, 242);
                panel4_button = false;
                label3.ForeColor = Color.Black;
                pictureBox3.Image = Properties.Resources.arrow228;
                panel4.BackColor = Color.FromArgb(242, 242, 242);
                panel5_button = false;
                label4.ForeColor = Color.Black;
                pictureBox4.Image = Properties.Resources.arrow228;
                panel5.BackColor = Color.FromArgb(242, 242, 242);
            }
        }

        private void Panel4_Click(object sender, EventArgs e)
        {
            if (panel4_button == false)
            {
                panel4_button = true;

                programun.Hide();
                file.Hide();
                oldfiles.Show();
                videofiles.Hide();

                label3.ForeColor = Color.White;
                pictureBox3.Image = Properties.Resources.arrow255;
                panel4.BackColor = Color.FromArgb(47, 132, 133);

                panel2_button = false;
                label1.ForeColor = Color.Black;
                pictureBox1.Image = Properties.Resources.arrow228;
                panel2.BackColor = Color.FromArgb(242, 242, 242);
                panel3_button = false;
                label2.ForeColor = Color.Black;
                pictureBox2.Image = Properties.Resources.arrow228;
                panel3.BackColor = Color.FromArgb(242, 242, 242);
                panel5_button = false;
                label4.ForeColor = Color.Black;
                pictureBox4.Image = Properties.Resources.arrow228;
                panel5.BackColor = Color.FromArgb(242, 242, 242);
            }
        }

        private void Panel5_Click(object sender, EventArgs e)
        {
            if (panel5_button == false)
            {
                panel5_button = true;

                programun.Hide();
                file.Hide();
                oldfiles.Hide();
                videofiles.Show();

                label4.ForeColor = Color.White;
                pictureBox4.Image = Properties.Resources.arrow255;
                panel5.BackColor = Color.FromArgb(47, 132, 133);

                panel2_button = false;
                label1.ForeColor = Color.Black;
                pictureBox1.Image = Properties.Resources.arrow228;
                panel2.BackColor = Color.FromArgb(242, 242, 242);
                panel3_button = false;
                label2.ForeColor = Color.Black;
                pictureBox2.Image = Properties.Resources.arrow228;
                panel3.BackColor = Color.FromArgb(242, 242, 242);
                panel4_button = false;
                label3.ForeColor = Color.Black;
                pictureBox3.Image = Properties.Resources.arrow228;
                panel4.BackColor = Color.FromArgb(242, 242, 242);
            }
        }
    }
}
