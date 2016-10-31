using System.Collections.Generic;

namespace UserSearch.Model
{
    class DbHelper
    {
        UserContext context;

        public DbHelper()
        {
            context = new UserContext();
        }

        public void initDb()
        {
            // Start db empty data - this will be removed later
            
        }

        /// <summary>
        /// Add one user to the database. User Id is not necessary
        /// </summary>
        /// <param name="user"></param>
        /// <returns>1 if user is successfully added, otherwise 0</returns>
        public int addUser(User user)
        {

            return 0;
        }

        /// <summary>
        /// Remove one user out of the database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>1 if user is successfully deleted, otherwise 0</returns>
        public int removeUser(string userId)
        {
            return 0;
        }

        public void removeAllUsers()
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Users]");
        }

        public List<User> getAllUsers()
        {
            return new List<User>();
        }

        public List<User> getUserList(string searchName)
        {
            return new List<User>();
        }
    }
}
