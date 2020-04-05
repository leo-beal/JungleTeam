using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class playerController : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public float speed = 1;

    private CharacterController character;

    private void Start()
    {
        character = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (input.axis.magnitude > 0.1f)
        {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));

            var motion = speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up);
            motion = motion - (new Vector3(0, 9.81f, 0) * Time.deltaTime);

            character.Move(motion);
        }
    }
}
