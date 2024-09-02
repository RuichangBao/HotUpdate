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
        // ��ʼ�� Addressables
        Addressables.InitializeAsync().Completed += InitializeAsyncCompleted;
    }

    private void InitializeAsyncCompleted(AsyncOperationHandle<IResourceLocator> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("��ʼ�����");
            CheckForUpdates();
        }
        else
        {
            Debug.LogError("��ʼ��ʧ��");
            statusText.text = "��ʼ��ʧ��";
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
            Debug.LogError("��ȡ�����б�ɹ�:");
            Debug.LogError(handle.Result.Count);
            statusText.text = handle.Result.Count.ToString();

            if (handle.Result.Count > 0)
            {
                // ����и��µ� catalog�����ز�����
                Addressables.UpdateCatalogs(handle.Result).Completed += UpdateCatalogsCompleted;
            }
        }
        else
        {
            Debug.LogError("��ȡ�����б�ʧ��");
            statusText.text = "ʧ��";
        }
    }

    private void UpdateCatalogsCompleted(AsyncOperationHandle<List<IResourceLocator>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Catalog ���³ɹ�");
            statusText.text = "Catalog ���³ɹ�";
        }
        else
        {
            Debug.LogError("Catalog ����ʧ��");
            statusText.text = "Catalog ����ʧ��";
        }
    }
}