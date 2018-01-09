using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GRAPHICS
{
    public partial class Form1 : Form
    {
        bool draw = false;
        bool image_exists = false;
        bool is_saved = true;

        string filepath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar tb = sender as TrackBar;
            int value = tb.Value;
            string labelname = tb.Name + "val";
            Label val = Controls["panel1"].Controls[labelname] as Label;
            val.Text = value.ToString();
        }

        private Graphics img;

        private void newMenu_Click(object sender, EventArgs e)
        {
            Bitmap im = new Bitmap(pictureBox.Width, pictureBox.Height);
            //Graphics img = Graphics.FromImage(im);
            img = Graphics.FromImage(im);
            img.FillRectangle(Brushes.White, 0, 0, pictureBox.Width, pictureBox.Height);
            pictureBox.Image = im;
            image_exists = true;
            filepath = "";
            this.Text = "NewImage.bmp*";
        }

        private void openMenu_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open...";
            ofd.Filter = "Bitmap(*.bmp) | *.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
                this.Text = Path.GetFileName(filepath);
                if (pictureBox.Image != null)
                    pictureBox.Image.Dispose();
                pictureBox.Image = new Bitmap(ofd.FileName);
                image_exists = true;
            }
        }

        private void saveMenu_Click(object sender, EventArgs e)
        {
            if (!is_saved)
                Save();
        }

        private void Save()
        {
            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (filepath != "")
            {
                string message = "Сохранить изменения в файле " + filepath + "?";
                DialogResult result = MessageBox.Show(message, "GRAPHICS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    string temp = Path.GetDirectoryName(filepath) + "\\temp.bmp";
                    pictureBox.Image.Save(temp);
                    pictureBox.Image.Dispose();
          
                    File.Delete(filepath);
                    File.Move(temp, filepath);
                    File.Delete(temp);         
                     pictureBox.Image = new Bitmap(filepath);
                    this.Text = this.Text.Replace("*", "");
                    is_saved = true;
                }
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save";
                sfd.Filter = "Bitmap|*.bmp";
                DialogResult result = sfd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    filepath = sfd.FileName;
                    pictureBox.Image.Save(filepath);
                    this.Text = Path.GetFileName(filepath);
                    is_saved = true;
                }
            }
        }

        private void exitMenu_Click(object sender, EventArgs e)
        {
           
            Close();
        }

        private void color_l_Click(object sender, EventArgs e)
        { 
            Label l = sender as Label;
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                l.BackColor = colorDialog1.Color;
            }
        }

        private Color get_color()
        {
            return Color.FromArgb(trackBarOP.Value, color_l.BackColor.R, color_l.BackColor.G, color_l.BackColor.B);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (image_exists)
            {
                draw = true;
                Draw(e.X, e.Y);
                is_saved = false;
                if (!this.Text.Contains("*"))
                this.Text += "*";
            }
        }

        private void Draw(int ex, int ey)
        {
            Image im = pictureBox.Image;
            Color c = get_color();
            int thickness = trackBarTH.Value;
            Graphics g = Graphics.FromImage(im);

            Rectangle rect = new Rectangle(
                ex,ey,
                thickness,
                thickness);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(rect);

            using (PathGradientBrush brush = new PathGradientBrush(path))
            {
                brush.CenterColor = c;
                brush.SurroundColors = new Color[] { Color.Transparent }; ;
                g.FillEllipse(brush, rect);
            }
            g.Save();
            pictureBox.Image = im;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                Draw(e.X, e.Y);
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!is_saved)
                Save();
        }

    }
}
