using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    private void MainToPlay()
    {
        SceneManager.LoadScene("Play");
    }
}
