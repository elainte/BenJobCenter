using BenLampson.JobServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BenLampson.JobServer.WinService
{
    public partial class JobService : ServiceBase
    {
        CancellationTokenSource cancelToken = new CancellationTokenSource();

        public JobService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            new BenJobServer(cancelToken).Start();
        }


        protected override void OnStop()
        {
            cancelToken.Cancel();
        }
    }
}
