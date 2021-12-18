using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 根据名字索引Res
/// </summary>
namespace INFramework
{
    public class ResTable : Table<Res>
    {
        public TableIndex<string, Res> NameIndex { get; private set; }

        public ResTable()
        {
            NameIndex = new TableIndex<string, Res>(res => res.Name);
        }

        public Res GetResWithSearchKeys(ResSearchKeys rResSearchKeys)
        {
            return NameIndex.Get(rResSearchKeys.Address).FirstOrDefault(r => r.MatchResSearchKeysWithoutName(rResSearchKeys));
        }

        protected override void OnAdd(Res rItem)
        {
            NameIndex.Add(rItem);
        }

        protected override void OnRemove(Res rItem)
        {
            NameIndex.Remove(rItem);
        }

        protected override void OnClear()
        {
            NameIndex.Clear();
        }
    }
}

