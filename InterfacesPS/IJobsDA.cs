using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfacesPS
{
    public interface IJobsDA
    {
        int Add(JobModel job, List<JobModel> jobs);
        List<JobModel> GetAll();
        int Update(JobModel job, List<JobModel> jobs);
        int Remove(int id);
    }
}
