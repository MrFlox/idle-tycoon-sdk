using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.floxgames.IdleTycoonSDK {
    public class MoneyGenerator : MonoBehaviour
    {
        public float incomeValue;

        public void collectMoney() => MoneyManager.Instance.addMoney(incomeValue);
    }
}