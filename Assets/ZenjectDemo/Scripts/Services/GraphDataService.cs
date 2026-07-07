using System.Collections.Generic;
using WorldGraphEditor;

namespace ZenjectDemo
{
    public class GraphDataService : IGraphDataService
    {
        private readonly HashSet<string> _visitedPorts;
        private readonly HashSet<string> _shortcuts;

        private readonly IWorldGraph _worldGraph;

        public int OpenedShortcuts => _shortcuts.Count;

        public GraphDataService(IWorldGraph graph)
        {
            _visitedPorts = new HashSet<string>();
            _shortcuts = new HashSet<string>();
            
            _worldGraph = graph;
        }
        
        public void SetPortVisited(string guid)
        {
            _visitedPorts.Add(guid);

            if (_worldGraph.IsShortcutDestination(guid))
                _shortcuts.Add(guid);
        }

        public bool IsPortVisited(string guid)
        {
            return _visitedPorts.Contains(guid);
        }
    }
}