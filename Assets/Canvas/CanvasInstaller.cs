using UnityEngine;
using Zenject;

namespace Canvas
{
    public class CanvasInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WorldSpaceInput>().FromNewComponentOnRoot().AsSingle();
        }
    }
}
