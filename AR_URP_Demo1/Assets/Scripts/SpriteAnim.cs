using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnim : MonoBehaviour
{
    public bool loop = false;
    public float speed = 1f;
    public int frameRate = 30;
    private float timePerFrame = 0f;
    private float elapsedTime = 0f;
    private int currentFrame = 0;

    [SerializeField]
    private Image spriteImage;
    [SerializeField]
    private Sprite[] sprites;

    void Start()
    {
        spriteImage = GetComponent<Image>();
        enabled = false;
        LoadSpriteSheet();
    }

    private void LoadSpriteSheet()
    {
        sprites = Resources.LoadAll<Sprite>("qr_spritesheet");
        if (sprites != null && sprites.Length > 0)
        {
            timePerFrame = 1f / frameRate * speed;
            Play();
        }
        else
            Debug.LogError("Failed to load spritesheet");
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timePerFrame)
        {
            elapsedTime = 0;
            currentFrame++;
            SetSprite();
            if (currentFrame > sprites.Length)
            {
                if (loop)
                    currentFrame = 0;
                else
                    enabled = false;
            }
        }
    }

    void SetSprite()
    {
        if (currentFrame >= 0 && currentFrame < sprites.Length)
            spriteImage.sprite = sprites[currentFrame];
    }

    public void Play()
    {
        enabled = true;
    }
}
