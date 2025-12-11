using LinguaCorp.API.Models;

namespace LinguaCorp.API.Services
{
    public interface IPhraseService
    {
        public List<Phrase> GetAllPhrases();
        public Phrase GetPhraseById(int id);
        public Phrase CreatePhrase(Phrase phrase);
        public bool UpdatePhrase(int id, Phrase updated);
        public bool DeletePhrase(int id);
        public List<Phrase> GetPhrasesByLanguage(string languageCode);
    }
}
