using System.Drawing;
using UnityEngine;

namespace com.floxgames.IdleTycoonSDK {
    public class WayPoint : MonoBehaviour
    {
        public PointType pointType;

        public WayPoint(GameObject obj)
        {
            setPosition(obj.transform.position);
        }

        public void setPosition(Vector3 pos) => transform.position = pos;

        // void Awake() => GetComponent<MeshRenderer>().enabled = false;

        void OnDrawGizmos()
        {
            // Gizmos.color = Color.green;
            // Gizmos.DrawSphere(transform.position, .15f);
        }
    }
}