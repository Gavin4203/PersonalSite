using InterfacesPS;
using Microsoft.VisualBasic;
using Models;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;

namespace DataAccess
{
    public class ProjectsDA : IProjectDA
    {
        //Tower Connection String
        private string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

        public int Add(ProjectModels project, List<ProjectModels> projs)
        {
            int newPos = GetHighest(projs) + 1;
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO projects (title,projdesc,techstack,startdate,enddate,icon,position) VALUES (@Title,@ProjDesc,@TechStack,@StartDate,@EndDate,@Icon,@Position)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", project.Title);
                        cmd.Parameters.AddWithValue("@ProjDesc", project.ProjDesc);
                        cmd.Parameters.AddWithValue("@TechStack", ToCsvString(project.TechStack));
                        cmd.Parameters.AddWithValue("@StartDate", project.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", project.EndDate != null ? project.EndDate : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Icon", project.Icon);
                        cmd.Parameters.AddWithValue("@Position", newPos);

                        int result = cmd.ExecuteNonQuery();

                        return result;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return 0;
        }

        public List<ProjectModels> GetAll()
        {
            try
            {
                List<ProjectModels> projects = new List<ProjectModels>();

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT * FROM projects";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                projects.Add(ReadProjects(reader));
                            }
                        }
                    }
                }

                return projects;
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
                    string query = @"DELETE FROM projects WHERE projectid = @ProjectId";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProjectId", id);

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

        public int Update(ProjectModels project, List<ProjectModels> projs)
        {
            int oldPos = GetOldPostion(project.DBId, projs);
            
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    int result = -1;
                    
                    string posToNegStr = @"UPDATE projects SET position = -1 WHERE position = @position;";
                    
                    string queryWithoutPos = @"UPDATE projects SET title=@Title,projdesc=@ProjDesc,techstack=@TechStack,startdate=@StartDate,enddate=@EndDate,icon=@Icon WHERE projectid = @ProjectId";
                    string queryWithPos = 
                        @"UPDATE projects SET title=@Title,projdesc=@ProjDesc,techstack=@TechStack,startdate=@StartDate,enddate=@EndDate,icon=@Icon,position=@position WHERE projectid = @ProjectId";
                    
                    string finalUpdateStr = @"UPDATE projects SET position = @oldPos WHERE position = -1;";

                    if (oldPos > -1 && project.Position != oldPos)
                    {
                        using var transaction = conn.BeginTransaction();

                        try
                        {
                            using var cmd1 = new NpgsqlCommand(posToNegStr, conn, transaction);
                            cmd1.Parameters.AddWithValue("@Position", project.Position);
                            cmd1.ExecuteNonQuery();

                            using (NpgsqlCommand cmd = new NpgsqlCommand(queryWithPos, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Title", project.Title);
                                cmd.Parameters.AddWithValue("@ProjDesc", project.ProjDesc);
                                cmd.Parameters.AddWithValue("@TechStack", ToCsvString(project.TechStack));
                                cmd.Parameters.AddWithValue("@StartDate", project.StartDate);
                                cmd.Parameters.AddWithValue("@EndDate",
                                    project.EndDate != null ? project.EndDate : DBNull.Value);
                                cmd.Parameters.AddWithValue("@Icon", project.Icon);
                                cmd.Parameters.AddWithValue("@ProjectId", project.DBId);
                                cmd.Parameters.AddWithValue("@Position", project.Position);

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
                        cmd.Parameters.AddWithValue("@Title", project.Title);
                        cmd.Parameters.AddWithValue("@ProjDesc", project.ProjDesc);
                        cmd.Parameters.AddWithValue("@TechStack", ToCsvString(project.TechStack));
                        cmd.Parameters.AddWithValue("@StartDate", project.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", project.EndDate != null ? project.EndDate : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Icon", project.Icon);
                        cmd.Parameters.AddWithValue("@ProjectId", project.DBId);

                        result = cmd.ExecuteNonQuery();
                    }

                    return result;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"Projects Update Error:\n{err}");
                return -1;
            }
        }

        private ProjectModels ReadProjects(NpgsqlDataReader reader)
        {
            int dbId = reader.GetInt32(reader.GetOrdinal("ProjectId"));
            string title = reader.GetString(reader.GetOrdinal("Title"));
            string desc = reader.GetString(reader.GetOrdinal("ProjDesc"));
            string[] techStack = reader.GetString(reader.GetOrdinal("TechStack")).Split(",");
            DateOnly start = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("StartDate")));
            DateOnly? end = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? null : DateOnly.FromDateTime(reader.GetDateTime("EndDate"));
            string icon = reader.GetString(reader.GetOrdinal("Icon"));
            int position = reader.GetInt32(reader.GetOrdinal("position"));

            return new ProjectModels(dbId,title,desc,techStack,start,icon,position,end);
        }

        private string ToCsvString(string[] techStack)
        {
            string tech = "";
            for(int idx = 0; idx < techStack.Length; idx++)
            {
                if(idx + 1 == techStack.Length)
                {
                    tech += $"{techStack[idx]}";
                }
                else
                {
                    tech += $"{techStack[idx]},";
                }
            }

            return tech;
        }
        
        private int GetHighest(List<ProjectModels> projs)
        {
            int highest = -1;
            foreach (ProjectModels proj in projs)
            {
                if (proj.Position > highest)
                {
                    highest = proj.Position;
                }
            }

            return highest;
        }

        private int GetOldPostion(int id ,List<ProjectModels> projs)
        {
            foreach (ProjectModels proj in projs)
            {
                if (id == proj.DBId)
                {
                    return proj.Position;
                }
            }

            return -1;
        }
    }
}
