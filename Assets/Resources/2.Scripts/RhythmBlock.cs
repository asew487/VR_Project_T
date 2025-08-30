using UnityEngine;

public enum RhythmBlockState
{
    Bad,
    Good
}

public class RhythmBlock : MonoBehaviour
{
    [SerializeField] GameObject _rhythmBlock;
    [SerializeField] float _goodTimeDuration = 0.3f;
    [SerializeField] ParticleSystem _particle;
    [SerializeField] AudioClip _audioClip;
    [SerializeField] LayerMask _layer;
    [SerializeField] private RhythmBlockState state = RhythmBlockState.Bad;

    private Vector3 _baseScale = Vector3.zero;
    private Vector3 _targetScale = Vector3.one;
    private float _lerpTimer = 0;
    private float _lerpTime = 0;
    private float _goodTime = 0;

    void Start()
    {

    }

    void Update()
    {
        if (gameObject.activeSelf == false) return;

        switch(state)
        {
            case RhythmBlockState.Bad:
                _lerpTimer += Time.deltaTime;
                float t = Mathf.Clamp01(_lerpTimer / _lerpTime);
                _rhythmBlock.transform.localScale = Vector3.Lerp(_baseScale, _targetScale, t);
                if (t >= 1) state = RhythmBlockState.Good;
                break;
            case RhythmBlockState.Good:
                if(Time.time >  _goodTime)
                {
                    gameObject.SetActive(false);
                }
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _layer) return;

        EffectManager.Instance.PlayOnShot(transform.position);
        AudioManager.Instance.PlayOnShot(_audioClip);
        gameObject.SetActive(false);
    }

    public void Init(float bps)
    {
        _lerpTime = bps;
        _lerpTimer = 0;
        _goodTime = Time.time + bps + _goodTimeDuration;
        state = RhythmBlockState.Bad;
        _rhythmBlock.transform.localScale = _baseScale;
    }
}
