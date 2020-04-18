using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {
    public static bool IsPressed(string action) {

        switch(action.ToLower()) {
            case "left":
                return Input.GetKey("a") || Input.GetKey("left");
            case "right":
                return Input.GetKey("d") || Input.GetKey("right");
            case "up":
                return Input.GetKey("w") || Input.GetKey("up");
            case "down":
                return Input.GetKey("s") || Input.GetKey("down");
            case "jump":
                return Input.GetKey("space");
        }
        return false;
    }
}
