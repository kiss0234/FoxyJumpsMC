using UnityEngine;
using UnityEngine.Audio;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem instance;

    //Oggetto che viene creato per riprodurre un suono
    public GameObject soundPlayer;

    //Lista di SFX
    public AudioClip salto;
    public AudioClip prendiCiliegia;
    public AudioClip prendiGemma;
    public AudioClip win;

    private void Start()
    {
        instance = this;
    }

    public void Suona(string tipoSuono)
    {
        if (tipoSuono == "salto")
            CreaESuona(salto);
        else if(tipoSuono == "prendiCiliegia")
            CreaESuona(prendiCiliegia);
        else if (tipoSuono == "prendiGemma")
            CreaESuona(prendiGemma);
        else if (tipoSuono == "win")
            CreaESuona(win);

    }

    void CreaESuona(AudioClip suono)
    {
        //Creo un oggetto che è un prefabbricato con già un audiosource collegata
        GameObject copiaSoundPlayer = Instantiate(soundPlayer);

        AudioSource a = copiaSoundPlayer.GetComponent<AudioSource>();
        a.enabled = true;
        a.clip = suono;
        a.Play();
    }
}
