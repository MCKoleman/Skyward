using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMobileInput : MonoBehaviour
{
    [SerializeField]
    private Joystick moveJoystick;
    [SerializeField]
    private Joystick aimJoystick;
    private PlayerController player;

    private void OnEnable()
    {
        moveJoystick.OnInputChange += HandleMoveInput;
        aimJoystick.OnInputChange += HandleAimInput;
    }

    private void OnDisable()
    {
        moveJoystick.OnInputChange -= HandleMoveInput;
        aimJoystick.OnInputChange -= HandleAimInput;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void HandleMoveInput(Vector2 delta)
    {
        if (player == null)
            return;
        player.HandleMove(delta);
    }

    public void HandleAimInput(Vector2 delta)
    {
        if (player == null)
            return;
        player.HandleLookDelta(delta);
    }
}
