using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BenLampson.JobServer.WinService
{
    static class Program
    {

        public static string SERVICE_NAME = ConfigurationManager.AppSettings["ServiceName"];

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            bool _IsInstalled = false;
            bool serviceStarting = false;

            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in services)
            {
                if (service.ServiceName.Equals(SERVICE_NAME))
                {
                    _IsInstalled = true;
                    if (service.Status == ServiceControllerStatus.StartPending)
                    {
                        serviceStarting = true;
                    }
                    break;
                }
            }

            if (!serviceStarting)
            {
                if (_IsInstalled == true)
                {
                    // Thanks to PIEBALDconsult's Concern V2.0
                    DialogResult dr = new DialogResult();
                    dr = MessageBox.Show("Do you REALLY like to uninstall the " + SERVICE_NAME + "?", "Danger", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        //SelfInstaller.UninstallMe();
                        UninstallWindowsService(Assembly.GetExecutingAssembly().Location, SERVICE_NAME);
                        MessageBox.Show("Successfully uninstalled the " + SERVICE_NAME, "Status",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    DialogResult dr = new DialogResult();
                    dr = MessageBox.Show("Do you REALLY like to install the " + SERVICE_NAME + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        InstallWindowsService(Assembly.GetExecutingAssembly().Location, SERVICE_NAME);
                        //SelfInstaller.InstallMe();
                        MessageBox.Show("Successfully installed the " + SERVICE_NAME, "Status",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new JobService()
                };

                ServiceBase.Run(ServicesToRun);
            }
        }

        /// <summary>
        /// Use to install windows service
        /// </summary>
        /// <param name="filePath">your serviceFilePath</param>
        /// <param name="runRight">is run</param>
        /// <returns></returns>
        public static bool InstallWindowsService(string filePath, string serviceName)
        {
            if (ServiceController.GetServices().Where(serviceItem => serviceItem.ServiceName.Equals(serviceName)).Count() == 0)
            {
                try
                {
                    AssemblyInstaller asi = new AssemblyInstaller(filePath, new string[] { });
                    TransactedInstaller tranInstaller = new TransactedInstaller();
                    tranInstaller.Installers.Add(asi);
                    tranInstaller.Install(new Hashtable());
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Use to install windows service
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool UninstallWindowsService(string filePath, string serviceName)
        {
            if (ServiceController.GetServices().Where(serviceItem => serviceItem.ServiceName.Equals(serviceName)).Count() != 0)
            {
                try
                {
                    AssemblyInstaller asi = new AssemblyInstaller(filePath, new string[] { });
                    TransactedInstaller tranInstaller = new TransactedInstaller();
                    tranInstaller.Installers.Add(asi);
                    tranInstaller.Uninstall(null);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
}
