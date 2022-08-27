using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Linq;


namespace com.floxgames.IdleTycoonSDK
{
    public enum LoaderState
    {
        loading,
        unloading,
        idle,
        parking,
        none
    }
    // public enum LoaderType { loader, track }

    /// <summary>
    /// Класс подргузчика 
    /// </summary>
    [SelectionBase]
    public class Loader : MonoBehaviour
    {
        public LoaderState nextState = LoaderState.none;
        // [SerializeField] LoaderType type = LoaderType.loader;
        [Header("Resource Zones")]
        [SerializeField] List<ResourceZone> inZones;
        [SerializeField] List<ResourceZone> outZones;

        public ResourceZone inZone, outZone;

        //---------------------------------------------------------
        public int capacity;
        public int currentLoad;
        public bool isGoingToBase = false;
        public LoaderState state;
        [SerializeField] float targetColissionRadius = 0.05f;
        Func<bool> isReadyToProcess;
        Action stateCallback = null;

        public bool isWaiting = false;
        public int level;
        [SerializeField] WayPointNavController transport;
        [SerializeField] LoaderSettings settings;
        int getNextIncomeUpgradeLevel()
        {
            int nextLevel = level + 1;
            if (settings.upgrades.Exists(x => x.level == nextLevel))
                return nextLevel;
            return -1;
        }
        public float getPriceToUpgrade()
        {
            int nextLevel = getNextIncomeUpgradeLevel();
            if (nextLevel == -1) return -1;
            return getSettingsByLevel(nextLevel).price;
        }

        public string isEnoughForUpgrade()
        {
            int nLevel = getNextIncomeUpgradeLevel();
            if (nLevel == -1) return "";
            if (MoneyManager.Instance == null) return "";
            return MoneyManager.Instance.enoughMoney(getSettingsByLevel(nLevel).price) ? "" : "[Not Enough!] ";
        }
        public void upgrade()
        {
            var nextLevel = getNextIncomeUpgradeLevel();
            if (nextLevel != -1)
                if (MoneyManager.Instance.enoughMoney(nextLevel))
                {
                    float price = getSettingsByLevel(nextLevel).price;
                    MoneyManager.Instance.spendMoney(price);
                    level = nextLevel;
                    updateDataFromSettings();
                }
        }

        LoaderUpgrades getCurrentSettings() => getSettingsByLevel(level);
        LoaderUpgrades getSettingsByLevel(int lvl) => settings.upgrades[lvl];

        private void updateDataFromSettings()
        {
            transport.speed = getCurrentSettings().speed;
            transport.rotationSpeed = transport.speed * 3;
            capacity = getCurrentSettings().capacity;
        }

        void OnDrawGizmos()
        {
            //------------------
#if UNITY_EDITOR
            Handles.Label(transform.position + Vector3.up * .8f, $"{currentLoad}/{capacity}");
#endif
            //-----------------------------

            void drawInZoneConnectors()
            {
                if (inZones.Count == 0) return;
                Gizmos.color = Color.green;
                foreach (ResourceZone r in inZones)
                    Gizmos.DrawLine(transform.position, r.transform.position);
            }

            void drawOutZoneConnectors()
            {
                if (outZones.Count == 0) return;
                Gizmos.color = Color.red;
                foreach (ResourceZone r in outZones)
                    Gizmos.DrawLine(transform.position, r.transform.position);
            }

            drawInZoneConnectors();
            drawOutZoneConnectors();

            // Gizmos.color = Color.white;
            // Gizmos.DrawWireSphere(navController.transport.transform.position, targetColissionRadius);
        }

        ResourceZone getFreeInZone() => null;
        ResourceZone getFreeOutZone() => null;

        void Awake()
        {
            setActiveInOutZone();
            initNavigationController();
            updateDataFromSettings();
            foreach (var outZone in outZones) outZone.onCollectResources += OnOutZoneCollectResources;
            initTrasport();
            setState(LoaderState.loading);
        }

        void initNavigationController()
        {
            transport.moveToParking();
            transport.init(transform.position, inZone, outZone);
            transport.atThePoint += atThePointHandler;
        }

