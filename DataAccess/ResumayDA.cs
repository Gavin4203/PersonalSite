using Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ResumayDA
    {
        private string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

        public ResumayModel Get()
        {
            try
            {
                ResumayModel resumay = new ResumayModel();

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT * FROM resumay";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resumay = ReadResumay(reader);
                            }
                        }
                    }
                }

                return resumay;
            }
            catch
            {
                return null;
            }
        }

        public int UploadNewResumay(ResumayModel resumay)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE resumay SET PdfData=@PdfData,UploadedAt=@UploadedAt WHERE ResumeId = @ResumeId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ResumeId", 1);
                        cmd.Parameters.AddWithValue("@PdfData", resumay.ResumayPDF);
                        cmd.Parameters.AddWithValue("@UploadedAt", DateTime.UtcNow);

                        int result = cmd.ExecuteNonQuery();

                        return result;
                    }
                }
            }
            catch
            {
                return -1;
            }
        }

        private ResumayModel ReadResumay(NpgsqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("ResumeId"));
            string name = reader.GetString(reader.GetOrdinal("FileName"));
            string mimeType = reader.GetString(reader.GetOrdinal("MimeType"));
            byte[] pdf = (byte[])reader["PdfData"];
            DateTime uploadedAt = reader.GetDateTime(reader.GetOrdinal("UploadedAt"));
            return new ResumayModel(id,name,mimeType,pdf,uploadedAt);
        }
    }
}
