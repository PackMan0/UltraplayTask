using System;

namespace AbstractionProvider.Configurations
{
    public class BusinessConfig
    {
        public string SportDataUrl { get; set; }
        
        public int RefreshDataIntervalInSeconds { get; set; }

        public DateTime GetMatchesIntervalStart
        {
            get
            {
                return DateTime.UtcNow;
            }
            set { }
        }

        public int GetEventIntervalInSeconds { get; set; }
        
        public string SportCacheKey { get; set; }

        public int SportCahcheExperationTimeInSeconds { get; set; }
        
        public int UpdateSportDataIntervalInSeconds { get; set; }
    }
}
