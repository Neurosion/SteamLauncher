using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SteamLauncher.Domain.Input;

namespace SteamLauncher.Domain.Tests.Input
{
    [TestFixture]
    public class WindowsHookRegistrationControllerTests
    {
        [Test]
        public void RegisteringNullListenerThrowsException()
        {
            var controller = new WindowsHookRegistrationController();
            Assert.Throws<ArgumentNullException>(() => controller.Register(null));
        }

        [Test]
        public void UnregisteringNullListenerThrowsException()
        {
            var controller = new WindowsHookRegistrationController();
            Assert.Throws<ArgumentNullException>(() => controller.Unregister(null));
        }

        [Test]
        public void ListenerHookPointerIsSetAfterRegistering()
        {
            var listenerMock = MockRepository.GenerateMock<IHookListener>();
            listenerMock.Stub(x => x.HookId).Return((int)WindowsHooks.WH_KEYBOARD_LL);
            var controller = new WindowsHookRegistrationController();
            
            controller.Register(listenerMock);

            try
            {
                listenerMock.AssertWasCalled(x => x.HookPointer = Arg<IntPtr>.Is.Anything, c => c.Repeat.Once());
            }
            catch 
            { 
                throw; 
            }
            finally
            {
                controller.Unregister(listenerMock);
            }
        }

        [Test]
        public void ListenerHookPointerIsSetToZeroAfterUnregistering()
        {
            var listenerMock = MockRepository.GenerateMock<IHookListener>();
            var controller = new WindowsHookRegistrationController();
            
            controller.Unregister(listenerMock);

            listenerMock.AssertWasCalled(x => x.HookPointer = IntPtr.Zero, c => c.Repeat.Once());
        }
    }
}