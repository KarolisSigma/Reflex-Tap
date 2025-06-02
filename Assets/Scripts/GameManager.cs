using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timeUntilDifficult;
    [SerializeField] private Vector2 minMaxSpawnInterval;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Vector2 spawnBounds;
    private Camera cam;
    [SerializeField] private AudioClip tapped;
    [SerializeField] private AudioClip uiClick;
    [SerializeField] private AudioClip explode;
    private AudioSource audioSource;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float maxHealth=100;
    private float health;
    private bool playing=false;
    [SerializeField] private Image healthbar;
    private float difficulty=0;
    private float lastTimeUpdated=0;
    [SerializeField] private Score scoreManager;
    [SerializeField] private GameObject gameoverPanel;
    
    void Awake()
    {
        scoreManager.Restart();
        health=maxHealth;
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible=false;
    }

    void UpdateDifficulty(float value){
        difficulty = Mathf.Clamp01(Mathf.Clamp01(difficulty+(Time.time-lastTimeUpdated)*(1f/timeUntilDifficult))+value);
        lastTimeUpdated=Time.time;
    }
    void Update()
    {
        if(!playing) return;
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            Vector2 clickPos = cam.ScreenToWorldPoint(Input.mousePosition);
            foreach(GameObject dot in GameObject.FindGameObjectsWithTag("Dot")){
                if(Vector2.Distance(clickPos, dot.transform.position)<dot.transform.localScale.x/2f){
                    Destroy(dot);
                    scoreManager.Scored(true);
                    audioSource.PlayOneShot(tapped);
                    Instantiate(hitEffect, clickPos, Quaternion.identity);
                }
            }
        }
    }

    IEnumerator Healing(){
        while(true){
            if(playing){
                health=Mathf.Clamp(health+1,0,100);
                UpdateHealthbar();
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void Exploded(){
        audioSource.PlayOneShot(explode);
        UpdateDifficulty(-0.2f);
        health-=25;
        scoreManager.Scored(false);
        UpdateHealthbar();
        if(health<=0){
            health=0;
            Death();
        }
    }

    void UpdateHealthbar(){
        healthbar.fillAmount = health/maxHealth;
    }
    void Death(){
        playing=false;
        StopAllCoroutines();
        foreach(GameObject dot in GameObject.FindGameObjectsWithTag("Dot")) Destroy(dot); 
        gameoverPanel.SetActive(true);
        gameUI.SetActive(false);
    }

    public void Ready(){
        scoreManager.Restart();
        health=maxHealth;
        audioSource.PlayOneShot(uiClick);
        menuUI.SetActive(false);
        gameUI.SetActive(true);
        gameoverPanel.SetActive(false);
        gameUI.SetActive(true);

        scoreManager.Restart();
        health=maxHealth;
        UpdateHealthbar();
        StartCoroutine(Countdown());
    }
    IEnumerator Countdown(){
        countdownText.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = $"{i}";
            countdownText.color = Color.Lerp(Color.green, Color.red, 1-i/3f);
            yield return new WaitForSeconds(1);

        }
        countdownText.gameObject.SetActive(false);
        playing=true;
        lastTimeUpdated=Time.time;
        StartCoroutine(Healing());
        StartCoroutine(SpawnDots());
    }

    IEnumerator SpawnDots(){
        while(true){
            UpdateDifficulty(0);
            Vector2 position = new Vector2(Random.value*spawnBounds.x*2-spawnBounds.x, Random.value*spawnBounds.y*2-spawnBounds.y);
            Instantiate(dotPrefab, position, Quaternion.identity).GetComponent<Dot>().SetDifficulty(difficulty);
            float time = Mathf.Lerp(minMaxSpawnInterval.x, minMaxSpawnInterval.y, difficulty);
            yield return new WaitForSeconds(time);
        }
    }
}
