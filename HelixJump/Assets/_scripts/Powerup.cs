using System.Collections;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    public float PowerupDuration;
    public abstract IEnumerator ActivatePowerup();

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }

    public void ResetPowerup()
    {
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DeactivatePowerup()
    {
        gameObject.SetActive(false);
    }
}
