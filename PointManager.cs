using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PointManager : MonoBehaviour
{
    public static PointManager instance;
    public TextMeshProUGUI score;

    private int contatore = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        score.text = contatore.ToString();
    }

    //Funzione che viene chiamata dai Collectible quando vengono presi dal player
    public void AggiungiPunti(int punti)
    {
        contatore += punti;
        score.text = contatore.ToString();
    }
}