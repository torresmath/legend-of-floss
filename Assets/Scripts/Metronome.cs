using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    public double bpm = 120.0F;

    double nextTick = 0.0F;
    double sampleRate = 0.0F;
    bool ticked = false;
    bool canClick = false;
    bool ticktried = false;
    bool gameStarted = false;
    bool gameEnded = false;
    public AudioSource policeSiren;
    [SerializeField] public KeyCode[] inputs;
    AllowedKey[] allowedKeys;

    Queue<KeyCode> keyCodeQueue;

    KeyCode currentKey;

    public GameObject charactersHolder;
    public GameObject allowedKeysObj;

    public Image leftSprite;
    public Image rightSprite;
    public Image xSprite;
    public Image cSprite;
    public Image dSprite;
    public Image sSprite;

    public Image keyPanelImage;
    public Image currentKeySprite;

    public TextMeshProUGUI counter;
    public TextMeshProUGUI missedCounter;
    int count;
    int missedCount;
    int totalCount;
    public int streakCount;
    bool firstTick;

    public int flossCount = 1;

    public List<Character> characters;

    public GameObject endGamePanel;
    public TextMeshProUGUI endGameText;
    public Button endGameButton;
    private static Metronome _instance;

    public static Metronome Instance { get { return _instance; } }

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
    }

    void Start()
    {
        allowedKeys = allowedKeysObj.GetComponentsInChildren<AllowedKey>();
        characters = charactersHolder.GetComponentsInChildren<Character>(true).ToList();
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        nextTick = startTick + (60.0 / bpm);
        CreateQueue();
        StartCoroutine(DelayBeforeStart());
    }

    IEnumerator DelayBeforeStart()
    {
        yield return new WaitForSeconds(1f);
        yield return ObjectiveManager.Instance.PopupDialogue("C'mon boys lets make history!");
        yield return UIManager.Instance.RunCountdown();
        StartGame();
    }

    void StartGame()
    {
        GetComponent<AudioSource>().Play();
        firstTick = true;
        bpm = 40;
        gameStarted = true;
    }

    Queue<KeyCode> CreateQueue()
    {
        keyCodeQueue = new Queue<KeyCode>(inputs);
        return keyCodeQueue;
    }
    private void Update()
    {
        if (!gameStarted || gameEnded)
            return;
        TryInput();
    }

    void FixedUpdate()
    {
        if (!gameStarted || gameEnded)
            return;

        double timePerTick = 60.0f / bpm;
        double dspTime = AudioSettings.dspTime;

        while (dspTime >= nextTick)
        {
            if (ticktried == false)
            {
                missedCount++;
                missedCounter.text = missedCount.ToString();
                totalCount++;
                streakCount = 0;
                ResetTempo();
                UIManager.Instance.DisableStreakCounter();
                UIManager.Instance.PlayParticle(false);
            }

            canClick = false;
            ticked = false;
            ticktried = false;
            nextTick += timePerTick;
            HideCurrentKey();
        }
    }

    void LateUpdate()
    {
        if (!gameStarted || gameEnded)
            return;
        if (!ticked && nextTick >= AudioSettings.dspTime)
        {
            ticked = true;
            BroadcastMessage("OnTick");
        }

        if (flossCount >= 100 && !gameEnded)
        {
            gameEnded = true;
            StartCoroutine(Endgame());
        }
    }

    IEnumerator Endgame()
    {
        endGamePanel.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        GetComponent<AudioSource>().volume -= .2f;
        yield return new WaitForSeconds(1.0f);
        GetComponent<AudioSource>().volume -= .2f;
        yield return new WaitForSeconds(1.0f);
        GetComponent<AudioSource>().volume -= .2f;
        yield return new WaitForSeconds(1.0f);
        GetComponent<AudioSource>().volume -= .2f;
        yield return new WaitForSeconds(1.0f);
        GetComponent<AudioSource>().volume -= .1f;
        yield return new WaitForSeconds(1.0f);
        GetComponent<AudioSource>().volume -= .05f;

        policeSiren.Play();
        endGameText.gameObject.SetActive(true);
        endGameButton.gameObject.SetActive(true);
    }

    void OnTick()
    {
        if (firstTick)
        {
            firstTick = false;
            bpm = 60;
        }
        canClick = true;
        if (keyCodeQueue.Count >= 1)
        {
            SetCurrentKey();
        } else
        {
            CreateQueue();
            SetCurrentKey();
        }

        ShowCurrentKey();
    }

    KeyCode SetCurrentKey()
    {
        currentKey = keyCodeQueue.Dequeue();
        return currentKey;
    }

    void ShowCurrentKey()
    {
        switch (currentKey)
        {
            case KeyCode.S:
                currentKeySprite = sSprite;
                break;
            case KeyCode.D:
                currentKeySprite = dSprite;
                break;
            case KeyCode.LeftArrow:
                currentKeySprite = leftSprite;
                break;
            case KeyCode.RightArrow:
                currentKeySprite = rightSprite;
                break;
            case KeyCode.X:
                currentKeySprite = xSprite;
                break;
            case KeyCode.C:
                currentKeySprite = cSprite;
                break;
            default:
                break;
        }

        currentKeySprite.GetComponent<AllowedKey>().SetLerp(float.Parse(nextTick.ToString()) / 10000f);
        allowedKeys.Where(key => key.sprite != currentKeySprite)
            .ToList()
            .ForEach(key => key.SetLerp(0f));
        currentKeySprite.enabled = true;
    }

    void HideCurrentKey()
    {
        if (currentKeySprite != null)
            currentKeySprite.enabled = false;
    }

    void TryInput()
    {
        keyPanelImage.color = new Color32(255, 255, 255, 255);
        foreach (AllowedKey key in allowedKeys)
        {
            key.sprite.color = new Color32(255, 255, 255, 255);
            
            if (Input.GetKey(key.keyCode))
            {
                characters.ForEach(ch => ch.UpdateSprite(key.keyCode));
                keyPanelImage.color = new Color32(255, 255, 255, 200);

                if (canClick && !ticktried)
                    TryClick(key.keyCode);
            }
        }

    }

    void TryClick(KeyCode keyCode)
    {
        if (keyCode == currentKey)
        {
            IncreasePoints();
            UIManager.Instance.PlayParticle(true);
        } else
        {
            missedCount++;
            missedCounter.text = missedCount.ToString();
            streakCount = 0;
            ResetTempo();
            UIManager.Instance.DisableStreakCounter();
            UIManager.Instance.PlayParticle(false);
        }
        ticktried = true;

    }

    public void IncreasePoints()
    {
        
        count++;
        counter.text = count.ToString();
        totalCount++;
        streakCount++;
        IncreaseTempo(streakCount);

        if (streakCount >= 6)
            UIManager.Instance.IncreaseStreakCounter(streakCount);

        ObjectiveManager.Instance.CheckObjective(streakCount);
    }

    public void SpawnCharacter(int count = 1)
    {   
        for (int i = 0; i < count; i++)
        {
            Character character = characters.FirstOrDefault(ch => !ch.gameObject.activeSelf);
            if (character != null)
                character.Spawn();
        }

        IncreaseFloss(count);
    }

    void IncreaseFloss(int increase)
    {
        flossCount += increase;
        UIManager.Instance.IncreaseFlossingCounter(flossCount);
    }

    void IncreaseTempo(int streak)
    {
        switch (streak)
        {
            case 10:
                bpm = 80;
                break;
            case 25:
                bpm = 100;
                break;
            case 60:
                bpm = 120;
                break;
            case 100:
                bpm = 140;
                break;
            case 150:
                bpm = 160;
                break;
            case 250:
                bpm = 180;
                break;
        }
    }

    void ResetTempo()
    {
        bpm = 60;
    }
}
