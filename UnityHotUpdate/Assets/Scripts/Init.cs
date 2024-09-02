using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class Init : MonoBehaviour
{
    public Text lab;
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(BtnOnClick);
        InitAddressable();
    }

    public void InitAddressable()
    {
        Debug.Log("初始化Addressable");
        AsyncOperationHandle<IResourceLocator> init = Addressables.InitializeAsync();//开始连接服务器初始化，检测是否连接服务器成功!
        init.Completed += InitializeAsyncCompleted;
    }

    private void InitializeAsyncCompleted(AsyncOperationHandle<IResourceLocator> handle)
    {
        Debug.Log("初始化完成");
        AsyncOperationHandle<List<string>> checkForCatalogUpdates = Addressables.CheckForCatalogUpdates(false);
        checkForCatalogUpdates.Completed += CheckForCatalogUpdatesCompleted;
        Addressables.Release(handle);
    }

    private void CheckForCatalogUpdatesCompleted(AsyncOperationHandle<List<string>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"获取更新列表成功：{handle.Result.Count}");
            lab.text = "获取更新列表成功";
            if (handle.Result.Count > 0)
            {
                lab.text = handle.Result.Count.ToString();
                Addressables.UpdateCatalogs(handle.Result).Completed += UpdateCatalogsCompleted;
            }
        }
        else
        {
            lab.text = "失败";
        }
        Addressables.Release(handle);
    }

    private void UpdateCatalogsCompleted(AsyncOperationHandle<List<IResourceLocator>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Catalog 更新成功");
            for (int i = 0; i < handle.Result.Count; i++)
            {
                Debug.Log(handle.Result[i].ToString());
            }
            lab.text = "更新成功";
        }
        else
        {
            Debug.LogError("Catalog 更新失败");
            lab.text = "更新失败";
        }
    }

    private void BtnOnClick()
    {
        SceneManager.LoadScene("Main");
    }
}