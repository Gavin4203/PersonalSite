using GavinHsSecurity;
using InterfacesPS;
using Models;

namespace PersonalGUI.Server.Models
{
    public class ProjectViewModel : ISanitize
    {
        public int DBId { get; set; }
        private string _title;
        public string Title { get => _title; set => _title = Sanitize(value); }
        private string _projDesc;
        public string ProjDesc { get => _projDesc; set => _projDesc = Sanitize(value); }
        private string _techStack;
        public string TechStack { get => _techStack; set => _techStack = Sanitize(value); }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        private string _icon;
        public string Icon { get => _icon; set => _icon = Sanitize(value); }
        public int Position { get; set; }

        public ProjectModels ToProjModel()
        {
            return new ProjectModels(DBId,Title,ProjDesc,ToStringArr(),StartDate,Icon,Position,EndDate);
        }

        private string[] ToStringArr()
        {
            Sanitize(TechStack);
            return TechStack.Split(',');
        }

        public string Sanitize(string input)
        {
            return Sanitization.StripHtml(input);
        }
    }
}
