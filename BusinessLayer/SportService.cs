using AbstractionProvider.Interfaces.Services;
using AbstractionProvider.Models;
using System;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class SportService : ISportService
    {
        private readonly IExternalSportService _externalSportService;

        public SportService(IExternalSportService externalSportService)
        {
            this._externalSportService = externalSportService;
        }

        public async Task<Sport> GetAllSportDataAsync()
        {
            return await this._externalSportService.GetSportDataAsync();
        }
    }
}
