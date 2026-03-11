//Edgar, 2026-03-08
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    
    [Header("Source")]
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;
    [Header("Clips")]
    public AudioClip background;
    //Becouse of the small scope we can get away with an audio bank system that can be switched out//
    //Never do this in a bigger scope//
    public AudioClip[] sfxBankSlot;
    void Start()
    {
        if (musicSource)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }

    public void PlaySfx(int slot)
    {
        if (sfxBankSlot[slot] != null)
        {
            sfxSource.Stop();
            sfxSource.PlayOneShot(sfxBankSlot[slot]);
        }
    }


}
