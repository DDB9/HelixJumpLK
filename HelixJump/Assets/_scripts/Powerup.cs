using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    public float PowerupDutaion;
    public abstract IEnumerator ActivatePowerup();
}
