using Unity.VisualScripting;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBox : MonoBehaviour
{
    [Header("Information display UI")]
    public TextMeshProUGUI textDisplay;

    [Header("Ammo in box")]
    public int ammoInBox = 3;

    [Header("Ammo UI")]
    public Text ammoTextUI;

    [Header("Audio Volume")]
    public float audioVolume = 1f;

    private AudioSource ammoSound;

    private bool hasGivenAmmo = false; // <-- prevent double-trigger

    void Awake()
    {
        ammoSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasGivenAmmo) return; // <-- guard against double-trigger

        if (other.CompareTag("Player"))
        {
            if (Globals.ammo < Globals.magazineCapacity)
            {
                Globals.ammo += ammoInBox;
                if (Globals.ammo > Globals.magazineCapacity)
                    Globals.ammo = Globals.magazineCapacity;

                UpdateAmmoUI();
                textDisplay.text = "";

                hasGivenAmmo = true; // <-- mark as already used
                StartCoroutine(PlayAndDestroy());
            }
            else
            {
                textDisplay.text = "Full Ammo";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textDisplay.text = "";
        }
    }

    IEnumerator PlayAndDestroy()
    {
        ammoSound.PlayOneShot(ammoSound.clip, audioVolume);
        yield return new WaitForSeconds(ammoSound.clip.length);
        Destroy(gameObject);
    }

    private void UpdateAmmoUI()
    {
        ammoTextUI.text = Globals.ammo.ToString() + "/" + Globals.magazineCapacity.ToString();
    }
}
