using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



//namespace WebApplicationtest.Models
//{
//    public static class UserStore
//    {
//        // Database connection object
//        private static ApplicationDbContext db = new ApplicationDbContext();


//        // Register User
//        public static bool Register(string username, string password, string role)
//        {
//            // Check if username already exists in database

//            User existingUser = db.Users
//                                  .FirstOrDefault(u => u.Username == username);


//            if (existingUser != null)
//            {
//                return false;
//            }


//            // Create new user object

//            User user = new User
//            {
//                Username = username,
//                Password = password,
//                Role = role
//            };


//            // Add user into database

//            db.Users.Add(user);


//            // Save changes permanently

//            db.SaveChanges();


//            return true;
//        }



//        // Login User
//        public static User Login(string username, string password)
//        {

//            // Search user from database

//            User user = db.Users
//                          .FirstOrDefault(u =>
//                              u.Username == username &&
//                              u.Password == password
//                          );


//            return user;
//        }
//    }
//}




namespace WebApplicationtest.Models
{

   public static class UserStore
   {       private static List<User> users = new List<User>();
       // Register User
       public static bool Register(string username, string password, string role)
        {
            // Check if username already exists
            if (users.Any(u => u.Username == username))
            {
                return false;
            }

            users.Add(new User
            {
                Username = username,
                Password = password,
                Role = role
           });

            return true;
        }

        // Login User
        public static User Login(string username, string password)
        {
            return users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == password);
        }
   }
}