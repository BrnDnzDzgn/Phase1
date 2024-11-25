﻿using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IBlogService
    {
        Service Create(Blog blog);
        Service Update(Blog blog);
        Service Delete(int id);
        IQueryable<BlogModel> Query();
    }

    public class BlogService : Service, IBlogService
    {
        public BlogService(DB db) : base(db)
        {
        }
        public Service Create(Blog blog)
        {

            if (_db.Blogs.Any(u => u.Title.ToLower() == blog.Title.ToLower()))
            {
                return Error("A blow with the same title already exists.");
            }

            try
            {
                _db.Blogs.Add(blog);
                _db.SaveChanges();
                return Success("Blog created successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error creating blog: {ex.Message}");
            }
        }
        public Service Update(Blog blog)
        {
            var existingBlog = _db.Blogs.FirstOrDefault(b => b.Id == blog.Id);
            if (existingBlog == null)
            {
                return Error("Blog not found.");
            }

            if (_db.Blogs.Any(u => u.Title.ToLower() == blog.Title.ToLower()))
            {
                return Error("A blow with the same title already exists.");
            }

            try
            {
                existingBlog.Title = blog.Title;
                existingBlog.Content = blog.Content;
                existingBlog.Rating = blog.Rating;
                existingBlog.PublishDate = blog.PublishDate;
                existingBlog.UserId = blog.UserId;

                _db.SaveChanges();
                return Success("Blog updated successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error updating blog: {ex.Message}");
            }
        }
        public Service Delete(int id)
        {
            var blog = _db.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog == null)
            {
                return Error("Blog not found.");
            }

            try
            {
                _db.Blogs.Remove(blog);
                _db.SaveChanges();
                return Success("Blog deleted successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error deleting blog: {ex.Message}");
            }
        }
        public IQueryable<BlogModel> Query()
        {
            return _db.Blogs
                .Include(c => c.User)
                .Select(b => new BlogModel { Record = b });
        }
    }
}