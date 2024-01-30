using MovieManager.Data;

namespace MovieManager.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int Genres { get; set; }
        public int Movies { get; set; }
        public int Users { get; set; }
        public int Comments { get; set; }
        public List<Movie> Top3Movies { get; set; }
        public List< AppUser> usersdetail { get; set; }

    }
}
