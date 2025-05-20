using HW1.BL;
using System.Data;
using System.Data.SqlClient;

namespace HW1.DAL
{
    public class DBservices
    {
        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the appsettings.json 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        //---------------------------------------------------------------------------------
        // Create the SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }



       ////////////////////////////////////////////////////Methods/////////////////

       ///////////    MOVIES DB    //////////

        //Add movie to movies table
        public bool AddNewMovie(Movies movie)
        {
            SqlConnection con = null;
            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            {"@url", movie.Url},
            {"@primaryTitle", movie.PrimaryTitle},
            {"@description", movie.Description},
            {"@primaryImage", movie.PrimaryImage},
            {"@year", movie.Year},
            {"@releaseDate", movie.ReleaseDate},
            {"@language", movie.Language},
            {"@budget", movie.Budget},
            {"@grossWorldWide", movie.GrossWorldWide},
            {"@genres", movie.Genres},
            {"@isAdult", movie.IsAdult},
            {"@runtimeMinutes", movie.RuntimeMinutes},
            {"@averageRating", movie.AverageRating},
            {"@numVotes", movie.NumVotes},
            {"@priceToRent", movie.PriceToRent},
        };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_InsertMovie_2025", con, paramDic);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }


        public bool DeleteMovie(int id)
        {
            SqlConnection con = null;
            try
            {
                con = connect("myProjDB");
                Dictionary<string, object> paramDic = new Dictionary<string, object>
                {
                    {"@MovieId", id}
                };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_DeleteMovie_2025", con, paramDic);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        public bool UpdateMovie(int id, Movies movie)
        {
            SqlConnection con = null;
            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            
            { "@Url", movie.Url },
            { "@PrimaryTitle", movie.PrimaryTitle },
            { "@Description", movie.Description },
            { "@PrimaryImage", movie.PrimaryImage },
            { "@Year", movie.Year },
            { "@ReleaseDate", movie.ReleaseDate },
            { "@Language", movie.Language },
            { "@Budget", movie.Budget },
            { "@GrossWorldWide", movie.GrossWorldWide },
            { "@Genres", movie.Genres },
            { "@IsAdult", movie.IsAdult },
            { "@RuntimeMinutes", movie.RuntimeMinutes },
            { "@AverageRating", movie.AverageRating },
            { "@NumVotes", movie.NumVotes },
            { "@PriceToRent", movie.PriceToRent },
           
        };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_UpdateMovie_2025", con, paramDic);
                 cmd.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        //Add all movies from JSON 
        public int InsertMoviesList(List<Movies> moviesList)
        {
            int counter = 0;
            SqlConnection con = null;

            try
            {
                con = connect("myProjDB");

                foreach (Movies movie in moviesList)
                {
                    SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_InsertMoviesList_2025", con, new Dictionary<string, object>
            {
                { "@url", movie.Url },
                { "@primaryTitle", movie.PrimaryTitle },
                { "@description", movie.Description },
                { "@primaryImage", movie.PrimaryImage },
                { "@year", movie.Year },
                { "@releaseDate", movie.ReleaseDate },
                { "@language", movie.Language },
                { "@budget", movie.Budget },
                { "@grossWorldWide", movie.GrossWorldWide },
                { "@genres", movie.Genres },
                { "@isAdult", movie.IsAdult },
                { "@runtimeMinutes", movie.RuntimeMinutes },
                { "@averageRating", movie.AverageRating },
                { "@numVotes", movie.NumVotes }
            });

                    cmd.ExecuteNonQuery();
                    counter++;
                }
            }
            finally
            {
                if (con != null) con.Close();
            }

            return counter;
        }

        public bool RentMovie(RentRequest rentObj)
        {
            SqlConnection con = null;
            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            { "@UserId", rentObj.UserId },
            { "@MovieId", rentObj.MovieId },
            { "@RentStart", rentObj.RentStart },
            { "@RentEnd", rentObj.RentEnd },
            { "@TotalPrice", rentObj.TotalPrice }
        };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_RentMovie_2025", con, paramDic);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }


        public List<Movies> GetCurrentlyRentedMoviesByUser(int userId)
        {
            SqlConnection con = null;
            List<Movies> rentedMovies = new List<Movies>();

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            { "@UserId", userId }
        };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_GetCurrentlyRentedMoviesByUser_2025", con, paramDic);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Movies movie = new Movies
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        PrimaryTitle = reader["PrimaryTitle"].ToString(),
                        Description = reader["Description"].ToString(),
                        PrimaryImage = reader["PrimaryImage"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                        Language = reader["Language"].ToString(),
                        Budget = Convert.ToDouble(reader["Budget"]),
                        GrossWorldWide = Convert.ToDouble(reader["GrossWorldWide"]),
                        Genres = reader["Genres"].ToString(),
                        IsAdult = Convert.ToBoolean(reader["IsAdult"]),
                        RuntimeMinutes = Convert.ToInt32(reader["RuntimeMinutes"]),
                        AverageRating = Convert.ToSingle(reader["AverageRating"]),
                        NumVotes = Convert.ToInt32(reader["NumVotes"]),
                        PriceToRent = Convert.ToInt32(reader["PriceToRent"])

                    };

                    rentedMovies.Add(movie);
                }

