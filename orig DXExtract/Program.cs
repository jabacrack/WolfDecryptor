// Decompiled with JetBrains decompiler
// Type: DXExtract.Program
// Assembly: DXExtract, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 593F5E8D-8016-45A8-94A1-EA2F500EB23F
// Assembly location: U:\Unpackers\DXExtract\DXExtract.exe

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DXExtract
{
  internal static class Program
  {
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeConsole();

    [DllImport("kernel32", SetLastError = true)]
    private static extern bool AttachConsole(int dwProcessId);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [STAThread]
    private static void Main(string[] args)
    {
      if (args.Length > 0)
      {
        int lpdwProcessId;
        int windowThreadProcessId = (int) Program.GetWindowThreadProcessId(Program.GetForegroundWindow(), out lpdwProcessId);
        Process processById = Process.GetProcessById(lpdwProcessId);
        if (processById.ProcessName == "cmd")
          Program.AttachConsole(processById.Id);
        else
          Program.AllocConsole();
        DXParser dxParser = new DXParser(args[0]);
        if (dxParser.checkForKey() != "")
        {
          dxParser.parseFile();
          dxParser.exportArchive();
        }
        Program.FreeConsole();
      }
      else
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run((Form) new Form1());
      }
    }
  }
}
