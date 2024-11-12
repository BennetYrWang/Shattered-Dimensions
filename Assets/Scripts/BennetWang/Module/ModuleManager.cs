using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BennetWang.Module
{
    public class ModuleManager : MonoBehaviour
    {
        private static ModuleManager _instance;
        private readonly Dictionary<Type, AbstractModule> registeredModule = new();

        public static ModuleManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameObject("Modules").AddComponent<ModuleManager>();
                return _instance;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticInstance()
        {
            _instance = null;
        }

        private void Awake()
        {
            //gameObject.hideFlags = HideFlags.HideInHierarchy;
            DontDestroyOnLoad(gameObject);
        }
        
        // Register
        public bool TryRegisterModule<T>(T module) where T : AbstractModule
        {
            return registeredModule.TryAdd(typeof(T),module);
        }

        public void DeregisterModule<T>() where T : AbstractModule
        {
            registeredModule.Remove(typeof(T));
        }
    }
}