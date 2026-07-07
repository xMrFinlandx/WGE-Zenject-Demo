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
        public IWorldGraph Graph { get; }
        public ITransitionComponent OutputTransitionComponent { get; private set; }
        public ITransitionComponent InputTransitionComponent { get; private set; }
        
        public event Action TransitionStarted;
        public event Action SceneLoaded;
        public event Action TransitionEnded;

        public Vector3 PlayerSpawnPosition { get; private set; }

        private readonly CancellationTokenSource _cts;
        private readonly IScreenFadeService _screenFadeService;
        private readonly IGraphDataService _graphDataService;
        
        public TransitionManagerService(IWorldGraph graph, IScreenFadeService screenFadeService, IGraphDataService graphDataService)
        {
            Graph = graph;
            _cts = new CancellationTokenSource();
            _screenFadeService = screenFadeService;
            _graphDataService = graphDataService;
        }   
        
        public async Task GoToAsync(string currentPortGuid, string targetPortGuid, ITransitionContext context = null)
        {
            RuntimeTransitionData data = Graph.GetTeleportTransitionData(currentPortGuid, targetPortGuid);
            
            await GoInternalAsync(data);
        }

        public async Task GoFromAsync(string currentPortGuid, ITransitionContext context = null)
        {
            var isPortVisited = _graphDataService.IsPortVisited(currentPortGuid);
            
            if (!Graph.CanPassTransition(currentPortGuid, isPortVisited, out var status))
            {
                Debug.LogWarning($"Can`t pass transition, status: {status}");
                return;
            }
            
            if (!Graph.TryGetPassageTransitionData(currentPortGuid, out RuntimeTransitionData data))
            {
                Debug.LogError("No transition data for this passage");
                return;
            }
            
            await GoInternalAsync(data);
        }

        private async Task GoInternalAsync(RuntimeTransitionData data)
        {
            try
            {
                TransitionStarted?.Invoke();

                await _screenFadeService.ShowAsync(_cts.Token);

                InputTransitionComponent = TransitionComponentUtility.FindByGuid(data.CurrentPassageGuid);
                
                _graphDataService.SetPortVisited(data.CurrentPassageGuid);

                await SceneManager.LoadSceneAsync(data.TargetSceneBuildIndex);

                OutputTransitionComponent = TransitionComponentUtility.FindByGuid(data.TargetPassageGuid);
                PlayerSpawnPosition = OutputTransitionComponent?.GetSpawnPosition() ?? Vector3.zero;

                if (OutputTransitionComponent == null)
                {
                    Debug.LogError("failed to find output");
                }
                else
                {
                    _graphDataService.SetPortVisited(data.TargetPassageGuid);
                }
                
                SceneLoaded?.Invoke();
                
                await _screenFadeService.HideAsync(_cts.Token);
                
                TransitionEnded?.Invoke();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Transition cancelled");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}