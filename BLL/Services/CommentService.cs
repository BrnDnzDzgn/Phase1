using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public interface ICommentService
    {
        Service Create(Comment comment);
        Service Update(Comment comment);
        Service Delete(int id);
        IQueryable<CommentModel> Query();
    }

    public class CommentService : Service, ICommentService
    {
        public CommentService(DB db) : base(db)
        {
        }
        public Service Create(Comment comment)
        {
            try
            {
                _db.Comments.Add(comment);
                _db.SaveChanges();
                return Success("Comment created successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error creating comment: {ex.Message}");
            }
        }
        public Service Update(Comment comment)
        {
            var existingComment = _db.Comments.FirstOrDefault(c => c.Id == comment.Id);
            if (existingComment == null)
            {
                return Error("Comment not found.");
            }

            try
            {
                existingComment.Content = comment.Content;
                existingComment.PublishDate = comment.PublishDate;
                existingComment.BlogId = comment.BlogId;
                existingComment.UserId = comment.UserId;

                _db.SaveChanges();
                return Success("Comment updated successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error updating comment: {ex.Message}");
            }
        }
        public Service Delete(int id)
        {
            var comment = _db.Comments.FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return Error("Comment not found.");
            }

            try
            {
                _db.Comments.Remove(comment);
                _db.SaveChanges();
                return Success("Comment deleted successfully.");
            }
            catch (Exception ex)
            {
                return Error($"Error deleting comment: {ex.Message}");
            }
        }
        public IQueryable<CommentModel> Query()
        {
            return _db.Comments
                .Include(c => c.User) // Include User entity
                .Select(c => new CommentModel { Record = c });
        }
    }
}