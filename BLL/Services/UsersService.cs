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
        public Service Create(User user)
        {
            if (_db.Users.Any(u => u.UserName.ToLower() == user.UserName.ToLower()))
            {
                return Error("A user with the same username already exists.");
            }

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
        public Service Update(User user)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                return Error("User not found.");
            }

            if (_db.Users.Any(u => u.UserName.ToLower() == user.UserName.ToLower()))
            {
                return Error("A user with the same username already exists.");
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
        public IQueryable<UsersModel> Query()
        {
            return _db.Users.Select(u => new UsersModel { Record = u });
        }
    }
}