using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.floxgames.IdleTycoonSDK
{
    [CreateAssetMenu(fileName = "Resource", menuName = "Resources/NewResource", order = 1)]
    public class ResourceScriptableObject : ScriptableObject
    {
        public string resourceName;
        public GameObject visualPrefab;
    }

}

