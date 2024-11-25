#nullable disable

using BLL.DAL;
using System;
using System.ComponentModel;

namespace BLL.Models
{
    public class CommentModel
    {
        public Comment Record { get; set; }

        [DisplayName("Comment ID")]
        public int CommentId => Record.Id;

        [DisplayName("Content")]
        public string Content => Record.Content;

        [DisplayName("Publish Date")]
        public DateTime PublishDate => Record.PublishDate;

        [DisplayName("Blog ID")]
        public int? BlogId => Record.BlogId;

        [DisplayName("Blog Title")]
        public string BlogTitle => Record.Blog?.Title;

        [DisplayName("User ID")]
        public int? UserId => Record.UserId;

        [DisplayName("Username")]
        public string UserName => Record.User?.UserName;
    }
}