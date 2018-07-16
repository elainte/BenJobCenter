using BenDotNet.AdoNet.Core;
using BenDotNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenLampson.JobServer.DataRepository.DB.Table
{
    /// <summary>
    /// Job defined model.
    /// </summary>
    public class JobDefinition : ICanGetParameter
    {
        /// <summary>
        /// job defined's id
        /// </summary>
        public long JDID { get; set; } = IDWorker.GetInstance().NextId();
        /// <summary>
        /// description this job's detail info
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// this property's value is the tasks job class. this class must be realization benlampson.jobserver.interface.iJobClient
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// excution mode, detail in this enum :ExecutionModeEnum
        /// </summary>
        public int ExecutionMode { get; set; }
        /// <summary>
        /// if run this job needs some parameters, please serialize object and save in this property
        /// </summary>
        public string ExecutionSettings { get; set; }
        /// <summary>
        /// the time of this job defined
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// the time of this job next execution time 
        /// </summary>
        public DateTime NextExecutionTime { get; set; } = DateTime.Now;
        /// <summary>
        /// the time of this job previous excution time
        /// </summary>
        public DateTime PreExecutionTime { get; set; } = DateTime.Now;
        /// <summary>
        /// this job's previous excution result
        /// </summary>
        public string PreExecutionResult { get; set; }
        /// <summary>
        /// current status, detail in this enum:JobStatusEnum
        /// </summary>
        public int JobStatus { get; set; }


        /// <summary>
        /// current status's enum
        /// </summary>
        public JobStatusEnum JobStatusEnum
        {
            get => (JobStatusEnum)JobStatus;
            set => JobStatus = (int)value;
        }

        /// <summary>
        /// this job's excution mode enum
        /// </summary>
        public ExecutionModeEnum ExecutionModeEnum
        {
            get => (ExecutionModeEnum)ExecutionMode;


            set => ExecutionMode = (int)value;
        }

    }

    /// <summary>
    /// Status enum
    /// </summary>
    public enum JobStatusEnum
    {
        /// <summary>
        /// closed, maybe this job have some error
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// waitting to dispatcher, means this job can be invoke
        /// </summary>
        WaittingDispatcher = 1,
        /// <summary>
        /// already invoked waitting result
        /// </summary>
        WaittingResult = 2
    }

    /// <summary>
    /// job's execution mode
    /// </summary>
    public enum ExecutionModeEnum
    {
        /// <summary>
        /// only once , where removed when job executioned
        /// </summary>
        Once = 0,
        /// <summary>
        /// the job will restart in some time
        /// </summary>
        Alternate = 1,
        /// <summary>
        /// the job will started in every day's fixed time.
        /// </summary>
        FixedTime = 2,
    }
}
