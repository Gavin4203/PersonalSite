using DataAccess;
using Microsoft.AspNetCore.Http;
using Models;
using System.Collections.Generic;

namespace GavinHsLogic
{
    public class DataAccessBridge
    {
        private ProjectsDA projectsDA;
        private JobsDA jobsDA;
        private ResumayDA resumayDA;

        private List<ProjectModels> _projects;
        private List<JobModel> _jobs;

        private ResumayModel _resumay;

        public DataAccessBridge()
        {
            projectsDA = new ProjectsDA();
            jobsDA = new JobsDA();
            resumayDA = new ResumayDA();

            _projects = new List<ProjectModels>();
            _jobs = new List<JobModel>();

            _resumay = new ResumayModel();
        }

        //Project--------------------------------------------------------
        public async Task<int> AddProject(ProjectModels newProj)
        {
            int result = projectsDA.Add(newProj, _projects);

            if(result > 0)
            {
                GetAllProjects();
            }

            return result;
        }

        public async Task<int> UpdateProject(ProjectModels updateProj)
        {
            int result = projectsDA.Update(updateProj, _projects);

            if(result > 0)
            {
                GetAllProjects();
            }

            return result;
        }

        public async Task<int> DeleteProject(int id)
        {
            int result = projectsDA.Remove(id);

            if(result > 0)
            {
                GetAllProjects();
            }

            return projectsDA.Remove(id);
        }

        public async Task GetAllProjects()
        {
            //check for cached data
            _projects = projectsDA.GetAll();
        }

        public async Task<List<ProjectModels>> GetProjectsList()
        {
            return _projects;
        }

        //Jobs-----------------------------------------------------------
        public async Task<int> AddJobs(JobModel job)
        {
            int result = jobsDA.Add(job, _jobs);

            if(result > 0)
            {
                GetAllJobs();
            }

            return result;
        }

        public async Task<int> UpdateJobs(JobModel job)
        {
            int result = jobsDA.Update(job, _jobs);

            if(result > 0)
            {
                GetAllJobs();
            }

            return result;
        }

        public async Task<int> DeleteJobs(int id)
        {
            int result = jobsDA.Remove(id);

            if (result > 0)
            {
                GetAllJobs();
            }

            return result;
        }

        public async Task GetAllJobs()
        {
            _jobs = jobsDA.GetAll();
        }

        public async Task<List<JobModel>> GetJobsList()
        {
            return _jobs;
        }

        //Resumay--------------------------------------------------
        public async Task GetResumay()
        {
            _resumay = resumayDA.Get();
        }

        public async Task<ResumayModel> ReturnResumay()
        {
            return _resumay;
        }

        public async Task<int> UploadResumay(IFormFile pdf)
        {
            using var memoryStream = new MemoryStream();
            await pdf.CopyToAsync(memoryStream);
            byte[] pdfBytes = memoryStream.ToArray();

            _resumay.ResumayPDF = pdfBytes;

            int result = resumayDA.UploadNewResumay(_resumay);
            
            if(result > 0)
            {
                GetResumay();
            }

            return result;
        }
    }
}
