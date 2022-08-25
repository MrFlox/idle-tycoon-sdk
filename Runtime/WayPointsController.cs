using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.floxgames.IdleTycoonSDK {
    public class WayPointsController : MonoBehaviour
    {
        public List<WayPoint> points;
        public WayPoint start, end, parking;

        void Awake()
        {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
                points.Add(transform.GetChild(i).GetComponent<WayPoint>());
        }
        public void setStartAndEndPoints(WayPoint start, WayPoint end, WayPoint parking)
        {
            this.start = start;
            points.Add(start);
            this.end = end;
            points.Add(end);
            this.parking = parking;
            points.Add(parking);
        }
    }
}
