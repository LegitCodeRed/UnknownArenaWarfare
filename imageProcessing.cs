using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using WindowsInput;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace AssaultCubeEmguCV
{
    
    class imageProcessing
    {
        public static bool scanning = false;

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private int correction = -10;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        getScreenshot gs = new getScreenshot();
        Random rnd = new Random();
        InputSimulator sim = new InputSimulator();

        Point screenRel = screenRelation();
        public void scan()
        {
            Image<Bgr, Byte> screenShot = gs.takeScreenshot();
            Image<Gray, Byte> grayImg = screenShot.InRange(new Bgr(39, 49, 220), new Bgr(73, 109, 255));
            Mat structElement = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(8, 8), new Point(-1, -1));
            CvInvoke.Dilate(grayImg.Mat, grayImg.Mat, structElement, new Point(-1, -1), 4, Emgu.CV.CvEnum.BorderType.Default, CvInvoke.MorphologyDefaultBorderValue);
            Rectangle enCooords = FindContours(grayImg.Mat, "", "CHAIN_APPROX_SIMPLE", 0);
            Point hitzPoint = new Point(enCooords.X + enCooords.Width / 2 - 100, enCooords.Y + enCooords.Height / 2 - 100+correction);

            //grayImg.ROI = enCooords;


            if (enCooords.Height > 0 && scanning)
            {
                Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
                // Set the Current cursor, move the cursor's Position,
                // and set its clipping rectangle to the form. 
                int X = Screen.PrimaryScreen.Bounds.Width / 2 + hitzPoint.X;
                int Y = Screen.PrimaryScreen.Bounds.Height / 2 + hitzPoint.Y;
                int sW = Screen.PrimaryScreen.Bounds.Width / 2;
                int sH = Screen.PrimaryScreen.Bounds.Height / 2;
                Cursor.Position = new Point(sW, sH);

                sim.Mouse.MoveMouseBy(X - sW, Y - sH);

                //Image<Bgr, Byte> imageToDraw = grayImg.Convert<Bgr, Byte>();
                //imageToDraw.Draw(enCooords, new Bgr(0,0,255), 3);
                //Rectangle toHitz = new Rectangle(enCooords.X + enCooords.Width/2, enCooords.Y+enCooords.Height/2, 1, 1);
                //imageToDraw.Draw(toHitz, new Bgr(0, 0, 255), 3);
                
                //Console.WriteLine(enCooords);

                //var display = new ImageViewer(grayImg);
                //display.ShowDialog();


                //mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)(int)sW, (uint)(int)sH, 0, 0);

            }
        }

        private static Point screenRelation()
        {
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            int valHeight = 655535 / height / 10;
            int valWidth = 655535 / width / 10;
            return new Point(valWidth, valHeight);

        }

        public Rectangle FindContours(Mat image, string mode, string method, int offset)
        {
           
            Mat hirachy = new Mat();
            Emgu.CV.CvEnum.RetrType retType = Emgu.CV.CvEnum.RetrType.External;
            Emgu.CV.CvEnum.ChainApproxMethod chainMethods = Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxNone;

            if (mode.Equals("RETR_LIST"))
            {
                retType = Emgu.CV.CvEnum.RetrType.List;
            }
            else if (mode.Equals("RETR_CCOMP"))
            {
                retType = Emgu.CV.CvEnum.RetrType.Ccomp;
            }
            else if (mode.Equals("RETR_TREE"))
            {
                retType = Emgu.CV.CvEnum.RetrType.Tree;
            }


            if (method.Equals("CHAIN_APPROX_SIMPLE"))
            {
                chainMethods = Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple;
            }
            else if (method.Equals("CHAIN_APPROX_TC89_L1"))
            {
                chainMethods = Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxTc89L1;
            }
            else if (method.Equals("CHAIN_APPROX_TC89_KCOS "))
            {
                chainMethods = Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxTc89Kcos;
            }

            Emgu.CV.Util.VectorOfVectorOfPoint vector_of_contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            CvInvoke.FindContours(image, vector_of_contours, hirachy, retType, chainMethods, new Point(offset, offset));

            Rectangle maxRect = new Rectangle(0,0,0,0);
            for (int i = 0; i < vector_of_contours.Size; i++)
            {
                Rectangle rect = CvInvoke.BoundingRectangle(vector_of_contours[i]);

                if (rect.Height > 40 && rect.Width > 40 && rect.Width < 200 && rect.Height < 200 && rect.Width < rect.Height)
                {
                    if (rect.Height > 150)
                        correction = -10;
                    else if (rect.Height > 50)
                        correction = 0;
                        maxRect = CvInvoke.BoundingRectangle(vector_of_contours[i]);
                    Console.WriteLine("Width: " + maxRect.Width);
                    Console.WriteLine("height: " + maxRect.Height);
                }
            }


            return maxRect;
        }


    }
}
