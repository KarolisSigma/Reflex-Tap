using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCursor : MonoBehaviour
{
    public Sprite cursorTexture;
    public Image cursorObj;
    private Transform cursorTransform;
    private Camera cam;
    public ParticleSystem clickEffect;
    void Awake()
    {
        cursorObj.sprite = cursorTexture;
        cam=Camera.main;
        cursorTransform = transform;
    }

    void Update()
    {
        cursorTransform.position = Input.mousePosition;
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            clickEffect.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
            clickEffect.Play();
        }
    }
}
