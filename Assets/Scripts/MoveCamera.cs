using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform Player;
    public Transform cameraInitPos;
   

    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;

    private float oldPosition;
    private Vector3 previousPlayerPos;

    public void Start()
    {
        oldPosition = transform.position.x;
        previousPlayerPos = Player.position;
    }

    void Update()
    {
        GameState gsInstace = GameControl.instance.GetGameState();
        if (gsInstace > GameState.NewGame && gsInstace < GameState.EndRace && gsInstace != GameState.Pause)
        {
            Vector3 tempPos = (Player.position) - previousPlayerPos;
            transform.position = new Vector3(transform.position.x + tempPos.x, transform.position.y, transform.position.z);//Adds x difference for camera
            previousPlayerPos = Player.position;

            if (transform.position.x != oldPosition)
            {
                if (onCameraTranslate != null)
                {
                    float delta = oldPosition - transform.position.x;
                    onCameraTranslate(delta);
                }
                oldPosition = transform.position.x;
            }
        }
    }

    public void ResetCamera()
    {
        transform.position = cameraInitPos.position;
        previousPlayerPos = Player.position;
        oldPosition = transform.position.x;
    }
}