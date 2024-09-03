using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Collections;

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
        Debug.Log("初始化完成，请求目录变化");
        AsyncOperationHandle<List<string>> checkForCatalogUpdates = Addressables.CheckForCatalogUpdates(false);
        checkForCatalogUpdates.Completed += CheckForCatalogUpdatesCompleted;
        Addressables.Release(handle);
    }

    private void CheckForCatalogUpdatesCompleted(AsyncOperationHandle<List<string>> checkForCatalogUpdates)
    {
        if (checkForCatalogUpdates.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("初始化完成，请求目录变化成功");
            lab.text = "获取目录变化成功";
            if (checkForCatalogUpdates.Result.Count > 0)
            {
                Debug.Log("目录文件有变化，需要更新！");
                lab.text = $"目录文件有变化{checkForCatalogUpdates.Result.Count}";
                AsyncOperationHandle<List<IResourceLocator>> updateCatalogs = Addressables.UpdateCatalogs(checkForCatalogUpdates.Result);
                updateCatalogs.Completed += UpdateCatalogsCompleted;
            }
            else
            {
                Debug.Log("目录文件无变化，不需要更新！");
                lab.text = "目录文件无变化，不需要更新！";
            }
        }
        else
        {
            Debug.LogError("请求目录变化错误");
            lab.text = "失败";
        }
        Addressables.Release(checkForCatalogUpdates);
    }
    //更新目录文件
    private void UpdateCatalogsCompleted(AsyncOperationHandle<List<IResourceLocator>> updateCatalogs)
    {
        if (updateCatalogs.Status == AsyncOperationStatus.Succeeded)
        {
            List<object> key = new();
            Debug.Log("目录文件, 更新成功！");
            for (int i = 0; i < updateCatalogs.Result.Count; i++)
            {
                Debug.Log(updateCatalogs.Result[i].ToString());
            }
            lab.text = "目录文件,更新成功！";
            foreach (IResourceLocator resourceLocator in updateCatalogs.Result)
            {
                Debug.Log("aaa:" + resourceLocator.Keys);
                foreach (object item in resourceLocator.Keys)
                {
                    key.Add(item);
                }
            }
            AsyncOperationHandle<long> downloadSizeAsync = Addressables.GetDownloadSizeAsync((IEnumerable)key);
            downloadSizeAsync.Completed += DownloadSizeAsyncCompleted;
            AsyncOperationHandle downloadDependenciesAsync = Addressables.DownloadDependenciesAsync((IEnumerable)key, Addressables.MergeMode.Union, false);
            downloadDependenciesAsync.Completed += DownloadDependenciesAsyncCompleted;
        }
        else
        {
            Debug.LogError("目录文件 更新失败！");
            lab.text = "目录文件,更新失败！";
        }
        //Addressables.Release(updateCatalogs);
    }

    private void DownloadSizeAsyncCompleted(AsyncOperationHandle<long> downloadSizeAsync)
    {
        if (downloadSizeAsync.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"获取需要下载文件大小成功{downloadSizeAsync.Result}!");
            lab.text = $"需要下载{downloadSizeAsync.Result}";
        }
        else
        {
            Debug.Log("获取需要下载文件大小失败!");
        }
        Addressables.Release(downloadSizeAsync);
    }
    private void DownloadDependenciesAsyncCompleted(AsyncOperationHandle downloadDependenciesAsync)
    {
        if (downloadDependenciesAsync.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("下载更新文件成功!");
            Debug.Log("所有更新完成！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
        }
        else
        {
            Debug.LogError("下载更新文件失败!");
        }
    }

    private void BtnOnClick()
    {
        SceneManager.LoadScene("Main");
    }
}