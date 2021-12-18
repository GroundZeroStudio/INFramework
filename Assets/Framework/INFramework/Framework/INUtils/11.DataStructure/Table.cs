using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace INFramework
{
    public abstract class Table<TDataItem>:IEnumerable<TDataItem> where TDataItem : class
    {
		List<TDataItem> m_Items = new List<TDataItem>();
        public void Add(TDataItem rItem)
        {
			m_Items.Add(rItem);
            OnAdd(rItem);
        }

        public void Remove(TDataItem rItem)
        {
            m_Items.Remove(rItem);
            OnRemove(rItem);
        }

        public void Update()
        {

        }

        public IEnumerable<TDataItem> Get(Func<TDataItem, bool> rConditiron)
        {
            return m_Items.Where(rConditiron);
        }

        public void Clear()
        {
            m_Items.Clear();
            OnClear();
        }

        public IEnumerator<TDataItem> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected abstract void OnAdd(TDataItem rItem);
        protected abstract void OnRemove(TDataItem rItem);
        protected abstract void OnClear();


    }
}

