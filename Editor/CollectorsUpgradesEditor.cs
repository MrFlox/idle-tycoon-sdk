#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using com.floxgames.IdleTycoonSDK;

namespace com.floxgames.IdleTycoonSDK
{

    [CustomEditor(typeof(CollectorsManager))]
    public class CollectorsUpgradesEditor : Editor
    {
        CollectorsManager manager;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (manager == null)
                manager = (CollectorsManager)target;

            float priceToUpgrade = manager.getUpgradePrice();
            float priceToHire = manager.getHirePrice();
            if (priceToUpgrade != -1f)
                if (GUILayout.Button($"Upgrade [${priceToUpgrade}]")) manager.upgrade();
            if (priceToHire != -1)
                if (GUILayout.Button($"Hire [${priceToHire}]")) manager.addCollector();
        }

        void OnEnable()
        {
            manager = target as CollectorsManager;
            manager.GetComponent<Transform>().hideFlags = HideFlags.HideInInspector;
        }
    }
}
#endif