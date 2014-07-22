﻿using System.Windows;
﻿using Caliburn.Micro;
﻿using Motorcycle.ViewModels;

namespace Motorcycle
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}