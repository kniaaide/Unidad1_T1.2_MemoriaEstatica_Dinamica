using System;
using System.Windows.Forms;

namespace Unidad1
{
    /// <summary>Punto de entrada de la aplicación de la Tarea 1.2.</summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
