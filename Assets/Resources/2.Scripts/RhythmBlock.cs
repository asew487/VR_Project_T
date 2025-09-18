using Meta.WitAi;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
using System.Collections;

public enum RhythmBlockState
{
    Bad,
    Good,
    Perfact
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

    public RhythmBlockState State
    {
        get { return state; }
        set { state = value; }
    }

    void Start()
    {

    }

    void Update()
    {
        
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

    public void Release()
    {
        rhythmPool.Release(this);
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

        rhythmPool.Release(this);
        yield return null;
    }
}
