using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using System;

public class ResManager : Singleton<ResManager>
{
    public T Load<T>(string key) where T : UnityEngine.Object
    {
        return Addressables.LoadAsset<T>(key).Result;
    }

    public void LoadAsync<T>(string key, Action<T> callback) where T : UnityEngine.Object
    {
        Addressables.LoadAssetAsync<T>(key).Completed += (AsyncOperationHandle<T> obj) =>
        {
            callback(obj.Result);
        };
    }

    public void LoadAsync<T>(string key, Action<T> callback, Action<float> progress) where T : UnityEngine.Object
    {
        Addressables.LoadAssetAsync<T>(key).Completed += (AsyncOperationHandle<T> obj) =>
        {
            callback(obj.Result);
        };
    }

    public void LoadAsync<T>(string key, Action<T> callback, Action<float> progress, Action<Exception> error) where T : UnityEngine.Object
    {
        Addressables.LoadAssetAsync<T>(key).Completed += (AsyncOperationHandle<T> obj) =>
        {
            callback(obj.Result);
        };
    }
    public async UniTask<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        return await Addressables.LoadAssetAsync<T>(key);
    }


    public void Unload(string key)
    {
        Addressables.Release(key);
    }

    public void Unload<T>(T obj) where T : UnityEngine.Object
    {
        Addressables.Release(obj);
    }

    public void UnloadAll()
    {
        Addressables.ReleaseInstance(Addressables.InstantiateAsync("Assets/Prefabs/Player.prefab").Result);
    }
}
