using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : MonoBehaviour
{
    public Transform CameraFollowTarget;

    public Rigidbody2D ropebody;

    public Sprite ArmHoldingEmpty;
    public Sprite ArmHoldingTreasure;

    public SpriteRenderer HoldingArm;

    public GameObject DeathPrefub;
    public GameObject FlameDeathPrefub;
    public GameObject GhostPrefub;

    public float delayBeforeRemoving = 3.0f;
    public float delayBeforeReleasingGhost = 0.25f;

    public GameObject BloodFountainPrefub;

    private bool _dead = false;

    private bool _holdingTreasure;

    public bool HoldingTreasure
    {
        get
        {
            return _holdingTreasure;
        }

        set
        {
            if (_dead == true) return;

            _holdingTreasure = value;

            if (HoldingArm != null)
            {
                if (_holdingTreasure == true)
                {
                    HoldingArm.sprite = ArmHoldingTreasure;
                }
                else if (_holdingTreasure == false)
                {
                    HoldingArm.sprite = ArmHoldingEmpty;
                }

                //HoldingArm.sprite = _holdingTreasure ? ArmHoldingTreasure : ArmHoldingEmpty;
            }
        }
    }

    public enum DamageType
    {
        Slicing,
        Burning
    }

    public void ShowDamageEffect(DamageType type)
    {
        switch(type)
        {
            case DamageType.Burning:
                if(FlameDeathPrefub != null)
                {
                    Instantiate(FlameDeathPrefub, CameraFollowTarget.position, CameraFollowTarget.rotation);
                }

                break;

            case DamageType.Slicing:
                if (DeathPrefub != null)
                {
                    Instantiate(DeathPrefub, CameraFollowTarget.position, CameraFollowTarget.rotation);
                }

                break;
        }
    }

    public void DestroyGnome(DamageType type)
    {
        HoldingTreasure = false;

        _dead = true;

        foreach (BodyPart part in GetComponentsInChildren<BodyPart>())
        {
            switch (type)
            {
                case DamageType.Burning:
                    bool shouldBurn = Random.Range(0, 2) == 0;

                    if (shouldBurn)
                    {
                        part.ApplyDamageSprite(type);
                    }

                    break;

                case DamageType.Slicing:
                    part.ApplyDamageSprite(type);

                    break;
            }

            bool shouldDeath = Random.Range(0, 2) == 0;

            if(shouldDeath)
            {
                part.Detach();

                if(type == DamageType.Slicing)
                {
                    if(part.BloodFountainOrigin != null && BloodFountainPrefub != null)
                    {
                        GameObject fountain = Instantiate(BloodFountainPrefub,
                                                          part.BloodFountainOrigin.position,
                                                          part.BloodFountainOrigin.rotation);

                        fountain.transform.SetParent(CameraFollowTarget, false);
                    }
                }

                var AllJoints = part.GetComponentsInChildren<Joint2D>();

                foreach(Joint2D joint in AllJoints)
                {
                    Destroy(joint);
                }
            }
        }

        var remove = gameObject.AddComponent<RemoveAfterDelay>();
        remove.Delay = delayBeforeRemoving;

        StartCoroutine(ReleaseGhost());
    }

    IEnumerator ReleaseGhost()
    {
        if (GhostPrefub == null) yield break;

        yield return new WaitForSeconds(delayBeforeReleasingGhost);

        Instantiate(GhostPrefub, transform.position, Quaternion.identity);
    }
}
