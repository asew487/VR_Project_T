using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager _instance;
    public static EffectManager Instance => _instance;

    [SerializeField] ParticleSystem _particles;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public void PlayOnShot(Vector3 pos)
    {
        _particles.Stop();
        _particles.Clear();
        _particles.gameObject.transform.position = pos;
        _particles.Play();
    }
}
