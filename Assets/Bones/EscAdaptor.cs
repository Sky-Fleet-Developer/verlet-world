using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using UnityEngine;

namespace Bones
{
    public abstract class EscAdaptor : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems[] _systems;

        public EcsWorld World => _world;
        public event Action WorldDestroying;
        private TaskCompletionSource<bool> _outsideSystemsAdd = new TaskCompletionSource<bool>();
        public bool IsInitialized => _isInitialized;
        private bool _isInitialized;
        public virtual void Init()
        {
            _world = new EcsWorld();
            _systems = Create(NewSystemGroup).ToArray();
            _outsideSystemsAdd?.SetResult(true);
            foreach (EcsSystems group in _systems)
            {
                Inject(group.Inject(_world)).Init();   
#if UNITY_EDITOR
                EcsSystemsObserver.Create(group);
#endif
            }
            _outsideSystemsAdd = new TaskCompletionSource<bool>();
            _isInitialized = true;
        }

        public async void AddSystemDelayed(int groupIdx, IEcsSystem system)
        {
            await _outsideSystemsAdd.Task;
            _systems[groupIdx].Add(system);
        }

        private EcsSystems NewSystemGroup() => new EcsSystems(_world);
        
        protected virtual IEnumerable<EcsSystems> Create(Func<EcsSystems> creator)
        {
            yield break;
        }
        
        protected virtual EcsSystems Inject(EcsSystems systems)
        {
            return systems;
        }

        public EcsSystems GetSystemsGroup(int index) => _systems[index];

        private void OnDestroy()
        {
            WorldDestroying?.Invoke();
        }

        public void DestroyWorld()
        {
            WorldDestroying?.Invoke();
            _world?.Destroy();
        }
    }
}
