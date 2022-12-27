using learnmvc.DataAccess.Repository.IRepository;
using learnmvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learnmvc.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objfromDb = _db.products.FirstOrDefault(p => p.Id == obj.Id);
            if(objfromDb != null)
            {
                objfromDb.Title       = obj.Title;
                objfromDb.ISBN        = obj.ISBN;
                objfromDb.Description = obj.Description;
                objfromDb.Price       = obj.Price;
                objfromDb.ListPrice   = obj.ListPrice;
                objfromDb.Price50     = obj.Price50;
                objfromDb.Price100    = obj.Price100;
                objfromDb.Author      = obj.Author;
                objfromDb.CategoryId  = obj.CategoryId;
                objfromDb.CoverTypeId = obj.CoverTypeId;
                if(objfromDb.ImageUrl != null) { objfromDb.ImageUrl = obj.ImageUrl; }
            }
        }
    }
}
