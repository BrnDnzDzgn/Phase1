#nullable disable

using BLL.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BLL.Models
{
    public class BlogModel
    {
        public Blog Record { get; set; }

        [DisplayName("Blog Title")]
        public string Title => Record.Title;

        [DisplayName("Content")]
        public string Content => Record.Content;

        [DisplayName("Rating")]
        public decimal? Rating => Record.Rating;

        [DisplayName("Publish Date")]
        public DateTime PublishDate => Record.PublishDate;

        [DisplayName("Blog ID")]
        public int BlogId => Record.Id;

        [DisplayName("User")]
        public string UserName => Record.User?.UserName; // Assuming User entity has UserName property

        [DisplayName("Tags")]
        public ICollection<string> Tags
        {
            get
            {
                var tags = new List<string>();
                if (Record.BlogTags != null)
                {
                    foreach (var blogTag in Record.BlogTags)
                    {
                        tags.Add(blogTag.Tag?.Name); // Assuming BlogTag has a Tag navigation property with a Name field
                    }
                }
                return tags;
            }
        }
    }
}