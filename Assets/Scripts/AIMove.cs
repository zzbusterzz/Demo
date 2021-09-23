using UnityEngine;

public class AIMove : MonoBehaviour
{
    public Transform initPos;
    private bool movePlayer = false;
    private float speed = 4.5f;

    // Update is called once per frame
    void Update()
    {
        if (movePlayer)
        {
            Vector3 position = transform.position;
            position.x += Time.deltaTime * speed;
            transform.position = position;
        }   
    }

    public void StartMove()
    {
        movePlayer = true;
    }

    public void StopMove()
    {
        movePlayer = false;
    }

    public void ResetAI()
    {
        transform.position = initPos.position;
        movePlayer = false;
    }
}
