using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class Init : MonoBehaviour
{
    private List<object> _updateKeys = new List<object>();

    private void Start()
    {
        UpdateCatalog();
    }

    public async void UpdateCatalog()
    {
        Debug.LogError("初始化Addressable");
        var init = Addressables.InitializeAsync();//开始连接服务器初始化，检测是否连接服务器成功!
        await init.Task;

        //开始连接服务器检查更新
        var handle = Addressables.CheckForCatalogUpdates(false);
        await handle.Task;
        Debug.LogError("check catalog status " + handle.Status);
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            List<string> catalogs = handle.Result;
            if (catalogs != null && catalogs.Count > 0)
            {
                //foreach (var catalog in catalogs)
                //{
                //    Debug.LogError("catalog  " + catalog);
                //}
                Debug.LogError("download catalog start ");
                var updateHandle = Addressables.UpdateCatalogs(catalogs, false);
                await updateHandle.Task;

                foreach (var item in updateHandle.Result)
                {
                    //Debug.LogError("catalog result " + item.LocatorId);
                    //foreach (var key in item.Keys)
                    //{
                    //    Debug.LogError("catalog key " + key);
                    //}
                    _updateKeys.AddRange(item.Keys);
                }
                Debug.LogError("download catalog finish " + updateHandle.Status);

                DownLoad();
            }
            else
            {
                Debug.LogError("不需要更新");
            }
        }
        Addressables.Release(handle);
    }

    public IEnumerator DownAssetImpl()
    {
        //foreach(var v in _updateKeys)
        //{
        //    Debug.LogError("v:" + v.ToString());
        //}


        var downloadsize = Addressables.GetDownloadSizeAsync(_updateKeys);
        yield return downloadsize;
        Debug.LogError("start download size :" + downloadsize.Result);

        if (downloadsize.Result > 0)
        {
            var download = Addressables.DownloadDependenciesAsync(_updateKeys, Addressables.MergeMode.Union);
            yield return download;
            //await download.Task;
            //Debug.LogError("download result type " + download.Result.GetType());
            foreach (var item in download.Result as List<UnityEngine.ResourceManagement.ResourceProviders.IAssetBundleResource>)
            {
                var ab = item.GetAssetBundle();
                //Debug.LogError("ab name " + ab.name);
                foreach (var name in ab.GetAllAssetNames())
                {
                    Debug.LogError("asset name " + name);
                }
            }
            Addressables.Release(download);
        }
        Addressables.Release(downloadsize);
    }

    public void DownLoad()
    {
        StartCoroutine(DownAssetImpl());
    }

    public async void DownloadAsset(object assetName, Action action)
    {
        var downloadsize = Addressables.GetDownloadSizeAsync(assetName);
        await downloadsize.Task;
        Debug.LogError("start download size :" + downloadsize.Result);

        if (downloadsize.Result > 0)
        {
            var download = Addressables.DownloadDependenciesAsync(assetName);
            await download.Task;
            foreach (var item in download.Result as List<UnityEngine.ResourceManagement.ResourceProviders.IAssetBundleResource>)
            {
                var ab = item.GetAssetBundle();
                //Debug.LogError("ab name " + ab.name);
                foreach (var name in ab.GetAllAssetNames())
                {
                    Debug.LogError("asset name " + name);
                }
            }
            Addressables.Release(download);
        }
        Addressables.Release(downloadsize);

        action();
    }
}