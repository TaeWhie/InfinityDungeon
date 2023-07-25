using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        _audioSource.Play();
    }
    void Update()
    {
        if (!_audioSource.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
