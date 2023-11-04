using UnityEngine;

namespace CG
{
    [CreateAssetMenu(fileName = "Chunk Data", menuName = "Configs/Chunk Data")]
    public class ChunkConfig : ScriptableObject
    {
        public Vector2 worldChunkSize = new Vector2(4, 4);
        public Texture[] textures;

        public int obstaclesAvailable;
        public int decorationsAvailable;

        [SerializeField] private AnimationCurve _obstaclesDistribution;
        [SerializeField] private AnimationCurve _decorationsDistribution;
        [SerializeField] private AnimationCurve _wallDistribution;

        public int GetNumberOfObstacles(System.Random rnd)
        {
            int num = rnd.Next(0, 100);
            int result = Mathf.RoundToInt(_obstaclesDistribution.Evaluate(num / 100f));
            return result;
        }

        public int GetNumberOfDecorations(System.Random rnd)
        {
            int num = rnd.Next(0, 100);
            int result = Mathf.RoundToInt(_decorationsDistribution.Evaluate(num / 100f));
            return result;
        }

        public int GetNumberOfWalls(System.Random rnd)
        {
            int num = rnd.Next(0, 100);
            int result = Mathf.RoundToInt(_wallDistribution.Evaluate(num / 100f));
            if (result > 4) throw new System.Exception("Cant be more then 4 walls");
            return result;
        }
    }
}
