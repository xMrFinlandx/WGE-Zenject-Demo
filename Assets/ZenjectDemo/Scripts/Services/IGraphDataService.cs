namespace ZenjectDemo
{
    public interface IGraphDataService
    {
        public int OpenedShortcuts { get; }

        public void SetPortVisited(string guid);
        public bool IsPortVisited(string guid);
    }
}