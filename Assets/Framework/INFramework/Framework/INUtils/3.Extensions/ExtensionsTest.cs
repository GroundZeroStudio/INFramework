using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
	public class ExtensionsTest : MonoBehaviour
	{
        private void Start()
        {
            GameObject obj = new GameObject();
            obj.transform.PositionY(10);
        }
    }
}

