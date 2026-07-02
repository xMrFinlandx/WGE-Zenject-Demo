namespace ZenjectDemo
{
    public interface IGraphDataService
    {
        public void SetVisited(string guid);
        public bool IsVisited(string guid);
    }
}