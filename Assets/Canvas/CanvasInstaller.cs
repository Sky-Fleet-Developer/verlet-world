using UnityEngine;
using Zenject;

namespace Canvas
{
    public class CanvasInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WorldSpaceInput>().FromNewComponentOnRoot().AsSingle();
            Container.Bind<TimeService>().FromComponentOn(gameObject).AsSingle().NonLazy();
        }
    }
}
