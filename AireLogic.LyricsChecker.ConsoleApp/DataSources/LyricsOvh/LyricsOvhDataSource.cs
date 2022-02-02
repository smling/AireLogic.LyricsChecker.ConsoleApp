using AireLogic.LyricsChecker.ConsoleApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AireLogic.LyricsChecker.ConsoleApp.DataSources.LyricsOvh
{
    public class LyricsOvhDataSource : ILyricsDataSource
    {
        private string _rootUrl;
        private JsonSerializerOptions _jsonSerializerOption;
        public LyricsOvhDataSource(LyricsOvhSettings settings = null)
        {
            _rootUrl = settings !=null ? settings.RootUrl : Constants.Settings.LyricsOvh.DEFAULT_ROOR_URL;
            _jsonSerializerOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<string> GetLyricsAsync(string artice, string title)
        {
            string result = String.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                string requestURl = $"{_rootUrl}/{artice}/{title}";
                var response = await httpClient.GetAsync(requestURl);
                if (response.IsSuccessStatusCode)
                {
                    string lyricsJson = await response.Content.ReadAsStringAsync();
                    LyricsResponse lyricsResponse = JsonSerializer.Deserialize<LyricsResponse>(lyricsJson, _jsonSerializerOption);
                    result = lyricsResponse.Lyrics;
                }
            }
            return result;
        }
    }
}
