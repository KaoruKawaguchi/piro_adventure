using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static float Horizontal { get; private set; }
    public static bool Left { get; private set; }
    public static bool Right { get; private set; }
    public static bool Jump { get; private set; }
    public static event System.Action onJump;
    public static bool Fire1 { get; private set; }
    public static bool Fire2 { get; private set; }
    public static bool Fire3 { get; private set; }


	// Update is called once per frame
	void Update () {
        Horizontal = Input.GetAxis("Horizontal");
        Left = Input.GetKeyDown("left");
        Right = Input.GetKeyDown("right");
        Jump = 0 < Input.GetAxis("Jump");
        Fire1 = 0 < Input.GetAxis("Fire1");
        Fire2 = 0 < Input.GetAxis("Fire2");
        Fire3 = 0 < Input.GetAxis("Fire3");

        if(Jump) {
            if (onJump != null)
            {
                onJump();
            }
        }
	}
}
