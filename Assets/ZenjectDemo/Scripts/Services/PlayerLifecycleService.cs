using System;
using UnityEngine;
using WorldGraphEditor;
using Object = UnityEngine.Object;

namespace ZenjectDemo
{
    public class PlayerLifecycleService : IDisposable
    {
        private readonly ITransitionManager _manager;
        private readonly ZenjectPlayerController _playerPrefab;
        private ZenjectPlayerController _playerController;
        
        public PlayerLifecycleService(ITransitionManager manager, ZenjectPlayerController playerController)
        {
            _manager = manager;
            _playerPrefab = playerController;
            
            _manager.TransitionEnded += SpawnPlayer;
            _manager.TransitionStarted += PullPlayer;
            SpawnPlayer();
        }

        private void PullPlayer()
        {
            if (_playerController == null)
                return;

            _playerController.DisableMovement();
            
            if (_manager.InputTransitionComponent is IPuller puller)
                _playerController.SetPullForce(puller.GetPullData());
        }

        private void SpawnPlayer()
        {
            _playerController = Object.Instantiate(_playerPrefab, _manager.NextSpawnPosition, Quaternion.identity);

            if (_manager.OutputTransitionComponent is IPusher pusher)
                _playerController.SetPushForce(pusher.GetPushData());

            _playerController.EnableMovement();
        }

        public void Dispose()
        {
            _manager.TransitionEnded -= SpawnPlayer;
            _manager.TransitionStarted -= PullPlayer;
        }
    }
}