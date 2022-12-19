using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Shutdown_Confirm
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			string argstr = string.Join(" ", args);
			if (argstr == "/a" || MessageBox.Show($"Some app requested a shutdown, accept?\n\n{argstr}", "Shutdown request", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "shutdown_.exe",
					Arguments = argstr
                });
			}
		}
	}
}
