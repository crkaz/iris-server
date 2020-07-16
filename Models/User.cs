using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace iris_server.Models
{
    public class User
    {
        public enum UserRole { admin, formalcarer, informalcarer, patient };

        // DB fields.
        [Key] // Make primary key via EF convention.
        public string ApiKey { get; set; } // Primary key.
        public string Role { get; set; }
        public virtual ICollection<DbLog> DbLogs { get; set; }
        [Timestamp] // Enable optimistic concurrency measures by timestamping transactions (EF convention).
        public byte[] RowVersion { get; set; }

        public User() { }
    }

    public static class UserDatabaseAccess
    {
        // 1. Create a new user, using a username given as a parameter and creating a new GUID which is saved
        // as a string to the database as the ApiKey.This must return the ApiKey or the User object so that
        // the server can pass the Key back to the client.
        //public static string CreateUser(DatabaseContext ctx, string username)
        //{
        //    try
        //    {
        //        string apiKey = Guid.NewGuid().ToString();
        //        string userRole = Enum.GetName(typeof(User.UserRole), User.UserRole.User);

        //        User user = new User() { ApiKey = apiKey, Role = userRole, UserName = username };

        //        if (ctx.Users.Count() == 0)
        //        {
        //            user.Role = Enum.GetName(typeof(User.UserRole), User.UserRole.Admin);
        //        }
        //        ctx.Users.Add(user);
        //        ctx.SaveChanges();
        //        return apiKey;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Unexpected error: " + e.Message);
        //        return e.Message;
        //    }

        //}

        // 2. Check if a user with a given ApiKey string exists in the database, returning true or false.
        public static bool LookupApiKey(DatabaseContext ctx, string apiKey)
        {
            try
            {
                User user = ctx.Users.Find(apiKey);
                bool userExists = user != null;
                return userExists;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }

            return false;
        }

        // 3. Check if a user with a given ApiKey and UserName exists in the database, returning true or false.
        /// Could be combined with method 2.
        //public static bool LookupUsernameAndApiKey(DatabaseContext ctx, string apiKey, string username)
        //{
        //    try
        //    {
        //        foreach (var user in ctx.Users)
        //        {
        //            if (user.UserName == username && user.ApiKey == apiKey)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Unexpected error: " + e.Message);
        //    }

        //    return false;
        //}

        // 4. Check if a user with a given ApiKey string exists in the database, returning the User object.
        public static User GetUserByApiKey(DatabaseContext ctx, string apiKey)
        {
            try
            {
                User user = ctx.Users.Find(apiKey);
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }

            return null;
        }

        //// 5. Delete a user with a given ApiKey from the database.
        //public static bool DeleteUserByApiKey(DatabaseContext ctx, string apiKey)
        //{
        //    bool success =
        //    AccessDbFacade(() =>
        //    {
        //        User userToDelete = ctx.Users.Find(apiKey);

        //        if (userToDelete != null)
        //        {
        //            ArchiveDatabaseAcess.ArchiveUser(ctx, userToDelete);
        //            ctx.Users.Remove(userToDelete);
        //            ctx.SaveChanges();
        //        }
        //    });

        //    return success;
        //}

        //// 6. Etc…
        //// This is only possible if usernames are unique but not sure how else can check from usercontroller given query in GET.
        //public static bool CheckUsernameExists(DatabaseContext ctx, string username)
        //{
        //    try
        //    {
        //        foreach (var user in ctx.Users)
        //        {
        //            if (user.UserName == username)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Unexpected error: " + e.Message);
        //    }

        //    return false;
        //}

        //public static bool ChangeRole(DatabaseContext ctx, string username, string role)
        //{
        //    bool success = AccessDbFacade(() =>
        //    {
        //        User userToChange = null; // Cannot modify collection in foreach.
        //        foreach (var user in ctx.Users)
        //        {
        //            if (user.UserName == username)
        //            {
        //                userToChange = user;
        //                break;
        //            }
        //        }

        //        userToChange.Role = role;
        //        ctx.SaveChanges();
        //    });

        //    return success;
        //}

        ///// <summary>
        ///// Wraps database modification code in try-catch statements to monitor concurrent changes or other mishaps.
        ///// </summary>
        ///// <param name="action">Database modification to monitor with DbUpdateConcurrencyException</param>
        ///// <returns></returns>
        //public static bool AccessDbFacade(Action action)
        //{
        //    const int RETRIES = 3;

        //    try
        //    {
        //        for (int i = 0; i < RETRIES; i++)
        //        {
        //            try
        //            {
        //                action.Invoke();
        //                return true;
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                Console.WriteLine("Failed to fulfil action: database was modified by somebody else");
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Unexpected error: " + e.Message);
        //    }
        //    return false;
        //}
    }
}
