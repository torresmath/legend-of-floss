using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Sprite leftSprite;
    public Sprite xSprite;
    public Sprite cSprite;
    public Sprite rightSprite;
    public Sprite dSprite;
    public Sprite sSprite;

    public SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>(true);

        if (tag != "Player")
            gameObject.SetActive(false);
    }
    public void UpdateSprite(KeyCode currentKey)
    {
        if (!gameObject.activeSelf)
            return;

        switch (currentKey)
        {
            case KeyCode.LeftArrow:
                sprite.sprite = leftSprite;
                break;
            case KeyCode.X:
                sprite.sprite = xSprite;
                break;
            case KeyCode.C:
                sprite.sprite = cSprite;
                break;
            case KeyCode.RightArrow:
                sprite.sprite = rightSprite;
                break;
            case KeyCode.D:
                sprite.sprite = dSprite;
                break;
            case KeyCode.S:
                sprite.sprite = sSprite;
                break;
            default:
                break;
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        transform.localScale = new Vector2(0f, 0f);
        transform.DOScale(1.25f, .5f).SetEase(Ease.InBounce)
            .OnComplete(() =>
            {
                transform.DOScale(1f, .25f);
            });
    }
}
