using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using FrameWork;

namespace UI
{
    public class UIMain : MonoBehaviour
    {
        public InputField inputField1, inputField2;
        public Button btn;
        public Text labResult;

        void Start()
        {
            btn.onClick.AddListener(BtnOnClick);
        }

        private void BtnOnClick()
        {
            int num1 = int.Parse(inputField1.text);
            int num2 = int.Parse(inputField2.text);
            labResult.text = (num1 - num2).ToString();
        }

    }
}