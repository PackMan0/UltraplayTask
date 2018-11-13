using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AbstractionProvider.Models;

namespace AbstractionProvider.Interfaces
{
    public interface ISportService
    {
        Task<Sport> GetSportDataAsync();
    }
}
