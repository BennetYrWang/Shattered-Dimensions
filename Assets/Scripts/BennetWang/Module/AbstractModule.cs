using System;
using BennetWang.Module.Timer;
using UnityEngine;

namespace BennetWang.Module
{
    /// <summary>
    /// Only Support C# 11 or higher
    /// </summary>
    public abstract class AbstractModule : MonoBehaviour
    {
        private bool _initialized;
        public void Init()
        {
            if (_initialized)
                throw new NotSupportedException("Module does not allow Init more than once.");
            InitBehaviour();
            _initialized = true;
        }

        /// <summary>
        /// What will be executed during Init().
        /// Highly recommend include "RegisterToModuleManagerAs&lt;YourType&gt;();"
        /// </summary>
        protected abstract void InitBehaviour();

        private void OnDestroy()
        {
            OnDestroyBehaviour();
        }

        /// <summary>
        /// What will be executed during OnDestroy().
        /// Highly recommend include "DeregisterModule&lt;YourType&gt;();"
        /// </summary>
        protected abstract void OnDestroyBehaviour();

        /// <summary>
        /// This is not suggested
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        protected void RegisterToModuleManagerAs(Type type)
        {
            if (!type.IsSubclassOf(typeof(AbstractModule)))
                throw new InvalidCastException("The parameter is not a Module Type!");
            var method = typeof(ModuleManager).GetMethod("TryRegisterModule");
            var genericMethod = method.MakeGenericMethod(type);
            bool result = (bool)genericMethod.Invoke(ModuleManager.Instance, new object[] { this });
            if (!result)
                throw new InvalidOperationException("Module registration failed!");
        }
    }
}