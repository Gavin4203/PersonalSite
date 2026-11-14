namespace Models
{
    public class ProjectModels
    {
        public int DBId { get; private set; }
        public string Title { get; private set; }
        public string ProjDesc { get; private set; }
        public string[] TechStack { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly? EndDate { get; private set; }
        public string Icon { get; private set; }
        public int Position { get; private set; }

        public ProjectModels(int dbId, string title, string projDesc, string[] techStack, DateOnly start, string icon, int position, DateOnly? end = null)
        {
            DBId = dbId;
            Title = title;
            ProjDesc = projDesc;
            TechStack = techStack;
            StartDate = start;
            EndDate = end;
            Icon = icon;
            Position = position;
        }
    }
}
