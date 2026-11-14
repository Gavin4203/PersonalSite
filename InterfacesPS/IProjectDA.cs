using Models;

namespace InterfacesPS
{
    public interface IProjectDA
    {
        int Add(ProjectModels project, List<ProjectModels> projs);
        List<ProjectModels> GetAll();
        int Update(ProjectModels project, List<ProjectModels> projs);
        int Remove(int id);
    }
}
