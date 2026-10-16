using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TD
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        public readonly Queue<T> freeObjects = new();

        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                T obj = CreateObject();
                Release(obj);
            }
        }

        private T CreateObject()
        {
            return Object.Instantiate(_prefab, _parent);
        }

        public T Get()
        {
            T obj;

            if (freeObjects.Count > 0)
            {
                obj = freeObjects.Dequeue();
            }
            else
            {   
                obj = CreateObject();
            }

            obj.OnGet();
            return obj;
        }

        public void Release(T obj)
        {
            obj.OnRelease();
            freeObjects.Enqueue(obj);
        }

        public void Release(T obj, float delay, Action callback, CancellationToken token = default)
        {
            DelayedRelease(obj, delay, callback, token).Forget();
        }

        private async UniTask DelayedRelease(T obj, float delay, Action callback, CancellationToken token = default)
        {
            await UniTask.Delay((int)delay * 1000, cancellationToken: token);

            Release(obj);
            callback?.Invoke();
        }
    }
}
