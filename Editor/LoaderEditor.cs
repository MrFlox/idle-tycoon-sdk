#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;

using com.floxgames.IdleTycoonSDK;

using UnityEditor;

using UnityEngine;

namespace com.floxgames.IdleTycoonSDK
{
    [CustomEditor(typeof(Loader))]
    public class LoaderEditor : Editor
    {
        Loader target;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (target == null) target = base.target as Loader;
            float priceForUpgrade = target.getPriceToUpgrade();
            string isEnoughForUpgrade() => target.isEnoughForUpgrade();

            if (priceForUpgrade != -1f)
                if (GUILayout.Button($"{isEnoughForUpgrade()} Upgrade [${priceForUpgrade}]")) target.upgrade();
            // if (priceToProcessUpgrade != -1)
            //     if (GUILayout.Button($" {isEnoughForSpeed()} Upgrade Speed [${priceToProcessUpgrade}]")) target.upgradeSpeed();


        }
    }
}
#endif