using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] float waypointRadius = 0.3f;
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = GetChild(i);
                Gizmos.DrawSphere(child.position, waypointRadius);
                Gizmos.DrawLine(child.position, GetChild(GetNextIndex(i)).position);

            }
        }

        private int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }

            return i + 1;
        }

        private Transform GetChild(int i)
        {
            return transform.GetChild(i);
        }
    }

}