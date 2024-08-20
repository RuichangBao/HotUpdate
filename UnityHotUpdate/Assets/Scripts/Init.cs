using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Init : MonoBehaviour
{
    private string url = @"http://192.168.3.88:8080";
    private string listPath = "list.txt"; 
    private static string platform = "StandaloneWindows64";
    private string savePath = $"{Application.streamingAssetsPath}/{platform}/";
   
    void Start()
    {
        StartCoroutine(GetFileList());
    }

    ///<summary>获取文件列表</summary>
    private IEnumerator GetFileList()
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get($"{url}/{listPath}");
        yield return unityWebRequest.SendWebRequest();

        string allFiles = Encoding.UTF8.GetString(unityWebRequest.downloadHandler.data);
        //unityWebRequest.Dispose();
        string[] downLoadFiles = allFiles.Split("\r\n");
        for (int i = 0; i < downLoadFiles.Length; i++)
        {
            if (!string.IsNullOrEmpty(downLoadFiles[i]))
            {
                DownloadFile(downLoadFiles[i]);
            }
        }
        SceneManager.LoadScene("SampleScene");
    }
    void DownloadFile(string fileName)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get($"{url}/{platform}/{fileName}");
        unityWebRequest.downloadHandler = new DownloadHandlerFile($"{savePath}/{fileName}");
        FileInfo file = new FileInfo(savePath);
        unityWebRequest.SendWebRequest();
        while (!unityWebRequest.isDone)
        { }
        unityWebRequest.Dispose();
    }
}
