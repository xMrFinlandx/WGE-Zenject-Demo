using System.Collections.Generic;

namespace ZenjectDemo
{
    public class GraphDataService : IGraphDataService
    {
        private readonly HashSet<string> _visitedPorts;

        public GraphDataService()
        {
            _visitedPorts = new HashSet<string>();
        }
        
        public void SetVisited(string guid)
        {
            _visitedPorts.Add(guid);
        }

        public bool IsVisited(string guid)
        {
            return _visitedPorts.Contains(guid);
        }
    }
}