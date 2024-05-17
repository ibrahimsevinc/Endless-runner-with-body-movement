using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using System.IO;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject Platform;

    public UnityEngine.UI.Button pauseClickButton;   //Pause butonu için
    public UnityEngine.UI.Button resumeClickButton;  //Resume butonu için
    public UnityEngine.UI.Button restartClickButton;

    private float platformGenisligi;

    public float karakterHizi; // Karakterin hareket hýzý
    public float ziplamaYuksekligi = 10f;
    public float dususHizi = 20f; // Karakterin düþüþ hýzý
    public float seritGenisligi; // Þeritler arasý mesafe

    public float[] hareketKoordinatlari = new float[3];

    private int mevcutSerit = 1; // Baþlangýçta orta þeritte baþlar
    private bool isMoving = false;
    private Rigidbody rb; // Rigidbody deðiþkeni

    private bool isJumping = false; // Zýplama iþlemi kontrolü
    public UDPReceive UdpVeriAlma; //Udp transferi için gerekli nesne

    private string yollananVeri;
    private float yonBilgisi;
    private int bilekBilgisi;

    public bool oyunDurumu = true;
    public bool tuslarlaOyna = true;



    void Start()
    {
        platformGenisligi = Platform.transform.localScale.x;
        seritGenisligi = platformGenisligi / 3;
        hareketKoordinatlari[0] = seritGenisligi * (-1);
        hareketKoordinatlari[1] = 0;
        hareketKoordinatlari[2] = seritGenisligi * (1);

        rb = gameObject.AddComponent<Rigidbody>(); // Rigidbody bileþenini ekleyin  


        //Setting araylarýný yukleme
        string json = File.ReadAllText(Application.dataPath + "/settings.json");
        SettingsData data = JsonUtility.FromJson<SettingsData>(json);

        
        karakterHizi = data.oyunHizi;
        tuslarlaOyna = data.tuslarlaOyna;

        oyunDurumu = true;

        Time.timeScale = 1;


        GameObject buttonObject = GameObject.FindWithTag("PauseButtonTag");
        pauseClickButton = buttonObject.GetComponent<UnityEngine.UI.Button>();
        //pauseClickButton.onClick.Invoke();

        buttonObject = GameObject.FindWithTag("ResumeButtonTag");
        resumeClickButton = buttonObject.GetComponent<UnityEngine.UI.Button>();
        //resumeClickButton.onClick.Invoke();

        buttonObject = GameObject.FindWithTag("RestartButtonTag");
        restartClickButton = buttonObject.GetComponent<UnityEngine.UI.Button>();

    }

    void Update()
    {
        if(oyunDurumu)
        {
            //Karekterin sürekli olarak ileri hareket etmesi

            transform.Translate(Vector3.forward * karakterHizi * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);


            //Yon tuslariyla oynamak istendigi zaman;
            if(tuslarlaOyna)
            {
                TuslaOynamaFonk();
            }

            //Beden hareketleriyle oynanmak istendigi zaman
            else if (!tuslarlaOyna)
            {
                VucutOynamaFonk();
            }
        }
        else
        {
            Durdur();
        }
    }

    void ChangeLaneTus(int direction)
    {
        int targetLane = mevcutSerit + direction;

        // Hedef þerit sýnýrlarýnýn kontrolü
        if (targetLane < 0 || targetLane > 2)
        {
            return; // Hedef þerit sýnýrlarýn dýþýnda ise iþlemi sonlandýr
        }

        // Hedef konumun belirlenmesi
        float targetX = hareketKoordinatlari[targetLane];

        // Karakterin hedef konuma hareket etmesi
        transform.DOMoveX(targetX, 0.2f).SetEase(Ease.OutQuad).OnStart(() =>
        {
            //isMoving = true;
        }).OnComplete(() =>
        {
            //isMoving = false;
            mevcutSerit = targetLane; // Þerit güncellemesi
        });
    }

    void ChangeLaneVucut(int direction)
    {
        int targetLane = direction;

        // Hedef þerit sýnýrlarýnýn kontrolü
        if (targetLane < 0 || targetLane > 2)
        {
            return; // Hedef þerit sýnýrlarýn dýþýnda ise iþlemi sonlandýr
        }

        // Hedef konumun belirlenmesi
        float targetX = hareketKoordinatlari[targetLane];

        // Karakterin hedef konuma hareket etmesi
        transform.DOMoveX(targetX, 0.2f).SetEase(Ease.OutQuad).OnStart(() =>
        {
            //isMoving = true;
        }).OnComplete(() =>
        {
            //isMoving = false;
            mevcutSerit = targetLane; // Þerit güncellemesi
        });
    }

    void Jump()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Yalpalanmayý engelle

        //Zýplama yüksekligi
        rb.AddForce(Vector3.up * ziplamaYuksekligi, ForceMode.Impulse);

        isJumping = true; // Zýplama iþlemi baþladý
    }

    public void Durdur()
    {
        karakterHizi = 0;

        // Karakterin tüm yönlerdeki hýzlarýný sýfýrla
        rb.velocity = Vector3.zero;

        // Karakterin tüm yönlerde hareketini engellemek için RigidbodyConstraints.FreezeAll flag'i aktif edilir.
        rb.constraints = RigidbodyConstraints.FreezeAll;

    }

    public void tusOrVucut(bool deger)
    {
        tuslarlaOyna = deger;
    }

    public void VucutOynamaFonk()
    {
        UdpVeriAlma.startRecieving = true;

        //Udp ile yollanan verileri alma
        try
        {
            yollananVeri = UdpVeriAlma.data;
            string[] splitData = yollananVeri.Split(',');



            yonBilgisi = float.Parse(splitData[0]);
            bilekBilgisi = int.Parse(splitData[1]);

            Debug.Log(yonBilgisi);
            //Debug.Log(bilekBilgisi);
        }
        catch (Exception err)
        {
            Debug.Log("Python verisi bulunamadi");
            Debug.Log(err.ToString());
            yonBilgisi = -1;
        }

        if (yonBilgisi == 3)
        {
            ChangeLaneVucut(2);
        }
        else if (yonBilgisi == 2)
        {
            ChangeLaneVucut(1);
        }
        else if (yonBilgisi == 1)
        {
            ChangeLaneVucut(0);
        }


        if (bilekBilgisi == 0)
        {
            resumeClickButton.onClick.Invoke();
            //resumeClickButton.onClick.AddListener(a);
        }
        else if (bilekBilgisi == 1)
        {
            pauseClickButton.onClick.Invoke();
            //pauseClickButton.onClick.AddListener(a);
        }
        else if (bilekBilgisi == 2)
        {
            Debug.Log("Restart Button");
            restartClickButton.onClick.Invoke();
        }

    }

    public void TuslaOynamaFonk()
    {
        UdpVeriAlma.startRecieving = false;

        // Karakterin hareketi sadece þu an hareket etmiyorsa gerçekleþir
        if (!isMoving)
        {
            //UDP haberleþmesini kapatýyor
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeLaneTus(1); // Saða git
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeLaneTus(-1); // Sola git
            }
            else if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0f) // Yere deðdiðinde ve zýplamaya hazýr olduðunda zýplama iþlemini yap
            {
                Jump();
            }

            // Karakter zýplýyorsa
            if (isJumping)
            {
                // Düþüþ hýzýný kontrol et ve düþüþ hýzýný sabit tut
                if (rb.velocity.y < 0)
                {
                    rb.velocity += Vector3.down * dususHizi * Time.deltaTime;
                    isJumping = false;
                }
            }
        }
    }

    public void a()
    {

    }
}
