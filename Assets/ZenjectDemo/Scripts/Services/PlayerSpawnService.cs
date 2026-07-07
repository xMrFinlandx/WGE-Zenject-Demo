using System;
using UnityEngine;
using WorldGraphEditor;
using Object = UnityEngine.Object;

namespace ZenjectDemo
{
    public class PlayerSpawnService : IDisposable
    {
        private readonly ITransitionManager _manager;
        private readonly ZenjectPlayerController _playerPrefab;
        
        public PlayerSpawnService(ITransitionManager manager, ZenjectPlayerController playerController)
        {
            _manager = manager;
            _playerPrefab = playerController;
            
            _manager.TransitionEnded += SpawnPlayer;
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            var playerController = Object.Instantiate(_playerPrefab, _manager.PlayerSpawnPosition, Quaternion.identity);

            if (_manager.OutputTransitionComponent is IPusher pusher)
                playerController.SetPushForce(pusher.GetPushData());

            playerController.Enable();
        }

        public void Dispose()
        {
            _manager.TransitionEnded -= SpawnPlayer;
        }
    }
}