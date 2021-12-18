using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework
{
	public interface IPool<T>
    {
		T Allocate();
		bool Recycle(T obj);
    }

    public interface IObjectFactory<T>
    {
        T Create();
    }

    public abstract class Pool<T> : IPool<T>
    {
        public int CurCount => this.m_PoolStack.Count;

        private int mMaxCount = 5;

        protected Stack<T> m_PoolStack = new Stack<T>();

        protected IObjectFactory<T> m_Factory;

        public virtual T Allocate()
        {
            return m_PoolStack.Count > 0 ? m_PoolStack.Pop() : m_Factory.Create();
        }


        public abstract bool Recycle(T obj);
    }

    public class CustomObjectFactory<T> : IObjectFactory<T>
    {
        //返回T
        protected Func<T> m_FactoryMethod;

        public CustomObjectFactory(Func<T> factoryMethod)
        {
            m_FactoryMethod = factoryMethod;
        }
            

        public T Create()
        {
            return m_FactoryMethod();
        }
    }

    public class SimpleObjectPool<T> : Pool<T>
    {
        readonly Action<T> m_ResetMethod;
        public SimpleObjectPool(Func<T> factoryMethod, Action<T> resetMethod = null, int initCount = 0)
        {
            m_Factory = new CustomObjectFactory<T>(factoryMethod);
            m_ResetMethod = resetMethod;
            for (int i = 0; i < initCount; i++)
            {
                m_PoolStack.Push(m_Factory.Create());
            }
        }

        public override bool Recycle(T obj)
        {
            if(m_ResetMethod != null)
            {
                m_ResetMethod(obj);
            }
            m_PoolStack.Push(obj);
            return false;
        }
    }
}

