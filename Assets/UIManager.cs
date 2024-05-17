using UnityEngine;
using UnityEngine.SceneManagement;


//Gameover ekranýnda ve Pause sahneleri degistirmek icin kullandigim .cs dosyasi

public class UIManager : MonoBehaviour
{

    public GameObject PauseManu;

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PauseManu.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
    }

    public void ShowPauseMenu()
    {
        PauseManu.GetComponent<Canvas>().enabled = true;
        Pause();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void returnMenu()
    {
        SceneManager.LoadScene(0);
    }
}
