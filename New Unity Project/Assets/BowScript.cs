using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{

    [RequireComponent(typeof(Interactable))]
    public class BowScript : MonoBehaviour
    {
        private bool noched;
        private Hand hand;

        private Collider nochCollider;

        public GameObject nochFollow;

        public GameObject nochStart;

        public GameObject nochEnd;

        private GameObject currentArrow;
        // Start is called before the first frame update
        void Start()
        {
            nochCollider = GetComponent<Collider>();
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider noch)
        {
            if (noch.name == "Tail" && hand.otherHand.currentAttachedObject.name == "Arrow")
            {
                Debug.Log("Bro, that is an arrow right there");
                currentArrow = hand.otherHand.currentAttachedObject;
                noched = true;
                nochCollider.isTrigger = false;
            }


        }
        
        private void OnAttachedToHand( Hand attachedHand )
        {
            hand = attachedHand;
        }
    }
}
