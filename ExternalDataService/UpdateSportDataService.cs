using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractionProvider.Interfaces.Services;
using AbstractionProvider.Models;
using Microsoft.AspNetCore.SignalR;

namespace ExternalDataService
{
    public class UpdateSportDataHub : Hub, IUpdateSportDataService
    {
        private readonly IHubContext<UpdateSportDataHub> _hubContext;

        public UpdateSportDataHub(IHubContext<UpdateSportDataHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public async Task DeleteSportData(Sport oldSporData, Sport newSportData)
        {
            var oldMatchIDs = oldSporData.Events.Select(ev => ev.Matches.Select(m => m.EventExtarnalID));
            var newMatchIDs = newSportData.Events.Select(ev => ev.Matches.Select(m => m.EventExtarnalID));

            var matchIDsToDelete = oldMatchIDs.Where(id => newMatchIDs.Contains(id) == false);

            if (this._hubContext?.Clients != null)
            {
                await this._hubContext.Clients.All.SendAsync("DeleteMatches", matchIDsToDelete.ToArray());
            }
        }
    }
}
