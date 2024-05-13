using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneManagerSettings : MonoBehaviour
{
    public Toggle toogleSecimi;
    bool toogleDegeri;


    void Start()
    {
        toogleSecimi.GetComponent<Toggle>();

    }

    public void geriDonmeFonksiyonu()
    {
        SceneManager.LoadScene(0);
    }
    
    //public void kaydetFonksiyonu()
    //{

    //}

    public void tuslarlaOyna(bool deger)
        {
            toogleDegeri = toogleSecimi.isOn;

            if (deger)
            {
                Debug.Log("Tuþlarla oynanacak");
            }
            else
            {
                Debug.Log("Tuþlarla oynanmayacak");
            }
        }
    
}
