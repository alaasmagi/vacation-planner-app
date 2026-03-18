using Base.Contracts.DataAccess;
using Base.DataAccess.EF;

namespace DataAccess;

public class DataAccessUow : BaseUow<AppDbContext>, IBaseUow
{
    public DataAccessUow(AppDbContext uowDbContext) : base(uowDbContext)
    {
    }
}