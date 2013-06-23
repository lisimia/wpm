using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WallpaperManagerGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            WallpaperManager.Layout[] n = new WallpaperManager.Layout[1];
            n[0] = new WallpaperManager.Layout
            {
                style = WallpaperManager.WallpaperStyle.CENTER,
                source = @"C:\Q\test.jpg",
                screens = new Rectangle[]{new Rectangle(0, 0, 1920, 1080)}
            };

            WallpaperManager.Manager wm = new WallpaperManager.Manager(n);

        }
    }
}
