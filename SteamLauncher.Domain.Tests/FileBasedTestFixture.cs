using System;
using System.IO;

namespace SteamLauncher.Domain.Tests
{
    public abstract class FileBasedTestFixture
    {
        protected void AssertFileBasedTest(string[] fileNames, Action<string[]> assertionMethod)
        {
            try
            {
                fileNames.ForEach(x => File.WriteAllText(x, "Test File: " + x));
                assertionMethod(fileNames);
            }
            catch
            {
                throw;
            }
            finally
            {
                fileNames.ForEach(x => File.Delete(x));
            }
        }
    }
}
