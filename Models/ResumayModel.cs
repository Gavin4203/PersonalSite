using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ResumayModel
    {
        public int ResumayId { get; set; }
        public string ResumayName { get; set; }
        public string ResumayMimeType { get; set; }
        public byte[] ResumayPDF { get; set; }
        public DateTime ResumayUploadedAt { get; set; }

        public ResumayModel() { }

        public ResumayModel(int resumayId, string resumayName, string resumayMimeType, byte[] resumayPDF, DateTime resumayUploadedAt)
        {
            ResumayId = resumayId;
            ResumayName = resumayName;
            ResumayMimeType = resumayMimeType;
            ResumayPDF = resumayPDF;
            ResumayUploadedAt = resumayUploadedAt;
        }
    }
}
