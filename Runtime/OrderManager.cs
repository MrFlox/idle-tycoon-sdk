using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public List<Order> newOrders;
    public List<Order> activeOrders;
    [SerializeField] float orderGenTime = 3.0f;
    [SerializeField] ResourceZone targetOrderZone;
    Order orderInProcess = null;
    public bool isOrderInProgress = false;

    void Awake()
    {
        StartCoroutine(GenerateNewOrders());
    }

    IEnumerator GenerateNewOrders()
    {
        yield return new WaitForSeconds(orderGenTime);
        addNewRandomOrder();
        StartCoroutine(GenerateNewOrders());
    }

    public void onTargetOrderZoneCompelete()
    {
        completeOrder();
        StartCoroutine(selectNextOrder());
    }

    IEnumerator selectNextOrder()
    {
        while (activeOrders.Count == 0) yield return null;
        setTargetResourceTarget();
    }

    private void setTargetResourceTarget()
    {
        isOrderInProgress = true;
        orderInProcess = activeOrders[0];
        targetOrderZone.maxCapacity = orderInProcess.productCount;
    }

    void Update() => checkNewOrders();

    private void checkNewOrders()
    {
        // foreach (Order order in newOrders)
        for (int i = newOrders.Count - 1; i >= 0; i--)
        {
            Order order = newOrders[i];
            if (order.isActive)
            {
                addActiveOrder(order);
                break;
            }
        }
    }

    void completeOrder()
    {

        MoneyManager.Instance.addMoney(activeOrders[0].reward.moneyValue);
        activeOrders.RemoveAt(0);
        targetOrderZone.maxCapacity = 0;
        targetOrderZone.resourceValue = 0;
        isOrderInProgress = false;
    }

    private void addActiveOrder(Order order)
    {
        activeOrders.Add(order);
        newOrders.Remove(order);
        order.isActive = false;

        if (!isOrderInProgress)
        {
            setTargetResourceTarget();
        }
    }

    void addNewRandomOrder()
    {
        Order newOrder = new Order()
        {
            productCount = 10,
            productType = ProductType.wine,
            reward = new Reward() { moneyValue = 100 }
        };

        newOrders.Add(newOrder);
    }
}


