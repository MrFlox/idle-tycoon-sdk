using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderManager : MonoBehaviour
{
    public int quantity;
    public int maxQuantity;
    public List<Loader> loaders;
    public int level;

    [SerializeField] LoaderSettings settings;
}
