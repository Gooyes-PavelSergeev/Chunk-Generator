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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TEMP_CREATE_MAP();
            }
        }

        private void TEMP_CREATE_MAP()
        {
            for (int i = 0; i < TEMP_MAP_SIZE; i++)
            {
                for (int j = 0; j < TEMP_MAP_SIZE; j++)
                {
                    Vector2 chunkSize = Configs.Instance.Chunk.worldSize;
                    Vector3 worldPos = new Vector3(chunkSize.x * i, 0, chunkSize.y * j);
                    Chunk chunk = Pool.GetObject(Constants.Pool.BASE_CHUNK) as Chunk;
                    chunk.transform.position = worldPos;
                    chunk.Init(_seed, new Vector2Int(i, j));
                }
            }
        }

        public int GenerateIntPerlin(int x, int y)
        {
            //float perlinF = (float)Perlin.OctavePerlin(x / 255f, y / 255f, 0, 5, 4);
            float perlinF = Mathf.PerlinNoise(_seed + x * _octave / 100, _seed + y  * _octave / 100);
            Debug.Log($"{perlinF}. {x}. {y}");
            perlinF *= _bits - 1;
            perlinF = Mathf.Clamp(perlinF, 0f, _bits - 1);
            int perlin = Mathf.RoundToInt(perlinF);
            return perlin;
        }
    }
}
