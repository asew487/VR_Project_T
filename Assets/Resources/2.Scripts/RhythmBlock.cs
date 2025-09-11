using Meta.WitAi;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using System.Collections;

public enum RhythmBlockState
{
    Bad,
    Good
}

public class RhythmBlock : MonoBehaviour
{
    [SerializeField] GameObject rhythmBlock;
    [SerializeField] float moveSpeed;
    [SerializeField] float goodTimeDuration = 0.3f;
    [SerializeField] ParticleSystem particle;
    [SerializeField] AudioClip audioClip;
    [SerializeField] LayerMask layer;
    [SerializeField] private RhythmBlockState state = RhythmBlockState.Bad;

    private Vector3 baseScale = Vector3.zero;
    private Vector3 targetScale = Vector3.one;
    private float lerpTimer = 0;
    private float lerpTime = 0;
    private float goodTime = 0;
    private Vector3[] track;
    private int trackIndex;
    private IObjectPool<RhythmBlock> rhythmPool;

    void Start()
    {

    }

    void Update()
    {
        if (gameObject.activeSelf == false) return;

        //switch(state)
        //{
        //    case RhythmBlockState.Bad:
        //        lerpTimer += Time.deltaTime;
        //        float t = Mathf.Clamp01(lerpTimer / lerpTime);
        //        //_rhythmBlock.transform.localScale = Vector3.Lerp(_baseScale, _targetScale, t);
        //        if (t >= 1) 
        //        {
        //            //Debug.Log($"{Time.time} | {_lerpTimer}");
        //            state = RhythmBlockState.Good; 
        //        }
        //        break;
        //    case RhythmBlockState.Good:
        //        if(Time.time >  goodTime)
        //        {
        //            //gameObject.SetActive(false);
        //            rhythmPool.Release(this);
        //        }
        //        break;
        //}

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layer) return;

        EffectManager.Instance.PlayOnShot(transform.position);
        AudioManager.Instance.PlayOnShot(audioClip);
        //gameObject.SetActive(false);
        rhythmPool.Release(this);
    }

    public void SetPool(IObjectPool<RhythmBlock> pool)
    {
        rhythmPool = pool;
    }

    public void SetTrack(Vector3[] track)
    {
        this.track = track;
        gameObject.transform.position = track[0];
    }

    public void Init(float bps)
    {
        lerpTime = bps;
        lerpTimer = 0;
        goodTime = Time.time + bps + goodTimeDuration;
        state = RhythmBlockState.Bad;
        StartCoroutine(MoveToTrack());
    }

    IEnumerator MoveToTrack()
    {
        for (trackIndex = 0; trackIndex + 1 < track.Length; trackIndex++)
        {
            float distance = Vector3.Distance(track[trackIndex], track[trackIndex + 1]);
            float remainingDistance = distance;

            while (remainingDistance > 0)
            {
                transform.position = Vector3.Lerp(track[trackIndex], track[trackIndex + 1], 1 - (remainingDistance / distance));
                remainingDistance -= moveSpeed * Time.deltaTime;
                yield return null;
            }

            yield return null;
        }
        

        yield return null;
    }
}
