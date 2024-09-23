using UnityEngine;

public class QuitManager : MonoBehaviour
{
    void Start () {
        Time.timeScale = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
