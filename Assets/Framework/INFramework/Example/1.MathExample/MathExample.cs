using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class MathExample : MonoBehaviour
	{
        private void Start()
        {
            ////十进制转二进制
            //int number = 10;
            ////将整数放入4个字节数组中
            //byte[] bytes = BitConverter.GetBytes(number);
            //Debug.Log(Convert.ToString(number, 2));
            ////二进制转十进制
            //string bin = "1010";
            //Debug.Log(Convert.ToInt32(bin, 2));

            Debug.Log(10 >> 1);
            Debug.Log(10 >> 2);
            Debug.Log(10 >> 3);
        }
    }
}

