using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryTimer : MonoBehaviour
{
    public float distance;
    private void Update()
    {
        var checkFront = Player.Instance.transform.position.z - transform.position.z;

        if (checkFront > distance)
        {
            gameObject.SetActive(false);
        }
    }
}
