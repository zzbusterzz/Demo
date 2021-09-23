using UnityEngine;

public class NotifyWinner : MonoBehaviour
{
    public Transform aiPlayerTransform;
    public Transform userPlayerTransform;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform == userPlayerTransform)
        {
            userPlayerTransform.GetComponent<PlayerMove>().BeginEndMove();
        }

        GameControl.instance.UpdateWinner(col.transform);
        Debug.Log("OnCollisionEnter2D" + col.transform);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform == aiPlayerTransform)
        {
            aiPlayerTransform.GetComponent<AIMove>().StopMove();
        }
    }
}
