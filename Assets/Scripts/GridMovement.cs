using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 0.5f;

    [SerializeField]
    private float maxMoveDistance = 5.0f;

    [SerializeField]
    private float lerpSpeed = 5.0f;

    private Vector2 initialPosition;

    [SerializeField] private AnimationCurve zooming;
    [SerializeField] private float zoomDuration=5;
    [SerializeField] private float colorCycleDuration =10;
    [SerializeField] private float  gridSaturation = 0.75f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 offset = (mousePos - initialPosition) * moveSpeed;
        Vector2 clamped = Vector2.ClampMagnitude(offset, maxMoveDistance);
        transform.position = Vector2.Lerp(transform.position, initialPosition + clamped, lerpSpeed * Time.deltaTime);

        transform.localScale = Vector2.one*zooming.Evaluate((Time.time%zoomDuration)/zoomDuration);
        spriteRenderer.color = Color.HSVToRGB((Time.time%colorCycleDuration)/colorCycleDuration, gridSaturation, 1f);
    }
}
