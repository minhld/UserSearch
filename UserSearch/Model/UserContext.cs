using System.Data.Entity;

namespace UserSearch.Model
{
    class UserContext : DbContext
    {
        public UserContext() : base("UserDb")
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
