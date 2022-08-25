using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGenerator : MonoBehaviour
{
    public float incomeValue;

    public void collectMoney() => MoneyManager.Instance.addMoney(incomeValue);
}
