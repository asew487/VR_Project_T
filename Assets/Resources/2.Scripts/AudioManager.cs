using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    [SerializeField] AudioSource _source;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public void PlayOnShot(AudioClip clip)
    {
        _source.Stop();
        _source.clip = clip;
        _source.Play();
    }
}
