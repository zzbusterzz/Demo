using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameControl control;
    public Transform initPos;

    private float rateofChange = 0;
    private float dampning = 0.1f;

    private float acceleration = 4f;
    private float brakedampen = 1.5f;
    private float dampen = 1.5f;
    private float maxSpeed = 12;
    private float speed = 0;    

    // Update is called once per frame
    void Update()
    {
        if(control.GS == GameState.StartRace || control.GS ==  GameState.CheckWinner)//Move player on start
        {
            Vector3 position = transform.position;

            //0-90 accelrateion
            //90-180 deccelration

            //rateofChange = maxSpeed * Mathf.Sin(2);
            //if (speed > maxSpeed)
            //  speed = maxSpeed;


            if (Input.GetKey(KeyCode.RightArrow))
            {
                speed += acceleration * Time.deltaTime;
            }

            speed -= dampen * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                speed -= dampen * Time.deltaTime;
            }

            speed = Mathf.Clamp(speed, 0, maxSpeed);

            position.x +=  speed * Time.deltaTime;

            transform.position = position;

            //if (Input.GetKey(KeyCode.RightArrow))
            //    rateofChange += 2.5f;
            //else
            //    rateofChange -= dampning;

            //if (Input.GetKey(KeyCode.LeftArrow))
            //    rateofChange -= dampning;

            //rateofChange = Mathf.Clamp(rateofChange, 0, 90);

            //position.x += Time.deltaTime * maxSpeed * Mathf.Sin(rateofChange);

            //Debug.Log(rateofChange);

            //transform.position = position;
        }
    }

    public void ResetPlayer()
    {
        transform.position = initPos.position;
        speed = 0;
    }

}
