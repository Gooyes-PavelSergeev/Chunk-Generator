using UnityEngine;

namespace GooyesPlugin
{
    public class BasePoolObject : MonoBehaviour, IPoolObject
    {
        protected PoolObject _poolComponent;

        public virtual void PoolInit(PoolObject poolObject)
        {
            _poolComponent = poolObject;
        }

        public virtual void Recycle()
        {
            Pool.ReturnObject(_poolComponent);
        }
    }
}
