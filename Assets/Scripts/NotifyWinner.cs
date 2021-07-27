﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyWinner : MonoBehaviour
{
    public Transform AIPlayerTransform;

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameControl.instance.UpdateWinner(col.transform);
        Debug.Log("OnCollisionEnter2D" + col.transform);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform == AIPlayerTransform)
        {
            AIPlayerTransform.GetComponent<AIMove>().StopMove();
        }
    }
}