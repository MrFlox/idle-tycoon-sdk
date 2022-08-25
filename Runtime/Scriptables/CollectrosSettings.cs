using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.floxgames.IdleTycoonSDK {
    [CreateAssetMenu(fileName = "Settings", menuName = "Resources/CollectorsSettings", order = 2)]
    public class CollectrosSettings : ScriptableObject
    {
        public List<CollectorsUpgrades> upgrades;
        public List<CollectorsHireUpgrades> hireLevels;
        public Collector prefab;
    }

    [Serializable]
    public class CollectorsUpgrades
    {
        public int level;
        public int income;
        public float productivity;
        public float price;
    }

    [Serializable]
    public class CollectorsHireUpgrades
    {
        public int level;
        public float price;
    }
}