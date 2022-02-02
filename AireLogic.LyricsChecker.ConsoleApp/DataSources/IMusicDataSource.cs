using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AireLogic.LyricsChecker.ConsoleApp.DataSources
{
    internal interface IMusicDataSource
    {
        string GetArtist(string artistName);
        Task<string> GetArtistAsync(string name);
        IList<string> GetDistinctRecords(string artistGuid);
        Task<IList<string>> GetDistinctRecordsAsync(string artistGuid);
    }
}
