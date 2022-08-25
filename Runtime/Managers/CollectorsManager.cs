using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.floxgames.IdleTycoonSDK {
    public class CollectorsManager : MonoBehaviour
    {
        public int maxQuantity;
        public int level;
        public List<Collector> collectors;

        [SerializeField] CollectrosSettings settings;
        [SerializeField] ResourceYard yard;
        [SerializeField] ResourceZone outZone;

        public float getUpgradePrice()
        {
            int nextLevel = getNextUpgradeLevel();
            if (nextLevel == -1) return -1;
            return settings.upgrades[nextLevel].price;
        }

        public float getHirePrice()
        {
            int nextLevel = settings.hireLevels.Count > collectors.Count + 1 ? collectors.Count + 1 : -1;
            if (nextLevel == -1) return -1;
            return settings.hireLevels[nextLevel].price;
        }

        int getNextUpgradeLevel()
        {
            int nextLevel = level + 1;
            if (settings.upgrades.Exists(x => x.level == nextLevel))
                return nextLevel;
            return -1;
        }

        public void addCollector()
        {
            float hirePrice = settings.hireLevels[collectors.Count].price;
            if (!MoneyManager.Instance.enoughMoney(hirePrice)) return;
            MoneyManager.Instance.spendMoney(hirePrice);
            Collector newCollector = Instantiate(settings.prefab);
            collectors.Add(newCollector);
            newCollector.productivity = settings.upgrades[level].productivity;
            newCollector.outZone = outZone;
            newCollector.generators = yard;
            newCollector.Initialize();
        }

        public void upgrade()
        {
            var nextLevel = getNextUpgradeLevel();
            if (nextLevel != -1)
                if (MoneyManager.Instance.enoughMoney(nextLevel))
                {
                    MoneyManager.Instance.spendMoney(settings.upgrades[nextLevel].price);
                    level = nextLevel;

                    updateValuesFromSettings(nextLevel);
                }
        }

        private void updateValuesFromSettings(int nextLevel)
        {
            var data = settings.upgrades[nextLevel];
            foreach (Collector c in collectors)
            {
                c.productivity = data.productivity;
                c.setIncome(data.income);
            }
        }
    }
}
