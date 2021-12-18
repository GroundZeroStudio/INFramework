/****************************************************
    文件：ClassObjectPool.cs
    作者：TA94
    日期：2021/10/20 23:18:57
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INFramework 
{
    public class ClassObjectPool<T> where T : class, new()
    {
        protected Stack<T> _pool = new Stack<T>();

        //小于等于0则无限制个数
        protected int _maxPoolCount = 0;

        protected int _noRecycleCount = 0;

        public ClassObjectPool(int nCount)
        {
            this._maxPoolCount = nCount;
            for (int i = 0; i < nCount; i++)
            {
                _pool.Push(new T());
            }
        }

        /// <summary>
        /// 从池对象里获取类
        /// </summary>
        /// <param name="bCreateIfPoolEmpty"></param>
        /// <returns></returns>
        public T Spwan(bool bCreateIfPoolEmpty)
        {
            if (_pool.Count > 0)
            {
                T rtn = _pool.Pop();
                if (rtn == null)
                {
                    if (bCreateIfPoolEmpty)
                    {
                        rtn = new T();
                    }
                }
                _noRecycleCount++;
                return rtn;
            }
            else
            {
                if (bCreateIfPoolEmpty)
                {
                    T rtn = new T();
                    _noRecycleCount++;
                    return rtn;
                }
            }

            return null;
        }


        /// <summary>
        /// 回收类对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Recycle(T obj)
        {
            if (obj == null) return false;

            if (_pool.Count >= _maxPoolCount && _maxPoolCount > 0)
            {
                obj = null;
                return false;
            }
            _pool.Push(obj);
            return true;
        }

    }

}


