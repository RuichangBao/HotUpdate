using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button btn;
    public TextMeshProUGUI lab;
    int num = 1;
    void Start()
    {
        btn.onClick.AddListener(BtnOnClick);
    }

    // Update is called once per frame
    private void BtnOnClick()
    {
        lab.text = (num++).ToString();
    }
}
