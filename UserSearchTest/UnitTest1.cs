using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UserSearch.ViewModel;
using UserSearch.Model;

namespace UserSearchTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DbHelper helper = new DbHelper();
            List<> helper.getAllUsers();
        }
    }
}
