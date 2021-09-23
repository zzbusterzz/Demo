using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform player;
    public Transform cameraInitPos;   

    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;

    private float oldPosition;
    private Vector3 previousPlayerPos;

    public void Start()
    {
        oldPosition = transform.position.x;
        previousPlayerPos = player.position;
    }

    void Update()
    {
        GameState gsInstace = GameControl.instance.GetGameState();
        if (gsInstace > GameState.NewGame && gsInstace < GameState.EndRace && gsInstace != GameState.Pause)
        {
            Vector3 tempPos = (player.position) - previousPlayerPos;
            transform.position = new Vector3(transform.position.x + tempPos.x, transform.position.y, transform.position.z);//Adds x difference for camera
            previousPlayerPos = player.position;

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
        previousPlayerPos = player.position;
        oldPosition = transform.position.x;
    }
}