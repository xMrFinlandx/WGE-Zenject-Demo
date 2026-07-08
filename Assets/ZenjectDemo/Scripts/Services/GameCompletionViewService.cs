using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WorldGraphEditor;
using Zenject;

namespace ZenjectDemo
{
    public class GameCompletionViewService : MonoBehaviour
    {
        [SerializeField] private Text _sceneNameText;
        [SerializeField] private Text _shortcutsText;
        [SerializeField] private Text _scenesText;

        private ITransitionManager _manager;
        private IWorldGraph _graph;
        private IGraphDataService _graphDataService;
        private int _shortcutsCount;
        
        [Inject]
        private void Constructor(ITransitionManager manager, IGraphDataService graphDataService)
        {
            _manager = manager;
            _graph = manager.Graph;
            _graphDataService = graphDataService;
            
            _manager.SceneLoaded += OnSceneLoaded;

            _shortcutsCount = _graph.GetEdgesData().Count(item =>
                _graph.IsShortcutDestination(item.GetFromPortGuid()) ||
                _graph.IsShortcutDestination(item.GetToPortGuid()));
        }

        private void Start()
        {
            OnSceneLoaded();
        }

        private void OnSceneLoaded()
        {
            var portGuid = _manager.OutputTransitionComponent != null 
                ? _manager.OutputTransitionComponent?.GetGuid() 
                : TransitionComponentUtility.FindAny()?.GetGuid();

            if (_graph.TryGetSceneDataByPortGuid(portGuid, out var data))
                _sceneNameText.text = $"Current Scene: {data.SceneName}";

            _shortcutsText.text = $"Visited shortcuts: {_graphDataService.OpenedShortcuts}/{_shortcutsCount}";
        }

        private void OnDestroy()
        {
            _manager.SceneLoaded -= OnSceneLoaded;
        }
    }
}