using AireLogic.LyricsChecker.ConsoleApp.DataSources.MusicBrainz;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AireLogic.LyricsChecker.ConsoleApp.UnitTests.DataSources.MusicBrainz
{
    internal class MusicBrainzDataSourceTest
    {
        private MusicBrainzDataSource morkDataSource;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            morkDataSource = new MusicBrainzDataSource();
        }

        [Test]
        public void GivenData_WhenConnectServer_ThenExpectedSuccess()
        {
            Assert.DoesNotThrow(() =>
            {
                MusicBrainzDataSource dataSource = new MusicBrainzDataSource();
            });
        }

        [Test]
        [TestCase("Bee Gees")]
        [TestCase("Queen")]
        [TestCase("Justin Timberlake")]
        [Parallelizable]
        public void GivenArtistName_WhenQuuery_ThenGetArtistGuid(string artist)
        {
            Assert.DoesNotThrow(() =>
            {
                string artistGuid = morkDataSource.GetArtist(artist);
                Assert.NotNull(artistGuid);
                TestContext.Out.WriteLine($"Artist {artist} MUID: {artistGuid}");
            });
        }

        [Test]
        [TestCase("Bee Gees")]
        [TestCase("Queen")]
        [TestCase("Justin Timberlake")]
        [Parallelizable]
        public async Task GivenArtistName_WhenQuueryAsync_ThenGetArtistGuid(string artist)
        {
            string artistGuid = await morkDataSource.GetArtistAsync(artist);
            Assert.NotNull(artistGuid);
            TestContext.Out.WriteLine($"Artist {artist} MUID: {artistGuid}");
        }

        [Test]
        [TestCase("Bee Gees")]
        [TestCase("Queen")]
        [TestCase("Justin Timberlake")]
        [Parallelizable]
        public void GivenArtistName_WhenQuuery_ThenGetRecords(string artist)
        {
            Assert.DoesNotThrow(() =>
            {
                string artistGuid = morkDataSource.GetArtist(artist);
                IList<string> records = morkDataSource.GetDistinctRecords(artistGuid);
                TestContext.Out.WriteLine($"Artist {artist} search records: {records.Count}");
                Assert.IsTrue(records.Count > 0);
            });
        }

        [Test]
        [TestCase("Bee Gees")]
        [TestCase("Queen")]
        [TestCase("Justin Timberlake")]
        [Parallelizable]
        public async Task GivenArtistName_WhenQuueryAsync_ThenGetRecords(string artist)
        {
            string artistGuid = await morkDataSource.GetArtistAsync(artist);
            IList<string> records = await morkDataSource.GetDistinctRecordsAsync(artistGuid);
            TestContext.Out.WriteLine($"Artist {artist} search records: {records.Count}");
            Assert.IsTrue(records.Count > 0);
        }
    }
}
