#nullable disable

using BLL.DAL;
using System.Collections.Generic;
using System.ComponentModel;

namespace BLL.Models
{
    public class RoleModel
    {
        public Role Record { get; set; }

        [DisplayName("Role ID")]
        public int RoleId => Record.Id;

        [DisplayName("Role Name")]
        public string RoleName => Record.Name;

        [DisplayName("Users")]
        public ICollection<UsersModel> Users
        {
            get
            {
                var usersList = new List<UsersModel>();
                if (Record.Users != null)
                {
                    foreach (var user in Record.Users)
                    {
                        usersList.Add(new UsersModel { Record = user });
                    }
                }
                return usersList;
            }
        }
    }
}