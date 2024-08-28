using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;

public class CopyDll:Editor
{
    [MenuItem("HybridCLR/¸´ÖÆdll")]
    public static void _CopyDll()
    {
        string outPutPath = Application.dataPath;
        outPutPath = outPutPath.Remove(outPutPath.LastIndexOf('/'));
        outPutPath = $"{outPutPath}/HybridCLRData/HotUpdateDlls/StandaloneWindows64";
        Debug.Log(outPutPath);
        string[] fileNames = AssetDatabase.FindAssets(outPutPath);
        foreach (string fileName in fileNames)
        {
            Debug.LogError(fileName);
        }
        Debug.LogError("Copy Dll");
    }
}