using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Canvas
{
    public class CanvasInstaller : MonoInstaller
    {
        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap tilemap;
        public override void InstallBindings()
        {
            Container.Bind<WorldSpaceInput>().FromNewComponentOnRoot().AsSingle();
            Container.Bind<TimeService>().FromComponentOn(gameObject).AsSingle().NonLazy();
            Container.Bind<Grid>().FromInstance(grid);
            Container.Bind<Tilemap>().FromInstance(tilemap);
        }
    }
}
