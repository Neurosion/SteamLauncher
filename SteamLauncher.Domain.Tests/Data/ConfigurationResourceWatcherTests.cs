using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Data;

namespace SteamLauncher.Domain.Tests.Data
{
    [TestFixture]
    public class ConfigurationResourceWatcherTests : FileBasedTestFixture
    {
        [Test]
        public void ThrowsExceptionWhenNullPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceWatcher(null, null));
        }

        [Test]
        public void ThrowsExceptionWhenEmptyPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceWatcher(string.Empty, null));
        }

        [Test]
        public void ThrowsExceptionWhenInvalidPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceWatcher(Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString()), null));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("*.a")]
        [TestCase("*.b")]
        public void NotifiesWhenFileWithProvidedFilterIsCreated(string filter)
        {
            PerformNotificationTestSetupAndCleanup(filter,
                (fileNames, watcher) =>
                {
                    var notifiedFileNames = new List<string>();
                    
                    watcher.ResourceAdded += (id, name) => notifiedFileNames.Add(Path.GetFileName(name));

                    fileNames.ForEach(x => File.WriteAllText(x, string.Empty));

                    System.Threading.Thread.Sleep(50);

                    return notifiedFileNames;
                });
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("*.a")]
        [TestCase("*.b")]
        public void NotifiesWhenFileWithProvidedFilterIsDeleted(string filter)
        {
            PerformNotificationTestSetupAndCleanup(filter,
                (fileNames, watcher) =>
                {
                    var notifiedFileNames = new List<string>();
                    
                    watcher.ResourceRemoved += (id, name) => notifiedFileNames.Add(Path.GetFileName(name));

                    fileNames.ForEach(x => File.WriteAllText(x, string.Empty));
                    fileNames.ForEach(x => File.Delete(x));

                    System.Threading.Thread.Sleep(50);

                    return notifiedFileNames;
                });
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("*.a")]
        [TestCase("*.b")]
        public void NotifiesWhenFileWithProvidedFilterIsUpdated(string filter)
        {
            PerformNotificationTestSetupAndCleanup(filter,
                (fileNames, watcher) =>
                {
                    var notifiedFileNames = new List<string>();

                    watcher.ResourceUpdated += (id, name) => notifiedFileNames.Add(Path.GetFileName(name));

                    fileNames.ForEach(x => File.WriteAllText(x, string.Empty));
                    fileNames.ForEach(x => File.WriteAllText(x, string.Empty));

                    System.Threading.Thread.Sleep(50);

                    return notifiedFileNames;
                });
        }

        private void PerformNotificationTestSetupAndCleanup(string filter, Func<IEnumerable<string>, ConfigurationResourceWatcher, List<string>> testBody)
        {
            var fileNames = new[] { "test", "test.a", "test.b", "test_2.a", "test_3", "test_4" };

            try
            {
                var converterMock = MockRepository.GenerateMock<IIdConverter>();
                converterMock.Stub(x => x.Convert(Arg<string>.Is.Anything)).Return(0);
                var watcher = new ConfigurationResourceWatcher(Environment.CurrentDirectory, converterMock, filter);
                var expectedFileNames = fileNames.Where(x => string.IsNullOrEmpty(filter) || x.EndsWith(filter));

                var notifiedFileNames = testBody(expectedFileNames, watcher);

                Assert.AreEqual(expectedFileNames.Count(), notifiedFileNames.Count);

                foreach (var currentFileName in expectedFileNames)
                    Assert.Contains(currentFileName, notifiedFileNames);
            }
            catch
            {
                throw;
            }
            finally
            {
                fileNames.Where(x => File.Exists(x)).ForEach(x => File.Delete(x));
            }
        }
    }
}