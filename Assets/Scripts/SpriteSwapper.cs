using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    public Sprite SpriteToUse;
    public SpriteRenderer CurrentSpriteRenderer;
    private Sprite _originalSprite;

    public void SpriteSwap()
    {
        if(SpriteToUse != CurrentSpriteRenderer.sprite)
        {
            _originalSprite = CurrentSpriteRenderer.sprite;

            CurrentSpriteRenderer.sprite = SpriteToUse;
        }
    }

    public void ResetSprite()
    {
        if(_originalSprite != null)
        {
            CurrentSpriteRenderer.sprite = _originalSprite;
        }
    }
}
