using System.Threading.Tasks;
using AbstractionProvider.Models;

namespace AbstractionProvider.Interfaces.Services
{
    public interface IExternalSportService
    {
        Task<Sport> GetSportDataAsync();
    }
}
