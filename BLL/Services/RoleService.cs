using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using System;
using System.Linq;

namespace BLL.Services
{
    public interface IRoleService
    {
        Service Create(Role role);
        Service Update(Role role);
        Service Delete(int id);
        IQueryable<RoleModel> Query();
    }

    public class RoleService : Service, IRoleService
    {
        public RoleService(DB db) : base(db)
        {
        }

        // Create a new role
        public Service Create(Role role)
        {
            try
            {
                _db.Roles.Add(role);
                _db.SaveChanges();
                return Success("Role created successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error creating role: {ex.Message}");
            }
        }

        // Update an existing role
        public Service Update(Role role)
        {
            var existingRole = _db.Roles.FirstOrDefault(r => r.Id == role.Id);
            if (existingRole == null)
            {
                return Error("Role not found.");
            }

            try
            {
                existingRole.Name = role.Name;

                _db.SaveChanges();
                return Success("Role updated successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error updating role: {ex.Message}");
            }
        }

        // Delete a role by ID
        public Service Delete(int id)
        {
            var role = _db.Roles.FirstOrDefault(r => r.Id == id);
            if (role == null)
            {
                return Error("Role not found.");
            }

            try
            {
                _db.Roles.Remove(role);
                _db.SaveChanges();
                return Success("Role deleted successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error deleting role: {ex.Message}");
            }
        }

        // Query to get all roles or filter as needed
        public IQueryable<RoleModel> Query()
        {
            return _db.Roles.Select(r => new RoleModel { Record = r });
        }
    }
}