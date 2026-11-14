using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class JobModel
    {
        public int DBId { get; private set; }
        public string Company { get; private set; }
        public string PositionTitle { get; private set; }
        public string PosDesc { get; private set; }
        public DateOnly Start { get; private set; }
        public DateOnly? End { get; private set; }
        public int Position { get; private set; }

        public JobModel(int dbId, string company, string positionTitle, string posDesc, DateOnly start, DateOnly? end, int postion)
        {
            DBId = dbId;
            Company = company;
            PositionTitle = positionTitle;
            PosDesc = posDesc;
            Start = start;
            End = end;
            Position = postion;
        }
    }
}
