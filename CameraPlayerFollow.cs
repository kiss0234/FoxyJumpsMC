using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    public Transform player; 
    public float offset = 5f;
    public float Ystop = 3f;

    void FixedUpdate()
    {
        if (player.position.y > Ystop) { 
        //Camera che segue la y del player ma si ferma se il player è caduto troppo in basso
        transform.position = new Vector3(transform.position.x, player.position.y + offset, transform.position.z);
        }
    }
}
