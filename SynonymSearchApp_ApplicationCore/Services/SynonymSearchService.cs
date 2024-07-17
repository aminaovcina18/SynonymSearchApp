using SynonymSearchApp_ApplicationCore.IServices;
using SynonymSearchApp_Domain.Models;
using System.Text.Json.Nodes;

namespace SynonymSearchApp_ApplicationCore.Services
{
    public class SynonymSearchService : ISynonymSearchService
    {
        private static Dictionary<string,HashSet<string>> SynonymData = new Dictionary<string, HashSet<string>>();
        public Response AddSynonym(Synonym request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Key) || string.IsNullOrEmpty(request.Value))
                {
                    throw new ArgumentException("Values cannot be empty.");
                }
                else if (request.Key == request.Value)
                {
                    throw new ArgumentException("Values cannot be equal.");
                }
                else if (SynonymData.ContainsKey(request.Key) && SynonymData.ContainsKey(request.Value))
                {
                    //connect two sets case
                    var unionList = new HashSet<string>(SynonymData[request.Value].Concat(SynonymData[request.Key])){ request.Key, request.Value };

                    foreach (var item in unionList)
                        SynonymData[item].UnionWith(unionList.Where(x => x != item));
                }
                else if (SynonymData.ContainsKey(request.Key) && !SynonymData.ContainsKey(request.Value))
                {
                    //contains key does not contain value case
                    var modifyLists = new HashSet<string>(SynonymData[request.Key]);

                    foreach (var item in modifyLists)
                        SynonymData[item].Add(request.Value);

                    SynonymData.Add(request.Value, modifyLists);
                    SynonymData[request.Key].Add(request.Value);
                    SynonymData[request.Value].Add(request.Key);
                }
                else if (SynonymData.ContainsKey(request.Value) && !SynonymData.ContainsKey(request.Key))
                {
                    //contains value does not contain key case
                    var modifyLists = new HashSet<string>(SynonymData[request.Value]);

                    foreach (var item in modifyLists)
                        SynonymData[item].Add(request.Key);

                    SynonymData.Add(request.Key, modifyLists);
                    SynonymData[request.Key].Add(request.Value);
                    SynonymData[request.Value].Add(request.Key);
                }
                else
                {
                    //completely new sets case
                    SynonymData.Add(request.Key, new HashSet<string> { request.Value });
                    SynonymData.Add(request.Value, new HashSet<string> { request.Key });
                }

                return new Response { Message = "Synonym successfully added!" };
            }
            catch(Exception ex)
            {
                throw;
            }

        }
        public HashSet<string> GetSynonymList(string key)
        {
            if (SynonymData.ContainsKey(key))
                return SynonymData[key];
            
            return new HashSet<string>(); // Return null if dictionary does not contain the key
        }
        public bool DeleteSynonym(Synonym request)
        {
            //would be implemented so that value is removed from values of this particular key
            throw new NotImplementedException();
        }
        public Response UpdateSynonym(Synonym request)
        {
            //would be implemented so that this key is updated with this value
            throw new NotImplementedException();
        }
    }
}
