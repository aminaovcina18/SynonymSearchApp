using SynonymSearchApp_Domain.Models;


namespace SynonymSearchApp_ApplicationCore.IServices
{
    public interface ISynonymSearchService
    {
        Response AddSynonym(Synonym request);
        HashSet<string> GetSynonymList(string key);
        bool DeleteSynonym(Synonym request);
        Response UpdateSynonym(Synonym request);
    }
}