        private void atThePointHandler() => atTheTarget();

        void setActiveInOutZone()
        {
            if (inZones.Count > 0) inZone = inZones[0];
            if (outZones.Count > 0) outZone = outZones[0];
        }

        void OnOutZoneCollectResources()
        {
            if (currentLoad == 0) return;
            setState(LoaderState.unloading);
        }

        void initTrasport() => transport.initTransport(transform);

        bool readyToUnload()
        {
            int value = outZones.Max(x => x.freeSpace());
            ResourceZone item = outZones.First(x => x.freeSpace() == value);
            outZone = item;
            return outZone.isStorageSpace();
        }

        bool readyToLoad()
        {
            //выбираем зону, где больше ресурсов 
            int value = inZones.Min(x => x.freeSpace());
            ResourceZone item = inZones.First(x => x.freeSpace() == value);
            inZone = item;
            return inZone.readyToCollect() && currentLoad < capacity;
        }

        private void goToParking() => goToBase();

        public void setState(LoaderState newState) => _setState(newState);

        void _setState(LoaderState newState, LoaderState nextState = LoaderState.none)
        {
            state = newState;
            if (nextState != LoaderState.none) this.nextState = nextState;

            switch (newState)
            {
                case LoaderState.loading:
                    if (!readyToLoad())
                        _setState(LoaderState.parking, newState);
                    else
                        onLoadingState();
                    break;
                case LoaderState.unloading:
                    if (!readyToUnload())
                        _setState(LoaderState.parking, newState);
                    else
                        onUnloadingState();
                    break;
                case LoaderState.parking:
                    goToParking();
                    break;
                case LoaderState.idle:
                    onIdleState();
                    break;
                default:
                    throw new Exception("asdfasdf");
            }
        }

        void onLoadingState() => setTransportTargetToUnload();

        private void setTransportTargetToUnload()
        {
            if (transport.points == null)
                transport.setTargetPosition(inZone.transform.position);
            else
                transport.setTarget(PointType.start);

            transport.isCanGo = true;
        }
        private void setTransportTargetToLoad()
        {
            if (transport.points == null)
                transport.setTargetPosition(outZone.transform.position);
            else
                transport.setTarget(PointType.end);

            transport.isCanGo = true;
        }
        private void setTransportTargetToParking()
        {
            if (transport.points == null)
                transport.setTargetPosition(transform.position);
            else
                transport.setTarget(PointType.parking);
            transport.isCanGo = true;
        }
        void onUnloadingState()
        {
            setTransportTargetToLoad();
        }

        void onIdleState()
        {
            transport.isCanGo = true;
            goToBase();
        }

        public void atTheTarget()
        {
            transport.isCanGo = false;
            if (state == LoaderState.unloading)
            {
                currentLoad -= outZone.addResource(currentLoad);
                updateCurrentInZones();
                setState(LoaderState.loading);
                return;
            }
            if (state == LoaderState.loading)
            {
                StartCoroutine(Collect());
                return;
            }
            if (state == LoaderState.parking)
            {
                isGoingToBase = false;

                if (nextState != LoaderState.none)
                {
                    StartCoroutine(WaitForNextState(nextState));
                }
                return;
            }
        }

        private void updateCurrentInZones()
        {
            foreach (ResourceZone r in inZones)
            {
                if (r.readyToCollect())
                {
                    inZone = r;
                    break;
                }
            }
        }

        IEnumerator WaitForNextState(LoaderState nState)
        {
            bool readyToProcess() => nState == LoaderState.loading ? readyToLoad() : readyToUnload();

            while (!readyToProcess())
            {
                isWaiting = true;
                yield return null;
            }
            isWaiting = false;
            nextState = LoaderState.none;
            _setState(nState);
        }
        void goToBase()
        {
            setTransportTargetToParking();
            isGoingToBase = true;
        }

        int getAvailableCopactiy() => capacity - currentLoad;
        IEnumerator Collect()
        {
            currentLoad += inZone.collect(getAvailableCopactiy());
            setState(LoaderState.unloading);
            yield return null;
        }


    }
}


