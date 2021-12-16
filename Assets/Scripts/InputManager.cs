using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Inputs inputs;

    void Update()
    {
        inputs.movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));
        inputs.camera = new Vector2(Input.GetAxisRaw("CameraHorizontal"), Input.GetAxis("CameraVertical"));
        inputs.jump = Input.GetButton("Jump");
        inputs.sprint = Input.GetButton("Sprint");
        inputs.sprintUp = Input.GetButtonUp("Sprint");
        inputs.sneak = Input.GetButton("Sneak");
        inputs.sneakUp = Input.GetButtonUp("Sneak");

        //Debug.Log(inputs.ToString());
    }
}
