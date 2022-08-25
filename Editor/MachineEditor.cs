using UnityEditor;
using UnityEngine;
using com.floxgames.IdleTycoonSDK;
namespace com.floxgames.IdleTycoonSDK
{
    [CustomEditor(typeof(Machine))]
    public class MachineEditor : Editor
    {
        Machine target;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (target == null) target = base.target as Machine;
            float priceToIncomeProcess = target.getPriceToIncomeProcess();
            float priceToProcessUpgrade = target.getPriceToProcessUpgrade();

            string isEnoughForIncome() => target.isEnoughForIncome();
            string isEnoughForSpeed() => target.isEnoughForSpeed();

            if (priceToIncomeProcess != -1f)
                if (GUILayout.Button($"{isEnoughForIncome()} Upgrade Income [${priceToIncomeProcess}]")) target.upgradeIncome();
            if (priceToProcessUpgrade != -1)
                if (GUILayout.Button($" {isEnoughForSpeed()} Upgrade Speed [${priceToProcessUpgrade}]")) target.upgradeSpeed();
        }
    }
}
