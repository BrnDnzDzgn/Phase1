#nullable disable

using BLL.DAL;
using System.Collections.Generic;
using System.ComponentModel;

namespace BLL.Models
{
    public class UsersModel
    {
        public User Record { get; set; }

        [DisplayName("User ID")]
        public int UserId => Record.Id;

        [DisplayName("Username")]
        public string UserName => Record.UserName;

        [DisplayName("Password")]
        public string Password => Record.Password;

        [DisplayName("Is Active")]
        public bool IsActive => Record.IsActive;

        [DisplayName("Blogs")]
        public ICollection<BlogModel> Blogs
        {
            get
            {
                var blogModals = new List<BlogModel>();
                if (Record.Blogs != null)
                {
                    foreach (var blog in Record.Blogs)
                    {
                        blogModals.Add(new BlogModel { Record = blog });
                    }
                }
                return blogModals;
            }
        }
    }
}