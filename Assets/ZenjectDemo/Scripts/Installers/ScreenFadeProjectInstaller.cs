using UnityEngine;
using Zenject;

namespace ZenjectDemo
{
    public class ScreenFadeProjectInstaller : MonoInstaller
    {
        [SerializeField] private ScreenFadeService _screenFadePrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScreenFadeService>()
                .FromComponentInNewPrefab(_screenFadePrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}
