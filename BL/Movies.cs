using HW1.DAL;

namespace HW1.BL
{
    public class Movies
    {
        private int id;
        private string url;
        private string primaryTitle;
        private string description;
        private string primaryImage;
        private int year;
        private DateTime releaseDate;
        private string language;
        private double budget;
        private double grossWorldWide;
        private string genres;
        Boolean isAdult;
        private int runtimeMinutes;
        private float averageRating;
        private int numVotes;
        private int priceToRent;
        private int rentalCount;

        public int Id { get => id; set => id = value; }
        public string Url { get => url; set => url = value; }
        public string PrimaryTitle { get => primaryTitle; set => primaryTitle = value; }
        public string Description { get => description; set => description = value; }
        public string PrimaryImage { get => primaryImage; set => primaryImage = value; }
        public int Year { get => year; set => year = value; }
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public string Language { get => language; set => language = value; }
        public double Budget { get => budget; set => budget = value; }
        public double GrossWorldWide { get => grossWorldWide; set => grossWorldWide = value; }
        public string Genres { get => genres; set => genres = value; }
        public bool IsAdult { get => isAdult; set => isAdult = value; }
        public int RuntimeMinutes { get => runtimeMinutes; set => runtimeMinutes = value; }
        public float AverageRating { get => averageRating; set => averageRating = value; }
        public int NumVotes { get => numVotes; set => numVotes = value; }
        public int PriceToRent { get => priceToRent; set => priceToRent = value; }
        public int RentalCount { get => rentalCount;  }

        public Movies(int id,string url, string primaryTitle, string description, string primaryImage,
              int year, DateTime releaseDate, string language, double budget,
              double grossWorldWide, string genres, bool isAdult, int runtimeMinutes,
              float averageRating, int numVotes,int priceToRent)
        {
            Id = id;
            Url = url;
            PrimaryTitle = primaryTitle;
            Description = description;
            PrimaryImage = primaryImage;
            Year = year;
            ReleaseDate = releaseDate;
            Language = language;
            Budget = budget;
            GrossWorldWide = grossWorldWide;
            Genres = genres;
            IsAdult = isAdult;
            RuntimeMinutes = runtimeMinutes;
            AverageRating = averageRating;
            NumVotes = numVotes;
            PriceToRent = priceToRent;
           
        }


        public Movies()
        {
        }

        public bool InsertMovie()
        {
            DBservices dbs = new DBservices();
            return dbs.AddNewMovie(this);
        }

        public bool DeleteMovie(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteMovie(id);
        }

        public bool UpdateMovie(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateMovie(id, this);
        }

        public bool RentMovie(RentRequest rentobj)
        {
            DBservices dbs = new DBservices();
            return dbs.RentMovie(rentobj);
        }

        public List<Movies> GetRentedMovies(int userId)
        {
            DBservices db = new DBservices();
            return db.GetCurrentlyRentedMoviesByUser(userId);
        }


        public List<Movies> GetMoviesFiltered(string title, DateTime? startDate, DateTime? endDate, int page, int pageSize, out int totalCount)
        {
            DBservices db = new DBservices();
            return db.GetMoviesByFilters(title, startDate, endDate, page, pageSize, out totalCount);
        }


        public bool TransferRentedMovie(int fromUserId, int toUserId, int movieId)
        {
            DBservices db = new DBservices();
            return db.TransferRentedMovie(fromUserId, toUserId, movieId);
        }


        //Once//
        public int InsertMoviesList(List<Movies> moviesList)
        {
            DBservices dbs = new DBservices();
            return dbs.InsertMoviesList(moviesList);
        }



    }
}