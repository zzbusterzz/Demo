using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform Player;
    public Transform cameraInitPos;
    private Vector3 previousPlayerPos;
    private Vector3 playerXOffset = new Vector3(9, 0, 0);

    public void Start()
    {
        previousPlayerPos = Player.position;
    }

    void Update()
    {
        Vector3 tempPos = (Player.position) - previousPlayerPos;
        //Vector3.Distance(Player.position, previousPlayerPos);
        transform.position = new Vector3(transform.position.x + tempPos.x, transform.position.y, transform.position.z) ;//Adds x difference for camera
        previousPlayerPos = Player.position;
    }

    public void ResetCamera()
    {
        transform.position = cameraInitPos.position;
        previousPlayerPos = Player.position;
    }
}