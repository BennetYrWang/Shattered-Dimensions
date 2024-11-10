using System;
using UnityEngine;

namespace Bennet.Module
{
    public class ModuleManager : MonoBehaviour
    {
        
        private static ModuleManager instance;

        public static ModuleManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameObject("Modules").AddComponent<ModuleManager>();
                return instance;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticInstance()
        {
            instance = null;
        }

        private void Awake()
        {
            //gameObject.hideFlags = HideFlags.HideInHierarchy;
            DontDestroyOnLoad(gameObject);
        }
    }
}