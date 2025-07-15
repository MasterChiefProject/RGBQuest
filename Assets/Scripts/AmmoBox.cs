using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider), typeof(AudioSource))]
public class AmmoBox : MonoBehaviour
{
    [Header("Box contents")]
    [SerializeField] private int ammoInBox = 3;

    [Header("UI")]
    [SerializeField] private Text ammoTextUI;

    [Header("Audio")]
    [SerializeField] private float audioVolume = 1f;

    private AudioSource ammoSound;
    private Collider myTrigger;

    private bool picked;        // ← guard flag

    /* ─────────────────────────────── */
    private void Awake()
    {
        ammoSound = GetComponent<AudioSource>();
        myTrigger = GetComponent<Collider>();
    }

    /* ─────────────────────────────── */
    private void OnTriggerEnter(Collider other)
    {
        if (picked) return;                        // already collected

        if (other.CompareTag("Player"))
        {
            picked = true;                         // lock out further hits
            myTrigger.enabled = false;             // optional, extra safety

            Globals.ammo = Mathf.Min(Globals.ammo + ammoInBox,
                                     Globals.magazineCapacity);

            UpdateAmmoUI();
            StartCoroutine(PlayAndDestroy());
        }
    }

    /* ─────────────────────────────── */
    private IEnumerator PlayAndDestroy()
    {
        ammoSound.PlayOneShot(ammoSound.clip, audioVolume);
        yield return new WaitForSeconds(ammoSound.clip.length);
        Destroy(gameObject);
    }

    private void UpdateAmmoUI()
    {
        ammoTextUI.text = $"{Globals.ammo}/{Globals.magazineCapacity}";
    }
}
