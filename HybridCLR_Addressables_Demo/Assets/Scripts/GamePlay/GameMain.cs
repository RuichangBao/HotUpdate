using System;
using UnityEngine;

namespace GamePlay
{
    public class GameMain : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("Hello 2222222222");
            GameTest.Instance.Test();
        }
    }
}