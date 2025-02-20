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
    public class FormulationTypeService : IFormulationTypeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FormulationTypeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FormulationTypeResponse>> GetAllAsync()
        {
            var entities = await _context.FormulationTypes.Include(ft => ft.Products).ToListAsync();
            return _mapper.Map<IEnumerable<FormulationTypeResponse>>(entities);
        }

        public async Task<FormulationTypeResponse> GetByIdAsync(int id)
        {
            var entity = await _context.FormulationTypes.Include(ft => ft.Products).FirstOrDefaultAsync(ft => ft.Id == id);
            return _mapper.Map<FormulationTypeResponse>(entity);
        }

        public async Task<FormulationTypeResponse> CreateAsync(FormulationTypeRequest request)
        {
            var entity = _mapper.Map<FormulationType>(request);
            _context.FormulationTypes.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<FormulationTypeResponse>(entity);
        }

        public async Task<FormulationTypeResponse> UpdateAsync(int id, FormulationTypeRequest request)
        {
            var entity = await _context.FormulationTypes.FindAsync(id);
            if (entity == null) return null;

            _mapper.Map(request, entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<FormulationTypeResponse>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.FormulationTypes.FindAsync(id);
            if (entity == null) return false;

            _context.FormulationTypes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
