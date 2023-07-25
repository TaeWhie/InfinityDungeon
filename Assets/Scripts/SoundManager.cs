using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _sounds;
    private AudioSource _mAudioSource;
    void Start()
    {
        _mAudioSource = GetComponent<AudioSource>();
    }
}
