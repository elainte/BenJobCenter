using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenLampson.JobServer.DataRepository.Model
{
    /// <summary>
    /// Alternate job's settings model
    /// </summary>
    public class AlternateJobSettings
    {
        /// <summary>
        /// Seconds info 
        /// </summary>
        public int IntervalSeconds { get; set; }
        /// <summary>
        /// hour info
        /// </summary>
        public int IntervalHour { get; set; }
        /// <summary>
        /// minute info 
        /// </summary>
        public int IntervalMinute { get; set; }

        /// <summary>
        /// format the data to timeSpan 
        /// </summary>
        public TimeSpan IntervalTimeSpan { get { return new TimeSpan(this.IntervalHour, this.IntervalMinute, this.IntervalSeconds); } set { } }
    }
}
