using Leopotam.Ecs;

namespace Bones
{
    public abstract class GraphHandlerSystemBase : IEcsRunSystem
    {
         private EcsFilter<NodeComponent> _nodesFilter;
         private EcsFilter<EdgeComponent> _edgesFilter;

         public virtual void Run()
         {
             foreach (int i in _edgesFilter)
             {
                 ref EdgeComponent edgeComponent = ref _edgesFilter.Get1(i);
                 ref NodeComponent nodeA = ref _nodesFilter.Get1(edgeComponent.NodeA);
                 ref NodeComponent nodeB = ref _nodesFilter.Get1(edgeComponent.NodeB);
                 Handle(ref nodeA, ref nodeB, ref edgeComponent);
             }
         }

         public abstract void Handle(ref NodeComponent nodeA, ref NodeComponent nodeB, ref EdgeComponent edge);
    }
}