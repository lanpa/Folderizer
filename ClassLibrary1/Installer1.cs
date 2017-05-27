using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ClassLibrary1
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);
            // Get the location of regasm
            string regasmPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() + @"regasm.exe";
            if (System.Environment.Is64BitOperatingSystem) //to prevent wow6432 issue
            {
                regasmPath = regasmPath.Replace("Framework", "Framework64");
            }
            //
            // Get the location of our DLL
            string componentPath = typeof(Installer1).Assembly.Location;
            //MessageBox.Show(regasmPath + "\"" + componentPath + "\"" + " /codebase");
            System.Threading.Thread.Sleep(1000);
            // assert file is copied
            System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
            processInfo.Verb = "runas";
            processInfo.Arguments = "/K "+ regasmPath + " /codebase" + " \"" + componentPath + "\"";
            //System.Diagnostics.Process.Start(processInfo);
            // Execute regasm
            System.Diagnostics.Process.Start(regasmPath, "\"" + componentPath +"\"" + " /codebase");
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        //public override void Uninstall(System.Collections.IDictionary savedState)
        protected override void OnBeforeUninstall(System.Collections.IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);
            string regasmPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() + @"regasm.exe";
            if (System.Environment.Is64BitOperatingSystem) //to prevent wow6432 issue
            {
                regasmPath = regasmPath.Replace("Framework", "Framework64");
            }
            string componentPath = typeof(Installer1).Assembly.Location;
            //MessageBox.Show(regasmPath + "\"" + componentPath + "\"" + " /codebase");

            System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
            processInfo.Verb = "runas";
            processInfo.Arguments = "/K "+ regasmPath + " /unregister \"" + componentPath + "\"" + " /codebase";
            //System.Diagnostics.Process.Start(processInfo);
            System.Diagnostics.Process.Start(regasmPath, "\"" + componentPath + "\"" + " /codebase /unregister");

            System.Threading.Thread.Sleep(1000);
        }


          
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);

        }
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }
    }
}
