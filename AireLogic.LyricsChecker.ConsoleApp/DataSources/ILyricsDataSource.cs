using System.Threading.Tasks;

namespace AireLogic.LyricsChecker.ConsoleApp.DataSources
{
    internal interface ILyricsDataSource
    {
        Task<string> GetLyricsAsync(string artice, string title);
    }
}
