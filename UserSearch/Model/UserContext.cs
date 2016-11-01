using System.Data.Entity;

namespace UserSearch.Model
{
    class UserContext : DbContext
    {
        public UserContext() : base("name=UserContext")
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
