using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Mission11.API.Data
{
    public class Movie
    {
        [Key]
        public string show_id { get; set; }
        public string? type { get; set; }
        public string? title { get; set; }
        public string? director { get; set; }
        public string? cast { get; set; }
        public string? country { get; set; }
        public int release_year { get; set; }
        public string? rating { get; set; }
        public string? duration { get; set; }
        public string? description { get; set; }

        public string? Action { get; set; }
        public string? Adventure { get; set; }

        [Column("Anime Series International TV Shows")]
        public string? AnimeSeriesInternationalTVShows { get; set; }

        [Column("British TV Shows Docuseries International TV Shows")]
        public string? BritishDocuseriesInternationalTVShows { get; set; }

        public string? Children { get; set; }
        public string? Comedies { get; set; }

        [Column("Comedies Dramas International Movies")]
        public string? ComediesDramasInternationalMovies { get; set; }

        [Column("Comedies International Movies")]
        public string? ComediesInternationalMovies { get; set; }

        [Column("Comedies Romantic Movies")]
        public string? ComediesRomanticMovies { get; set; }

        [Column("Crime TV Shows Docuseries")]
        public string? CrimeTVShowsDocuseries { get; set; }

        public string? Documentaries { get; set; }

        [Column("Documentaries International Movies")]
        public string? DocumentariesInternationalMovies { get; set; }

        public string? Docuseries { get; set; }
        public string? Dramas { get; set; }

        [Column("Dramas International Movies")]
        public string? DramasInternationalMovies { get; set; }

        [Column("Dramas Romantic Movies")]
        public string? DramasRomanticMovies { get; set; }

        [Column("Family Movies")]
        public string? FamilyMovies { get; set; }

        public string? Fantasy { get; set; }

        [Column("Horror Movies")]
        public string? HorrorMovies { get; set; }

        [Column("International Movies Thrillers")]
        public string? InternationalMoviesThrillers { get; set; }

        [Column("International TV Shows Romantic TV Shows TV Dramas")]
        public string? IntlTVShowsRomanticTVDramas { get; set; }

        [Column("Kids' TV")]
        public string? KidsTV { get; set; }

        [Column("Language TV Shows")]
        public string? LanguageTVShows { get; set; }

        public string? Musicals { get; set; }

        [Column("Nature TV")]
        public string? NatureTV { get; set; }

        [Column("Reality TV")]
        public string? RealityTV { get; set; }

        public string? Spirituality { get; set; }

        [Column("TV Action")]
        public string? TVAction { get; set; }

        [Column("TV Comedies")]
        public string? TVComedies { get; set; }

        [Column("TV Dramas")]
        public string? TVDramas { get; set; }

        [Column("Talk Shows TV Comedies")]
        public string? TalkShowsTVComedies { get; set; }

        public string? Thrillers { get; set; }
    }
}
