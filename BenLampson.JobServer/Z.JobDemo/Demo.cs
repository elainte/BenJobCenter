using BenLampson.JobServer.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.JobDemo
{
    public class Demo : IJobClient
    {
        public bool Execute()
        {
            using (var fw= File.CreateText($"C://{Guid.NewGuid().ToString()}.txt"))
            {
                fw.Write("啊啊啊啊啊啊啊啊啊啊啊");
                fw.Close();
            }
            return true;
        }
    }
}
