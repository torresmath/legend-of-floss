using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI peopleFlossingCounter;
    public RectTransform peopleFlossingRect;
    public TextMeshProUGUI streakCounter;
    public RectTransform streakCounterRect;
    public ParticleSystem hitParticle;
    public ParticleSystem missParticle;

    public TextMeshProUGUI countdownText;
    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        streakCounterRect.DOAnchorPosY(-120f, .5f)
            .SetLoops(-1, LoopType.Yoyo);
        streakCounterRect.DORotate(new Vector3(0f, 0f, 2f), .5f)
            .OnComplete(() => streakCounterRect.DORotate(new Vector3(0f, 0f, -1f), .2f))
            .SetLoops(-1, LoopType.Yoyo);
        peopleFlossingRect.DOAnchorPosY(-130, .5f)
            .SetLoops(-1, LoopType.Yoyo);
        Refresh();

    }

    private void FixedUpdate()
    {
        
    }

    public void Refresh()
    {
        peopleFlossingCounter.text = "1";
        DisableStreakCounter();
    }

    public void IncreaseFlossingCounter(int count)
    {
        peopleFlossingCounter.text = count.ToString();
        peopleFlossingCounter.rectTransform.DOScale(1.25f, 1f).SetEase(Ease.InBack)
            .OnComplete(() => peopleFlossingCounter.rectTransform.DOScale(1f, .5f));
    }

    public void IncreaseStreakCounter(int count)
    {
        streakCounterRect.gameObject.SetActive(true);
        streakCounter.text = $"{count.ToString()} Moves!";    
    }

    public void DisableStreakCounter()
    {
        streakCounterRect.gameObject.SetActive(false);
    }

    public void PlayParticle(bool hitted)
    {
        if (hitted)
        {
            hitParticle.Play();
        }
            
        else
        {
            missParticle.Play();
        }
            
    }

    public IEnumerator RunCountdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "GO";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";
    }
}
