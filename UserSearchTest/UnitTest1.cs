using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserSearch.ViewModel;
using UserSearch.Model;
using System.Collections.Generic;

namespace UserSearchTest
{
    [TestClass]
    public class UnitTest1
    {
        DbHelper helper = new DbHelper();

        [TestMethod]
        public void AddUserWithoutImage()
        {
            User newUser = new User() { Fullname = "John Lennon", Age = 22, Address = "London" };
            int res = helper.addUser(newUser);
            Assert.IsTrue(res == 1);

            newUser = new User() { Fullname = "Paul Cartney", Age = 25, Address = "Liverpool" };
            res = helper.addUser(newUser);
            Assert.IsTrue(res == 1);

        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            List<User> userList = helper.getAllUsers();
            Assert.IsTrue(userList.Count > 0);
        }

        [TestMethod]
        public void SearchUsers()
        {
            // search 1 name
            List<User> userResults = helper.searchUsers("john");
            // should be 1
            // Assert.IsTrue(userResults.Count == 1);


            // two names must be returned
            userResults = helper.searchUsers("n");
            // should be 2
            // Assert.IsTrue(userResults.Count == 2);
        }

        [TestMethod]
        public void RemoveUser()
        {
            List<User> userResults = helper.searchUsers("john");
            int res = helper.removeUser(userResults[0].UserId);
            Assert.IsTrue(res == 1);
        }
        
        [TestMethod]
        public void RemoveAllUsers()
        {
            helper.removeAllUsers();
            List<User> userList = helper.getAllUsers();
            Assert.IsTrue(userList.Count == 0);
        }
    }
}
