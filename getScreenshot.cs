using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace AssaultCubeEmguCV
{
    class getScreenshot
    {
        public Image<Bgr, Byte> takeScreenshot()
        {
            // Shot size = screen size
            Size shotSize = new Size(200, 200);

            // the upper left point in the screen to start shot
            // 0,0 to get the shot from upper left point
            Point upperScreenPoint = new Point(0, 0);

            // the upper left point in the image to put the shot
            Point upperDestinationPoint = new Point(0, 0);

            // create image to get the shot in it
            Bitmap shot = new Bitmap(shotSize.Width, shotSize.Height);

            // new Graphics instance 
            Graphics graphics = Graphics.FromImage(shot);

            // get the shot by Graphics class 
            graphics.CopyFromScreen(new Point(Screen.PrimaryScreen.Bounds.Width / 2 - 100, Screen.PrimaryScreen.Bounds.Height/2 -100), upperDestinationPoint, shotSize);

            Bitmap bmpImage = new Bitmap(300, 300, graphics);
            return new Emgu.CV.Image<Bgr, Byte>(shot);
        }
    }
}
