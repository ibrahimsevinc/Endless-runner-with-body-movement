using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;

//Settings ekranýnda ayarlarý ve sahneleri yonetmek icin kullandigim .cs dosyasi

public class SceneManagerSettings : MonoBehaviour
{
    public Toggle toogleSecimi;
    public TMP_Dropdown dropdownSecimi;
    public TextMeshProUGUI karkaterHiziText;
    public TextMeshProUGUI bekleyinText;
    bool toogleDegeri;

    string zorlukSecimi;
    int zorlukSecimiSayisalDegeri;
    int karakterHizi;



    void Start()
    {
        yuklemeFonksiyonu();
        //toogleSecimi.GetComponent<Toggle>();
        //dropdownSecimi.GetComponent<Dropdown>();
        //karkaterHiziText = GetComponent<TextMeshProUGUI>();
    }

    public void geriDonmeFonksiyonu()
    {
        SceneManager.LoadScene(0);
    }

    public void kaydetFonksiyonu()
    {

        toogleDegeri = toogleSecimi.isOn;

        SettingsData data = new SettingsData();
        data.oyunHizi = karakterHizi;
        data.tuslarlaOyna = toogleDegeri;
        data.zorlukSecimi = zorlukSecimi;
        data.zorlukSecimiSayisalDegeri = zorlukSecimiSayisalDegeri;



        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/settings.json", json);
    }

    public void yuklemeFonksiyonu()
    {
        string json = File.ReadAllText(Application.dataPath + "/settings.json");
        SettingsData data = JsonUtility.FromJson<SettingsData>(json);


        toogleSecimi.isOn = data.tuslarlaOyna;
        dropdownSecimi.value = data.zorlukSecimiSayisalDegeri;
        karkaterHiziText.text = data.oyunHizi.ToString();
        karakterHizi = data.oyunHizi;
    }

    public void comboBoxSecimi(int val)
    {
        if(val == 0)
        {
            zorlukSecimi = "Basit";
            zorlukSecimiSayisalDegeri = 0;
            karakterHizi = 30;
            karkaterHiziText.text = karakterHizi.ToString();
        }
        if(val == 1)
        {
            zorlukSecimi = "Orta";
            zorlukSecimiSayisalDegeri = 1;
            karakterHizi = 50;
            karkaterHiziText.text = karakterHizi.ToString();
        }
        if(val == 2)
        {
            zorlukSecimi = "Zor";
            zorlukSecimiSayisalDegeri = 2;
            karakterHizi = 70;
            karkaterHiziText.text = karakterHizi.ToString();
        }
    }


    public void toogleDegeriDegisirse()
    {
        toogleDegeri = toogleSecimi.isOn;

        Process process = new Process();


        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = "/C python \"" + Application.dataPath + "/vucutTakip.py\"";
        //process.StartInfo.RedirectStandardOutput = true; // Çýktýyý yeniden yönlendirmek için
        process.StartInfo.UseShellExecute = false; // Komutun çýktýsýný almak için bu false olmalý
        process.StartInfo.CreateNoWindow = true; // Konsol penceresi göstermemek için

        if (toogleDegeri)
        {
            // Tikli ise (Tuþla oyna)

        }
        if (!toogleDegeri)
        {
            // Tiksiz ise (Hareket ile oyna)

            bekleyinText.text = "Python kodu çalýþana kadar lütfen bekleyiniz...";
            process.Start();

            //string output = process.StandardOutput.ReadLine();
        }
    }



}
