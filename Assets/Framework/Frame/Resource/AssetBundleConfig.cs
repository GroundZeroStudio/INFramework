/****************************************************
    文件：AssetBuidlConfig.cs
    作者：TA94
    日期：2021/10/16 17:56:22
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace INFramework.Framework.AssetBundles 
{
    [Serializable]
    public class AssetBundleConfig
    {
        [XmlElement("ABList")]
        public List<ABBase> ABList { get; set; }
    }


    [Serializable]
    public class ABBase
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }
        [XmlAttribute("Crc")]
        public uint Crc { get; set; }
        [XmlAttribute("ABName")]
        public string ABName { get; set; }
        [XmlAttribute("AssetName")]
        public string AssetName { get; set; }
        [XmlElement("ABDependce")]
        public List<string> ABDependce { get; set; }
    }

}


