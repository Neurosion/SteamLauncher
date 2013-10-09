using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.UI.Core;

namespace SteamLauncher.UI.Tests.Core
{
    [TestFixture]
    public class RelayCommandTests
    {
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void CanExecuteReturnsSameValueAsProvidedMethod(bool actual, bool expected)
        {
            var command = new RelayCommand(null, x => actual);

            Assert.AreEqual(expected, command.CanExecute(null));
        }

        [Test]
        public void ExecuteDoesNotExecuteProvidedActionIfCanExecuteIsFalse()
        {
            var wasExecuted = false;
            var command = new RelayCommand(x => wasExecuted = true, x => false);
            
            Assert.IsFalse(wasExecuted);
            command.Execute(null);
            Assert.IsFalse(wasExecuted);
        }

        [Test]
        public void ExecuteDoesExecuteProvidedActionIfCanExecuteIsTrue()
        {
            var wasExecuted = false;
            var command = new RelayCommand(x => wasExecuted = true, x => true);

            Assert.IsFalse(wasExecuted);
            command.Execute(null);
            Assert.IsTrue(wasExecuted);
        }
    }
}