using AireLogic.LyricsChecker.ConsoleApp.DataSources.LyricsOvh;
using AireLogic.LyricsChecker.ConsoleApp.DataSources.MusicBrainz;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using AireLogic.LyricsChecker.ConsoleApp.Extensions;

namespace AireLogic.LyricsChecker.ConsoleApp
{
    public class Program
    {
        private static MusicBrainzDataSource _musicDataSource;
        private static LyricsOvhDataSource _lyricsDataSource;
        private static LyricsOvhSettings _lyricsOvhSettings;
        private static string _searchArtist;

        public static async Task Main(string[] args)
        {
            long totalLyricsCharactors = 0;
            ResolveParameter(args);
            InitialConfiguration();
            if (String.IsNullOrEmpty(_searchArtist))
            {
                Console.WriteLine("Input artist name: ");
                _searchArtist = Console.ReadLine();
            }
            string artistGuid = await _musicDataSource.GetArtistAsync(_searchArtist);
            if (String.IsNullOrEmpty(artistGuid))
            {
                Console.WriteLine("No artist found.");
                return;
            }
            IList<string> records = await _musicDataSource.GetDistinctRecordsAsync(artistGuid);
            List<string> recordList = new List<string>(records);
            recordList.AsParallel().ForAll(record => {
                var task = Task.Run(async () => await _lyricsDataSource.GetLyricsAsync(_searchArtist, record));
                task.Wait();
                string lyrics = task.Result;
                Console.WriteLine($"Search with artist {_searchArtist} record {record}. total characters: {lyrics.Length}.");
                totalLyricsCharactors += lyrics.WordCount();
            }); 
            Console.WriteLine($"Total records: {records.Count}.");
            Console.WriteLine($"Total words in all records: {totalLyricsCharactors}.");
            decimal averageWordCount = totalLyricsCharactors / records.Count;
            Console.WriteLine($"Average words in all records: {Math.Round(averageWordCount, 2)}.");
        }

        static void InitialConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _lyricsOvhSettings = config.GetSection("lyricsOvh").Get<LyricsOvhSettings>();

            _musicDataSource = new MusicBrainzDataSource();
            _lyricsDataSource = new LyricsOvhDataSource();
        }

        static void ResolveParameter(string[] args) {
            List<string> arguements = args.ToList();
            _searchArtist = GetParameterValueByKey(Constants.Parameters.ARTICE, arguements);
        }

        static string GetParameterValueByKey(string key, List<string> arugements) {
            string arguement = arugements.FirstOrDefault(o=>o.Contains(key));
            string parameterKey = Constants.Parameters.PARAMETER_PREFIX+ key+Constants.Parameters.PARAMETER_VALUE_SEPERATOR;
            return arguement.Replace(parameterKey, String.Empty);
        }
    }
}
