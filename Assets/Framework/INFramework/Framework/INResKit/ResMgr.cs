using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace INFramework
{
	public class ResMgr : Singleton<ResMgr>
	{
		private ResMgr()
        {

        }

		private ResTable m_LoadedReses = new ResTable();

		public void AddRes(Res rRes)
        {
			m_LoadedReses.Add(rRes);
        }

		public void RemoveRes(string rName)
		{
			var res2Remove = m_LoadedReses.NameIndex.Get(rName).FirstOrDefault();
			m_LoadedReses.Remove(res2Remove);
		}

		public Res GetRes(string rName)
        {
			return m_LoadedReses.NameIndex.Get(rName).FirstOrDefault();
        }

		public Res GetRes(ResSearchKeys rKeys)
        {
			return m_LoadedReses.GetResWithSearchKeys(rKeys);
        }
	}
}

