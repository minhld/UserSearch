using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using UserSearch.Model;
using Microsoft.Win32;
using System.Drawing;
using System.Windows;

namespace UserSearch.ViewModel
{
    public class UIHelper : ObserveObject
    {
        #region Linked Variables

        private DbHelper dbHelper = new DbHelper();

        /// <summary>
        /// list of all in-memory users - for fast querying
        /// </summary>
        private List<User> allUserList = new List<User>();

        /// <summary>
        /// 
        /// </summary>
        private readonly ObservableCollection<User> userList = new ObservableCollection<User>();
        public IEnumerable<User> UserList
        {
            get { return userList; }
        }

        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                RaisePropertyChangedEvent("SelectedUser");
            }
        }

        private byte[] userAddPhoto;
        public byte[] UserAddPhoto
        {
            get { return userAddPhoto; }
            set
            {
                userAddPhoto = value;
                RaisePropertyChangedEvent("UserAddPhoto");
            }
        }

        private string userAddFullname;
        public string UserAddFullname
        {
            get { return userAddFullname; }
            set
            {
                userAddFullname = value;
                RaisePropertyChangedEvent("UserAddFullname");
            }
        }

        private string userAddAge;
        public string UserAddAge
        {
            get { return userAddAge; }
            set
            {
                userAddAge = value;
                RaisePropertyChangedEvent("UserAddAge");
            }
        }

        private string userAddAddress;
        public string UserAddAddress
        {
            get { return userAddAddress; }
            set
            {
                userAddAddress = value;
                RaisePropertyChangedEvent("UserAddAddress");
            }
        }

        private string userAddInterests;
        public string UserAddInterests
        {
            get { return userAddInterests; }
            set
            {
                userAddInterests = value;
                RaisePropertyChangedEvent("UserAddInterests");
            }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                RaisePropertyChangedEvent("SearchText");
            }
        }

        System.Windows.Visibility progressVisibility = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility ProgressVisibility
        {
            get { return progressVisibility; }
            set
            {
                progressVisibility = value;
                RaisePropertyChangedEvent("ProgressVisibility");
            }
        }

        #endregion

        #region Commands

        public ICommand ExitCommand
        {
            get { return new DelegateCommand(ConfirmExit); }
        }

        public ICommand LoadAllUserCommand
        {
            get { return new DelegateCommand(LoadAllUsers); }
        }

        public ICommand AddImageCommand
        {
            get { return new DelegateCommand(AddImage); }
        }

        public ICommand WindowLoaded
        {
            get { return new DelegateCommand(WindowIsLoaded); }
        }

        public ICommand AddUserCommand
        {
            get { return new DelegateCommand(AddUser); }
        }

        public ICommand SearchNameCommand
        {
            get { return new DelegateCommand(SearchUser); }
        }

        #endregion

        #region Delegate Functions

        private void AddImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg;*.bmp) | *.jpg;*.bmp";
            
            if (ofd.ShowDialog().Value)
            {
                String imageFile = ofd.FileName;
                Image originalImage = Image.FromFile(imageFile);
                int newSize = Utils.Utils.PHOTO_SIZE;
                Image resizedImage = originalImage.GetThumbnailImage(newSize, newSize, null, IntPtr.Zero);
                UserAddPhoto = Utils.Utils.imageToByteArray(resizedImage);
            }
        }
        
        private void WindowIsLoaded()
        {
            // preload the database, existing user information
            loadDB();
        }

        private void ConfirmExit()
        {
            if (Utils.Utils.showYNDialog("Are you sure you want to quit?") == System.Windows.MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void LoadAllUsers()
        {
            searchUsers("");
        }

        private void AddUser()
        {
            int userAge = 0;
            // precheck fullname, age and address
            if (userAddFullname == null || userAddFullname.Equals(""))
            {
                Utils.Utils.showErrorDialog("Fullname is empty");
                return;
            }
            if (userAddAge == null || userAddAge.Equals("") || !int.TryParse(userAddAge, out userAge))
            {
                Utils.Utils.showErrorDialog("Age is empty or invalid");
                return;
            }
            if (userAddAddress == null || userAddAddress.Equals(""))
            {
                Utils.Utils.showErrorDialog("Address is empty");
                return;
            }
            userAge = Convert.ToInt32(userAddAge);
            User user = new User() { Fullname = userAddFullname, Age = userAge, Address = userAddAddress, Interests = userAddInterests, Photo = userAddPhoto };
            int res = dbHelper.addUser(user);
            if (res == 1)
            {
                Utils.Utils.showInfoDialog("User [" + userAddFullname + "] has been added successfully!");
            }
        }

        private void SearchUser()
        {
            searchUsers(searchText);
        }

        /// <summary>
        /// Load the database, including some premature User rows (if they are available in database)
        /// </summary>
        private void searchUsers(string name)
        {
            ProgressVisibility = System.Windows.Visibility.Visible;
            BackgroundWorker searchUsersLoader = new BackgroundWorker();
            searchUsersLoader.DoWork += SearchUsers_DoWork;
            searchUsersLoader.RunWorkerCompleted += SearchUsers_RunWorkerCompleted;
            searchUsersLoader.RunWorkerAsync(name);
        }

        private void SearchUsers_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // add data to the list-view
            renewCurrentUserList((List<User>)e.Result);
            ProgressVisibility = System.Windows.Visibility.Hidden;
        }

        private void SearchUsers_DoWork(object sender, DoWorkEventArgs e)
        {
            // preload all user into memory for later easy manipulating
            string searchName = e.Argument != null ? e.Argument.ToString() : "";
            List<User> searchedUsers = dbHelper.searchUsers(searchName);
            e.Result = searchedUsers;
        }

        /// <summary>
        /// Load the database, including some premature User rows (if they are available in database)
        /// </summary>
        private void loadDB()
        {
            ProgressVisibility = System.Windows.Visibility.Visible;
            BackgroundWorker dbLoader = new BackgroundWorker();
            dbLoader.DoWork += DbLoader_DoWork;
            dbLoader.RunWorkerCompleted += DbLoader_RunWorkerCompleted;
            dbLoader.RunWorkerAsync();
        }

        private void DbLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // add data to the list-view
            renewCurrentUserList(allUserList);
            ProgressVisibility = System.Windows.Visibility.Hidden;
        }

        private void DbLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            // init database 
            dbHelper.initDb();
            // preload all user into memory for later easy manipulating
            List<User> allUsers = dbHelper.getAllUsers();
            allUserList.AddRange(allUsers);
        }

        /// <summary>
        /// This function will reload the data list of the main list-view
        /// When user types search name, the data list will be updated instantly to display instant search results
        /// </summary>
        /// <param name="newList"></param>
        private void renewCurrentUserList(List<User> newList)
        {
            userList.Clear();
            foreach (User user in newList)
            {
                userList.Add(user);
            }
        }

        #endregion

    }
}
