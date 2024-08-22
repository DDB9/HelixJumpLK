using System.Collections;
using UnityEngine;

public class Key : Powerup
{
    public override IEnumerator ActivatePowerup()
    {
        if (!GameManager.Instance.ActivePowerups.Contains(this)) GameManager.Instance.ActivePowerups.Add(this);
        transform.GetChild(0).gameObject.SetActive(false);

        BallBehavior[] _balls = FindObjectsByType<BallBehavior>(FindObjectsSortMode.None);
        foreach (BallBehavior ball in _balls)
        {
            if (!ball.HasKey) ball.HasKey = true;
        }

        DeactivatePowerup();
        yield return null;
    }
}
