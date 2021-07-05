using System;
using System.Windows.Forms;

namespace ClipSync
{
    static class Program
    {

        internal static ClipSyncControlForm loginSignUpForm { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            void LaunchApp()
            {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            loginSignUpForm = new ClipSyncControlForm();
            Application.Run(loginSignUpForm);
            }


            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {
                LaunchApp();
            }
            else
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    Arguments = string.Join(" ", Args),
                    Verb = "runas"
                };

                System.Diagnostics.Process.Start(startInfo);
                Application.Exit();
            }

        }
    }
}
