using System;

namespace AbstractionProvider.Configurations
{
    public interface IBusinessConfig
    {
        string AddNewBetsMethodName { get; set; }
        string AddNewEventsMethodName { get; set; }
        string AddNewMatchesMethodName { get; set; }
        string AddNewOddsMethodName { get; set; }
        string DeleteMatchesMethodName { get; set; }
        int GetEventIntervalInSeconds { get; set; }
        DateTime GetMatchesIntervalStart { get; set; }
        string NewBetCollectionCacheKey { get; set; }
        int NewColectionExperationTimeInSeconds { get; set; }
        string NewEventCollectionCacheKey { get; set; }
        string NewMatchCollectionCacheKey { get; set; }
        string NewOddCollectionCacheKey { get; set; }
        int RefreshDataIntervalInSeconds { get; set; }
        string SportCacheKey { get; set; }
        int SportCahcheExperationTimeInSeconds { get; set; }
        string SportDataUrl { get; set; }
        int UpdateSportDataIntervalInSeconds { get; set; }
    }
}