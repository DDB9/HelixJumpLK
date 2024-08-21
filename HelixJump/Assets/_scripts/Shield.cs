using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Powerup
{
    public override IEnumerator ActivatePowerup()
    {
        if (!GameManager.Instance.ActivePowerups.Contains(this)) GameManager.Instance.ActivePowerups.Add(this);

        transform.GetChild(0).gameObject.SetActive(false);

        BallBehavior[] _balls = FindObjectsByType<BallBehavior>(FindObjectsSortMode.None);
        foreach (BallBehavior ball in _balls)
        {
            if (!ball.ShieldActive) ball.ShieldActive = true;
        }

        yield return new WaitForSeconds(PowerupDuration);

        foreach (BallBehavior ball in _balls)
        {
            if (ball.ShieldActive) ball.ShieldActive = false;
        }

        DeactivatePowerup();
    }
}
