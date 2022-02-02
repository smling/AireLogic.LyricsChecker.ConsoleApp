using NUnit.Framework;
using System.Threading.Tasks;

namespace AireLogic.LyricsChecker.ConsoleApp.UnitTests
{
    public class ProgramTests
    {
        [Test]
        [TestCase("--artist=coldplay")]
        [TestCase("--artist=Meat Loaf")]
        [Parallelizable]
        public void GivenArtist_WhenQueryRecords_GetWorkCount(string arguement)
        {
            Assert.DoesNotThrowAsync( async () => {
                string[] args = { arguement };
                await Program.Main(args);
            });
        }
    }
}