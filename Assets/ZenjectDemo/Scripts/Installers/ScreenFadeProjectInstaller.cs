using UnityEngine;
using Zenject;

namespace ZenjectDemo
{
    public class ScreenFadeProjectInstaller : MonoInstaller
    {
        [SerializeField] private ScreenFade _screenFadePrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScreenFade>()
                .FromComponentInNewPrefab(_screenFadePrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}
