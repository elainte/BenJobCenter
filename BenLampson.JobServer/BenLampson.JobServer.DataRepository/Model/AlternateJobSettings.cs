using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenLampson.JobServer.DataRepository.Model
{
    /// <summary>
    /// 间隔式Job设置信息
    /// </summary>
    public class AlternateJobSettings
    {
        public int IntervalSeconds { get; set; }
        public int IntervalHour { get; set; }
        public int IntervalMinute { get; set; }
    }
}
