/****************************************************
    文件：AssetBundleExample.cs
    作者：Olivia
    功能：Nothing
*****************************************************/
using System.IO;
using UnityEngine;

namespace INFramework.Playground
{
    public class AssetBundleExample : MonoBehaviour
    {
        ResLoader m_Resloader = new ResLoader();

        void Start()
        {
            var coinGetPrefab = m_Resloader.Load<GameObject>("coin_get_prefab", "coin_get");
            Instantiate(coinGetPrefab);
        }

        private void OnDestroy()
        {
            m_Resloader.UnloadAllAssets();
            m_Resloader = null;
        }
    }
}
