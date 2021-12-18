/****************************************************
    文件：DoubleLinkedList.cs
    作者：TA94
    日期：2021/10/24 20:47:54
    功能：Nothing
*****************************************************/
using INFramework.Framework.AssetBundles;
using UnityEngine;

namespace INFramework.Framework.AssetBundles
{
    /// <summary>
    /// 双向链表结构节点
    /// </summary>
    public class DoubleLinkedListNode<T> where T : class, new()
    {
        //前节点
        public DoubleLinkedListNode<T> prev = null;
        //后节点
        public DoubleLinkedListNode<T> next = null;
        //当前节点
        public T t = null;
    }

    /// <summary>
    /// 双线链表结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DoubleLinkedList<T> where T : class, new()
    {
        //表头
        public DoubleLinkedListNode<T> Head = null;
        //表尾
        public DoubleLinkedListNode<T> Tail = null;
        //双向链表结构类对象池
        protected ClassObjectPool<DoubleLinkedListNode<T>> mDoubleLinkedNodePool =
            ObjectManager.Instance.GetOrCreateClassPool<DoubleLinkedListNode<T>>(500);
        //链表节点个数
        protected int mCount = 0;

        public int Count => this.mCount;

        /// <summary>
        /// 添加节点到头部
        /// </summary>
        public DoubleLinkedListNode<T> AddToHeader(T t)
        {
            DoubleLinkedListNode<T> pListNode = mDoubleLinkedNodePool.Spwan(true);
            pListNode.next = null;
            pListNode.prev = null;
            pListNode.t = t;
            return AddToHeader(pListNode);
        }

        /// <summary>
        /// 添加节点到头部
        /// </summary>
        public DoubleLinkedListNode<T> AddToHeader(DoubleLinkedListNode<T> pNode)
        {
            if(pNode == null) return null;
            
            pNode.prev = null;
            //空链表
            if (Head == null)
            {
                Head = Tail = pNode;
            }
            else
            {
                pNode.next = Head;
                Head.prev = pNode;
                Head = pNode;
            }

            mCount++;
            return Head;
        }
        
        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        public DoubleLinkedListNode<T> AddToTail(T t)
        {
            DoubleLinkedListNode<T> pListNode = mDoubleLinkedNodePool.Spwan(true);
            pListNode.prev = null;
            pListNode.next = null;
            pListNode.t = t;
            return AddToTail(pListNode);
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        public DoubleLinkedListNode<T> AddToTail(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null) return null;

            pNode.next = null;
            if (Tail == null)
            {
                Head = Tail = pNode;
            }
            else
            {
                Tail.next = pNode;
                pNode.prev = Tail;
                Tail = pNode;
            }

            mCount++;
            return Tail;
        }

        public void RemoveNode(DoubleLinkedListNode<T> pNode)
        {
            if(pNode == null) return;

            if (pNode == Head)
            {
                Head = pNode.next;
            }

            if (pNode == Tail)
            {
                Tail = pNode.prev;
            }

            if (pNode.prev != null)
            {
                pNode.prev.next = pNode.next;
            }

            if (pNode.next != null)
            {
                pNode.next.prev = pNode.prev;
            }
            
            //清空节点
            pNode.prev = pNode.next = null;
            pNode.t = null;
            mDoubleLinkedNodePool.Recycle(pNode);
            mCount--;
        }

        public void MoveToHead(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null || Head == pNode)
            {
                return;
            }
            //被回收
            if (pNode.prev == null && pNode.next == null)
            {
                return;
            }

            if (pNode == Tail)
            {
                Tail = pNode.prev;
            }

            if (pNode.prev != null)
            {
                pNode.prev.next = pNode.next;
            }

            if (pNode.next != null)
            {
                pNode.next.prev = pNode.prev;
            }
            
            //设置头节点
            pNode.prev = null;
            pNode.next = Head;
            Head = pNode;

            //只有两个的情况
            if (Tail == null)
            {
                Tail = Head;
            }
        }
    }
}
