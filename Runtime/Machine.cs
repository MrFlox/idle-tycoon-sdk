using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Events;

namespace com.floxgames.IdleTycoonSDK
{
    [SelectionBase]
    public class Machine : MonoBehaviour
    {
        [Header("Levels")]
        public int incomeLevel;
        public int speedLevel;
        [Space(10)]
        //--------------------------------------------------
        [Header("Zones")]
        [SerializeField] ResourceZone inZone;
        [SerializeField] ResourceZone outZone;
        [SerializeField] UnityEvent onProductionComplete;

        [SerializeField] MachinesSettings settings;
        [SerializeField] float startProcessDelay, speed;
        public float progress = 0;
        public bool isActive = false;
        public bool isWaitingForStart = false;
        Vector3 fTextInitPos;
        //-----------------------------------

        public string isEnoughForIncome()
        {

            int nLevel = getNextIncomeUpgradeLevel();
            if (nLevel == -1) return "";
            if (MoneyManager.Instance == null) return "";
            return MoneyManager.Instance.enoughMoney(settings.upgradesIncome[nLevel].price) ? "" : "[Not Enough!] ";
        }

        public string isEnoughForSpeed()
        {
            int nLevel = getNextSpeedUpgradeLevel();
            if (nLevel == -1) return "";
            if (MoneyManager.Instance == null) return "";
            return MoneyManager.Instance.enoughMoney(settings.upgradesSpeed[nLevel].price) ? "" : "[Not Enough!] ";
        }
        int getNextIncomeUpgradeLevel()
        {
            int nextLevel = incomeLevel + 1;
            if (settings.upgradesIncome.Exists(x => x.level == nextLevel))
                return nextLevel;
            return -1;
        }
        int getNextSpeedUpgradeLevel()
        {
            int nextLevel = speedLevel + 1;
            if (settings.upgradesSpeed.Exists(x => x.level == nextLevel))
                return nextLevel;
            return -1;
        }

        public float getPriceToIncomeProcess()
        {
            int nextLevel = getNextIncomeUpgradeLevel();
            if (nextLevel == -1) return -1;
            return settings.upgradesIncome[nextLevel].price;
        }

        public float getPriceToProcessUpgrade()
        {
            int nextLevel = getNextSpeedUpgradeLevel();
            if (nextLevel == -1) return -1;
            return settings.upgradesSpeed[nextLevel].price;
        }

        public enum UpgradeType { income, speed };
        void upgrade(UpgradeType type)
        {
            var nextLevel = type == UpgradeType.income ? getNextIncomeUpgradeLevel() : getNextSpeedUpgradeLevel();
            if (nextLevel != -1)
                if (MoneyManager.Instance.enoughMoney(nextLevel))
                {
                    float price = type == UpgradeType.income ? settings.upgradesIncome[nextLevel].price : settings.upgradesSpeed[nextLevel].price;
                    MoneyManager.Instance.spendMoney(price);
                    if (type == UpgradeType.income)
                        incomeLevel = nextLevel;
                    else
                        speedLevel = nextLevel;
                    updateDataFromSettings();
                }
        }
        public void upgradeIncome() => upgrade(UpgradeType.income);
        public void upgradeSpeed() => upgrade(UpgradeType.speed);

        void updateDataFromSettings()
        {
            GetComponent<MoneyGenerator>().incomeValue = settings.upgradesIncome[incomeLevel].income;
            speed = settings.upgradesSpeed[speedLevel].speed;
        }

        void Awake()
        {
            inZone.onLoadResources += onInZoneLoadResource;
            updateDataFromSettings();
        }

        void onInZoneLoadResource()
        {
            StartCoroutine(loadResource());
        }
        IEnumerator loadResource()
        {
            if (isWaitingForStart) yield return null;
            if (isActive) yield return null;
            isWaitingForStart = true;
            while (!readyToStartProcess()) yield return null;
            StartCoroutine(loadWithTimeout(startProcessDelay));
        }

        bool readyToStartProcess() => outZone.isStorageSpace() && inZone.readyToCollect() && !isActive;

        void OnDrawGizmos()
        {
            Vector3 pos = transform.position + Vector3.up * .8f;
            // pos.x -= transform.GetChild(0).localScale.x / 2;
            pos.x -= 1.5f;
#if UNITY_EDITOR
            Handles.Label(pos, $"{name}: {Mathf.Round(progress * 100)}%");
#endif
            if (!isActive) return;
            // Gizmos.color = Color.red;
            // Gizmos.DrawSphere(transform.position + Vector3.up * 4.3f, 1.0f);
        }

        IEnumerator loadWithTimeout(float time = 0)
        {
            yield return new WaitForSeconds(time);
            while (!readyToStartProcess()) yield return null;
            isWaitingForStart = false;
            isActive = true;
            inZone.collect();
            startWork();
        }

        void startWork()
        {
            // progress.SetActive(true);
            StartCoroutine(workProgress());
        }

        IEnumerator workProgress()
        {
            while (!outZone.isStorageSpace()) yield return null;
            // float startTime = Time.time;
            // while (Time.time - startTime < upgradableSettings.productionTime)
            // {
            //     progress.value = (Time.time - startTime) / upgradableSettings.productionTime;
            //     yield return null;
            // }
            float prodTime = speed;
            float startTime = Time.time;
            while (Time.time - startTime < prodTime)
            {
                progress = (Time.time - startTime) / prodTime;
                yield return null;
            }
            // yield return new WaitForSeconds(0);
            complete();
        }



        void complete()
        {
            outZone.addResource();
            isActive = false;
            onProductionComplete?.Invoke();
            if (inZone.readyToCollect()) StartCoroutine(loadResource());
            // progress.SetActive(false);
            // flyText(upgradableSettings.profitPerTime);

            // MoneyManager.Instance.addMoney(upgradableSettings.profitPerTime);
        }

        // private void flyText(float amount)
        // {
        //     fText.transform.position = fTextInitPos;
        //     fText.text = "+" + amount;
        //     StartCoroutine(flyingText(1.0f));
        // }

        // IEnumerator flyingText(float time)
        // {
        //     float startTime = Time.time;
        //     while (Time.time - startTime < time)
        //     {
        //         Vector3 pos = fText.transform.position;
        //         fText.transform.position = new Vector3(pos.x, pos.y + Time.deltaTime * 3, pos.z);
        //         yield return null;
        //     }
        //     fText.text = "";
        // }
    }
}
