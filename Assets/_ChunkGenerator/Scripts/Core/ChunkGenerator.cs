using GooyesPlugin;
using UnityEngine;

namespace CG
{
    public class ChunkGenerator : MonoBehaviour
    {
        [SerializeField] private float _octave = 100;
        [SerializeField] private int _seed = 0;
        [SerializeField] private int _bitDepth = 3;
        [SerializeField] private int TEMP_MAP_SIZE;
        private int _bits;

        private void Start()
        {
            _bits = Mathf.RoundToInt(Mathf.Pow(2, _bitDepth));
            if (Configs.Instance.Chunk.textures.Length != _bits)
            {
                Debug.LogError("Wrong textures number");
            }
        }

        public Chunk GenerateChunk(Vector2Int position)
        {
            Vector2 chunkSize = Configs.Instance.Chunk.worldSize;
            Vector3 worldPos = new Vector3(chunkSize.x * position.x, 0, chunkSize.y * position.y);
            Chunk chunk = Pool.GetObject(Constants.Pool.BASE_CHUNK) as Chunk;
            chunk.transform.position = worldPos;
            chunk.Init(_seed, position);
            return chunk;
        }

        public int GenerateIntPerlin(int x, int y)
        {
            float perlinF = Mathf.PerlinNoise(_seed + x * _octave / 100, _seed + y  * _octave / 100);
            perlinF *= _bits - 1;
            perlinF = Mathf.Clamp(perlinF, 0f, _bits - 1);
            int perlin = Mathf.RoundToInt(perlinF);
            return perlin;
        }
    }
}
