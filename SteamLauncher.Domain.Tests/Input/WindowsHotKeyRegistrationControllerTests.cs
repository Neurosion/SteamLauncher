using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace SteamLauncher.Domain.Input.Tests
{
    [TestFixture]
    public class WindowsHotKeyRegistrationControllerTests
    {
        [Test]
        public void RegisterThrowsExceptionWhenProvidedNullHotKey()
        {
            var controller = new WindowsHotKeyRegistrationController(null);

            Assert.Throws<ArgumentNullException>(() => controller.Register(null));
        }

        [Test]
        public void RegisterReturnsProvidedHotKeyIdWhenHotKeyIsSetToNone()
        {
            var hotKeyId = 15;
            var hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(hotKeyId);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.None);
            var controller = new WindowsHotKeyRegistrationController(null);

            var value = controller.Register(hotKeyMock);
            Assert.AreEqual(hotKeyId, value);
        }

        [Test]
        public void UnregisterThrowsExceptionWhenProvidedNullHotKey()
        {
            var controller = new WindowsHotKeyRegistrationController(null);

            Assert.Throws<ArgumentNullException>(() => controller.Unregister(null));
        }

        [Test]
        public void ProvidesNewIdWhenSuccessfullyRegisteringAHotKey()
        {
            var hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(0);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.A);

            var hookRegistrationControllerMock = MockRepository.GenerateMock<IHookRegistrationController>();
            var controller = new WindowsHotKeyRegistrationController(hookRegistrationControllerMock);
            var returnedId = controller.Register(hotKeyMock);

            Assert.AreNotEqual(hotKeyMock.Id, returnedId);
            
            controller.Unregister(hotKeyMock);
        }

        [Test]
        public void DoesReturnProvidedHotKeyIdHotKeyAlreadyRegistered()
        {
            var hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(0);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.A);

            var hookRegistrationControllerMock = MockRepository.GenerateMock<IHookRegistrationController>();
            var controller = new WindowsHotKeyRegistrationController(hookRegistrationControllerMock);
            var firstReturnedId = controller.Register(hotKeyMock);

            Assert.AreNotEqual(hotKeyMock.Id, firstReturnedId);

            // Rebuilding mock with new id
            hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(firstReturnedId);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.A);
            
            var secondReturnedId = controller.Register(hotKeyMock);

            Assert.AreEqual(firstReturnedId, secondReturnedId);

            controller.Unregister(hotKeyMock);
        }

        [Test]
        public void DoesThrowExceptionWhenUnregisteringHotKeyFails()
        {
            var hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(0);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.A);
            
            var hookRegistrationControllerMock = MockRepository.GenerateMock<IHookRegistrationController>();
            hookRegistrationControllerMock.Stub(x => x.Unregister(Arg<IHookListener>.Is.Anything)).Throw(new Exception());
            var controller = new WindowsHotKeyRegistrationController(hookRegistrationControllerMock);

            Assert.Throws<Exception>(() => controller.Unregister(hotKeyMock));
        }
    }
}