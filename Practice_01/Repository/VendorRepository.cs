using Microsoft.EntityFrameworkCore;
using Practice_01.Data;
using Practice_01.Models;
using Practice_01.ViewModel;
using System.Numerics;

namespace Practice_01.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ApplicationDbContext _context;
        public VendorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<VendorModel> AddVendorAsync(VendorModel newvendor,Guid UserId)
        {
           
            var vendor = new Vendor
            {
               // UserId = newvendor.UserId,
                UserId=UserId.ToString(),
                WebsiteUrl = newvendor.WebsiteUrl,
                Address = newvendor.Address,
                CityName = newvendor.CityName,
                District = newvendor.District,
                FirmName = newvendor.FirmName,
                TypeOfVendor = newvendor.TypeOfVendor,
            };
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return newvendor;
           
        }

        public async Task<bool> DeleteVendorAsync(Guid vendorId)
        {
                var vendor = await _context.Vendors.FindAsync(vendorId);
                if (vendor == null)
                    return false;

                _context.Vendors.Remove(vendor);
                await _context.SaveChangesAsync();

                return true;
            
        }

        public async Task<IEnumerable<VendorModel>> GetAllVendor()
        {

            var vendors = await _context.Vendors.ToListAsync(); // Retrieve all vendors from the database
            var vendorModels = vendors.Select(vendor => new VendorModel
            {
                VendorId=vendor.Id.ToString(),
                WebsiteUrl = vendor.WebsiteUrl,
                Address = vendor.Address,
                CityName = vendor.CityName,
                District = vendor.District,
                FirmName = vendor.FirmName,
                TypeOfVendor = vendor.TypeOfVendor
            }).ToList();

            return vendorModels;
        }

        public async Task<List<string>> GetAlTypeOfvendor()
        {
            //throw new NotImplementedException();
            try
            {
                    var typeofvendor=await _context.Vendors.
                                Select(x=>x.TypeOfVendor).Distinct().ToListAsync();
                    return typeofvendor;
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<VendorModel> GetVendorById(Guid vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId);
            if(vendor == null)
            {
                return null;
            }
            var vendormodel = new VendorModel
            {
                VendorId=vendor.Id.ToString(),
                //UserId = vendor.UserId,
                WebsiteUrl = vendor.WebsiteUrl,
                CityName = vendor.CityName,
                Address = vendor.Address,
                District = vendor.District,
                FirmName = vendor.FirmName,
                TypeOfVendor = vendor.TypeOfVendor,

            };
            return vendormodel;
        }

        public async Task<VendorModel> GetVendorByUserId(string userId)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(v => v.UserId == userId);
            if (vendor == null)
            {
                return null;
            }
            var vendormodel = new VendorModel
            {
                VendorId = vendor.Id.ToString(),
                //UserId = vendor.UserId,
                WebsiteUrl = vendor.WebsiteUrl,
                CityName = vendor.CityName,
                Address = vendor.Address,
                District = vendor.District,
                FirmName = vendor.FirmName,
                TypeOfVendor = vendor.TypeOfVendor,

            };
            return vendormodel;
        }

        public async Task<VendorModel> UpdateVendorAsync(Guid vendorId, VendorModel Updatevendor)
        {
            var existingVen = await _context.Vendors.FindAsync(vendorId);
            if (existingVen == null)
            {
                return null;
            }
            //existingVen.UserId=Updatevendor.UserId;
            existingVen.WebsiteUrl=Updatevendor.WebsiteUrl;
            existingVen .Address = Updatevendor.Address;
            existingVen.CityName= Updatevendor.CityName;
            existingVen.FirmName= Updatevendor.FirmName;
            existingVen.District= Updatevendor.District;
            existingVen.TypeOfVendor= Updatevendor.TypeOfVendor;

            await _context.SaveChangesAsync();
            return Updatevendor;

        }
    }
}
