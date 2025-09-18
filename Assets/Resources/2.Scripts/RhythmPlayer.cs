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
    [SerializeField] Monster monster;
    [SerializeField] TrackController trackCtrl;

    private float badValue = 0;
    private float perfactValue = 0;
    private int noteCount = 0;
    private int endTrack = 0;
    private bool isPlaying = false;
    private Player playerSetup;
    private IObjectPool<RhythmBlock> rhythmPool;

    public int EndTrack
    {
        get { return endTrack; }
        set { endTrack = value; }
    }

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
        playerSetup = FindAnyObjectByType<Player>();

        for (int i = 0; i < rhythm.BlueTrackRhythmNotes.Length; i++)
        {
            for (int j = 0; j < rhythm.BlueTrackRhythmNotes[i].RhythmNoteTrack.Count; j++)
            {
                if (rhythm.BlueTrackRhythmNotes[i].RhythmNoteTrack[j].IsRest == false)
                {
                    noteCount++;
                }
            }
        }

        for (int i = 0; i < rhythm.RedTrackRhythmNotes.Length; i++)
        {
            for (int j = 0; j < rhythm.RedTrackRhythmNotes[i].RhythmNoteTrack.Count; j++)
            {
                if (rhythm.RedTrackRhythmNotes[i].RhythmNoteTrack[j].IsRest == false)
                {
                    noteCount++;
                }
            }
        }

        UIManager.Instance.PerfactValue = 100 / noteCount;
        UIManager.Instance.BadValue = UIManager.Instance.PerfactValue / 2;

        //StartCoroutine(PlayRhythm(rhythm.RedRhythmNotes));
        StartCoroutine(PlayRhythm(rhythm.RedTrackRhythmNotes[0].RhythmNoteTrack, TrackType.Red));
        StartCoroutine(PlayRhythm(rhythm.RedTrackRhythmNotes[1].RhythmNoteTrack, TrackType.Red, 1));
        StartCoroutine(PlayRhythm(rhythm.BlueTrackRhythmNotes[0].RhythmNoteTrack, TrackType.Blue));
        StartCoroutine(PlayRhythm(rhythm.BlueTrackRhythmNotes[1].RhythmNoteTrack, TrackType.Blue, 1));
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
        //SFX, VFXÃß°¡
    }

    public void PlayRhythm(Rhythm rhythm)
    {
        playerSetup.Active(true);

        StartCoroutine(PlayRhythm(rhythm.RedTrackRhythmNotes[0].RhythmNoteTrack, TrackType.Red));
        StartCoroutine(PlayRhythm(rhythm.RedTrackRhythmNotes[1].RhythmNoteTrack, TrackType.Red, 1));
        StartCoroutine(PlayRhythm(rhythm.BlueTrackRhythmNotes[0].RhythmNoteTrack, TrackType.Blue));
        StartCoroutine(PlayRhythm(rhythm.BlueTrackRhythmNotes[1].RhythmNoteTrack, TrackType.Blue, 1));
    }

    public void MonsterSpawnStart()
    {
        StartCoroutine(MonsterSpawn());
    }

    IEnumerator PlayRhythm(List<RhythmNote> rhythmNotes, TrackType type, int trackIndex = 0)
    {
        int rhythmIndex = 0;
        float beatDuration = 60f / rhythm.Bpm;
        float bps = beatDuration * rhythmNotes[rhythmIndex].Beats;
        float nextNote = Time.time + bps;
        if (rhythmNotes[rhythmIndex].IsRest == false) { ActiveBlock(bps, type, trackIndex); }
        

        while (Time.time < nextNote)
        {
            yield return null;
        }

        rhythmIndex++;

        while (rhythmIndex < rhythmNotes.Count)
        {
            bps = beatDuration * rhythmNotes[rhythmIndex].Beats;
            nextNote = Time.time + bps;
            if (rhythmNotes[rhythmIndex].IsRest == false) { ActiveBlock(bps, type, trackIndex); }

            while (Time.time < nextNote)
            {
                yield return null;
            }

            rhythmIndex++;
        }

        endTrack++;
    }

    IEnumerator MonsterSpawn()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(2f);

            Monster mons = Instantiate(monster).GetComponent<Monster>();
            int trackType = Random.Range(0, 2);
            int trackIndex = Random.Range(0, 2);
            mons.gameObject.SetActive(true);
            mons.Init(trackCtrl.GetTrack((TrackType)trackType, trackIndex));
        }

        yield return new WaitForSeconds(10f);

        GameManager.Instance.IsEventEnd = true;
        yield break;
    }
}
