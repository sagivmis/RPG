using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointRadius = 0.3f;
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetChildPosition(i), waypointRadius);
                Gizmos.DrawLine(GetChildPosition(i), GetChildPosition(GetNextIndex(i)));

            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }

            return i + 1;
        }

        public Vector3 GetChildPosition(int i)
        {
            return transform.GetChild(i).position;
        }
    }

}