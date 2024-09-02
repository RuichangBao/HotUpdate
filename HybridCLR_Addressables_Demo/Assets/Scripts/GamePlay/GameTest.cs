using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GamePlay
{
    /// <summary>
    /// 测试热更新中的一些特性,如泛型，RuntimeInitializeOnLoadMethod等
    /// </summary>
    public class GameTest
    {
        private const string PREFAB_PATH = "Assets/Prefabs/TestPrefab.prefab";
        
        private static GameTest _instance;

        public static GameTest Instance => _instance ??= new GameTest();

        private List<HotUpdateClass> _hotUpdateClassList = new();

        private List<HotUpdateStruct> _hotUpdateStructList = new();

        #region Test RuntimeInitializeOnLoadMethod

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void TestRuntimeInitialize1()
        {
            Debug.LogError("当第一个场景的对象加载到内存中时，在调用Awake之前调用回调。");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void TestRuntimeInitialize2()
        {
            Debug.LogError("在加载所有程序集和初始化预加载资源时调用回调。此时，第一个场景的对象还没有加载。");
        }

        #endregion

        #region Test GenericType

        private class HotUpdateClass
        {
            public int i;

            public HotUpdateClass(int i)
            {
                this.i = i;
            }
        }

        private struct HotUpdateStruct
        {
            public int i;

            public HotUpdateStruct(int i)
            {
                this.i = i;
            }
        }

        private void TestGenericType()
        {
            for (int i = 0; i < 5; i++)
            {
                _hotUpdateClassList.Add(new(i));
            }

            for (int i = 0; i < 5; i++)
            {
                _hotUpdateStructList.Add(new(i));
            }

            for (int i = 0; i < 5; i++)
            {
                Debug.LogError(_hotUpdateClassList[i].i);
            }

            for (int i = 0; i < 5; i++)
            {
                Debug.LogError(_hotUpdateStructList[i].i);
            }
        }

        #endregion

        #region TestPrefab

        private void TestPrefab()
        {
            var prefab = Addressables.LoadAssetAsync<GameObject>(PREFAB_PATH).WaitForCompletion();
            if (prefab == null)
            {
                Debug.LogError($"Load Prefab Failed,path:{PREFAB_PATH}");
                return;
            }

            var instance = Object.Instantiate(prefab);
        }

        #endregion

        #region TestScriptableObject

        private void TestScriptableObject()
        {   
            Debug.Log("ScriptableObjectTest Test");
            var scriptableObj = Addressables.LoadAssetAsync<ScriptableObjectTest>(ScriptableObjectTest.TEST_OBJ_PATH).WaitForCompletion();
            if (scriptableObj == null)
            {
                Debug.LogError($"Load ScriptableObject Failed,path:{ScriptableObjectTest.TEST_OBJ_PATH}");
                return;
            }
            
            Debug.Log($"ScriptableObjectTest Test intValue:{scriptableObj.intValue}");
        }

        #endregion
        
        public void Test()
        {
            TestGenericType();
            TestPrefab();
            TestScriptableObject();
        }
    }
}