using BenLampson.JobServer.DataRepository.DB.Table;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenDotNet.AdoNet.Core;
using BenDotNet.Core;

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
        private const string PreFix = "JobDefinition_";
        /// <summary>
        /// get all job in mysql data base
        /// </summary>

        public const string GET_ALL = PreFix + "GetAllJob";
        /// <summary>
        /// get all job in mysql data base
        /// </summary>

        public const string UPDATE_INFO = PreFix + "Executed";
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
        public static List<JobDefinition> JobDefinition_GetAll(this DBManager manager)
        {
            var ds = manager.ExecuteDataSet(JobDefinedProc.GET_ALL);
            return ds.GetModelList();
        }
        /// <summary>
        /// update some job's database info.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool JobDefinition_Executed(this DBManager manager, long jlid, long jdid, LTypeEnum type, string content, JobStatusEnum jobStatus, DateTime nextExecutionTime)
        {
            var parameters = new MySqlParameter[] {
                 new MySqlParameter("_JDID",jdid),
                 new MySqlParameter("_JLID",jlid),
                 new MySqlParameter("_Type", type),
                 new MySqlParameter("_Content",content),
                 new MySqlParameter("_JobStatus", jobStatus),
                 new MySqlParameter("_NextExecutionTime",nextExecutionTime)

            };
            var result = manager.ExecuteNonQuery(JobDefinedProc.UPDATE_INFO, CommandType.StoredProcedure, parameters);
            return result == 1;
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
        public static List<JobDefinition> GetModelList(this DataSet ds)
        {
            var result = new List<JobDefinition>();
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
        public static JobDefinition GetModel(this DataRow item)
        {
            return new JobDefinition()
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
