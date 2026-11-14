using Microsoft.Extensions.Caching.Memory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachePS
{
    public class ProjectCache
    {
        private readonly IMemoryCache _cache;
        private const string _cacheKey = "ProjectData";

        public ProjectCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void LoadProjects(List<ProjectModels> projects)
        {
            _cache.Set(_cacheKey, projects);
        }

        public List<ProjectModels> GetProjects()
        {
            return _cache.Get<List<ProjectModels>>(_cacheKey);
        }

        public bool IsCachEmpty()
        {
            try
            {
                List<ProjectModels> projects = GetProjects();
                
                if(projects.Count() <= 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                //I believe there will be some sort of error if I try to run the try part with a null value
                return true;
            }
        }
    }
}
