using AireLogic.LyricsChecker.ConsoleApp.DataSources;
using MetaBrainz.MusicBrainz;
using MetaBrainz.MusicBrainz.Interfaces;
using MetaBrainz.MusicBrainz.Interfaces.Entities;
using MetaBrainz.MusicBrainz.Interfaces.Searches;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AireLogic.LyricsChecker.ConsoleApp.DataSources.MusicBrainz
{
    public class MusicBrainzDataSource : IMusicDataSource
    {
        private const string KEY_MUSICBRAINZ_APP_ID = "musicbrainz-app-id";
        private const string KEY_MUSICBRAIN_APP_SECRET = "musicbrainz-app-secret";
        private const int TARGET_SCORE = 100;
        private const int BROWSE_DEFAULT_LIMIT = 100;
        private OAuth2 _oauth2;
        private IAuthorizationToken _authorizationToken;
        private Uri _uri;

        private string _appName;
        private string _appVersion;

        public MusicBrainzDataSource() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            _appName = assembly.GetName().Name;
            _appVersion = assembly.GetName().Version.ToString();
        }

        public Guid GetArtist(string name)
        {
            IArtist artist = null;
            Query query = CreateDefaultQuery();
            var worker = query.FindArtists(name, limit: BROWSE_DEFAULT_LIMIT);
            if (worker != null)
            {
                while (artist == null)
                {
                    foreach (var item in worker.Results)
                    {
                        if (item.Score >= TARGET_SCORE)
                        {
                            artist = item.Item;
                            break;
                        }
                    }
                    worker.Next();
                }
            }
            return artist.Id;
        }

        public async Task<Guid> GetArtistAsync(string name) {
            IArtist artist = null;
            Query query = CreateDefaultQuery();
            var worker = await query.FindArtistsAsync(name, limit: BROWSE_DEFAULT_LIMIT);
            if (worker != null)
            {
                while (artist == null)
                {
                    foreach (var item in worker.Results)
                    {
                        if (item.Score >= TARGET_SCORE)
                        {
                            artist = item.Item;
                            break;
                        }
                    }
                    await worker.NextAsync();
                }
            }
            return artist.Id;
        }

        public IList<string> GetDistinctRecords(Guid artistGuid) {
            IList<string> records = new List<string>();
            Query query = CreateDefaultQuery();
            
            var response = query.BrowseArtistRecordings(artistGuid, limit: BROWSE_DEFAULT_LIMIT);
            while (response.Results.Count > 0)
            {
                foreach (var item in response.Results)
                {
                    string title = item.Title;
                    if (records.Contains(title)) {
                        continue;
                    }
                    records.Add(title);
                }
                response.Next();
            };
            return records.Distinct().OrderBy(o=>o.FirstOrDefault()).ToList();
        }

        public async Task<IList<string>> GetDistinctRecordsAsync(Guid artistGuid)
        {
            IList<string> records = new List<string>();
            Query query = CreateDefaultQuery();

            var response = await query.BrowseArtistRecordingsAsync(artistGuid, limit: BROWSE_DEFAULT_LIMIT);
            while (response.Results.Count > 0)
            {
                foreach (var item in response.Results)
                {
                    string title = item.Title;
                    if (records.Contains(title))
                    {
                        continue;
                    }
                    records.Add(title);
                }
                await response.NextAsync();
            };
            return records.Distinct().OrderBy(o => o.FirstOrDefault()).ToList();
        }

        internal Query CreateDefaultQuery() { 
            return new Query(_appName, _appVersion);
        }
    }
}
