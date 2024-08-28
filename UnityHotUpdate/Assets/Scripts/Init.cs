using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Reflection;

public class Init : MonoBehaviour
{
    public Text lab;
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(BtnOnClick);
        InitAddressable();
    }

    //public void InitAddressable()
    //{
    //    Debug.LogError("初始化Addressable");
    //    AsyncOperationHandle<IResourceLocator> init = Addressables.InitializeAsync();//开始连接服务器初始化，检测是否连接服务器成功!
    //    init.Completed += InitializeAsyncCompleted;
    //}

    //private void InitializeAsyncCompleted(AsyncOperationHandle<IResourceLocator> handle)
    //{
    //    Debug.LogError("初始化完成");
    //    AsyncOperationHandle<List<string>> checkForCatalogUpdates = Addressables.CheckForCatalogUpdates(false);
    //    checkForCatalogUpdates.Completed += CheckForCatalogUpdatesCompleted;
    //}

    //private void CheckForCatalogUpdatesCompleted(AsyncOperationHandle<List<string>> handle)
    //{
    //    if (handle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        Debug.LogError("获取更新列表成功：");
    //        Debug.LogError(handle.Result.Count);
    //        lab.text = handle.Result.Count.ToString();
    //    }
    //    else
    //    {
    //        lab.text = "失败";
    //    }
    //}
    public void InitAddressable()
    {
        Addressables.LoadAssetAsync<TextAsset>("UI.dll").Completed += (obj) =>
        {
            Assembly assembly = Assembly.Load(obj.Result.bytes);
        };
    }

    private void BtnOnClick()
    {
        SceneManager.LoadScene("Main");
    }
}