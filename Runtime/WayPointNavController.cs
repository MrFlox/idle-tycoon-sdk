using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace com.floxgames.IdleTycoonSDK {
    public enum PointType { start = 0, end = 1, parking = 2, move = 3 };
    // public enum TargetType { start, end, parking };
    public class WayPointNavController : MonoBehaviour
    {
        public delegate void AtThePoint();
        public event AtThePoint atThePoint;
        public WayPointsController points;
        public PointType target;
        public Vector3 currentPoint;
        public WayPoint curWayPoint;
        public int currentPointIndex = 0;
        public Vector3 start, end, parking;
        [SerializeField] WayPoint wpPrefab;
        [SerializeField] WayPointsController wpControllerPrefab;
        public float speed, rotationSpeed;
        public bool isCanGo = true;
        bool initialized = false;

        void Awake()
        {
            if (points != null)
            {
                WayPoint parkingPt = points.points.Find(x => x.pointType == PointType.parking);
                currentPointIndex = points.points.IndexOf(parkingPt);
                currentPoint = parkingPt.transform.position;
            }
        }


        void Update()
        {
            if (this.name == "Transport 1")
            {
                Debug.Log("asdfasdf");
            }
            if (!initialized && points != null)
            {
                // currentPoint = points.start.transform.position;
                // initialized = true;
            }

            if (!isCanGo || !initialized) return;
            bool isAtTarget = Vector3.Distance(transform.position, currentPoint) < 0.1f;
            if (isAtTarget) atTheWayPoint();
            move();
        }

        void move()
        {
            // if (currentPoint == Vector3.zero)
            // {
            //     return;
            // }
            if (!initialized) return;

            Vector3 direction = Vector3.zero;
            direction = (currentPoint - transform.position).normalized;
            var curPos = transform.position;
            transform.position = curPos + direction * speed * Time.deltaTime;
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }
        public void atTheWayPoint()
        {
            if (points != null)
            {
                if (points.points[currentPointIndex].pointType == target)
                    atThePoint?.Invoke();
            }
            else
            {
                atThePoint?.Invoke();
            }
            nextPoint();
        }

        public void nextPoint()
        {
            if (points == null) return;
            currentPointIndex++;
            if (currentPointIndex > points.points.Count - 1) currentPointIndex = 0;
            curWayPoint = points.points[currentPointIndex];
            currentPoint = curWayPoint.transform.position;
        }

        public void init(Vector3 parkingPosition, ResourceZone inZone = null, ResourceZone outZone = null)
        {
            initialized = true;
        }

        internal void initTransport(Transform transform)
        {
            // throw new NotImplementedException();
        }

        internal void setTarget(PointType target)
        {
            this.target = target;
        }

        public void setTargetPosition(Vector3 newPos)
        {
            currentPoint = newPos;

            if (!initialized) initialized = true;
        }

        internal void moveToParking()
        {
            // if (points == null) return;
            if (points == null) return;
            Debug.Log(points.points.Count);
            WayPoint parkingPt = points.points.Find(x => x.pointType == PointType.parking);
            currentPointIndex = points.points.IndexOf(parkingPt);
            transform.position = parkingPt.transform.position;
        }
    }
}