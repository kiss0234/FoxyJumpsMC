using UnityEngine;
using UnityEngine.Rendering;

public class Collectible : MonoBehaviour
{
    public string nomeAudioClip;
    public int punti;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PointManager.instance.AggiungiPunti(punti);
            SoundSystem.instance.Suona(nomeAudioClip);
            Destroy(gameObject);
        }
    }
}
