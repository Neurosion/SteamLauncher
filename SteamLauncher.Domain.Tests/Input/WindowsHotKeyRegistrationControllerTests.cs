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
        public void RegisterReturnsZeroWhenProvidedNullHotKey()
        {
            var controller = new WindowsHotKeyRegistrationController();
            var value = controller.Register(null);
            Assert.AreEqual(0, value);
        }

        [Test]
        public void RegisterReturnsProvidedHotKeyIdWhenHotKeyIsSetToNone()
        {
            var hotKeyId = 15;
            var hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(hotKeyId);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.None);
            var controller = new WindowsHotKeyRegistrationController();

            var value = controller.Register(hotKeyMock);
            Assert.AreEqual(hotKeyId, value);
        }

        [Test]
        public void UnregisterReturnsFalseWhenProvidedNullHotKey()
        {
            var controller = new WindowsHotKeyRegistrationController();
            var value = controller.Unregister(null);

            Assert.IsFalse(value);
        }

        [Test]
        public void ProvidesNewIdWhenSuccessfullyRegisteringAHotKey()
        {
            var form = new System.Windows.Forms.Form();
            var hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(0);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.A);
            hotKeyMock.Stub(x => x.ParentWindowHandle).Return(form.Handle);

            var controller = new WindowsHotKeyRegistrationController();
            var returnedId = controller.Register(hotKeyMock);

            Assert.AreNotEqual(hotKeyMock.Id, returnedId);
            
            try
            {
                controller.Unregister(hotKeyMock);
            }
            catch { }

            form.Dispose();
        }

        [Test]
        public void DoesReturnProvidedHotKeyIdHotKeyAlreadyRegistered()
        {
            var form = new System.Windows.Forms.Form();
            var hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(0);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.A);
            hotKeyMock.Stub(x => x.ParentWindowHandle).Return(form.Handle);

            var controller = new WindowsHotKeyRegistrationController();
            var firstReturnedId = controller.Register(hotKeyMock);

            Assert.AreNotEqual(hotKeyMock.Id, firstReturnedId);

            // Rebuilding mock with new id
            hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(firstReturnedId);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.A);
            hotKeyMock.Stub(x => x.ParentWindowHandle).Return(form.Handle);
            
            var secondReturnedId = controller.Register(hotKeyMock);

            Assert.AreEqual(firstReturnedId, secondReturnedId);

            try
            {
                controller.Unregister(hotKeyMock);
            }
            catch { }

            form.Dispose();
        }

        [Test]
        public void DoesThrowExceptionWhenUnregisteringHotKeyWithWindowsFails()
        {
            var hotKeyMock = MockRepository.GenerateMock<IHotKey>();
            hotKeyMock.Stub(x => x.Id).Return(0);
            hotKeyMock.Stub(x => x.Key).Return(System.Windows.Forms.Keys.A);
            hotKeyMock.Stub(x => x.ParentWindowHandle).Return(new IntPtr(-1));

            var controller = new WindowsHotKeyRegistrationController();

            Assert.Throws<Exception>(() => controller.Unregister(hotKeyMock));
        }
    }
}