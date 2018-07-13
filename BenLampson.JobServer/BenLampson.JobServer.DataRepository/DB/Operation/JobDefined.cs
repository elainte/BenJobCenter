using BenLampson.JobServer.DataRepository.DB.Table;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenDotNet.AdoNet.Core;

namespace BenLampson.JobServer.DataRepository.DB.Operation
{
    /// <summary>
    /// job's mysql proc info 
    /// </summary>
    public static class JobDefinedProc
    {
        /// <summary>
        /// prefix info
        /// </summary>
        private const string PreFix = "JobDefined_";
        /// <summary>
        /// get all job in mysql data base
        /// </summary>

        public const string GET_ALL = PreFix + "GetAllJob";
    }

    /// <summary>
    /// job's database operation
    /// </summary>
    public static class JobDefinedDBOperation
    {
        /// <summary>
        /// get all job from mysql database
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static List<JobDefined> JobDefined_GetAll(this DBManager manager)
        {
            var ds = manager.ExecuteDataSet(JobDefinedProc.GET_ALL);
            return ds.GetModelList();
        }
    }

    /// <summary>
    /// jobdefined's class operation
    /// </summary>
    public static class JobDefinedClassOperation
    {
        /// <summary>
        /// get model list from data set
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<JobDefined> GetModelList(this DataSet ds)
        {
            var result = new List<JobDefined>();
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) { return result; }
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                result.Add(item.GetModel());
            }
            return result;
        }

        /// <summary>
        /// create new jobdefined model from data row
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static JobDefined GetModel(this DataRow item)
        {
            return new JobDefined()
            {
                CreateTime = item.GetDatetime("CreateTime") == null ? DateTime.Now : item.GetDatetime("CreateTime").Value,
                Description = item.GetString("Description"),
                ExecutionMode = item.GetInt("ExecutionMode") == null ? 0 : item.GetInt("ExecutionMode").Value,
                ExecutionSettings = item.GetString("ExecutionSettings"),
                JDID = item.GetLong("JDID") == null ? 0 : item.GetLong("JDID").Value,
                JobStatus = item.GetInt("JobStatus") == null ? 0 : item.GetInt("JobStatus").Value,
                NextExecutionTime = item.GetDatetime("NextExecutionTime") == null ? DateTime.Now : item.GetDatetime("NextExecutionTime").Value,
                PreExecutionResult = item.GetString("PreExecutionResult"),
                PreExecutionTime = item.GetDatetime("PreExecutionTime") == null ? DateTime.Now : item.GetDatetime("PreExecutionTime").Value,
                Target = item.GetString("Target")
            };
        }
    }
}
