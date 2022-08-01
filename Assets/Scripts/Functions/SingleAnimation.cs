using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleAnimation : MonoBehaviour
{
    public enum AnimationType
    {
        SPRITE,
        UI
    }

    public AnimationType type;
    public float frameSpeed = 0.05f;
    public Sprite[] animationsFrame;

    float duration;
    int idx;
    SpriteRenderer SpriteRenderer;
    Image Image;

    private void Start()
    {
        switch (type)
        {
            case AnimationType.SPRITE:
                SpriteRenderer = GetComponent<SpriteRenderer>();
                break;
            case AnimationType.UI:
                Image = GetComponent<Image>();
                break;
        }
    }

    private void Update()
    {
        duration += Time.deltaTime;
        if (duration >= frameSpeed)
        {
            switch (type)
            {
                case AnimationType.SPRITE:
                    SpriteRenderer.sprite = animationsFrame[idx];
                    break;
                case AnimationType.UI:
                    Image.sprite = animationsFrame[idx];
                    break;
            }
            duration = 0f;

            if (idx < animationsFrame.Length - 1) idx++;
            else idx = 0;
        }
    }

}
