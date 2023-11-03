using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooyesPlugin
{
    public class Pool : Singleton<Pool>
    {
        [SerializeField] private PoolSections[] _content;

        private Dictionary<string, List<PoolGameObject>> _elementsInUse;
        private Dictionary<string, Queue<PoolGameObject>> _elementsMap;
        private Dictionary<string, Transform> _parentsMap;
        private Dictionary<string, PoolElement> _uncastedContent;

        public void Initialize()
        {
            _elementsInUse = new Dictionary<string, List<PoolGameObject>>();
            _elementsMap = new Dictionary<string, Queue<PoolGameObject>>();
            _parentsMap = new Dictionary<string, Transform>();
            _uncastedContent = new Dictionary<string, PoolElement>();

            for (int i = 0; i < _content.Length; i++)
            {
                PoolElement[] elements = _content[i].objects;
                for (int j = 0; j < elements.Length; j++)
                {
                    PoolElement element = elements[j];
                    _elementsInUse[element.name] = new List<PoolGameObject>();
                    _uncastedContent[element.name] = element;
                    for (int k = 0; k < element.size; k++)
                    {
                        PoolGameObject obj = _InstantiateNewElement(element);
                        obj.Active = false;
                    }
                }
            }
        }

        public static Component GetObject(string name)
        {
            return Instance._GetObject(name);
        }

        public static void ReturnObject(PoolObject poolComponent)
        {
            PoolGameObject obj = poolComponent.PoolGameObject;
            Instance._ReturnObject(obj);
        }

        public static void ReturnObject(GameObject gameObject)
        {
            PoolObject poolComponent = gameObject.GetComponent<PoolObject>();
            ReturnObject(poolComponent);
        }

        private Component _GetObject(string name)
        {
            if (!_elementsMap.ContainsKey(name))
                Debug.LogError($"There are no elements with name: {name}");
            Queue<PoolGameObject> pool = _elementsMap[name];
            if (pool.Count != 0)
            {
                return _TakeFromPool(name);
            }
            else
            {
                PoolElement poolElement = _uncastedContent[name];
                switch (poolElement.policy)
                {
                    case ResizePolicy.Grow:
                        _InstantiateNewElement(poolElement);
                        return _TakeFromPool(name);

                    case ResizePolicy.Reuse:
                        _ReturnObject(_elementsInUse[name][0]);
                        return _TakeFromPool(name);

                    case ResizePolicy.Limit:
                        return null;

                    default:
                        return null;
                }
            }
        }

        private Component _TakeFromPool(string name)
        {
            Queue<PoolGameObject> pool = _elementsMap[name];
            PoolGameObject obj = pool.Dequeue();
            obj.Active = true;
            _elementsInUse[name].Add(obj);
            return obj.component;
        }


        private void _ReturnObject(PoolGameObject poolObject)
        {
            string name = poolObject.poolComponent.PoolName;
            List<PoolGameObject> objectsInUse = _elementsInUse[name];
            if (!objectsInUse.Contains(poolObject))
            {
                if (poolObject.Active)
                    Debug.LogError("Object active but not in use", poolObject.poolComponent.gameObject);
                return;
            }
            poolObject.Active = false;
            objectsInUse.Remove(poolObject);
            _AddElement(name, poolObject);
        }

        private PoolGameObject _InstantiateNewElement(PoolElement poolElement)
        {
            Type type = Type.GetType(poolElement.type);
            PoolObject poolObject = Instantiate(poolElement.prefab);
            Component component = poolObject.GetComponent(type);
            PoolGameObject obj = new PoolGameObject
            {
                component = component,
                poolComponent = poolObject
            };
            if (component == null) Debug.LogError($"Component {type.Name} doesn't exist");
            poolObject.PoolInit(poolElement.name, obj);
            poolObject.gameObject.name = poolElement.name;
            _AddElement(poolElement.name, obj);
            return obj;
        }

        private void _AddElement(string name, PoolGameObject obj, bool setParent = true)
        {
            if (!_elementsMap.ContainsKey(name))
            {
                _elementsMap[name] = new Queue<PoolGameObject>();
                _parentsMap[name] = new GameObject(name).transform;
                _parentsMap[name].SetParent(transform);
            }
            _elementsMap[name].Enqueue(obj);
            if (setParent) obj.poolComponent.transform.SetParent(_parentsMap[name]);
        }

        public class PoolGameObject
        {
            public Component component;
            public PoolObject poolComponent;
            public bool Active
            {
                get { return poolComponent.gameObject.activeInHierarchy; }
                set { poolComponent.gameObject.SetActive(value); }
            }
        }

        [Serializable]
        private enum ResizePolicy
        {
            Grow,
            Reuse,
            Limit
        }

        [Serializable]
        private struct PoolElement
        {
            public string type;
            public string name;
            public PoolObject prefab;
            public int size;
            public ResizePolicy policy;
        }

        [Serializable]
        private struct PoolSections
        {
            public string name;
            public PoolElement[] objects;
        }
    }
}
