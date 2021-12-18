using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace INFramework.Tests
{
    public class ResKitV0_0_8Tests
    {
        [Test]
        public void LoadAsyncTwiceBugTest()
        {
            var resLoader = new ResLoader();

            resLoader.LoadAsync<AudioClip>("resources://coin_get", (succeed, res) => { });
            resLoader.LoadAsync<AudioClip>("resources://coin_get", (succeed, res) => { });

            Assert.Pass();
        }
    }

}