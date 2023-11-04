using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace CG
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _chunkSpawnDistance = 40;
        private int _chunkSpawnGridDistance;
        [SerializeField] private Player _player;
        private Vector3 _offset;

        private Dictionary<Vector2Int, Chunk> _chunkDrawMap;

        private void Start()
        {
            _offset = transform.position - _player.transform.position;
            _chunkDrawMap = new Dictionary<Vector2Int, Chunk>();
            _chunkSpawnGridDistance = Mathf.RoundToInt(_chunkSpawnDistance / Configs.Instance.Chunk.worldChunkSize.x);
            StartCoroutine(DrawerRoutine());
        }

        private void Update()
        {
            transform.position = _player.transform.position + _offset;
        }

        private IEnumerator DrawerRoutine()
        {
            yield return null;
            while (true)
            {
                List<Vector2Int> nearestChunks = GetGridPositions();
                Vector2Int[] rendered = _chunkDrawMap.Keys.ToArray();
                for (int i = 0; i < rendered.Length; i++)
                {
                    Vector2Int pos = rendered[i];
                    if (!nearestChunks.Contains(pos))
                    {
                        _chunkDrawMap[pos].Recycle();
                        _chunkDrawMap.Remove(pos);
                    }
                }
                foreach (Vector2Int pos in nearestChunks)
                {
                    if (!_chunkDrawMap.ContainsKey(pos))
                    {
                        Chunk chunk = GameManager.Instance.WorldGenerator.GenerateChunk(pos);
                        _chunkDrawMap[pos] = chunk;
                    }
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        private List<Vector2Int> GetGridPositions()
        {
            Vector3 normalizedCurrent = _player.transform.position / Configs.Instance.Chunk.worldChunkSize.x;
            Vector2Int current = new Vector2Int(
                Mathf.RoundToInt(normalizedCurrent.x) - _chunkSpawnGridDistance / 2,
                Mathf.RoundToInt(normalizedCurrent.z) - _chunkSpawnGridDistance / 2);
            List<Vector2Int> posSet = new List<Vector2Int>();
            for (int i = 0; i < _chunkSpawnGridDistance; i++)
            {
                for (int j = 0; j < _chunkSpawnGridDistance; j++)
                {
                    Vector2Int pos = new Vector2Int(i, j);
                    pos += current;
                    posSet.Add(pos);
                }
            }
            return posSet;
        }
    }
}
