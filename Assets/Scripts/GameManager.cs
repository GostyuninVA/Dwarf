using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject StartingPoint;
    
    public Rope RopeComponent;
    
    public CameraFollow CameraFollowComponent;
    
    public GameObject GnomePrefub;
    
    public RectTransform MainMenu;
    public RectTransform GameplayMenu;
    public RectTransform GameOverMenu;

    public bool GnomeInvincible { get; set; }

    public float DelayAfterDeath = 1.0f;

    public AudioClip GnomeDiedSound;
    public AudioClip GameOverSound;

    private Gnome _currentGnome;


    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        if (GameOverMenu)
            GameOverMenu.gameObject.SetActive(false);

        if (MainMenu)
            MainMenu.gameObject.SetActive(false);

        if (GameplayMenu)
            GameplayMenu.gameObject.SetActive(true);

        var resetObjects = FindObjectsOfType<Ressettable>();

        foreach(Ressettable ressettable in resetObjects)
        {
            ressettable.Reset();
        }

        CreateNewGnome();

        Time.timeScale = 1.0f;
    }
    
    private void CreateNewGnome()
    {
        RemoveGnome();

        GameObject newGnome = Instantiate(GnomePrefub, StartingPoint.transform.position, Quaternion.identity);

        _currentGnome = newGnome.GetComponent<Gnome>();

        RopeComponent.gameObject.SetActive(true);
        RopeComponent.ConnectedObject = _currentGnome.ropebody;
        RopeComponent.ResetLength();

        CameraFollowComponent.Target = _currentGnome.CameraFollowTarget; // смена объекта наблюдения камеры
    }

    private void RemoveGnome()
    {
        if (GnomeInvincible)
            return;

        RopeComponent.gameObject.SetActive(false);

        CameraFollowComponent.Target = null;

        if(_currentGnome != null)
        {
            _currentGnome.HoldingTreasure = false;
            _currentGnome.gameObject.tag = "Untagged";

            foreach (Transform child in _currentGnome.transform)
            {
                child.gameObject.tag = "Untagged";
            }

            _currentGnome = null;
        }
    }

    private void KillGnome(Gnome.DamageType damageType)
    {
        var audio = GetComponent<AudioSource>();

        if (audio)
            audio.PlayOneShot(GnomeDiedSound);

        _currentGnome.ShowDamageEffect(damageType);

        if(GnomeInvincible == false)
        {
            _currentGnome.DestroyGnome(damageType);
            RemoveGnome();
            StartCoroutine(ResetAfterDelay());
        }
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(DelayAfterDeath);
        Reset();
    }

    public void TrapTouched()
    {
        KillGnome(Gnome.DamageType.Slicing);
    }

    public void FireTrapTouched()
    {
        KillGnome(Gnome.DamageType.Burning);
    }

    public void TreasureCollected()
    {
        _currentGnome.HoldingTreasure = true;
    }

    public void ExitReached()
    {
        if (_currentGnome != null && _currentGnome.HoldingTreasure == true)
        {
            var audio = GetComponent<AudioSource>();
            if (audio)
                audio.PlayOneShot(GameOverSound);

            Time.timeScale = 0.0f;

            if (GameOverMenu)
                GameOverMenu.gameObject.SetActive(true);
            if (GameplayMenu)
                GameplayMenu.gameObject.SetActive(false);
        }
    }

    public void SetPaused(bool paused)
    {
        if(paused == true)
        {
            Time.timeScale = 0.0f;
            MainMenu.gameObject.SetActive(true);
            GameplayMenu.gameObject.SetActive(false);
        }
        else if(paused == false)
        {
            Time.timeScale = 1.0f;
            MainMenu.gameObject.SetActive(false);
            GameplayMenu.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Destroy(_currentGnome.gameObject);
        _currentGnome = null;

        Reset();
    }
}
