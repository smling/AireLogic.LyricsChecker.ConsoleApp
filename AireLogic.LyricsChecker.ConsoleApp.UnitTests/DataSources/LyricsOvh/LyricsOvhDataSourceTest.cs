using AireLogic.LyricsChecker.ConsoleApp.DataSources.LyricsOvh;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AireLogic.LyricsChecker.ConsoleApp.UnitTests.DataSources.LyricsOvh
{
    internal class LyricsOvhDataSourceTest
    {
        private LyricsOvhDataSource morkDataSource;

        [OneTimeSetUp]
        public void OneTimeSetup() { 
            morkDataSource = new LyricsOvhDataSource();
        }

        [Test]
        [TestCase("Coldplay", "Adventure of a Lifetime", "https://api.lyrics.ovh/v1/")]
        [Parallelizable]
        public async Task GivenCorrectRoot_WhenQuery_ThenGetLyricsAsync(string artist, string title, string rootUrl)
        {
            LyricsOvhSettings setting = new LyricsOvhSettings()
            {
                RootUrl = rootUrl,
            };
            LyricsOvhDataSource dummyDataSource = new LyricsOvhDataSource(setting);
            string response = await dummyDataSource.GetLyricsAsync(artist, title);
            Assert.IsTrue(response.Length > 0);
        }

        [Test]
        [TestCase("Coldplay", "Adventure of a Lifetime", "https://www.duckduckgo.com")]
        [Parallelizable]
        public Task GivenIncorrectRoot_WhenQuery_ThenGetEmptyStringAsync(string artist, string title, string rootUrl)
        {
            LyricsOvhSettings setting = new LyricsOvhSettings()
            {
                RootUrl = rootUrl,
            };
            LyricsOvhDataSource dummyDataSource = new LyricsOvhDataSource(setting);
            Assert.DoesNotThrowAsync(async () => {
                string response = await dummyDataSource.GetLyricsAsync(artist, title);
                Assert.IsTrue(response.Length == 0);
            });
            return Task.CompletedTask;
        }

        [Test]
        [TestCase("Coldplay", "Adventure of a Lifetime")]
        [Parallelizable]
        public async Task GivenArtistAndTitle_WhenQuery_ThenGetLyricsAsync(string artist, string title) { 
            string response = await morkDataSource.GetLyricsAsync(artist, title);
            Assert.IsTrue(response.Length > 0);
        }
    }
}
