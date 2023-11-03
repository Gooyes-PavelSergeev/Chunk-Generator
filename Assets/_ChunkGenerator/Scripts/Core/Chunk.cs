using GooyesPlugin;
using UnityEngine;

namespace CG
{
    public class Chunk : BasePoolObject
    {
        [SerializeField] private MeshRenderer _renderer;

        private Transform _obstacles;
        private Transform _decor;

        private System.Random _rnd;
        private int _selfSeed;

        public Vector2Int GridPosition { get; private set; }

        public void Init(int seed, Vector2Int position)
        {
            GridPosition = position;
            _selfSeed = seed + Extensions.HashIntVector(new Vector2Int(position.x - 1, position.y + 1));
            _rnd = new System.Random(_selfSeed);
            int perlinDepth = GameManager.Instance.ChunkGenerator.GenerateIntPerlin(position.x, position.y);
            Texture texture = Configs.Instance.Chunk.textures[perlinDepth];
            _renderer.material.mainTexture = texture;
        }
    }
}
