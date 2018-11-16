using System.Threading.Tasks;
using AbstractionProvider.Models;

namespace AbstractionProvider.Interfaces.Services
{
    public interface IUpdateSportDataService 
    {
        Task DeleteSportData(Sport oldSporData, Sport newSportData);
    }
}