using UnityEngine;
using Zenject;

namespace ZenjectDemo
{
    public class PlayerControllerProjectInstaller : MonoInstaller
    {
        [SerializeField] private ZenjectPlayerController _playerPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<ZenjectPlayerController>().FromInstance(_playerPrefab);
            Container.BindInterfacesAndSelfTo<PlayerSpawnService>().AsSingle().NonLazy();
        }
    }
}