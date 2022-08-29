using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController PlayerController;
    private int horizontal = 0,vertical =0;

    public enum Axis
    {
        Horizontal,
        Vertical
    }
    void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        horizontal = 0;
        vertical = 0;
        GetKeyboardInput();
        SetMovement();
    }

    void GetKeyboardInput()
        {
            // horizontal = (int)Input.GetAxisRaw("Horizontal");
            // vertical = (int)Input.GetAxisRaw("Vertical");

                horizontal = GetAxisRaw(Axis.Horizontal);
                vertical = GetAxisRaw(Axis.Vertical);
            if(horizontal != 0)
            {
                vertical = 0;
            }
        }

    void SetMovement()
    {
        if(vertical !=0)
        {
            PlayerController.SetInputDirection((vertical == 1)?PlayerDirection.UP : PlayerDirection.DOWN);
        }
        else if(horizontal != 0)
        {
            PlayerController.SetInputDirection((horizontal == 1)?PlayerDirection.RIGHT : PlayerDirection.LEFT);
        }
    }

    int GetAxisRaw(Axis axis)
    {
        if(axis == Axis.Horizontal)
        {
            bool left = Input.GetKeyDown(KeyCode.LeftArrow);
            bool Right = Input.GetKeyDown(KeyCode.RightArrow);

            if(left)
            {
                return -1;
            }
            if(Right)
            {
                return 1;
            }
            return 0;

        } else if(axis == Axis.Vertical)
        {
            bool Up = Input.GetKeyDown(KeyCode.UpArrow);
            bool Down = Input.GetKeyDown(KeyCode.DownArrow);

            if(Up)
            {
                return 1;
            }

            if(Down)
            {
                return -1;
            }
            return 0;
        }
        return 0;
    }
}
