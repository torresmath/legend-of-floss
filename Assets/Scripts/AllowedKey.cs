using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllowedKey : MonoBehaviour
{
    public KeyCode keyCode;
    public Image sprite;
    public Vector2 defaultPosition;

    private void Awake()
    {
        sprite = GetComponentInChildren<Image>();
        defaultPosition = transform.position;
    }

    public void SetLerp(float lerpDuration)
    {
        this.lerpDuration = lerpDuration;
    }
    public void Move()
    {
        Vector3 target = new Vector3(transform.position.x, valueToLerp, 0f);

        if (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            transform.position = target;
            timeElapsed += Time.deltaTime;
        }
        
    }

    public void DefaultPosition()
    {
        transform.position = defaultPosition;
        lerpDuration = 0f;
    }
    private void FixedUpdate()
    {

    }

    float timeElapsed;

    public float startValue = -0.7f;
    public float endValue = -1;
    public float valueToLerp;
    public float lerpDuration;

    void Update()
    {
        
    }
}
