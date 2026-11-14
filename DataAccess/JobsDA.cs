using MySql.Data.MySqlClient;
using Npgsql;
using Models;
using InterfacesPS;
using System.Data;

namespace DataAccess
{
    public class JobsDA : IJobsDA
    {
        //Tower Connection String
        private string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

        public int Add(JobModel job, List<JobModel> jobs)
        {
            int newPos = GetHighest(jobs) + 1;
            
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO jobs (companyname,positiontitle,posdesc,startdate,enddate,position) VALUES (@CompanyName,@PositionTitle,@PosDesc,@StartDate,@EndDate,@Position)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyName", job.Company);
                        cmd.Parameters.AddWithValue("@PositionTitle", job.PositionTitle);
                        cmd.Parameters.AddWithValue("@PosDesc", job.PosDesc);
                        cmd.Parameters.AddWithValue("@StartDate", job.Start);
                        cmd.Parameters.AddWithValue("@EndDate", job.End != null ? job.End : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Position", newPos);

                        int result = cmd.ExecuteNonQuery();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<JobModel> GetAll()
        {
            try
            {
                List<JobModel> jobs = new List<JobModel>();

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT * FROM jobs";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                jobs.Add(ReadJobs(reader));
                            }
                        }
                    }
                }

                return jobs;
            }
            catch
            {
                return null;
            }
        }

        public int Remove(int id)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"DELETE FROM jobs WHERE jobid = @JobId";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobId", id);

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

        public int Update(JobModel job, List<JobModel> jobs)
        {
            int oldPos = GetOldPostion(job.DBId, jobs);
            
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    
                    int result = -1;
                    
                    string posToNegStr = @"UPDATE jobs SET position = -1 WHERE position = @position;";
                    
                    string queryWithoutPos = @"UPDATE jobs SET companyname = @CompanyName,positiontitle = @PositionTitle, posdesc = @PosDesc, startdate = @StartDate, enddate = @EndDate WHERE jobid = @JobId;";
                    string queryWithPos = 
                        @"UPDATE jobs SET companyname = @CompanyName,positiontitle = @PositionTitle, posdesc = @PosDesc, startdate = @StartDate, enddate = @EndDate, position = @position WHERE jobid = @JobId;";
                    
                    string finalUpdateStr = @"UPDATE jobs SET position = @oldPos WHERE position = -1;";

                    if (job.Position != oldPos)
                    {
                        using var transaction = conn.BeginTransaction();

                        try
                        {
                            using var cmd1 = new NpgsqlCommand(posToNegStr, conn, transaction);
                            cmd1.Parameters.AddWithValue("@position", job.Position);
                            cmd1.ExecuteNonQuery();
                    
                            using (NpgsqlCommand cmd = new NpgsqlCommand(queryWithPos, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CompanyName", job.Company);
                                cmd.Parameters.AddWithValue("@PositionTitle", job.PositionTitle);
                                cmd.Parameters.AddWithValue("@PosDesc", job.PosDesc);
                                cmd.Parameters.AddWithValue("@StartDate", job.Start);
                                cmd.Parameters.AddWithValue("@EndDate", job.End != null ? job.End : DBNull.Value);
                                cmd.Parameters.AddWithValue("@JobId", job.DBId);
                                cmd.Parameters.AddWithValue("@position", job.Position);

                                result = cmd.ExecuteNonQuery();
                            }

                            using var cmd2 = new NpgsqlCommand(finalUpdateStr, conn, transaction);
                            cmd2.Parameters.AddWithValue("@oldPos", oldPos);
                            cmd2.ExecuteNonQuery();

                            transaction.Commit();
                            return result;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                    
                    using (NpgsqlCommand cmd = new NpgsqlCommand(queryWithoutPos, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyName", job.Company);
                        cmd.Parameters.AddWithValue("@PositionTitle", job.PositionTitle);
                        cmd.Parameters.AddWithValue("@PosDesc", job.PosDesc);
                        cmd.Parameters.AddWithValue("@StartDate", job.Start);
                        cmd.Parameters.AddWithValue("@EndDate", job.End != null ? job.End : DBNull.Value);
                        cmd.Parameters.AddWithValue("@JobId", job.DBId);

                        result = cmd.ExecuteNonQuery();
                    }

                    return result;

                }
            }
            catch (Exception err)
            {
                //Console.WriteLine($"Jobs Update Error:\n{err}");
                return -1;
            }
        }

        private JobModel ReadJobs(NpgsqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("JobId"));
            string name = reader.GetString(reader.GetOrdinal("CompanyName"));
            string title = reader.GetString(reader.GetOrdinal("PositionTitle"));
            string desc = reader.GetString(reader.GetOrdinal("PosDesc"));
            DateOnly start = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("StartDate")));
            DateOnly? end = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? null : DateOnly.FromDateTime(reader.GetDateTime("EndDate"));
            int position = reader.GetInt32(reader.GetOrdinal("position"));

            return new JobModel(id,name,title,desc,start,end,position);
        }

        private int GetHighest(List<JobModel> jobs)
        {
            int highest = -1;
            foreach (JobModel job in jobs)
            {
                if (job.Position > highest)
                {
                    highest = job.Position;
                }
            }

            return highest;
        }

        private int GetOldPostion(int id ,List<JobModel> jobs)
        {
            foreach (JobModel job in jobs)
            {
                if (id == job.DBId)
                {
                    return job.Position;
                }
            }

            return -1;
        }
    }
}
