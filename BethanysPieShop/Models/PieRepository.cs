
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly BethanysPieShopDbContext _dbContext;

        public PieRepository(BethanysPieShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Pie> AllPies
        {
            get
            {
                return _dbContext.Pies.Include(c => c.Category);
            }
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return _dbContext.Pies.Include(x => x.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        public Pie? GetPieById(int pieId)
        {
            return _dbContext.Pies.Include(x => x.Category).FirstOrDefault(x => x.PieId == pieId);
        }

        public IEnumerable<Pie> SearchPies(string searchQuery)
        {
            return _dbContext.Pies.Include(x => x.Category).Where(x => x.Name == searchQuery);
        }
    }
}
