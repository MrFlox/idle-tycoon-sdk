using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.floxgames.IdleTycoonSDK
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Resources/MachinesSettings", order = 2)]
    public class MachinesSettings : ScriptableObject
    {
        public List<MachineIncomeUpgrades> upgradesIncome;
        public List<MachineSpeedUpgrades> upgradesSpeed;
        public List<UpgradeStates> upgradesStates;
    }
    [Serializable]
    public abstract class PriceClass { public float price; }

    [Serializable]
    public class MachineIncomeUpgrades : PriceClass
    {
        public int level;
        public int income;
        public float incomeMult;
    }

    [Serializable]
    public class MachineSpeedUpgrades : PriceClass
    {
        public int level;
        public int speed;
    }
    [Serializable]
    public class UpgradeStates
    {
        public int lockLevel;
        public GameObject prefab;
    }
}