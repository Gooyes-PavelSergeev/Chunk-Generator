using GooyesPlugin;
using System.Collections.Generic;
using UnityEngine;

namespace CG
{
    public class Chunk : BasePoolObject
    {
        [SerializeField] private MeshRenderer _renderer;

        [SerializeField] private Transform _obstacles;
        [SerializeField] private Transform _decor;
        [SerializeField] private Transform _walls;

        private System.Random _rnd;
        private int _selfSeed;

        private ChunkConfig _config;

        public Vector2Int GridPosition { get; private set; }

        public override void PoolInit(PoolObject poolObject)
        {
            base.PoolInit(poolObject);
            _config = Configs.Instance.Chunk;
        }

        public void Init(int seed, Vector2Int position)
        {
            GridPosition = position;
            _selfSeed = seed + Extensions.HashIntVector(new Vector2Int(position.x - 1, position.y + 1));
            _rnd = new System.Random(_selfSeed);
            int perlinDepth = GameManager.Instance.WorldGenerator.GenerateIntPerlin(position.x, position.y);
            Texture texture = Configs.Instance.Chunk.textures[perlinDepth];
            _renderer.material.mainTexture = texture;

            FillWillObjects();
        }

        private void FillWillObjects()
        {
            for (int i = 0; i < _config.GetNumberOfObstacles(_rnd); i++)
            {
                int index = _rnd.Next(0, _config.obstaclesAvailable - 1);
                string obstaclePoolName = $"O_{index + 1}";
                PoolObject obstacle = Pool.GetObject(obstaclePoolName) as PoolObject;
                obstacle.transform.SetParent(_obstacles);

                float rndFloat = _rnd.Next(0, 100) / 100f;

                obstacle.transform.localPosition = Vector3.zero + new Vector3(
                    (rndFloat - 0.5f) * _config.worldChunkSize.x,
                    0,
                    (rndFloat - 0.5f) * _config.worldChunkSize.y);
            }

            for (int i = 0; i < _config.GetNumberOfDecorations(_rnd); i++)
            {
                int index = _rnd.Next(0, _config.decorationsAvailable - 1);
                string decorPoolName = $"P_{index + 1}";
                PoolObject prop = Pool.GetObject(decorPoolName) as PoolObject;
                prop.transform.SetParent(_decor);

                float rndFloat = _rnd.Next(0, 100) / 100f;

                prop.transform.localPosition = Vector3.zero + new Vector3(
                    (rndFloat - 0.5f) * _config.worldChunkSize.x,
                    0,
                    (rndFloat - 0.5f) * _config.worldChunkSize.y);
            }

            List<int> freeWalls = new List<int> { 0, 1, 2, 3 };
            for (int i = 0; i < _config.GetNumberOfWalls(_rnd); i++)
            {
                int wallIndex = 0;
                while (freeWalls.Count > 0)
                {
                    wallIndex = _rnd.Next(0, 4);
                    if (freeWalls.Contains(wallIndex))
                    {
                        freeWalls.Remove(wallIndex);
                        break;
                    }
                }
                PoolObject wall = Pool.GetObject("WALL") as PoolObject;
                wall.transform.SetParent(_walls);
                wall.transform.eulerAngles = new Vector3(0, (wallIndex % 2 - 1) * 90, 0);
                if (wallIndex % 2 == 1) wall.transform.localPosition = new Vector3(-(wallIndex - 2) * _config.worldChunkSize.x / 2, 0, 0);
                else wall.transform.localPosition = new Vector3(0, 0, -(wallIndex - 1) * _config.worldChunkSize.y / 2);
            }
        }

        public override void Recycle()
        {
            foreach (Transform child in _obstacles)
            {
                Pool.ReturnObject(child.gameObject);
            }
            foreach (Transform child in _decor)
            {
                Pool.ReturnObject(child.gameObject);
            }
            foreach (Transform child in _walls)
            {
                Pool.ReturnObject(child.gameObject);
            }
            base.Recycle();
        }
    }
}
