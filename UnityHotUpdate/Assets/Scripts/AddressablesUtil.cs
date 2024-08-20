using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressablesUtil
{
    public static void InstantiateAsync(ref AsyncOperationHandle<GameObject> handle, string key, Action<AsyncOperationHandle<GameObject>> action, Transform parent = null)
    {
        ReleaseInstance(ref handle, action);
        handle = Addressables.InstantiateAsync(key, parent);
        if (action != null)
        {
            if (handle.IsDone)
                action(handle);
            else
                handle.Completed += action;
        }
    }
    public static bool ReleaseInstance(ref AsyncOperationHandle<GameObject> handle, Action<AsyncOperationHandle<GameObject>> action)
    {
        if (handle.IsValid())
        {
            if (action != null)
                handle.Completed -= action;
            bool result = Addressables.ReleaseInstance(handle);
            handle = default(AsyncOperationHandle<GameObject>);
            return result;
        }
        return false;
    }
}