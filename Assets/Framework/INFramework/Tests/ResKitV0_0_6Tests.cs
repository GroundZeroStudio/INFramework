using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace INFramework.Tests
{
    public class ResKitV0_0_6Tests
    {
        [UnityTest]
        public IEnumerator ResourcesResLoadAsyncTest()
        {
            var resLoader = new ResLoader();

            var loaded = false;
            AudioClip clip = null;

            // 异步加载一个资源
            resLoader.LoadAsync<AudioClip>("resources://coin_get", (succeed, res) =>
            {
                if (succeed)
                {
                    clip = res.Asset as AudioClip;
                    loaded = true;
                }
                else
                {
                    loaded = true;
                }
            });

            // 等待回调成功
            while (!loaded)
            {
                yield return null;
            }

            Assert.IsNotNull(clip);
        }
    }
}


