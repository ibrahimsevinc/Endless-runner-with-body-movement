using UnityEngine;
using UnityEngine.SceneManagement;


//Gameover ekran�nda sahneleri degistirmek icin kullandigim .cs dosyasi


public class UIManager : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void returnMenu()
    {
        SceneManager.LoadScene(0);
    }
}
