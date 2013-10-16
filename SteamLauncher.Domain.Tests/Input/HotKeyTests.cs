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
    public class HotKeyTests
    {
        [Test]
        public void EnablingHotKeyWithoutKeySetThrowsException()
        {
            var registrationControllerMock = MockRepository.GenerateMock<IHotKeyRegistrationController>();
            var hotKey = new HotKey(registrationControllerMock);
            Assert.Throws<ArgumentException>(() => hotKey.Enable());
        }

        [Test]
        public void DisablingIfNotEnabledSucceeds()
        {
            var registrationControllerMock = MockRepository.GenerateMock<IHotKeyRegistrationController>();
            var hotKey = new HotKey(registrationControllerMock);
            Assert.DoesNotThrow(() => hotKey.Disable());
        }

        [Test]
        public void DisablingIfNoKeySetSucceeds()
        {
            var registrationControllerMock = MockRepository.GenerateMock<IHotKeyRegistrationController>();
            var hotKey = new HotKey(registrationControllerMock);
            Assert.DoesNotThrow(() => hotKey.Disable());
        }

        [Test]
        public void EnablingChangesIsEnabledToTrue()
        {
            var registrationControllerMock = MockRepository.GenerateMock<IHotKeyRegistrationController>();
            registrationControllerMock.Stub(x => x.Register(Arg<IHotKey>.Is.Anything)).Return(0);
            var hotKey = new HotKey(registrationControllerMock);
            hotKey.Key = System.Windows.Forms.Keys.A;
            
            Assert.IsFalse(hotKey.IsEnabled);
            hotKey.Enable();
            Assert.IsTrue(hotKey.IsEnabled);
        }

        [Test]
        public void EnablingWhileEnabledSucceeds()
        {
            var registrationControllerMock = MockRepository.GenerateMock<IHotKeyRegistrationController>();
            registrationControllerMock.Stub(x => x.Register(Arg<IHotKey>.Is.Anything)).Return(0);
            var hotKey = new HotKey(registrationControllerMock);
            hotKey.Key = System.Windows.Forms.Keys.A;

            Assert.IsFalse(hotKey.IsEnabled);
            hotKey.Enable();
            Assert.IsTrue(hotKey.IsEnabled);
            hotKey.Enable();
            Assert.IsTrue(hotKey.IsEnabled);
        }

        [Test]
        public void DisablingWhileDisabledSucceeds()
        {
            var registrationControllerMock = MockRepository.GenerateMock<IHotKeyRegistrationController>();
            var hotKey = new HotKey( registrationControllerMock);
            Assert.DoesNotThrow(() => hotKey.Disable());
        }

        [Test]
        public void DisablingWhileEnabledChangesIsEnabledToFalse()
        {
            var registrationControllerMock = MockRepository.GenerateMock<IHotKeyRegistrationController>();
            registrationControllerMock.Stub(x => x.Register(Arg<IHotKey>.Is.Anything)).Return(0);
            registrationControllerMock.Stub(x => x.Unregister(Arg<IHotKey>.Is.Anything)).Return(true);
            var hotKey = new HotKey(registrationControllerMock);
            hotKey.Key = System.Windows.Forms.Keys.A;

            Assert.IsFalse(hotKey.IsEnabled);
            hotKey.Enable();
            Assert.IsTrue(hotKey.IsEnabled);
            hotKey.Disable();
            Assert.IsFalse(hotKey.IsEnabled);
        }

        [Test]
        public void IdIsSetToControllerProvidedValueWhenHotKeyIsEnabled()
        {
            var id = 15;
            var registrationControllerMock = MockRepository.GenerateMock<IHotKeyRegistrationController>();
            registrationControllerMock.Stub(x => x.Register(Arg<IHotKey>.Is.Anything)).Return(id);
            var hotKey = new HotKey(registrationControllerMock);
            hotKey.Key = System.Windows.Forms.Keys.A;

            Assert.AreNotEqual(id, hotKey.Id);
            hotKey.Enable();
            Assert.AreEqual(id, hotKey.Id);
        }

        [Test]
        public void ToStringOutputsKeyNameOnlyWhenModifierNoneIsSet()
        {
            var hotKey = new HotKey(null);
            hotKey.Modifiers = ModifierKeys.None;
            hotKey.Key = System.Windows.Forms.Keys.None;
            var stringValue = hotKey.ToString();

            Assert.AreEqual("None", stringValue);
        }

        [Test]
        public void ToStringOutputsModifierAndKeyNameWhenOneModifierIsSet()
        {
            var hotKey = new HotKey(null);
            hotKey.Modifiers = ModifierKeys.Control;
            hotKey.Key = System.Windows.Forms.Keys.A;
            var stringValue = hotKey.ToString();

            Assert.AreEqual("Control + A", stringValue);
        }

        [Test]
        public void ToStringOutputsAllSetModifiersAndKeyNameWhenMultipleModifiersAreSet()
        {
            var hotKey = new HotKey(null);
            hotKey.Modifiers = ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Windows;
            hotKey.Key = System.Windows.Forms.Keys.A;
            var stringValue = hotKey.ToString();

            Assert.AreEqual("Control + Shift + Windows + A", stringValue);
        }
    }
}