using SynonymSearchApp_ApplicationCore.IServices;
using SynonymSearchApp_Domain.Models;

namespace SynonymSearchApp_ApplicationCore.Services
{
    public class SynonymSearchService : ISynonymSearchService
    {
        private static readonly Dictionary<string,HashSet<string>> SynonymData = [];
        private static void ExpandHashsets(string existingKey, string newKey)
        {
            var modifyLists = new HashSet<string>(SynonymData[existingKey]);

            foreach (var item in modifyLists)
                SynonymData[item].Add(newKey);

            SynonymData.Add(newKey, modifyLists);
            SynonymData[existingKey].Add(newKey);
            SynonymData[newKey].Add(existingKey);
        }
        public Response AddSynonym(Synonym request)
        {
            if (string.IsNullOrEmpty(request.Key) || string.IsNullOrEmpty(request.Value))
            {
                throw new ArgumentException("Values cannot be empty.");
            }
            else if (request.Key == request.Value)
            {
                throw new ArgumentException("Values cannot be equal.");
            }
            if (SynonymData.TryGetValue(request.Key, out HashSet<string>? keyHashset) && SynonymData.TryGetValue(request.Value, out HashSet<string>? valueHashset))
            {
                //connect two sets case
                var unionHashset = new HashSet<string>(keyHashset.Concat(valueHashset)){ request.Key, request.Value };

                foreach (var item in unionHashset)
                    SynonymData[item].UnionWith(unionHashset.Where(x => x != item));
            }
            else if (SynonymData.ContainsKey(request.Key) && !SynonymData.ContainsKey(request.Value))
            {
                //contains key does not contain value case
                ExpandHashsets(request.Key, request.Value);
            }
            else if (SynonymData.ContainsKey(request.Value) && !SynonymData.ContainsKey(request.Key))
            {
                //contains value does not contain key case
                ExpandHashsets(request.Value, request.Key);
            }
            else
            {
                //completely new sets case
                SynonymData.Add(request.Key, new HashSet<string> { request.Value });
                SynonymData.Add(request.Value, new HashSet<string> { request.Key });
            }

            return new Response { Message = "Synonym successfully added!" };
        }
        public HashSet<string> GetSynonymList(string key)
        {
            if (SynonymData.TryGetValue(key, out HashSet<string>? value))
                return value;
            
            return []; // Return [] if dictionary does not contain the key
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
