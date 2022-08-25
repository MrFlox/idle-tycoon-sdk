using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    public ResourceZone outZone;
    public ResourceYard generators;
    public float productivity;
    [SerializeField] UnityEvent onProductionComplete;
    public int activeTarget = 0;
    ResouceGenerator activeGenerator;
    Vector3 currentTarget;

    public void Initialize() => StartCoroutine(Init());
    int getNextTargeIndex_old()
    {
        int newIndex = activeTarget + 1;
        if (newIndex > generators.generators.Count - 1) return 0;
        return newIndex;
    }


    int getNextTargeIndex()
    {
        var nextInd = 0;
        int generatorsCount = generators.generators.Count;
        for (int i = 0; i < generatorsCount; i++)
        {
            nextInd = activeTarget + i;
            if (nextInd > generatorsCount - 1) nextInd -= generatorsCount;
            ResouceGenerator g = generators.generators[nextInd];
            if (!g.isActivated && g.state == GeneratorState.Full)
                return nextInd;
        }
        return -1;
    }

    IEnumerator Init()
    {
        while (!generators.isInitialized) yield return null;
        StartCoroutine(setNextTarget());
    }

    bool isNextGeneratorIsFull()
    {
        return generators.generators[getNextTargeIndex()].state == GeneratorState.Full;
    }

    ResouceGenerator getGeneratorAtIndex(int ind) => generators.generators[ind];

    IEnumerator setNextTarget()
    {
        int nextIndex = getNextTargeIndex();
        while (nextIndex == -1)
            yield return null;
        activeTarget = nextIndex;
        if (activeGenerator != null) activeGenerator.isActivated = false;
        activeGenerator = generators.generators[activeTarget];
        activeGenerator.isActivated = true;
        currentTarget = activeGenerator.transform.position;

        moveToTarget();
        startCollection();
    }

    public void setIncome(int income) => GetComponent<MoneyGenerator>().incomeValue = income;

    void startCollection() => StartCoroutine(Collection());

    IEnumerator Collection()
    {
        yield return new WaitForSeconds(productivity);
        while (!isResouceZoneReadyToCollect()) yield return null;
        Collect();
        StartCoroutine(setNextTarget());
    }

    bool isResouceZoneReadyToCollect() => outZone.isStorageSpace();

    void Collect()
    {
        // MoneyManager.Instance.addMoney(moneyPerTick);
        activeGenerator.changeState(GeneratorState.Empty);
        outZone.addResource();
        onProductionComplete?.Invoke();
    }

    void moveToTarget() => transform.position = currentTarget;
}
