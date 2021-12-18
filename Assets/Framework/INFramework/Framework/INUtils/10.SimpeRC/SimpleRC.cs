/****************************************************
    文件：SimpleRC、.cs
    作者：TA94
    日期：#CreateTime#
    功能：Nothing
*****************************************************/
using UnityEngine;

namespace INFramework
{
    public interface IRefCounter
    {
        //引用计数
        int RefCount { get; }

        void Retain(object refOwner = null);
        void Release(object refOwner = null);
    }

    public class SimpleRC : IRefCounter
    {
        public int RefCount { get; private set; }
        public SimpleRC()
        {
            RefCount = 0;
        }



        public void Release(object refOwner = null)
        {
            --RefCount;
            if(RefCount == 0)
            {
                OnZeroRef();
            }
        }

        public void Retain(object refOwner = null)
        {
            ++RefCount;
        }

        protected virtual void OnZeroRef()
        {

        }
    }
}


