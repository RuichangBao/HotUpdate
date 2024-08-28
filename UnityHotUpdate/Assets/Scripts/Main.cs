using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using FrameWork;

public class Main : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> handle;
    private GameObject go;
    private void Start()
    {
        AddressablesUtil.InstantiateAsync(ref handle, "UIMain", OnLoaded);
    }

    private void OnLoaded(AsyncOperationHandle<GameObject> handle)
    {
        go = handle.Result;
    }

    private void OnDestroy()
    {
        Destroy(go);
        AddressablesUtil.ReleaseInstance(ref handle, OnLoaded);
    }
}