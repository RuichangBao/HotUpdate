using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.AddressableAssets.ResourceLocators;

public class AddressablesUpdater : MonoBehaviour
{
    public UnityEngine.UI.Text statusText;

    void Start()
    {
        // 初始化 Addressables
        Addressables.InitializeAsync().Completed += InitializeAsyncCompleted;
    }

    private void InitializeAsyncCompleted(AsyncOperationHandle<IResourceLocator> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("初始化完成");
            CheckForUpdates();
        }
        else
        {
            Debug.LogError("初始化失败");
            statusText.text = "初始化失败";
        }
    }

    private void CheckForUpdates()
    {
        AsyncOperationHandle<List<string>> checkForCatalogUpdates = Addressables.CheckForCatalogUpdates(false);
        checkForCatalogUpdates.Completed += CheckForCatalogUpdatesCompleted;
    }

    private void CheckForCatalogUpdatesCompleted(AsyncOperationHandle<List<string>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("获取更新列表成功:");
            Debug.LogError(handle.Result.Count);
            statusText.text = handle.Result.Count.ToString();

            if (handle.Result.Count > 0)
            {
                // 如果有更新的 catalog，下载并更新
                Addressables.UpdateCatalogs(handle.Result).Completed += UpdateCatalogsCompleted;
            }
        }
        else
        {
            Debug.LogError("获取更新列表失败");
            statusText.text = "失败";
        }
    }

    private void UpdateCatalogsCompleted(AsyncOperationHandle<List<IResourceLocator>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Catalog 更新成功");
            statusText.text = "Catalog 更新成功";
        }
        else
        {
            Debug.LogError("Catalog 更新失败");
            statusText.text = "Catalog 更新失败";
        }
    }
}