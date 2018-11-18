using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace AbstractionProvider.Configurations
{
    public class BusinessConfig
    {
        private DateTime _intervalStart;

        public string SportDataUrl { get; set; }
        
        public int RefreshDataIntervalInSeconds { get; set; }
        
        public DateTime GetMatchesIntervalStart
        {
            get => this._intervalStart;
            set { this._intervalStart = value == new DateTime() ? DateTime.UtcNow : value; }
        }

        public int GetEventIntervalInSeconds { get; set; }
        
        public string SportCacheKey { get; set; }

        public int SportCahcheExperationTimeInSeconds { get; set; }
        
        public int UpdateSportDataIntervalInSeconds { get; set; }

        public string NewEventCollectionCacheKey { get; set; }

        public string NewMatchCollectionCacheKey { get; set; }

        public string NewBetCollectionCacheKey { get; set; }

        public string NewOddCollectionCacheKey { get; set; }

        public int NewColectionExperationTimeInSeconds { get; set; }

        public string DeleteMatchesMethodName { get; set; }

        public string AddNewEventsMethodName { get; set; }

        public string AddNewMatchesMethodName { get; set; }

        public string AddNewBetsMethodName { get; set; }

        public string AddNewOddsMethodName { get; set; }
    }
}
