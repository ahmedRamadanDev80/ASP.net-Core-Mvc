using learnmvc.DataAccess.Repository.IRepository;
using learnmvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learnmvc.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly AppDbContext _db;
        public CoverTypeRepository(AppDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(CoverType obj)
        {
            _db.coverTypes.Update(obj);
        }
    }
}
