using AutoMapper;
using DianaShop.Data;
using DianaShop.Data.Entities;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CommentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentResponseModel>> GetAllAsync()
        {
            var comments = await _context.Comments.ToListAsync();
            return _mapper.Map<IEnumerable<CommentResponseModel>>(comments);
        }

        public async Task<CommentResponseModel> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            return comment != null ? _mapper.Map<CommentResponseModel>(comment) : null;
        }

        public async Task<CommentResponseModel> CreateAsync(CommentRequestModel request)
        {
            var comment = _mapper.Map<Comment>(request);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return _mapper.Map<CommentResponseModel>(comment);
        }

        public async Task<bool> UpdateAsync(int id, CommentUpdateRequestModel request)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;

            _mapper.Map(request, comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
