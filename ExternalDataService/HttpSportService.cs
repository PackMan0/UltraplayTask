using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AbstractionProvider.Interfaces;
using AbstractionProvider.Models;

namespace ExternalDataService
{
    public class HttpSportService : ISportService
    {
        private readonly string _sportDataUrl;
        
        public HttpSportService(string sportDataUrl)
        {
            this._sportDataUrl = sportDataUrl;
        }

        public async Task<Sport> GetSportDataAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(this._sportDataUrl);
                
                if(response.IsSuccessStatusCode)
                {
                    var resultStream = await response.Content.ReadAsStreamAsync();
                    var serializer = new XmlSerializer(typeof(XmlSportsContainer));
                    var result = (XmlSportsContainer)serializer.Deserialize(resultStream);

                    return result.Sport;
                }
            }

            return null;
        }
    }
}
