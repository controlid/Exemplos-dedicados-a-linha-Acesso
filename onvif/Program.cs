using System;
using System.Windows.Forms;

namespace onvif
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmTest());
            //Application.Run(new frmOnvifTest());
        }
    }
}
