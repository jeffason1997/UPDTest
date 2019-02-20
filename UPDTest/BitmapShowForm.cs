using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPDTest
{
    public partial class BitmapShowForm : Form
    {
        public BitmapShowForm()
        {
            InitializeComponent();
            //Bitmap b = new Bitmap(1, 1);
            //b.SetPixel(0, 0, Color.White);
            //Bitmap c = new Bitmap(1, 1);
            //c.SetPixel(0, 0, Color.Green);
            //ShowBitmap(new Bitmap(b, 200, 100), 0);
            //ShowBitmap(new Bitmap(c, 200, 100), 1);

        }

        public void ShowBitmap(Bitmap b, int screenIndex)
        {
            //Bitmap oldPictureBox = null;

            //if (PictureBox.Image != null)
            //{
            //    oldPictureBox = new Bitmap(PictureBox.Image);
            //} else
            //{
            //    Bitmap bitm = new Bitmap(1, 1);
            //    bitm.SetPixel(0, 0, Color.White);
            //    oldPictureBox = new Bitmap(bitm, 640, 400);
            //}
            //int xoffset = (screenIndex % 3) * (oldPictureBox.Width / 3);
            //int yoffset = (screenIndex / 3) * (oldPictureBox.Height / 3);
            //for (int x = 0; x < b.Width; x++)
            //{
            //    for (int y = 0; y < b.Height; y++)
            //    {
            //        oldPictureBox.SetPixel(xoffset + x, yoffset + y, b.GetPixel(x, y));

            //    }
            //}
            //PictureBox.Image = oldPictureBox;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    //PictureBox.Image = new Bitmap(b, 640, 480);
                    PictureBox.Image = b;
                    PictureBox.Refresh();
                }));
            } else
            {
                PictureBox.Image = b;
                //PictureBox.Image = new Bitmap(b, 640, 480);
                PictureBox.Refresh();
            }
        }
    }
}
