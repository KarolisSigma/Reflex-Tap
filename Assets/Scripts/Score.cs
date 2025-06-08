using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;
    private int streak;
    private float startTime;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI streakTxt;

    public void Restart(){
        score=0;
        streak=1;
        startTime=Time.time;
        scoreTxt.text = $"{score}";
        streakTxt.text = $"{streak}x";
        streakTxt.color = Color.HSVToRGB(Random.value, 0.8f, 1);
    }

    public void Scored(bool scored){
        if(scored){
            score+=Mathf.RoundToInt(Time.time-startTime)/8+streak;
            streak++;
        }
        else{
            streak=1;
        }
        scoreTxt.text = $"{score}";
        streakTxt.text = $"{streak}x";
        streakTxt.color = Color.HSVToRGB(Random.value, 0.8f, 1);
    }
}
