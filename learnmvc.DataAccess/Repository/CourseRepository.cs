using learnmvc.DataAccess.Repository.IRepository;
using learnmvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learnmvc.DataAccess.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly AppDbContext _db;
        public CourseRepository(AppDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Course obj)
        {
            _db.courses.Update(obj);
        }
    }
}
