using System.Collections.Generic;
using System.Threading.Tasks;

namespace Greggs.Products.Api.DataAccess;

public interface IDataAccess<T>
{
    IEnumerable<T> List(int? pageStart, int? pageSize);
    Task<IEnumerable<T>> ListLatestAsync(int? pageStart,
        int? pageSize);
    Task<IEnumerable<T>> ListInEurosAsync(int? pageStart,
        int? pageSize);
}