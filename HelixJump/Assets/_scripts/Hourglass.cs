using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : Powerup
{
    public override IEnumerator ActivatePowerup()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(PowerupDutaion);
        Time.timeScale = 1.0f;
    }
}
