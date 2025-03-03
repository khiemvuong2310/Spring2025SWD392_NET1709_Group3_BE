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

        public async Task<List<VoucherStorageResponseModel>> GetAllVoucherStoragesAsync()
        {
            var voucherStorages = await _context.VoucherStorages.Include(vs => vs.Storages).ToListAsync();
            return _mapper.Map<List<VoucherStorageResponseModel>>(voucherStorages);
        }

        public async Task<VoucherStorageResponseModel> GetVoucherStorageByIdAsync(int id)
        {
            var voucherStorage = await _context.VoucherStorages.Include(vs => vs.Storages).FirstOrDefaultAsync(vs => vs.Id == id);
            return _mapper.Map<VoucherStorageResponseModel>(voucherStorage);
        }

        public async Task<VoucherStorageResponseModel> CreateVoucherStorageAsync(VoucherStorageRequestModel request)
        {
            var voucherStorage = _mapper.Map<VoucherStorage>(request);
            _context.VoucherStorages.Add(voucherStorage);
            await _context.SaveChangesAsync();
            return _mapper.Map<VoucherStorageResponseModel>(voucherStorage);
        }

        public async Task<bool> DeleteVoucherStorageAsync(int id)
        {
            var voucherStorage = await _context.VoucherStorages.FindAsync(id);
            if (voucherStorage == null) return false;

            _context.VoucherStorages.Remove(voucherStorage);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> AddVoucherToUserAsync(AddVoucherStorageRequestModel request)
        //{
        //    var voucher = await _context.Vouchers.FindAsync(request.VoucherID);
        //    if (voucher == null) return false;

        //    var storage = await _context.VoucherStorages.FirstOrDefaultAsync(vs => vs.UserID == request.UserID);
        //    if (storage == null)
        //    {
        //        storage = new VoucherStorage { UserID = request.UserID, Voucher = new List<Voucher>() };
        //        _context.VoucherStorages.Add(storage);
        //    }

        //    storage.Voucher.Add(voucher);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
}
