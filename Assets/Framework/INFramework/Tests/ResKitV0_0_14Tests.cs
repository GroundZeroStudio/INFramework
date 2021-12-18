using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace INFramework.Tests
{
    public class ResKitV0_0_14Tests
    {
        [Test]
        public void LoadAssetResSyncTest()
        {
            var resLoader = new ResLoader();

            var coinGetClip = resLoader.Load<AudioClip>("coin_get", "coin_get");

            Assert.IsTrue(coinGetClip);

            resLoader.UnloadAllAssets();

            resLoader = null;
        }


        [UnityTest]
        public IEnumerator LoadAssetResAsyncTest()
        {
            var resLoader = new ResLoader();

            var loadDone = false;

            resLoader.LoadAsync<AudioClip>("coin_get", "coin_get", (succeed, res) =>
            {
                var coinGetClip = res.Asset as AudioClip;

                Assert.IsTrue(coinGetClip);

                loadDone = succeed;
            });

            while (!loadDone)
            {
                yield return null;
            }

            resLoader.UnloadAllAssets();

            resLoader = null;
        }
    }
}