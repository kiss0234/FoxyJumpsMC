using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathAndRespawn : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
