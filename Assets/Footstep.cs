using UnityEngine;

public class Footstep : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        if (!src) src = gameObject.AddComponent<AudioSource>();
    }

    // Called by AnimationEvent “PlayStep”
    public void PlayStep()
    {
        if (clip) src.PlayOneShot(clip);
    }
}
