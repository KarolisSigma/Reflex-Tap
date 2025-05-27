using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dot : MonoBehaviour
{
    private float difficulty=0;
    public float time=3;
    [SerializeField] private Vector2 minMaxSize;
    [SerializeField] private Vector2 minMaxDuration;
    public float size=2;
    public float duration=3;
    private float timer=0;
    public AnimationCurve transition;
    private bool selfDestroyed=false;

    void Start()
    {
        size = Mathf.Lerp(minMaxSize.x, minMaxSize.y, difficulty);
        duration = Mathf.Lerp(minMaxDuration.x, minMaxDuration.y, difficulty);

        transform.localScale = Vector3.one*size;
        Invoke(nameof(SelfDestroy), time);

    }

    public void SetDifficulty(float value){
        difficulty = value;
    }

    void SelfDestroy(){
        selfDestroyed=true;
        Destroy(gameObject);
    }

    void Update()
    {
        transform.localScale = Vector3.one*size*transition.Evaluate(timer/time);
        timer+=Time.deltaTime;
    }

    void OnDestroy()
    {
        if(selfDestroyed) FindObjectOfType<GameManager>().Exploded();
    }
}
