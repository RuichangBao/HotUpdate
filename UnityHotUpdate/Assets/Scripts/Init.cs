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
        Debug.Log("��ʼ��Addressable");
        AsyncOperationHandle<IResourceLocator> init = Addressables.InitializeAsync();//��ʼ���ӷ�������ʼ��������Ƿ����ӷ������ɹ�!
        init.Completed += InitializeAsyncCompleted;
    }

    private void InitializeAsyncCompleted(AsyncOperationHandle<IResourceLocator> handle)
    {
        Debug.Log("��ʼ����ɣ�����Ŀ¼�仯");
        AsyncOperationHandle<List<string>> checkForCatalogUpdates = Addressables.CheckForCatalogUpdates(false);
        checkForCatalogUpdates.Completed += CheckForCatalogUpdatesCompleted;
        Addressables.Release(handle);
    }

    private void CheckForCatalogUpdatesCompleted(AsyncOperationHandle<List<string>> checkForCatalogUpdates)
    {
        if (checkForCatalogUpdates.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("��ʼ����ɣ�����Ŀ¼�仯�ɹ�");
            lab.text = "��ȡĿ¼�仯�ɹ�";
            if (checkForCatalogUpdates.Result.Count > 0)
            {
                Debug.Log("Ŀ¼�ļ��б仯����Ҫ���£�");
                lab.text = $"Ŀ¼�ļ��б仯{checkForCatalogUpdates.Result.Count}";
                AsyncOperationHandle<List<IResourceLocator>> updateCatalogs = Addressables.UpdateCatalogs(checkForCatalogUpdates.Result);
                updateCatalogs.Completed += UpdateCatalogsCompleted;
            }
            else
            {
                Debug.Log("Ŀ¼�ļ��ޱ仯������Ҫ���£�");
                lab.text = "Ŀ¼�ļ��ޱ仯������Ҫ���£�";
            }
        }
        else
        {
            Debug.LogError("����Ŀ¼�仯����");
            lab.text = "ʧ��";
        }
        Addressables.Release(checkForCatalogUpdates);
    }
    //����Ŀ¼�ļ�
    private void UpdateCatalogsCompleted(AsyncOperationHandle<List<IResourceLocator>> updateCatalogs)
    {
        if (updateCatalogs.Status == AsyncOperationStatus.Succeeded)
        {
            List<object> key = new();
            Debug.Log("Ŀ¼�ļ�, ���³ɹ���");
            for (int i = 0; i < updateCatalogs.Result.Count; i++)
            {
                Debug.Log(updateCatalogs.Result[i].ToString());
            }
            lab.text = "Ŀ¼�ļ�,���³ɹ���";
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
            Debug.LogError("Ŀ¼�ļ� ����ʧ�ܣ�");
            lab.text = "Ŀ¼�ļ�,����ʧ�ܣ�";
        }
        //Addressables.Release(updateCatalogs);
    }

    private void DownloadSizeAsyncCompleted(AsyncOperationHandle<long> downloadSizeAsync)
    {
        if (downloadSizeAsync.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"��ȡ��Ҫ�����ļ���С�ɹ�{downloadSizeAsync.Result}!");
            lab.text = $"��Ҫ����{downloadSizeAsync.Result}";
        }
        else
        {
            Debug.Log("��ȡ��Ҫ�����ļ���Сʧ��!");
        }
        Addressables.Release(downloadSizeAsync);
    }
    private void DownloadDependenciesAsyncCompleted(AsyncOperationHandle downloadDependenciesAsync)
    {
        if (downloadDependenciesAsync.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("���ظ����ļ��ɹ�!");
            Debug.Log("���и�����ɣ�������������������������������������������������������������������������������������");
        }
        else
        {
            Debug.LogError("���ظ����ļ�ʧ��!");
        }
    }

    private void BtnOnClick()
    {
        SceneManager.LoadScene("Main");
    }
}