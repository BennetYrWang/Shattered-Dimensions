using System;
using BennetWang.Module.Timer;
using UnityEngine;

namespace BennetWang
{
    public class BennetTester : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SoundManager.Instance.PlaySoundEffect(SoundManager.Clip.FootStep);
        }
    }
}