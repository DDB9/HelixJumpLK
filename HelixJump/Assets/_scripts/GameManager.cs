using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        // Set the application's target framerate to 60.
        // Since it's a lightweight game, 60fps should be perfectly doable on most modern day devices.
        Application.targetFrameRate = 60;
    }
}
