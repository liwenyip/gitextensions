using System.Reflection;
using CommonTestUtils;
using FluentAssertions;
using GitCommands.ExternalLinks;
using GitCommands.Settings;
using NUnit.Framework;

namespace GitCommandsTests.ExternalLinks
{
    [TestFixture]
    public class ExternalLinksLoaderIntegrationTests
    {
        private ExternalLinksLoader _loader;


        [SetUp]
        public void Setup()
        {
            _loader = new ExternalLinksLoader();
        }


        [TestCase("level1_repogit_GitExtensions", 1)]
        [TestCase("level2_repodist_GitExtensions", 3)]
        [TestCase("level3_roaming_GitExtensions", 1)]
        public void Can_load_settings(string fileName, int expected)
        {
            var content = EmbeddedResourceLoader.Load(Assembly.GetExecutingAssembly(), $"{GetType().Namespace}.MockData.{fileName}.settings.xml");

            using (var testHelper = new GitModuleTestHelper())
            {
                var settingsFile = testHelper.CreateRepoFile(".git", "GitExtensions.settings", content);
                using (var settingsCache = new GitExtSettingsCache(settingsFile))
                {
                    var settings = new RepoDistSettings(null, settingsCache);

                    var definitions = _loader.Load(settings);
                    definitions.Count.Should().Be(expected);
                }
            }
        }
    }
}