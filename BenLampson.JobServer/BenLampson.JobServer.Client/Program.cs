using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenDotNet.AdoNet.Core;
using BenDotNet.Core;
using BenLampson.JobServer.DataRepository.DB.Table;
using BenLampson.JobServer.Interface;
using BenLampson.JobServer.DataRepository.DB.Operation;
using BenLampson.JobServer.DataRepository.Model;

namespace BenLampson.JobServer.Client
{
    /// <summary>
    /// the job client, use to hosted the target dll.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return;
            }

            var jobInfo = args[0].GetFromCache<JobDefinition>();
            if (jobInfo == null)
            {
                return;
            }
            jobInfo.PreExecutionTime = DateTime.Now;
            try
            {
                var assemblyInfo = jobInfo.Target.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                var targetObj = (IJobClient)Activator.CreateInstance(Assembly.LoadFile(Path.GetFullPath($"./{assemblyInfo[0]}.dll")).GetType(assemblyInfo[1]));
                targetObj.Execute();
                switch (jobInfo.ExecutionModeEnum)
                {
                    case ExecutionModeEnum.Alternate:
                        var alternateSettings = jobInfo.ExecutionSettings.Json_GetObject<AlternateJobSettings>();
                        jobInfo.NextExecutionTime = DateTime.Now.Add(alternateSettings.IntervalTimeSpan);
                        break;
                    case ExecutionModeEnum.FixedTime:
                        jobInfo.NextExecutionTime = DateTime.Now.AddDays(1);
                        break;
                    case ExecutionModeEnum.Once:
                    default:
                        break;
                }
                jobInfo.SaveToCache(args[0]);
            }
            catch (Exception ex)
            {
                jobInfo.PreExecutionResult = ex.Message;
                jobInfo.JobStatusEnum = JobStatusEnum.Disabled;
                jobInfo.SaveToCache(args[0]);
            }
            DBManager.GetInstance("conn").JobDefinition_Executed(jobInfo.JDID,
                IDWorker.GetInstance().NextId(),
                LTypeEnum.Normal,
                jobInfo.PreExecutionResult,
                jobInfo.JobStatusEnum,
                jobInfo.NextExecutionTime);
        }
    }
}
