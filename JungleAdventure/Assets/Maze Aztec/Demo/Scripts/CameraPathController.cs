using UnityEngine;
using System.Collections;

namespace Maze
{
    public class CameraPathController : MonoBehaviour
    {
        public Transform[] controlPath;
        public Transform character;
        public enum Direction { Forward, Reverse };

        private float pathPosition = 0;
        public float speed = .2f;
        private Direction characterDirection;
        private float lookAheadAmount = .01f;

        public Transform lookAt;
        public float damping = 6.0f;

        public bool drawGismos = true;

        void OnDrawGizmos()
        {
            if(drawGismos)
                iTween.DrawPath(controlPath, Color.blue);
        }

        void Start()
        {
            characterDirection = Direction.Forward;

            FindFloorAndRotation();
        }

        void Update()
        {
            if (characterDirection == Direction.Forward)
            {
                pathPosition += Time.deltaTime * speed;
            }
            else
            {
                pathPosition -= Time.deltaTime * speed;
            }

            FindFloorAndRotation();

            if (characterDirection == Direction.Forward && pathPosition >= 0.99f)
            {
                characterDirection = Direction.Reverse;
            }
            if (characterDirection == Direction.Reverse && pathPosition <= 0.01f)
            {
                characterDirection = Direction.Forward;
            }
        }

        void FindFloorAndRotation()
        {
            float pathPercent = pathPosition % 1;
            Vector3 coordinateOnPath = iTween.PointOnPath(controlPath, pathPercent);
            Vector3 lookTarget;

            if (pathPercent - lookAheadAmount >= 0 && pathPercent + lookAheadAmount <= 1)
            {
                if (characterDirection == Direction.Forward)
                {
                    lookTarget = iTween.PointOnPath(controlPath, pathPercent + lookAheadAmount);
                }
                else
                {
                    lookTarget = iTween.PointOnPath(controlPath, pathPercent - lookAheadAmount);
                }

                if (lookAt)
                {
                    Quaternion rotation = Quaternion.LookRotation(lookAt.position - character.position);
                    character.rotation = Quaternion.Slerp(character.rotation, rotation, Time.deltaTime * damping);
                }
                else
                {
                    Quaternion rotation = Quaternion.LookRotation(lookTarget - character.position);
                    character.rotation = Quaternion.Slerp(character.rotation, rotation, Time.deltaTime * damping);
                }

                character.position = coordinateOnPath;
            }
        }
    }
}