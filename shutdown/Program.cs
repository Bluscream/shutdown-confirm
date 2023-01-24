using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Shutdown_Confirm
{
	internal static class Program {
		private static List<string> allowedArgs = new List<string>(new[] { "/skip", "/a" });
		[STAThread]
		private static void Main(string[] args) {
            string argstr = string.Join(" ", args);
			var app = "shutdown";
			var skip = argstr.ToLowerInvariant().Contains(allowedArgs[0]) || argstr.ToLowerInvariant().Contains(allowedArgs[1]);
            argstr = argstr.Replace(allowedArgs[0], "");
			foreach (var arg in args) {
				if (arg.StartsWith("app:")) {
					app = arg.Replace("app:", "");
                    argstr= argstr.Replace("app:", "").Replace(app, "");
                }
                var matches = new Regex("app:(.*)").Matches(argstr);
                if (matches.Count > 0) {
                    argstr = argstr.Replace(matches[0].Groups[0].Value, "");
                    app = matches[0].Groups[1].Value;
                }
            }

            if (skip || MessageBox.Show($"Some app wants to run {app}, accept?\n\n{argstr}", $"{app} request", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				Shutdown(app, argstr);
			}
		}
		private static void Shutdown(string app, string argstr) {
			var proc = new ProcessStartInfo {
				FileName = app,
				Arguments = argstr,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false
			};
            Process.Start(proc);
			Environment.Exit(0);
        }
	}
}
