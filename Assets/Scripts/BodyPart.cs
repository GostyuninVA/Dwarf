using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour
{
    public Sprite DetachedSprite;
    public Sprite BurnedSprite;
    public Transform BloodFountainOrigin;
    private bool _detached;

    public void Detach()
    {
        _detached = true;

        tag = "Untagged";

        transform.SetParent(null, true);
    }

    private void Update()
    {
        if(_detached == false)
        {
            return;
        }

        var rigidbody = GetComponent<Rigidbody2D>();

        if(rigidbody.IsSleeping())
        {
            foreach(Joint2D joint in GetComponentsInChildren<Joint2D>() )
            {
                Destroy(joint);
            }

            foreach(Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
            {
                Destroy(rb);
            }

            foreach(Collider2D collider in GetComponentsInChildren<Collider2D>())
            {
                Destroy(collider);
            }

            Destroy(this);
        }
    }
    public void ApplyDamageSprite(Gnome.DamageType damageType)
    {
        Sprite spriteToUse = null;

        switch(damageType)
        {
            case Gnome.DamageType.Burning:
                spriteToUse = BurnedSprite;

                break;
            case Gnome.DamageType.Slicing:
                spriteToUse = DetachedSprite;

                break;
        }

        if (spriteToUse != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteToUse;
        }
    }
}
