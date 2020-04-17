using UnityEngine;
using System.Collections;

namespace Maze
{
    public class OpenChest : MonoBehaviour
    {
        public bool isOpened = false;
        public Animation anim;

        void Start()
        {
            anim = this.gameObject.GetComponent<Animation>();
        }
        
        void OnTriggerEnter(Collider col)
        {
            OpenChestNow();
        }

        [ContextMenu("Open")]
        public void OpenChestNow()
        {
            if (!isOpened)
            {
                anim.Play();
                isOpened = true;
            }
        }
    }
}