using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NewBehaviourScript : ScriptableObject
{
    public List<GameObject> avatars;
    public List<string> orderDescriptions;
    public int OrderTime;//118 s
}

public enum ProductType
{
    grapes,
    wine
}

[Serializable]
public class Order
{
    public GameObject avatar;
    public ProductType productType;
    public int productCount;
    public float orderValue;
    public string description;
    public bool isActive = false;
    public Reward reward;
}

public class Mission
{
    public GameObject icon;
    public string description;
    public Reward reward;
    public Task task;
}

[Serializable]
public class Reward
{
    public float moneyValue;
    public void getReward() { }
}

public class Task
{

}

public class UpgradeTask : Task
{

}

public class ProductionTask : Task
{

}

public class BuildTask : Task
{

}

public class HireTask : Task
{

}

public class CompleteOrderTask : Task
{

}

public class LevelTask : Task
{

}


