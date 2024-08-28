using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using FrameWork;

namespace UI
{
    public class UIMain : MonoBehaviour
    {
        public Button btn;
        private AsyncOperationHandle<GameObject> handle;
        private GameObject go;

        void Start()
        {
            btn.onClick.AddListener(BtnOnClick);
        }

        private void BtnOnClick()
        {
            AddressablesUtil.InstantiateAsync(ref handle, "UIStart", OnLoaded);
        }

        private void OnLoaded(AsyncOperationHandle<GameObject> handle)
        {
            go = handle.Result;
        }

        private void OnDestroy()
        {
            Destroy(go);
            AddressablesUtil.ReleaseInstance(ref handle, OnLoaded);
        }
    }
}