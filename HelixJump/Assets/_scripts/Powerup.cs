using System.Collections;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    public float PowerupDutaion;
    public abstract IEnumerator ActivatePowerup();
}
