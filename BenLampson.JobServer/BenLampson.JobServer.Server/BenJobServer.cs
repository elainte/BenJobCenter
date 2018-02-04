using BenLampson.JobServer.DataRepository.DB.Table;
using BenLampson.JobServer.DataRepository.DB.Operation;
using BenDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenDotNet.AdoNet.Core;
using System.Configuration;
using System.Collections;
using BenLampson.JobServer.DataRepository.Model;
using System.Collections.Concurrent;
using System.Reflection;
using System.Diagnostics;

namespace BenLampson.JobServer.Server
{
    public class BenJobServer
    {
        CancellationTokenSource token;
        DBManager manager;
        ConcurrentDictionary<long, Task> runningTasks = new ConcurrentDictionary<long, Task>();
        public BenJobServer(CancellationTokenSource token)
        {
            manager = DBManager.GetInstance("conn");
            manager.PushNewServer("conn", ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            this.token = token;
        }

        public void Start()
        {
            var allJobs = manager.JobDefined_GetAll();
            allJobs.ForEach(item =>
            {
                if (runningTasks.ContainsKey(item.JDID))
                {
                    return;
                }
                BeginTask(item);
            });
        }




        private void BeginTask(JobDefined item)
        {
            runningTasks.TryAdd(item.JDID, Task.Run(() =>
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                switch (item.ExecutionModeEnum)
                {
                    case ExecutionModeEnum.Alternate:
                        var alternateSettings = item.ExecutionSettings.Json_GetObject<AlternateJobSettings>();
                        Task.Delay(new TimeSpan(alternateSettings.IntervalHour, alternateSettings.IntervalMinute, alternateSettings.IntervalSeconds)).
                        ContinueWith((oldTask) =>
                        {
                            BeginTask(item);
                        }, token.Token);
                        break;
                    case ExecutionModeEnum.FixedTime:
                        var fixedTimeSettings = item.ExecutionSettings.Json_GetObject<FixedTimeSettings>();
                        var targetTime = DateTime.Parse(($"{DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")} {fixedTimeSettings.TimeInDay.ToString("HH:mm:ss")}"));
                        Task.Delay((targetTime - DateTime.Now)).
                           ContinueWith((oldTask) =>
                           {
                               BeginTask(item);
                           }, token.Token);
                        break;
                    case ExecutionModeEnum.Once:
                    default:
                        runningTasks.TryRemove(item.JDID, out Task t);
                        break;
                }

                if (token.IsCancellationRequested)
                {
                    return;
                }
                var cacheKey = $"BenJob_{item.JDID}";
                item.SaveToCache(cacheKey);
                Process.Start(ConfigurationManager.AppSettings["ClientPath"], cacheKey);



            }, token.Token));
        }
    }
}

