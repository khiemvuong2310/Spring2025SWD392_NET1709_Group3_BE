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
    public class VoucherStorageService : IVoucherStorageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public VoucherStorageService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VoucherStorageResponseModel>> GetAllAsync()
        {
            var storages = await _context.VoucherStorages.Include(vs => vs.Voucher).ToListAsync();
            return _mapper.Map<IEnumerable<VoucherStorageResponseModel>>(storages);
        }

        public async Task<VoucherStorageResponseModel> GetByIdAsync(int id)
        {
            var storage = await _context.VoucherStorages.Include(vs => vs.Voucher).FirstOrDefaultAsync(vs => vs.Id == id);
            return _mapper.Map<VoucherStorageResponseModel>(storage);
        }

        public async Task<VoucherStorageResponseModel> CreateAsync(VoucherStorageRequestModel request)
        {
            var storage = _mapper.Map<VoucherStorage>(request);
            _context.VoucherStorages.Add(storage);
            await _context.SaveChangesAsync();
            return _mapper.Map<VoucherStorageResponseModel>(storage);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var storage = await _context.VoucherStorages.FindAsync(id);
            if (storage == null) return false;

            _context.VoucherStorages.Remove(storage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddVoucherToUserAsync(AddVoucherStorageRequestModel request)
        {
            var voucher = await _context.Vouchers.FindAsync(request.VoucherID);
            if (voucher == null) return false;

            var storage = await _context.VoucherStorages.FirstOrDefaultAsync(vs => vs.UserID == request.UserID);
            if (storage == null)
            {
                storage = new VoucherStorage { UserID = request.UserID, Voucher = new List<Voucher>() };
                _context.VoucherStorages.Add(storage);
            }

            storage.Voucher.Add(voucher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
