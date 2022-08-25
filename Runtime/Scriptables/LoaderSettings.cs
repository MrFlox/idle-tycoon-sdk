using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.floxgames.IdleTycoonSDK
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Resources/LoadersSettings", order = 2)]
    public class LoaderSettings : ScriptableObject
    {
        public List<LoaderUpgrades> upgrades;
    }

    [Serializable]
    public class LoaderUpgrades
    {
        public int level;
        public int capacity;
        public float speed;
        public float price;
    }



    // [Serializable]
    // public class CollectorsSettings
    // {
    //     public string name = "vaasdf";
    // }

    // [Serializable]
    // public class LoadersSettings
    // {
    //     public string name = "adfadsf";
    // }

    // using System.Collections;
    // using System.Collections.Generic;
    // using UnityEngine;

    // [CreateAssetMenu(fileName = "Resource", menuName = "Resources/NewResource", order = 1)]
    // public class ResourceScriptableObject : ScriptableObject
    // {
    //     public string resourceName;
    //     public GameObject visualPrefab;
    // }


}