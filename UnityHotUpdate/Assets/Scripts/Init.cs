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
        Debug.Log("��ʼ��Addressable");
        AsyncOperationHandle<IResourceLocator> init = Addressables.InitializeAsync();//��ʼ���ӷ�������ʼ��������Ƿ����ӷ������ɹ�!
        init.Completed += InitializeAsyncCompleted;
    }

    private void InitializeAsyncCompleted(AsyncOperationHandle<IResourceLocator> handle)
    {
        Debug.Log("��ʼ�����");
        AsyncOperationHandle<List<string>> checkForCatalogUpdates = Addressables.CheckForCatalogUpdates(false);
        checkForCatalogUpdates.Completed += CheckForCatalogUpdatesCompleted;
        Addressables.Release(handle);
    }

    private void CheckForCatalogUpdatesCompleted(AsyncOperationHandle<List<string>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"��ȡ�����б�ɹ���{handle.Result.Count}");
            lab.text = "��ȡ�����б�ɹ�";
            if (handle.Result.Count > 0)
            {
                lab.text = handle.Result.Count.ToString();
                Addressables.UpdateCatalogs(handle.Result).Completed += UpdateCatalogsCompleted;
            }
        }
        else
        {
            lab.text = "ʧ��";
        }
        Addressables.Release(handle);
    }

    private void UpdateCatalogsCompleted(AsyncOperationHandle<List<IResourceLocator>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Catalog ���³ɹ�");
            for (int i = 0; i < handle.Result.Count; i++)
            {
                Debug.Log(handle.Result[i].ToString());
            }
            lab.text = "���³ɹ�";
        }
        else
        {
            Debug.LogError("Catalog ����ʧ��");
            lab.text = "����ʧ��";
        }
    }

    private void BtnOnClick()
    {
        SceneManager.LoadScene("Main");
    }
}