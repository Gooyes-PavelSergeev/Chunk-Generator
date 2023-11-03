using GooyesPlugin;
using UnityEngine;

namespace CG
{
    public class Configs : Singleton<Configs>
    {
        [field: SerializeField] public ChunkConfig Chunk { get; private set; }

        protected override void Init()
        {
            
        }
    }
}
