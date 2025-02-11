using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChangerSystem : MonoBehaviour
{ 
   public void SceneSwitch(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
