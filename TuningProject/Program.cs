using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace TuningHostProject
{
    static class Program
    {

        public static string TUNING_RESULTS_FILENAME = "Odessa_TuningGraph.csv";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TuningForm());
        }

        

    }
}
