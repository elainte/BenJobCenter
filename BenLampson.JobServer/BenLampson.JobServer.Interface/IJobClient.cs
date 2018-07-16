using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenLampson.JobServer.Interface
{
    /// <summary>
    /// interface of job client , the job want to running in this system must be realization this interface 
    /// </summary>
    public interface IJobClient
    {
        bool Execute();

    }
}
