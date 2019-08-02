using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Threading;

namespace AssaultCubeEmguCV
{
    class Program
    {
        static void Main(string[] args)
        {
            //InterceptKeys.InitializeComponent();
            AssaultCubeEmguCV.imageProcessing imgProc = new AssaultCubeEmguCV.imageProcessing();
            ThreadStart childref = new ThreadStart(InterceptKeys.InitializeComponent);
            Thread childThread = new Thread(childref);
            childThread.Start();

            while (true)
            {
                imgProc.scan();
                System.Threading.Thread.Sleep(35);
            }
        }

    
    }
}
