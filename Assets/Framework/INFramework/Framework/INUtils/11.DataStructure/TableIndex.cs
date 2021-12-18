using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace INFramework
{
	public class TableIndex<TKeyType, TDataItem>
	{
		private Dictionary<TKeyType, List<TDataItem>> m_Index = new Dictionary<TKeyType, List<TDataItem>>();

		private Func<TDataItem, TKeyType> m_GetKeyByDataItem = null;

		public TableIndex(Func<TDataItem, TKeyType> rKeyGetter)
        {
			m_GetKeyByDataItem = rKeyGetter;
        }

		public void Add(TDataItem rDataItem)
        {
			var rKey = m_GetKeyByDataItem(rDataItem);
            if (m_Index.ContainsKey(rKey))
			{
				m_Index[rKey].Add(rDataItem);
            }
            else
            {
				m_Index.Add(rKey, new List<TDataItem>()
				{
					rDataItem
				});
            }
        }

        public void Remove(TDataItem rDataItem)
        {
            var key = m_GetKeyByDataItem(rDataItem);

            m_Index[key].Remove(rDataItem);
        }

        public IEnumerable<TDataItem> Get(TKeyType rKey)
        {
            List<TDataItem> retList = null;
            if(m_Index.TryGetValue(rKey, out retList))
            {
                return retList;
            }
            return Enumerable.Empty<TDataItem>();
        }

        public void Clear()
        {
            m_Index.Clear();
        }
    }
}

