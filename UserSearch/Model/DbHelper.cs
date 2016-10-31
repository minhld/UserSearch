using System.Collections.Generic;
using System.Linq;

namespace UserSearch.Model
{
    class DbHelper
    {
        /// <summary>
        /// Db context holder
        /// </summary>
        UserContext context;

        public DbHelper()
        {
            context = new UserContext();
        }

        public void initDb()
        {
            // Start db empty data - this will be removed later
            // removeAllUsers();
            // seedData();
        }

        /// <summary>
        /// seed some beginning data 
        /// </summary>
        private void seedData()
        {
            // adding some beginning data
            addUser(new User() { Fullname = "Minh Le", Age = 33, Address = "Logan, USA", Interests = "Rice" });
            addUser(new User() { Fullname = "AnBa Nguyen", Age = 30, Address = "Logan, USA", Interests = "Everything" });
            addUser(new User() { Fullname = "Oanh Lethuy", Age = 36, Address = "Seoul, South Korea", Interests = "Clothes" });
            addUser(new User() { Fullname = "Thuy Lethi", Age = 45, Address = "Seoul, South Korea", Interests = "Houses" });
            addUser(new User() { Fullname = "Bach Le", Age = 1, Address = "5 Aggie, USA", Interests = "Toys" });

        }

        /// <summary>
        /// Add one user to the database. User Id is not necessary
        /// </summary>
        /// <param name="user"></param>
        /// <returns>1 if user is successfully added, otherwise 0</returns>
        public int addUser(User user)
        {
            // generate a new UUID 
            user.UserId = System.Guid.NewGuid().ToString();
            User addedUser = context.Users.Add(user);
            context.SaveChanges();
            return addedUser != null ? 1 : 0;
        }

        /// <summary>
        /// Remove one user out of the database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>1 if user is successfully deleted, otherwise 0</returns>
        public int removeUser(string userId)
        {
            User user = new User() { UserId = userId };
            User rmUser = context.Users.Remove(user);
            context.SaveChanges();
            return rmUser != null ? 1 : 0;
        }

        public void removeAllUsers()
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Users]");
        }

        /// <summary>
        /// Return all users in the database
        /// </summary>
        /// <returns></returns>
        public List<User> getAllUsers()
        {
            var users = from b in context.Users select b;
            List<User> userList = new List<User>();
            foreach (User user in users)
            {
                userList.Add(user);
            }
            return userList;
        }

        /// <summary>
        /// Search for users by name
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns></returns>
        public List<User> searchUsers(string searchName)
        {
            var lowerSearchName = searchName.ToLower();
            var users = from b in context.Users where b.Fullname.ToLower().Contains(lowerSearchName) select b;
            List<User> userList = new List<User>();
            foreach (User user in users)
            {
                userList.Add(user);
            }
            return userList;
        }
    }
}
