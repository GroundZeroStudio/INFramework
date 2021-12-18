using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFrameworkDesign.Example
{
	public class TypeEventSystemTest : MonoBehaviour
	{
		public class A
        {

        }

		public class B
        {

        }

        private void Start()
        {
            TypeEventSystem.Register<A>(ReceivedA);

            TypeEventSystem.Register<B>(ReceivedB);

        }

        public void ReceivedA(A a)
        {
            Debug.Log("Received A");
        }

        public void ReceivedB(B b)
        {
            Debug.Log("Received B");
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TypeEventSystem.Send<A>(new A());
            }

            if (Input.GetMouseButtonDown(1))
            {
                TypeEventSystem.Send<B>(new B());
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                TypeEventSystem.UnRegister<A>(ReceivedA);
                TypeEventSystem.UnRegister<B>(ReceivedB);
            }
        }
    }
}

