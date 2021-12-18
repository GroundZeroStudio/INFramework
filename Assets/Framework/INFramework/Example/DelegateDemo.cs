/****************************************************
    文件：DelegateDemo.cs
    作者：TA94
    功能：Nothing
*****************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

public class DelegateDemo : MonoBehaviour
{
    //定义有返回值和参数的委托类型
    public delegate int MyDelegate01(int a, int b);
    public delegate float MyDelegate02(float a, float b);
    public delegate double MyDelegate03(double a, double b);
    //泛型委托
    public delegate T MyDelegate04<T, U, V>(U a, V b);

    private void Start()
    {
        ////第一种方式
        //MyDelegate01 myDelegate01 = new MyDelegate01(Add);
        //int result = myDelegate01(1, 2);
        //Debug.Log(result);
        ////第二种方式
        //MyDelegate02 myDelegate02 = Add;
        //Debug.Log(myDelegate02(2.2f, 1.5f));
        ////第三种方式
        //MyDelegate03 myDelegate03 = (a, b) => a + b;  //MyDelegate03 myDelegate = (a, b) => {retutn a + b;};
        //double x = 4.0f;
        //double y = 4.1f;
        //Debug.Log(myDelegate03(x, y));
        //List<int> list = new List<int>();
        MyDelegate04<int, int, int> myDelegate04 = new MyDelegate04<int, int, int>(Add);
        Debug.Log(myDelegate04(1, 2));

        Func<int, int, int> myDelegate04_3 = (a, b) => a * b;
        Debug.Log(myDelegate04_3(4, 9));

        MyDelegate04<float, float, float> myDelegate04_1 = new MyDelegate04<float, float, float>(Add);
        Debug.Log(myDelegate04_1(2.2f, 1.5f));

        MyDelegate04<double, double, double> myDelegate04_2 = (a, b) => { return a + b; };
        Debug.Log(myDelegate04_1(4.0f, 4.1f));


    }


    public int Add(int a, int b)
    {
        return a + b;
    }

    public float Add(float a, float b)
    {
        return a + b;
    }


}
