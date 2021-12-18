/****************************************************
    文件：MonsterData.cs
    作者：TA94
    日期：2021/10/31 15:37:2
    功能：Nothing
*****************************************************/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class MonsterData:ExcelBase
{
    /// <summary>
    /// 編輯器下初始类转xml
    /// </summary>
    public override void Construction()
    {
        AllMonster = new List<MonsterBase>();
        for (int i = 0; i < 5; i++)
        {
            MonsterBase monster = new MonsterBase();
            monster.Id = i + 1;
            monster.Name = i + "sq";
            monster.OutLook = "Assets/GameData/Prefabs/Attack.prefab";
            monster.Rare = 2;
            monster.Height = 2 + i;
            AllMonster.Add(monster);
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public override void Init()
    {
        mAllMonsterDic.Clear();
        foreach (MonsterBase monster in AllMonster)
        {
            if (mAllMonsterDic.ContainsKey(monster.Id))
            {
                Debug.LogError(monster.Name + " 有重复ID");
            }
            else
            {
                mAllMonsterDic.Add(monster.Id, monster);
            }
        }
    }

    [XmlIgnore]
    public Dictionary<int, MonsterBase> mAllMonsterDic = new Dictionary<int, MonsterBase>();

    [XmlElement("AllMonster")]
    public List<MonsterBase> AllMonster { get; set; }
}

[Serializable]
public class MonsterBase
{
    //ID
    [XmlAttribute("Id")]
    public int Id { get; set; }
    //Name
    [XmlAttribute("Name")]
    public string Name { get; set; }
    //预知路径
    [XmlAttribute("OutLook")]
    public string OutLook { get; set; }
    //怪物等级
    [XmlAttribute("Level")]
    public int Level { get; set; }
    //怪物稀有度
    [XmlAttribute("Rare")]
    public int Rare { get; set; }
    //怪物高度
    [XmlAttribute("Height")]
    public float Height { get; set; }
}
