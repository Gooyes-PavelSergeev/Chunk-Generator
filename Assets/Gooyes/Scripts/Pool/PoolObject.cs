using UnityEngine;

namespace GooyesPlugin
{
    public class PoolObject : MonoBehaviour
    {
        public string PoolName { get; set; }
        public Pool.PoolGameObject PoolGameObject { get; set; }

        public void PoolInit(string poolName, Pool.PoolGameObject poolGO)
        {
            PoolName = poolName;
            PoolGameObject = poolGO;
            IPoolObject[] objectsToInit = GetComponents<IPoolObject>();
            foreach (IPoolObject obj in objectsToInit) obj.PoolInit(this);
        }
    }
}
