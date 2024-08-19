using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSlice : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void KillPartHit()
    {
        GameManager.Instance.RestartLevel();
    }
}
