using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using FrameWork;

public class Main : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> handleMain;
    private GameObject goMain;
    private AsyncOperationHandle<GameObject> handleStart;
    private GameObject goStart;
    private void Start()
    {
        AddressablesUtil.InstantiateAsync(ref handleMain, "UIMain", OnLoadedMain);
        AddressablesUtil.InstantiateAsync(ref handleStart, "UIStart", OnLoadedStar);
    
}

    private void OnLoadedMain(AsyncOperationHandle<GameObject> handle)
    {
        goMain = handle.Result;
    }
    private void OnLoadedStar(AsyncOperationHandle<GameObject> handle)
    {
        goStart = handle.Result;
    }
    private void OnDestroy()
    {
        Destroy(goMain);
        AddressablesUtil.ReleaseInstance(ref handleMain, OnLoadedMain);
        Destroy(goStart);
        AddressablesUtil.ReleaseInstance(ref handleStart, OnLoadedStar);
    }
}