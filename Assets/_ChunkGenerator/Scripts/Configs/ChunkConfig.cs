using UnityEngine;

namespace CG
{
    [CreateAssetMenu(fileName = "Chunk Data", menuName = "Configs/Chunk Data")]
    public class ChunkConfig : ScriptableObject
    {
        public Vector2 worldSize = new Vector2(10, 10);
        public Texture[] textures;

        [SerializeField] private Vector2Int _obstaclesRange;
        [SerializeField] private Vector2Int _decorationsRange;

        public int GetNumberOfObstacles(int chunkSeed)
        {
            System.Random r = new System.Random(chunkSeed);
            int num = r.Next(_obstaclesRange.x, _obstaclesRange.y);
            return num;
        }

        public int GetNumberOfDecorations(int chunkSeed)
        {
            System.Random r = new System.Random(chunkSeed);
            r.Next();
            int num = r.Next(_decorationsRange.x, _decorationsRange.y);
            return num;
        }
    }
}
