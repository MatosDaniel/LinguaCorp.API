using LinguaCorp.API.Models;

namespace LinguaCorp.API.Services
{
    public class PhraseService : IPhraseService
    {
        // Used in-memory storage for simplicity (should be replaced by a database ideally)
        private readonly List<Phrase> _phrases;

        public PhraseService()
        {
            // Initialize sample data
            _phrases =
            [
                new Phrase { Id = 1, OriginalText = "Hello", Language = "en", TranslatedText = "Olá" },
                new Phrase { Id = 2, OriginalText = "Thank you", Language = "en", TranslatedText = "Obrigado" },
                new Phrase { Id = 3, OriginalText = "Good morning", Language = "en", TranslatedText = "Bom dia" }
            ];
        }


        // Retrieve all phrases
        public List<Phrase> GetAllPhrases()
        {
            // Simulate a possible data access issue
            if (_phrases == null)
            {
                throw new InvalidOperationException("Phrase data could not be retrieved.");
            }
            return _phrases;
        }



        // Retrieve a phrase by ID
        public Phrase GetPhraseById(int id)
        {
            var phrase = _phrases.FirstOrDefault(p => p.Id == id);

            if (phrase == null)
            {
                throw new KeyNotFoundException($"Phrase with ID {id} not found.");
            }

            return phrase;
        }


        // Adds a new phrase
        public Phrase CreatePhrase(Phrase phrase)
        {
            // Assign a new unique ID to the phrase
            phrase.Id = _phrases.Any() ? _phrases.Max(p => p.Id) + 1 : 1;

            _phrases.Add(phrase);

            return phrase;
        }

        // Replaces an existing phrase
        public bool UpdatePhrase(int id, Phrase updated)
        {
            var existingPhrase = GetPhraseById(id);

            // If phrase not found, return false
            if (existingPhrase == null)
            {
                return false;
            }

            // Update fields of the existing phrase
            existingPhrase.OriginalText = updated.OriginalText;
            existingPhrase.Language = updated.Language;
            existingPhrase.TranslatedText = updated.TranslatedText;

            return true;
        }

        //Removes a Phrase
        public bool DeletePhrase(int id)
        {
            var phrase = GetPhraseById(id);
            if (phrase == null)
            {
                return false;
            }

            _phrases.Remove(phrase);
            return true;
        }
    }
}

