using UnityEngine;
using Zenject;

namespace ZenjectDemo
{
    public class GameCompletionViewProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameCompletionViewService _viewService;
        
        public override void InstallBindings()
        {
            Container.Bind<GameCompletionViewService>()
                .FromComponentInNewPrefab(_viewService)
                .AsSingle()
                .NonLazy();
        }
    }
}