                reader.Close();
                return rentedMovies;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }


        public List<Movies> GetMoviesByFilters(string title, DateTime? startDate, DateTime? endDate, int page, int pageSize, out int totalCount)
        {
            SqlConnection con = null;
            List<Movies> moviesList = new List<Movies>();
            totalCount = 0;

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>
                {
                    {"@Title", title ?? ""},
                    {"@StartDate", startDate ?? (object)DBNull.Value},
                    {"@EndDate", endDate ?? (object)DBNull.Value},
                    {"@PageNumber", page},
                    {"@PageSize", pageSize}
                };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_GetMoviesByFilters_2025", con, paramDic);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    totalCount = Convert.ToInt32(reader["TotalCount"]);

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        Movies movie = new Movies
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Url = reader["Url"].ToString(),
                            PrimaryTitle = reader["PrimaryTitle"].ToString(),
                            Description = reader["Description"].ToString(),
                            PrimaryImage = reader["PrimaryImage"].ToString(),
                            Year = Convert.ToInt32(reader["Year"]),
                            ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                            Language = reader["Language"].ToString(),
                            Budget = Convert.ToDouble(reader["Budget"]),
                            GrossWorldWide = Convert.ToDouble(reader["GrossWorldWide"]),
                            Genres = reader["Genres"].ToString(),
                            IsAdult = Convert.ToBoolean(reader["IsAdult"]),
                            RuntimeMinutes = Convert.ToInt32(reader["RuntimeMinutes"]),
                            AverageRating = Convert.ToSingle(reader["AverageRating"]),
                            NumVotes = Convert.ToInt32(reader["NumVotes"]),
                            PriceToRent = Convert.ToInt32(reader["PriceToRent"]),
                        };
                        moviesList.Add(movie);
                    }
                }

                reader.Close();
                return moviesList;
            }
            catch
            {
                totalCount = 0;
                return new List<Movies>();
            }
            finally
            {
                if (con != null) con.Close();
            }
        }
        public bool TransferRentedMovie(int fromUserId, int toUserId, int movieId)
        {
            SqlConnection con = null;
            try
            {
                con = connect("myProjDB");
                Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            { "@FromUserId", fromUserId },
            { "@ToUserId", toUserId },
            { "@MovieId", movieId }
        };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_TransferRentedMovie_2025", con, paramDic);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }






        ///////    USERS DB    ////////


        public bool AddNewUser(Users user)
        {
            SqlConnection con = null;
            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>
                {
                    {"@Name", user.Name},
                    {"@Email", user.Email},
                    {"@Password", user.Password},
                    {"@Active", user.Active}
                };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_InsertUser_2025", con, paramDic);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        public Users Login(string email, string hashedPassword)
        {
            SqlConnection con = null;

            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            { "@Email", email },
            { "@Password", (object)hashedPassword ?? DBNull.Value }
        };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_LoginUser_2025", con, paramDic);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Users user = new Users
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Active = Convert.ToBoolean(reader["Active"])
                    };

                    reader.Close();
                    return user;
                }

                reader.Close();
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }


        public bool DeleteUser(int id)
        {
            SqlConnection con = null;
            try
            {
                con = connect("myProjDB");
                Dictionary<string, object> paramDic = new Dictionary<string, object>
                {
                    {"@UserId", id}
                };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_DeleteUser_2025", con, paramDic);
                cmd.ExecuteNonQuery(); 
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        public bool UpdateUser(int id,Users user)
        {
            SqlConnection con = null;
            try
            {
                con = connect("myProjDB");

                Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            { "@Id", id },
            { "@Name", user.Name },
            { "@Email", user.Email },
            { "@Password", user.Password },
            { "@Active", user.Active }
        };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_UpdateUser_2025", con, paramDic);
                cmd.ExecuteNonQuery(); 
                return true; 
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        public List<Users> GetAllActiveUsers(int currentUserId)
        {
            SqlConnection con = null;
            List<Users> userList = new List<Users>();

            try
            {
                con = connect("myProjDB");
                Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            { "@CurrentUserId", currentUserId }
        };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_GetAllActiveUsers_2025", con, paramDic);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Users user = new Users
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString()
                    };

                    userList.Add(user);
                }

                reader.Close();
                return userList;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        public bool UpdateActiveStatus(int userId, bool active)
        {
            SqlConnection con = null;
            try
            {
                con = connect("myProjDB");
                Dictionary<string, object> paramDic = new Dictionary<string, object>
        {
            { "@UserId", userId }
        };

                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_UpdateUserActiveStatus_2025", con, paramDic);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        public List<Users> GetAllUsers()
        {
            SqlConnection con = null;
            List<Users> usersList = new List<Users>();

            try
            {
                con = connect("myProjDB");
                SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("sp_GetAllUsers_2025", con, null);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Users user = new Users
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Active = Convert.ToBoolean(reader["Active"])
                    };

                    usersList.Add(user);
                }

                reader.Close();
                return usersList;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }


    }
}
