using Zenject;

namespace ZenjectDemo
{
    public class GameStatsProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GraphDataService>().AsSingle();
        }
    }
}