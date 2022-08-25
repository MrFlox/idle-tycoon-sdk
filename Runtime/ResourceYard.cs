using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.floxgames.IdleTycoonSDK {
    public class ResourceYard : MonoBehaviour
    {
        [SerializeField] float growthTime;
        public List<ResouceGenerator> generators;
        public ResourceScriptableObject resource;

        public ResourceZone outZone;
        public bool isInitialized = false;

        void Awake() => initializeGenerators();

        void initializeGenerators()
        {
            int childrenCount = transform.childCount;
            ResouceGenerator rGen;
            for (int i = 1; i < childrenCount; i++)
            {
                rGen = transform.GetChild(i).GetComponent<ResouceGenerator>();
                rGen.growthTime = growthTime;
                generators.Add(rGen);

            }
            isInitialized = true;
        }

    }
}