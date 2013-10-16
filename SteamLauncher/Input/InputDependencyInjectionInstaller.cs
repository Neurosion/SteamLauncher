using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace SteamLauncher.Domain.Input
{
    public class InputDependencyInjectionInstaller : DependencyInjectionInstallerBase
    {
        public override void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IHookRegistrationController>()
                                        .ImplementedBy<WindowsHookRegistrationController>());
            
            container.Register(Component.For<IHotKeyRegistrationController>()
                                        .ImplementedBy<WindowsHotKeyRegistrationController>());

            container.Register(Component.For<IHotKey>()
                                        .ImplementedBy<HotKey>()
                                        .UsingFactoryMethod((kernel, context) => 
                                            new HotKey(kernel.Resolve<IHotKeyRegistrationController>())
                                            {
                                                Key = (Keys)Enum.Parse(typeof(Keys), (string)Dependency.OnAppSettingsValue("HotKey.Key").Value),
                                                Modifiers = (ModifierKeys)Enum.Parse(typeof(ModifierKeys), (string)Dependency.OnAppSettingsValue("HotKey.Modifiers").Value)
                                            }));
        }
    }
}