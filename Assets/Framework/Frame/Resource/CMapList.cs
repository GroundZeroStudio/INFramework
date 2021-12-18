/****************************************************
    文件：CMapList.cs
    作者：TA94
    日期：2021/10/24 20:47:40
    功能：Nothing
*****************************************************/

using System.Collections.Generic;

namespace INFramework.Framework.AssetBundles
{
    public class CMapList<T> where T : class, new()
    {
        DoubleLinkedList<T> mDLink = new DoubleLinkedList<T>();
        
        Dictionary<T, DoubleLinkedListNode<T>> mFindMap = new Dictionary<T, DoubleLinkedListNode<T>>();

        ~CMapList()
        {
            Clear();
        }
        
        /// <summary>
        /// 清空链表
        /// </summary>
        public void Clear()
        {
            while (mDLink.Tail != null)
            {
                Remove(mDLink.Tail.t);
            }
        }
        
        /// <summary>
        /// 插入一个节点到表头
        /// </summary>
        /// <param name="t"></param>
        public void InsertToHead(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (mFindMap.TryGetValue(t, out node) && node != null)
            {
                mDLink.AddToHeader(node);
                return;
            }

            mDLink.AddToHeader(t);
            mFindMap.Add(t, mDLink.Head);
        }

        //从表位弹出一个节点
        public void Pop()
        {
            if (mDLink.Tail != null)
            {
                Remove(mDLink.Tail.t);
            }
        }

        /// <summary>
        /// 删除链表中的一个节点
        /// </summary>
        /// <param name="t"></param>
        public void Remove(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (!mFindMap.TryGetValue(t, out node) || node == null)
            {
                return;
            }
            
            mDLink.RemoveNode(node);
            mFindMap.Remove(t);
        }

        /// <summary>
        /// 获取尾节点
        /// </summary>
        /// <returns></returns>
        public T Back()
        {
            return mDLink.Tail == null ? null : mDLink.Tail.t;
        }

        /// <summary>
        /// 返回节点个数
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return mFindMap.Count;
        }

        /// <summary>
        /// 查找是否存在该节点武器2
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Find(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (!mFindMap.TryGetValue(t, out node) || node == null)
            {
                return false;
            }

            return true;
        }

        public bool Reflesh(T t)
        {
            DoubleLinkedListNode<T> node = null;
            if (!mFindMap.TryGetValue(t, out node) || node == null)
            {
                return false;
            }
            mDLink.MoveToHead(node);
            return true;
        }
    }   
}

