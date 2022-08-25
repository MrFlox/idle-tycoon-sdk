using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    public float moneyAmount;
    public void addMoney(float value) => moneyAmount += value;
    public void spendMoney(float value)
    {
        if (value > moneyAmount)
        {
            Debug.Log("Not enough money!");
            return;
        }
        moneyAmount -= value;
    }

    // void Update() => text.text = moneyAmount.ToString();

    public bool enoughMoney(float price) => price <= moneyAmount;

    void OnGUI()
    {
        /*
        var style = new GUIStyle();
        style.fontSize = 80;
        style.normal.textColor = Color.red;
        GUI.Label(new Rect(400, 10, 50, 50), moneyAmount.ToString(), style);
        */


    }

}
