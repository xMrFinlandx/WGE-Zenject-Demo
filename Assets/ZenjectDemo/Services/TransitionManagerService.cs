using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using WorldGraphEditor;

namespace ZenjectDemo
{
    public class TransitionManagerService : ITransitionManager, IDisposable
    {
        public WorldGraph Graph { get; }
        public ITransitionComponent OutputTransitionComponent { get; private set; }
        public ITransitionComponent InputTransitionComponent { get; private set; }
        
        public event Action TransitionStarted;
        public event Action SceneLoaded;
        public event Action TransitionEnded;

        public Vector3 PlayerSpawnPosition { get; private set; }

        private readonly CancellationTokenSource _cts;
        private readonly IScreenFadeService _screenFadeService;
        
        public TransitionManagerService(WorldGraph graph, IScreenFadeService screenFadeService)
        {
            Graph = graph;
            _cts = new CancellationTokenSource();
            _screenFadeService = screenFadeService;
        }   
        
        public async void GoTo(string currentPortGuid, string targetPortGuid, ITransitionContext context = null)
        {
            RuntimeTransitionData data = Graph.GetTeleportTransitionData(currentPortGuid, targetPortGuid);
            
            await GoInternal(data);
        }

        public async void GoFrom(string currentPortGuid, bool ignoreShortcuts, ITransitionContext context = null)
        {
            if (!Graph.CanPassTransition(currentPortGuid, ignoreShortcuts, out var status))
            {
                Debug.LogWarning($"Can`t pass transition, status: {status}");
                return;
            }
            
            if (!Graph.TryGetPassageTransitionData(currentPortGuid, out RuntimeTransitionData data))
            {
                Debug.LogError("No transition data for this passage");
                return;
            }
            
            await GoInternal(data);
        }

        private async Task GoInternal(RuntimeTransitionData data)
        {
            TransitionStarted?.Invoke();
            
            await _screenFadeService.ShowAsync(_cts.Token);
            
            InputTransitionComponent = TransitionComponentUtility.FindByGuid(data.CurrentPassageGuid);

            await SceneManager.LoadSceneAsync(data.TargetSceneBuildIndex);

            OutputTransitionComponent = TransitionComponentUtility.FindByGuid(data.TargetPassageGuid);
            PlayerSpawnPosition = OutputTransitionComponent?.GetSpawnPosition() ?? Vector3.zero;

            if (OutputTransitionComponent == null)
                Debug.LogError("failed to find output");
            
            SceneLoaded?.Invoke();
            
            await _screenFadeService.HideAsync(_cts.Token);
            
            TransitionEnded?.Invoke();
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}