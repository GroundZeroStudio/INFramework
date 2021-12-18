using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace INFramework.Tests
{
    public class ResKitV0_0_7Tests
    {
        [Test]
        public void LoadAsyncTwiceBugTest()
        {
            var resLoader = new ResLoader();

            resLoader.LoadAsync<AudioClip>("resources://coin_get", (b, res) => { });
            resLoader.LoadAsync<AudioClip>("resources://coin_get", (b, res) => { });

            Assert.Pass();
        }
    }
}
