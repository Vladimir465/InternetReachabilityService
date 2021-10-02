﻿using Services.Impls;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InternetReachabilityService>().AsSingle();
        }
    }
}