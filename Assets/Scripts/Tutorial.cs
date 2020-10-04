using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    AllowedKey[] allowedKeys;
    public GameObject allowedKeysObj;
    public Character character;

    private void Awake()
    {
        allowedKeys = allowedKeysObj.GetComponentsInChildren<AllowedKey>();
    }
    void TryInput()
    {
        foreach (AllowedKey key in allowedKeys)
        {
            key.sprite.color = new Color32(255, 255, 255, 255);
            if (Input.GetKey(key.keyCode))
            {
                character.UpdateSprite(key.keyCode);

                key.sprite.color = new Color32(255, 255, 255, 200);
            }
        }
    }

    private void Update()
    {
        TryInput();
    }
}
