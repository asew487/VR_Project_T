using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RhythmPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Rhythm rhythm;
    [SerializeField] RhythmBlock redRhythmBlcok;
    [SerializeField] RhythmBlock blueRhythmBlock;
    [SerializeField] TrackController trackCtrl;

    private int rhythmIndex = 0;
    private float beatTime = 0;
    private float beatTimer = 0;
    private bool isPlaying = false;
    private IObjectPool<RhythmBlock> rhythmPool;

    private void Awake()
    {
        rhythmPool = new ObjectPool<RhythmBlock>(
            createFunc: CreateRhythmBlock,
            actionOnGet: GetRhythmBlock,
            actionOnRelease: ReleaseRhythmBlock
            );
    }

    private void Start()
    {
        //StartCoroutine(PlayRhythm(rhythm.RedRhythmNotes));
        StartCoroutine(PlayRhythm(rhythm.BlueRhythmNotes, TrackType.Blue));
    }

    private void ActiveBlock(float bps, TrackType Type, int index)
    {
        RhythmBlock block = rhythmPool.Get();
        block.SetTrack(trackCtrl.GetTrack(Type, index));
        block.Init(bps);
    }

    private RhythmBlock CreateRhythmBlock()
    {
        RhythmBlock block = Instantiate(blueRhythmBlock).GetComponent<RhythmBlock>();
        block.SetPool(rhythmPool);
        return block;
    }

    private void GetRhythmBlock(RhythmBlock block)
    {
        Vector3 targetPos = block.transform.position;
        Vector3 playerPos = player.transform.position;
        targetPos.y = 0;
        playerPos.y = 0;

        Vector3 dir = targetPos - playerPos;
        dir.Normalize();
        Vector3 moveVec = player.transform.position + dir * 0.5f;
        moveVec.y = block.transform.position.y;

        block.transform.position = moveVec;
        block.gameObject.SetActive(true);
    }

    private void ReleaseRhythmBlock(RhythmBlock block)
    {
        block.gameObject.SetActive(false);
    }

    IEnumerator PlayRhythm(List<RhythmNote> rhythmNotes, TrackType type)
    {
        float beatDuration = 60f / rhythm.Bpm;
        float bps = beatDuration * rhythmNotes[rhythmIndex].Beats;
        float nextNote = Time.time + bps;
        if (rhythmNotes[rhythmIndex].IsRest == false) { ActiveBlock(bps, type, 0); }
        

        while (Time.time < nextNote)
        {
            yield return null;
        }

        rhythmIndex++;

        while (rhythmIndex < rhythmNotes.Count)
        {
            bps = beatDuration * rhythmNotes[rhythmIndex].Beats;
            nextNote = Time.time + bps;
            if (rhythmNotes[rhythmIndex].IsRest == false) { ActiveBlock(bps, type, 0); }

            while (Time.time < nextNote)
            {
                yield return null;
            }

            rhythmIndex++;
        }
    }
}
