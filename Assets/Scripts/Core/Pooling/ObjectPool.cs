using System;
using System.Collections.Generic;
using UnityEngine;

namespace CFD.Core.Pooling
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        protected Transform parent;
        private readonly T _prefab;
        private readonly Queue<T> _pool;
        private readonly List<T> _activeObjects;
        private readonly bool _autoExpand;
        private readonly int _initialSize;

        public int ActiveCount => _activeObjects.Count;
        public int InactiveCount => _pool.Count;
        public int TotalCount => ActiveCount + InactiveCount;

        public ObjectPool(T prefab, Transform parent = null, int initialSize = 0, bool autoExpand = true)
        {
            _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
            this.parent = parent;
            _initialSize = initialSize;
            _autoExpand = autoExpand;
            _pool = new Queue<T>();
            _activeObjects = new List<T>();
        }

        public virtual void Initialize()
        {
            PreWarm();
        }
        
        private void PreWarm()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                T obj = CreateNewObject();
                _pool.Enqueue(obj);
            }
        }

        public T Get()
        {
            T obj;
            
            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
            }
            else if (_autoExpand)
            {
                obj = CreateNewObject();
            }
            else
            {
                throw new InvalidOperationException($"Pool is empty and auto-expand is disabled. Consider increasing initial size or enabling auto-expand.");
            }

            obj.gameObject.SetActive(true);
            _activeObjects.Add(obj);
            return obj;
        }

        public void Return(T obj)
        {
            if (obj == null) return;
            
            if (_activeObjects.Contains(obj))
            {
                _activeObjects.Remove(obj);
                obj.gameObject.SetActive(false);
                
                if (parent != null)
                {
                    obj.transform.SetParent(parent, false);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localRotation = Quaternion.identity;
                    obj.transform.localScale = Vector3.one;
                }
                
                _pool.Enqueue(obj);
            }
        }

        public void ReturnAll()
        {
            for (int i = _activeObjects.Count - 1; i >= 0; i--)
            {
                Return(_activeObjects[i]);
            }
        }

        public void Clear()
        {
            ReturnAll();
            
            while (_pool.Count > 0)
            {
                T obj = _pool.Dequeue();
                if (obj != null)
                {
                    UnityEngine.Object.DestroyImmediate(obj.gameObject);
                }
            }
        }

        protected virtual T CreateNewObject()
        {
            T obj = UnityEngine.Object.Instantiate(_prefab, parent);
            obj.gameObject.SetActive(false);
            return obj;
        }
    }
}
