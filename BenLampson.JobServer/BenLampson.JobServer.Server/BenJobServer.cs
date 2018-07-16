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
    /// <summary>
    /// Job server class 
    /// </summary>
    public class BenJobServer
    {
        /// <summary>
        /// use to control all task's cancellation operation
        /// </summary>
        CancellationTokenSource token;
        /// <summary>
        /// use to get data from database
        /// </summary>
        DBManager manager;
        /// <summary>
        /// this dictionary save every running task.
        /// </summary>
        ConcurrentDictionary<long, Task> runningTasks = new ConcurrentDictionary<long, Task>();
        /// <summary>
        /// construct method
        /// </summary>
        /// <param name="token"></param>
        public BenJobServer(CancellationTokenSource token)
        {
            manager = DBManager.GetInstance("conn");
            manager.PushNewServer("conn", ConfigurationManager.ConnectionStrings["conn"].ConnectionString);
            this.token = token;
        }
        /// <summary>
        /// start service
        /// </summary>
        public void Start()
        {
            var allJobs = manager.JobDefinition_GetAll();
            allJobs.ForEach(item =>
            {
                if (runningTasks.ContainsKey(item.JDID))
                {
                    return;
                }
                BeginTask(item);
            });
        }



        /// <summary>
        /// start the new job from jobdefined info
        /// </summary>
        /// <param name="item"></param>
        private void BeginTask(JobDefinition item)
        {
            runningTasks.TryAdd(item.JDID, Task.Run(() =>
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                #region Run job first
                var cacheKey = $"BenJob_{item.JDID}";
                item.SaveToCache(cacheKey);
                Process.Start(ConfigurationManager.AppSettings["ClientPath"], cacheKey);
                #endregion
                switch (item.ExecutionModeEnum)
                {
                    case ExecutionModeEnum.Alternate:
                        var alternateSettings = item.ExecutionSettings.Json_GetObject<AlternateJobSettings>();
                        Task.Delay(alternateSettings.IntervalTimeSpan).
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
            }, token.Token));
        }
    }
}

