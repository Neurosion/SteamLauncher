using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace SteamLauncher.Domain.Tests
{
    [TestFixture]
    public class CommandLineSteamProxyTests
    {
        [Test]
        public void LaunchAppProvidesCorrectPathToProcessProxy()
        {
            var steamPath = "testPath";
            var processProxyMock = MockRepository.GenerateMock<IProcessProxy>();
            var steamProxy = new CommandLineSteamProxy(processProxyMock, steamPath);
            
            steamProxy.LaunchApp(0);

            processProxyMock.AssertWasCalled(x => x.Start(Arg<string>.Is.Equal(steamPath), Arg<string>.Is.Anything), c => c.Repeat.Once());
        }

        [Test]
        public void LaunchAppProvidesCorrectIdToProcessProxy()
        {
            var processProxyMock = MockRepository.GenerateMock<IProcessProxy>();
            var steamProxy = new CommandLineSteamProxy(processProxyMock, "test");
            var appId = 15;
            steamProxy.LaunchApp(appId);

            processProxyMock.AssertWasCalled(x => x.Start(Arg<string>.Is.Anything, Arg<string>.Matches(p => p.EndsWith(appId.ToString()))), c => c.Repeat.Once());
        }

        [Test]
        public void LaunchAppProvidesSilentArgumentToProcessProxyWhenSilentIsTrue()
        {
            var processProxyMock = MockRepository.GenerateMock<IProcessProxy>();
            var steamProxy = new CommandLineSteamProxy(processProxyMock, "test");
            steamProxy.IsSilent = true;
            steamProxy.LaunchApp(15);

            processProxyMock.AssertWasCalled(x => x.Start(Arg<string>.Is.Anything, Arg<string>.Matches(p => p.Contains("silent"))), c => c.Repeat.Once());
        }

        [Test]
        public void LaunchAppDoesNotProvideSilentArgumentToProcessProxyWhenSilentIsFalse()
        {
            var processProxyMock = MockRepository.GenerateMock<IProcessProxy>();
            var steamProxy = new CommandLineSteamProxy(processProxyMock, "test");
            steamProxy.IsSilent = false;
            steamProxy.LaunchApp(15);

            processProxyMock.AssertWasCalled(x => x.Start(Arg<string>.Is.Anything, Arg<string>.Matches(p => !p.Contains("silent"))), c => c.Repeat.Once());
        }

        [Test]
        public void LaunchAppPassesAllProvidedParametersToProcessProxy()
        {
            var processProxyMock = MockRepository.GenerateMock<IProcessProxy>();
            var steamProxy = new CommandLineSteamProxy(processProxyMock, "test");
            var parameters = new[] { "one", "two", "three", "four" };
            steamProxy.LaunchApp(15, parameters);

            processProxyMock.AssertWasCalled(x => x.Start(Arg<string>.Is.Anything, Arg<string>.Matches(args => parameters.All(p => args.Contains(p)))), c => c.Repeat.Once());
        }
    }
}