using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace Kitchen.Scripts.Loading
{
    public static class LoadingManager
    {
        public static Dictionary<string, LoadingItem> LoadingOperations
        {
            get;
        } = new Dictionary<string, LoadingItem>();
        
        public static async UniTask Load(CancellationToken token, params string[] keys)
        {
            var handles = new List<UniTask>();
            foreach (var key in keys)
            {
                if (LoadingOperations.TryGetValue(key, out var handler))
                {
                    handles.Add(handler.Handle.ToUniTask(cancellationToken: token));
                    break;
                }
                LoadingOperations[key] = new LoadingItem
                {
                    Key = key,
                    Handle = Addressables.LoadAssetAsync<GameObject>(key),
                    InstantiationCount = 0,
                    Instances = new List<GameObject>()
                };
                handles.Add(LoadingOperations[key].Handle.ToUniTask(cancellationToken: token));
            }
            await UniTask.WhenAll(handles);
        }
        
        public static async UniTask<GameObject> Instantiate(string key, CancellationToken token)
        {
            if (!LoadingOperations.TryGetValue(key, out var handler))
            {
                await Load(CancellationToken.None, key);
            }
            Assert.IsTrue(LoadingOperations.TryGetValue(key, out handler), $"Can't load {key}");
            Assert.AreNotEqual(handler.Handle.Status, AsyncOperationStatus.Failed, $"Failed to load {key}");

            handler.InstantiationCount += 1;
            var prefab = handler.Handle.Result;
            var instance = Object.Instantiate(prefab);
            handler.Instances.Add(instance);
            return instance;
        }
        
        public static void Release(string key, GameObject gameObject)
        {
            Assert.IsTrue(LoadingOperations.TryGetValue(key, out var handler), $"{key} is not loaded");
            handler.InstantiationCount -= 1;
            handler.Instances.Remove(gameObject);
            Object.Destroy(gameObject);
        }
        
        public static void Clear()
        {
            LoadingOperations.Clear();
        }
    }

    public class LoadingItem
    {
        public string Key;
        public AsyncOperationHandle<GameObject> Handle;
        public int InstantiationCount;
        public List<GameObject> Instances;
    } 
}
