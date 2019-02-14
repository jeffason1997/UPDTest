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

            Bitmap b = new Bitmap(1, 1);
            b.SetPixel(0, 0, Color.Red);
            Bitmap result = new Bitmap(b, 640, 480);

            ShowBitmap(result);
        }

        public void ShowBitmap(Bitmap b)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    PictureBox.Image = new Bitmap(b, 640, 480);
                    //PictureBox.Image = b;
                    PictureBox.Refresh();
                }));
            } else
            {
                //PictureBox.Image = b;
                PictureBox.Image = new Bitmap(b, 640, 480);
                PictureBox.Refresh();
            }
        }
    }
}
