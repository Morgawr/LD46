﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {

    public static bool IsPressed(string action) {

        var horizAxis = Input.GetAxis("Horizontal");
        var vertAxis = Input.GetAxis("Vertical");
        switch(action.ToLower()) {
            case "left":
                return Input.GetKey("a") || Input.GetKey("left") || horizAxis < -0.2;
            case "right":
                return Input.GetKey("d") || Input.GetKey("right") || horizAxis > 0.2;
            case "up":
                return Input.GetKey("w") || Input.GetKey("up") || vertAxis < -0.2;
            case "down":
                return Input.GetKey("s") || Input.GetKey("down") || vertAxis > 0.2;
            case "jump":
                return Input.GetKey("space") || Input.GetButton("Jump");
            case "attack":
                return Input.GetKey("z") || Input.GetButton("Attack");
            case "pause":
                return Input.GetKey("escape") || Input.GetButton("Pause");
            case "quit":
                return Input.GetKey("q") || Input.GetButton("Quit");
        }
        return false;
    }
}
