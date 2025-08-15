using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
