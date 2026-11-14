using GavinHsSecurity;
using InterfacesPS;
using Models;

namespace PersonalGUI.Server.Models
{
    public class JobViewModel : ISanitize
    {
        public int DBId { get; set; }
        private string _company;
        public string Company { get => _company; set => _company = Sanitize(value); }
        private string _title;
        public string PositionTitle { get => _title; set => _title = Sanitize(value); }
        private string _desc;
        public string PosDesc { get => _desc; set => _desc = Sanitize(value); }
        public DateOnly Start { get; set; }
        public DateOnly? End { get; set; }
        public int Postion { get; set; }

        public JobModel ToJobModel()
        {
            return new JobModel(DBId,Company,PositionTitle,PosDesc,Start,End,Postion);
        }

        public string Sanitize(string input)
        {
            return Sanitization.StripHtml(input);
        }

    }
}
