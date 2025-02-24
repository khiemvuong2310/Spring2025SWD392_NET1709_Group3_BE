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
    public class StorageService : IStorageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StorageService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StorageResponseModel>> GetAllStoragesAsync()
        {
            var storages = await _context.Storages.ToListAsync();
            return _mapper.Map<List<StorageResponseModel>>(storages);
        }

        public async Task<StorageResponseModel> GetStorageByIdAsync(int voucherId, int storageId)
        {
            var storage = await _context.Storages.FindAsync(voucherId, storageId);
            return _mapper.Map<StorageResponseModel>(storage);
        }

        public async Task<StorageResponseModel> CreateStorageAsync(StorageRequestModel request)
        {
            var existingStorage = await _context.Storages.FirstOrDefaultAsync(s => s.VoucherId == request.VoucherId);

            if (existingStorage != null)
            {
                existingStorage.Quantity = (existingStorage.Quantity ?? 1) + 1;
                await _context.SaveChangesAsync();
                return _mapper.Map<StorageResponseModel>(existingStorage);
            }

            var storage = _mapper.Map<Storage>(request);
            storage.Quantity = 1; // Default quantity when first created
            _context.Storages.Add(storage);
            await _context.SaveChangesAsync();
            return _mapper.Map<StorageResponseModel>(storage);
        }

        public async Task<StorageResponseModel> UpdateStorageAsync(int voucherId, int storageId, StorageRequestModel request)
        {
            var storage = await _context.Storages.FindAsync(voucherId, storageId);
            if (storage == null) return null;

            _mapper.Map(request, storage);
            await _context.SaveChangesAsync();
            return _mapper.Map<StorageResponseModel>(storage);
        }

        public async Task<bool> DeleteStorageAsync(int voucherId, int storageId)
        {
            var storage = await _context.Storages.FindAsync(voucherId, storageId);
            if (storage == null) return false;

            _context.Storages.Remove(storage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StorageResponseModel>> GetStoragesByVoucherIdAsync(int voucherId)
        {
            var storages = await _context.Storages.Where(s => s.VoucherId == voucherId).ToListAsync();
            return _mapper.Map<List<StorageResponseModel>>(storages);
        }
    }
}
