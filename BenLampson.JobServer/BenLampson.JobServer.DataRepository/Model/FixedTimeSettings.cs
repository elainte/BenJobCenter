using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenLampson.JobServer.DataRepository.Model
{
    /// <summary>
    /// the fixed time task settings model
    /// </summary>
    public class FixedTimeSettings
    {
        /// <summary>
        /// Current time , in this case ,we just need HH:MM data..
        /// </summary>
        public DateTime TimeInDay { get; set; }
    }
}
