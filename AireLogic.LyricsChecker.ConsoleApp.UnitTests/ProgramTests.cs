using NUnit.Framework;

namespace AireLogic.LyricsChecker.ConsoleApp.UnitTests
{
    public class ProgramTests
    {
        [Test]
        public void Test1()
        {
            Assert.DoesNotThrow(() => {
                Program.Main(null);
            });
            Assert.Pass();
        }
    }
}