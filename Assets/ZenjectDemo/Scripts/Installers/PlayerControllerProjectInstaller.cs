using UnityEngine;
using Zenject;

namespace ZenjectDemo
{
    public class PlayerControllerProjectInstaller : MonoInstaller
    {
        [SerializeField] private ZenjectPlayerController _playerPrefab;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ZenjectPlayerController>()
                .FromComponentInNewPrefab(_playerPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}