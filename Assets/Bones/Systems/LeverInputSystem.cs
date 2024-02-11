using Bones.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Bones.Systems
{
    public class LeverInputSystem : IEcsRunSystem
    {
        private EcsFilter<EdgeComponent, EdgeLeverComponent> _edgesFilter; 

        public void Run()
        {
            foreach (int i in _edgesFilter)
            {
                ref EdgeComponent edge = ref _edgesFilter.Get1(i);
                EdgeLeverComponent lever = _edgesFilter.Get2(i);

                edge.Length = Input.GetKey(lever.Key)
                    ? lever.DefaultLengthValue * lever.ActivatedLengthPercent
                    : lever.DefaultLengthValue;
            }
        }
    }
}