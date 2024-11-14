using System;
using BennetWang.Module.Timer;
using UnityEditor;
using UnityEngine;

namespace BennetWang
{
    public class BennetTester : MonoBehaviour
    {
        private void Update()
        {
            print(LayerMask.LayerToName(gameObject.layer));
        }
    }
}