using MetaBrainz.MusicBrainz;
using MetaBrainz.MusicBrainz.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AireLogic.LyricsChecker.ConsoleApp.DataSources.MusicBrainz
{
    public class MusicBrainzDataSource : IMusicDataSource
    {
        private string _appName;
        private string _appVersion;

        public MusicBrainzDataSource()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            _appName = assembly.GetName().Name;
            _appVersion = assembly.GetName().Version.ToString();
        }

        public string GetArtist(string name)
        {
            IArtist artist = null;
            Query query = CreateDefaultQuery();
            var worker = query.FindArtists(name, limit: Constants.Settings.MusicBrainz.BROWSE_DEFAULT_LIMIT);
            if (worker != null)
            {
                while (artist == null)
                {
                    foreach (var item in worker.Results)
                    {
                        if (item.Score >= Constants.Settings.MusicBrainz.TARGET_SCORE)
                        {
                            artist = item.Item;
                            break;
                        }
                    }
                    worker.Next();
                }
            }
            return artist.Id.ToString();
        }

        public async Task<string> GetArtistAsync(string name)
        {
            IArtist artist = null;
            Query query = CreateDefaultQuery();
            var worker = await query.FindArtistsAsync(name, limit: Constants.Settings.MusicBrainz.BROWSE_DEFAULT_LIMIT);
            if (worker != null)
            {
                while (artist == null)
                {
                    foreach (var item in worker.Results)
                    {
                        if (item.Score >= Constants.Settings.MusicBrainz.TARGET_SCORE)
                        {
                            artist = item.Item;
                            break;
                        }
                    }
                    await worker.NextAsync();
                }
            }
            return artist.Id.ToString();
        }

        public IList<string> GetDistinctRecords(string artistId)
        {
            IList<string> records = new List<string>();
            Query query = CreateDefaultQuery();
            Guid artistGuid = new Guid(artistId);
            var response = query.BrowseArtistRecordings(artistGuid, limit: Constants.Settings.MusicBrainz.BROWSE_DEFAULT_LIMIT);
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
                response.Next();
            };
            return records.Distinct().OrderBy(o => o.FirstOrDefault()).ToList();
        }

        public async Task<IList<string>> GetDistinctRecordsAsync(string artistId)
        {
            IList<string> records = new List<string>();
            Query query = CreateDefaultQuery();
            Guid artistGuid = new Guid(artistId);
            var response = await query.BrowseArtistRecordingsAsync(artistGuid, limit: Constants.Settings.MusicBrainz.BROWSE_DEFAULT_LIMIT);
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

        internal Query CreateDefaultQuery()
        {
            return new Query(_appName, _appVersion);
        }
    }
}
