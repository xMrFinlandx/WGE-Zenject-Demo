using WorldGraphEditor;
using Zenject;

namespace ZenjectDemo
{
    public class TransitionManagerProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var graph = WGEProjectConfig.Instance.GetWorldGraph();

            Container.BindInterfacesAndSelfTo<WorldGraph>().FromInstance(graph).AsSingle();
            Container.BindInterfacesAndSelfTo<GraphDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TransitionManagerService>().AsSingle();
        }
    }
}