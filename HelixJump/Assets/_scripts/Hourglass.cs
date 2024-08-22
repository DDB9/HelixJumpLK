using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : Powerup
{
    public float TimeScale;

    public override IEnumerator ActivatePowerup()
    {
        if (!GameManager.Instance.ActivePowerups.Contains(this)) GameManager.Instance.ActivePowerups.Add(this);

        transform.GetChild(0).gameObject.SetActive(false);

        Time.timeScale = TimeScale;
        yield return new WaitForSeconds(TimeScale / PowerupDuration + 1);
        Time.timeScale = 1.0f;

        DeactivatePowerup();
    }
}
