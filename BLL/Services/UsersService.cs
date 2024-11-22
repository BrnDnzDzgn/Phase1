using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using System;
using System.Linq;

namespace BLL.Services
{
    public interface IUsersService
    {
        Service Create(User user);
        Service Update(User user);
        Service Delete(int id);
        IQueryable<UsersModel> Query();
    }

    public class UsersService : Service, IUsersService
    {
        public UsersService(DB db) : base(db)
        {
        }

        // Create a new user
        public Service Create(User user)
        {
            try
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                return Success("User created successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error creating user: {ex.Message}");
            }
        }

        // Update an existing user
        public Service Update(User user)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                return Error("User not found.");
            }

            try
            {
                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                existingUser.IsActive = user.IsActive;
                existingUser.RoleId = user.RoleId;

                _db.SaveChanges();
                return Success("User updated successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error updating user: {ex.Message}");
            }
        }

        // Delete a user by ID
        public Service Delete(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Error("User not found.");
            }

            try
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                return Success("User deleted successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error deleting user: {ex.Message}");
            }
        }

        // Query to get all users or filter as needed
        public IQueryable<UsersModel> Query()
        {
            return _db.Users.Select(u => new UsersModel { Record = u });
        }
    }
}