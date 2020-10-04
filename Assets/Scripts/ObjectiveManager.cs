using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private static ObjectiveManager _instance;

    public static ObjectiveManager Instance { get { return _instance; } }
    public TextMeshProUGUI dialogueText;
    public SpriteRenderer dialogueSprite;

    public string[] phrases;

    public int[] objectives;
    private Queue<int> objectiveQueue;

    public bool firstDialogue;

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

        objectiveQueue = new Queue<int>(objectives);
    }

    public void CheckObjective(int streak)
    {
        bool v = objectiveQueue.Count >= 1;
        
            switch (streak)
            {
                case 6:
                {
                    Metronome.Instance.SpawnCharacter(1);
                    StartCoroutine(PopupDialogue("I'm not seeing enough movement guys"));
                }
                    
                break;
                case 12:
                    Metronome.Instance.SpawnCharacter(2);
                    break;
                //case 20: 
                case 18:
                    Metronome.Instance.SpawnCharacter(3);
                    break;
                //case 25:
                case 24:
                    Metronome.Instance.SpawnCharacter(5);
                    break;
                //case 35:
                case 30:
                    Metronome.Instance.SpawnCharacter(8);
                    break;
                //case 38:
                case 40:
                    Metronome.Instance.SpawnCharacter(13);
                    break;
                //case 40:
                case 50:
                    Metronome.Instance.SpawnCharacter(21);
                    break;
                default:
                    break;
            }
            
    }

    public IEnumerator PopupDialogue(string text)
    {
        Debug.Log("Run dialogue");
        if (!firstDialogue && text == "I'm not seeing enough movement guys")
        {
            Debug.Log("Success");
            firstDialogue = true;
            yield return RunDialogue(text);
        }

        if (text != "I'm not seeing enough movement guys")
        {
            yield return RunDialogue(text);
        }
    }

    IEnumerator RunDialogue(string text)
    {
        dialogueText.text = "";
        dialogueSprite.transform.DOScale(new Vector2(1f, 1f), .5f).SetEase(Ease.OutBack);
        dialogueText.text = text;
        dialogueText.transform.DOScale(new Vector2(1f, 1f), .5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(3f);
        dialogueSprite.transform.DOScale(new Vector2(0f, 0f), .5f).SetEase(Ease.OutBack);
        dialogueText.transform.DOScale(new Vector2(0f, 0f), .5f).SetEase(Ease.OutBack);
    }
}
