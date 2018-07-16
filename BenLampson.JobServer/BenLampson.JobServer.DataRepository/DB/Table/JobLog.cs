using BenDotNet.AdoNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenLampson.JobServer.DataRepository.DB.Table
{
    public class JobLog : ICanGetParameter
    {
        /// <summary>
        /// this log's ID
        /// </summary>
        public Int64 JLID { get; set; }
        /// <summary>
        /// this log's type detail in :LTypeEnum
        /// </summary>
        public int LType { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// dependency job definition data id
        /// </summary>
        public Int64 JDID { get; set; }

        /// <summary>
        /// this log's type
        /// </summary>
        public LTypeEnum TypeEnum { get => (LTypeEnum)this.LType; set => this.LType = (int)value; }
    }
    /// <summary>
    /// Job log's type enum
    /// </summary>
    public enum LTypeEnum
    {
        Normal = 0,
        Warning = 1,
        Error = 5,
    }
}
