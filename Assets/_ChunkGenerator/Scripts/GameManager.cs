using GooyesPlugin;
using UnityEngine;

namespace CG
{
    public class GameManager : Singleton<GameManager>
    {
        [field: SerializeField] public ChunkGenerator ChunkGenerator { get; private set; }

        private void Start()
        {
            Pool.Instance.Initialize();
        }
    }
}
