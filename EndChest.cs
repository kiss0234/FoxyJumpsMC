using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndChest : MonoBehaviour
{
    Animator animator;
    
    public string nomeAudioClip;
    public GameObject panelEnd;

    private void Start()
    {
        panelEnd.SetActive(false);
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //La cassa finale quando viene aperta fa l'animazione e mostra l'end level UI
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            panelEnd.SetActive(true);
            animator.SetBool("isOpen", true);
            SoundSystem.instance.Suona(nomeAudioClip);

        }
    }
}
