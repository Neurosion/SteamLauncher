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
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceWatcher(null));
        }

        [Test]
        public void ThrowsExceptionWhenEmptyPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceWatcher(string.Empty));
        }

        [Test]
        public void ThrowsExceptionWhenInvalidPathIsProvided()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationResourceWatcher(Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString())));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("a")]
        [TestCase("b")]
        public void NotifiesWhenFileWithProvidedExtensionIsCreated(string extension)
        {
            PerformNotificationTestSetupAndCleanup(extension,
                (fileNames, watcher, notifiedFileNames) =>
                {
                    watcher.ResourceAdded += (id, name) => notifiedFileNames.Add(name);

                    fileNames.ForEach(x => File.WriteAllText(x, string.Empty));
                });
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("a")]
        [TestCase("b")]
        public void NotifiesWhenFileWithProvidedExtensionIsDeleted(string extension)
        {
            PerformNotificationTestSetupAndCleanup(extension,
                (fileNames, watcher, notifiedFileNames) =>
                {
                    watcher.ResourceRemoved += (id, name) => notifiedFileNames.Add(name);

                    fileNames.ForEach(x => File.WriteAllText(x, string.Empty));
                    fileNames.ForEach(x => File.Delete(x));
                });
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("a")]
        [TestCase("b")]
        public void NotifiesWhenFileWithProvidedExtensionIsUpdated(string extension)
        {
            PerformNotificationTestSetupAndCleanup(extension,
                (fileNames, watcher, notifiedFileNames) =>
                {
                    watcher.ResourceUpdated += (id, name) => notifiedFileNames.Add(name);

                    fileNames.ForEach(x => File.WriteAllText(x, string.Empty));
                    fileNames.ForEach(x => File.WriteAllText(x, string.Empty));
                });
        }

        private void PerformNotificationTestSetupAndCleanup(string extension, Action<IEnumerable<string>, ConfigurationResourceWatcher, List<string>> testBody)
        {
            var fileNames = new[] { "test", "test.a", "test.b", "test_2.a", "test_3", "test_4" };

            try
            {
                var watcher = new ConfigurationResourceWatcher(Environment.CurrentDirectory, extension);
                var notifiedFileNames = new List<string>();
                var expectedFileNames = fileNames.Where(x => string.IsNullOrEmpty(extension) || x.EndsWith(extension));

                testBody(expectedFileNames, watcher, notifiedFileNames);

                Assert.AreEqual(expectedFileNames.Count(), notifiedFileNames.Count);

                foreach (var currentFileName in fileNames)
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