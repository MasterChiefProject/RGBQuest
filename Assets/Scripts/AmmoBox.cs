using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBox : MonoBehaviour
{
    [Header("Ammo in box")]
    public int ammoInBox = 3;

    [Header("Ammo UI")]
    public Text ammoTextUI;

    [Header("Audio Volume")]
    public float audioVolume = 1f;

    private AudioSource ammoSound;

    void Awake()
    {
        ammoSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Globals.ammo += ammoInBox;
        if(Globals.ammo > Globals.magazineCapacity)
            Globals.ammo = Globals.magazineCapacity;

        UpdateAmmoUI();
        StartCoroutine(PlayAndDestroy());
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
