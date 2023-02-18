using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEffect : MonoBehaviour
{
    Animator anim;
    Vector2 moveInput;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        walk();
    }
    public void derictionInput(InputAction.CallbackContext context) //¤è¦V¿é¤J
    {
        moveInput = context.ReadValue<Vector2>();
    }
    void walk()
    {
        if (moveInput.x != 0)
            anim.SetBool("Walk", true);
        else
            anim.SetBool("Walk", false);
    }
}
