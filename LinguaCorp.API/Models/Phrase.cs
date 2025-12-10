using System.ComponentModel.DataAnnotations;

namespace LinguaCorp.API.Models
{
    /// <summary>
    /// Represents a phrase and its translation between languages.
    /// </summary>
    public class Phrase
    {
        /// <summary>Unique identifier for the phrase.</summary>
        public int Id { get; set; }

        /// <summary>Original text of the phrase.</summary>
        [Required(ErrorMessage = "OriginalText is required.")]
        public string OriginalText { get; set; }

        /// <summary>ISO language code of the phrase.</summary>
        [Required(ErrorMessage = "Language is required.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Language must be a 2-letter ISO code.")]
        public string Language { get; set; }

        /// <summary>Translated text for the phrase.</summary>
        public string TranslatedText { get; set; }
    }

}
