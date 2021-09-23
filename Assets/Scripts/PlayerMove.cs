using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameControl control;
    public Transform initPos;

    private float acceleration = 5f;
    private float dampen = 1.5f;
    private float maxSpeed = 5;
    private float speed = 0;

    public bool endMove = false;
    // Update is called once per frame
    void Update()
    {
        if(control.currentGS == GameState.StartRace || control.currentGS ==  GameState.CheckWinner || endMove)//Move player on start
        {
            Vector3 position = transform.position;


            if (Input.GetKey(KeyCode.RightArrow) && !endMove)
            {
                speed += acceleration * Time.deltaTime;
            }

            speed -= dampen * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftArrow) && !endMove)
            {
                speed -= dampen * Time.deltaTime;
            }

            speed = Mathf.Clamp(speed, 0, maxSpeed);

            position.x +=  speed * Time.deltaTime;

            transform.position = position;
        }
    }

    public void BeginEndMove()
    {
        endMove = true;
        dampen = 6.5f;
    }

    public void ResetPlayer()
    {
        endMove = false;
        dampen = 1.5f;
        transform.position = initPos.position;
        speed = 0;
    }

}
