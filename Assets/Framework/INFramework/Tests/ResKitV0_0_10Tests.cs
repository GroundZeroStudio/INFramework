using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace INFramework.Tests
{
    public class ResKitV0_0_10Tests
    {
        [UnityTest]
        public IEnumerator LoadAsyncTwiceBugTest()
        {
            var loadedCount = 0;

            var resLoader = new ResLoader();

            resLoader.LoadAsync<AudioClip>("resources://coin_get", (succeed, res) =>
            {
                Assert.AreEqual(EResState.Loaded, res.State);
                loadedCount++;
            });

            resLoader.LoadAsync<AudioClip>("resources://coin_get", (succeed, res) =>
            {
                Assert.AreEqual(EResState.Loaded, res.State);
                loadedCount++;
            });

            yield return new WaitUntil(() => loadedCount == 2);
        }
    }
}


