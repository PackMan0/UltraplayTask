using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using AbstractionProvider.Interfaces;
using AbstractionProvider.Interfaces.Services;
using AbstractionProvider.Models;

namespace ExternalDataService
{
    public class HttpSportService : IExternalSportService
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
                    var resultStream = await response.Content.ReadAsByteArrayAsync();
                    var serializer = new XmlSerializer(typeof(XmlSportsContainer));
                    using(TextReader reader = new StreamReader(new MemoryStream(resultStream),Encoding.UTF8))
                    {
                        try
                        {
                            var result = (XmlSportsContainer) serializer.Deserialize(reader);

                            return result.Sport;
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }
            }

            return null;
        }
    }
}
