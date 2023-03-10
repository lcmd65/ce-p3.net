using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Text;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace CE_Laser_App
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            /// Run task form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
