using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Shutdown_Confirm
{
	internal static class Program {
		private static List<string> allowedArgs = new List<string>(new[] { "/a", "/skip" });
		[STAThread]
		private static void Main(string[] args) {
            string argstr = string.Join(" ", args);
            foreach (var arg in args) {
				if (allowedArgs.Contains(arg)) Shutdown(argstr);
			}
			if (MessageBox.Show($"Some app requested a shutdown, accept?\n\n{argstr}", "Shutdown request", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				Shutdown(argstr);
			}
		}
		private static void Shutdown(string argstr) {
            Process.Start(new ProcessStartInfo {
                FileName = "shutdown_.exe",
                Arguments = argstr.Replace("/skip",""),
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            });
			Environment.Exit(0);
        }
	}
}
