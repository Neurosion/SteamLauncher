using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace SteamLauncher.Domain.Tests
{
    [TestFixture]
    public class ApplicationTests
    {
        [Test]
        public void DoesIdMatchProvidedId()
        {
            var providedId = 10;
            var steamApp = new Application(providedId, null);
            Assert.AreEqual(providedId, steamApp.Id);
        }

        [Test]
        public void DoesNameMatchProvidedName()
        {
            var providedName = "test";
            var steamApp = new Application(0, providedName);
            Assert.AreEqual(providedName, steamApp.Name);
        }
    }
}