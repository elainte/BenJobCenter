using BenDotNet.AdoNet.Core;
using BenDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenLampson.JobServer.DataRepository.DB.Table
{
    public class JobDefined : ICanGetParameter
    {
        public long JDID { get; set; } = IDWorker.GetInstance().NextId();
        public string Description { get; set; }
        public string Target { get; set; }
        public int ExecutionMode { get; set; }
        public string ExecutionSettings { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime NextExecutionTime { get; set; } = DateTime.Now;
        public DateTime PreExecutionTime { get; set; } = DateTime.Now;
        public string PreExecutionResult { get; set; }
        public int JobStatus { get; set; }



        public JobStatusEnum JobStatusEnum
        {
            get => (JobStatusEnum)JobStatus;
            set => JobStatus = (int)value;
        }


        public ExecutionModeEnum ExecutionModeEnum
        {
            get => (ExecutionModeEnum)ExecutionMode;


            set => ExecutionMode = (int)value;
        }

    }

    /// <summary>
    /// 作业状态枚举
    /// </summary>
    public enum JobStatusEnum
    {
        /// <summary>
        /// 关闭状态
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// 等待调度
        /// </summary>
        WaittingDispatcher = 1,
        /// <summary>
        /// 调度完成,等待结果
        /// </summary>
        WaittingResult = 2
    }

    /// <summary>
    /// 执行模式枚举
    /// </summary>
    public enum ExecutionModeEnum
    {
        /// <summary>
        /// 只执行一次
        /// </summary>
        Once = 0,
        /// <summary>
        /// 有间隔的执行
        /// </summary>
        Alternate = 1,
        /// <summary>
        /// 固定时间点执行
        /// </summary>
        FixedTime = 2,
    }
}
