using UnityEngine;
using System.Collections;

namespace Maze
{
    public class DoneWayPointGizmo : MonoBehaviour
    {
        public Color color;

        void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(this.transform.position, 2f);
        }
    }
}