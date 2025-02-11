using UnityEngine;

public class TrapDamageSystem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            FindFirstObjectByType<PlayerController>().Morte();
        }
    }
}
