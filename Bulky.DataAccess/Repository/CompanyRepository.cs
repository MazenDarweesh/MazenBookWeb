using Mazen.DataAccess.Data;
using Mazen.DataAccess.Repository.IRepository;
using Mazen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mazen.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository 
    {
        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Company obj)
        {

            _db.Companys.Update(obj);
        }
    }
}
