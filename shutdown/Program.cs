using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

namespace Shutdown_Confirm
{
	internal static class Program {
		private static List<string> allowedArgs = new List<string>(new[] { "/skip", "/a" });
		[STAThread]
		private static void Main(string[] args)
        {
            Console.WriteLine($"\"{Application.StartupPath}\" {string.Join(" ", args)}");
            var app = args[0] ?? ""; // Environment.GetCommandLineArgs()
            args = args.Skip(1).ToArray();
            var argstr = string.Join(" ", args);
			var skip = argstr.ToLowerInvariant().Contains(allowedArgs[0]) || argstr.ToLowerInvariant().Contains(allowedArgs[1]);
            argstr = argstr.Replace(allowedArgs[0], "");
			/*foreach (var arg in args)
            {
                if (arg.StartsWith("app:")) {
				app = arg.Replace("app:", "");
				argstr=argstr.Replace(arg, "");
                }
                var matches = new Regex("app:(.*)").Matches(argstr);
                if (matches.Count > 0) {
                    argstr = argstr.Replace(matches[0].Value, ""); // .Groups[0]
                    app = matches[0].Groups[1].Value;
                }
            }
			if (app.EndsWith("_")) argstr = argstr.Replace(app.Remove(app.Length - 1), "");*/
            if (skip || MessageBox.Show($"Some app wants to run {app}, accept?\n\n{argstr}", $"{app} request", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
                if (app.Contains(".")) app = app.Insert(app.LastIndexOf('.'), "_");
                else app = app + "_";
                Shutdown(app, argstr);
			}
		}
		private static void Shutdown(string app, string argstr) {
            Console.WriteLine($"\"{app}\" {argstr}");
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = app,
                    Arguments = argstr,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                Console.WriteLine(proc.StandardOutput.ReadLine());
            }
            while (!proc.StandardError.EndOfStream)
            {
                Console.WriteLine(proc.StandardError.ReadLine());
            }
            Environment.Exit(0);
        }
	}
}
