using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{

    public class CoroutineRunner : MonoSingleton<CoroutineRunner> { }
    public class ResourcesRes : Res
	{
		public const string PREFIX = "resources://";
        public override void Load()
        {
            var resourcesName = Name.Remove(0, ResourcesRes.PREFIX.Length);
            Asset = Resources.Load(resourcesName, ResType);
            State = EResState.Loaded;
            DispatchOnloadEvent(true);
        }

        public override void LoadAsync()
        {
            State = EResState.Loading;
            m_LoadAsyncTask = CoroutineRunner.Instance.StartCoroutine(DoLoadAsync());
        }

        private IEnumerator DoLoadAsync()
        {
            var resourceName = Name.Remove(0, ResourcesRes.PREFIX.Length);
            var loadRequest = Resources.LoadAsync(resourceName, ResType);
            yield return loadRequest;
            if (loadRequest.asset)
            {
                Asset = loadRequest.asset;
                State = EResState.Loaded;
                DispatchOnloadEvent(true);
            }
            else
            {
                State = EResState.NotLoad;
                DispatchOnloadEvent(false);
            }
            m_LoadAsyncTask = null;
        }

        public override void Unload()
        {
            Resources.UnloadAsset(Asset);
            State = EResState.NotLoad;
        }
    }
}

