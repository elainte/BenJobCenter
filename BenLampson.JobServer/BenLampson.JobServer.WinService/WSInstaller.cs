using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BenLampson.JobServer.WinService
{
    [RunInstaller(true)]
    public class WSInstaller : System.Configuration.Install.Installer
    {
        public WSInstaller()
        {
            ServiceProcessInstaller process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            ServiceInstaller serviceAdmin = new ServiceInstaller();
            serviceAdmin.StartType = ServiceStartMode.Automatic;
            serviceAdmin.ServiceName = Program.SERVICE_NAME;
            serviceAdmin.DisplayName = Program.SERVICE_NAME;
            Installers.Add(process);
            Installers.Add(serviceAdmin);
        }
    }
}
