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
    //    Debug.LogError("��ʼ��Addressable");
    //    AsyncOperationHandle<IResourceLocator> init = Addressables.InitializeAsync();//��ʼ���ӷ�������ʼ��������Ƿ����ӷ������ɹ�!
    //    init.Completed += InitializeAsyncCompleted;
    //}

    //private void InitializeAsyncCompleted(AsyncOperationHandle<IResourceLocator> handle)
    //{
    //    Debug.LogError("��ʼ�����");
    //    AsyncOperationHandle<List<string>> checkForCatalogUpdates = Addressables.CheckForCatalogUpdates(false);
    //    checkForCatalogUpdates.Completed += CheckForCatalogUpdatesCompleted;
    //}

    //private void CheckForCatalogUpdatesCompleted(AsyncOperationHandle<List<string>> handle)
    //{
    //    if (handle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        Debug.LogError("��ȡ�����б�ɹ���");
    //        Debug.LogError(handle.Result.Count);
    //        lab.text = handle.Result.Count.ToString();
    //    }
    //    else
    //    {
    //        lab.text = "ʧ��";
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