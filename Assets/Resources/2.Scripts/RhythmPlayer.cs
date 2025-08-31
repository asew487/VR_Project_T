using System.Collections;
using UnityEngine;

public class RhythmPlayer : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] Rhythm _rhythm;
    [SerializeField] RhythmBlock[] _rhythmBlocks;
    [SerializeField] float _nextNoteDuration;
    private int _rhythmIndex = 0;
    private float _beatTime = 0;
    private float _beatTimer = 0;
    private bool _isPlaying = false;

    private void Start()
    {
        StartCoroutine(PlayRhythm());
    }

    private void ActiveBlock(float bps)
    {
        //for(int i = 0; i < _rhythmBlocks.Length; i++)
        //{
        //    if(_rhythmBlocks[i].gameObject.activeSelf == false)
        //    {
        //        Vector3 targetPos = _rhythmBlocks[i].transform.position;
        //        Vector3 playerPos = _player.transform.position;
        //        targetPos.y = 0;
        //        playerPos.y = 0;
        //        Vector3 dir = targetPos - playerPos;
        //        dir.Normalize();
        //        Vector3 moveVec = _player.transform.position + dir * 0.5f;
        //        moveVec.y = _rhythmBlocks[i].transform.position.y;
        //        _rhythmBlocks[i].transform.position = moveVec;
        //        _rhythmBlocks[i].gameObject.SetActive(true);
        //        _rhythmBlocks[i].Init(bps);
        //        break;
        //    }
        //}

        Vector3 targetPos = _rhythmBlocks[_rhythmIndex].transform.position;
        Vector3 playerPos = _player.transform.position;
        targetPos.y = 0;
        playerPos.y = 0;
        Vector3 dir = targetPos - playerPos;
        dir.Normalize();
        Vector3 moveVec = _player.transform.position + dir * 0.5f;
        moveVec.y = _rhythmBlocks[_rhythmIndex].transform.position.y;
        _rhythmBlocks[_rhythmIndex].transform.position = moveVec;
        _rhythmBlocks[_rhythmIndex].gameObject.SetActive(true);
        _rhythmBlocks[_rhythmIndex].Init(bps);
    }

    IEnumerator PlayRhythm()
    {
        float beatDuration = 60f / _rhythm.Bpm;
        float bps = beatDuration * _rhythm.RhythmNotes[_rhythmIndex].Beats;
        float nextNote = Time.time + bps * _nextNoteDuration;
        ActiveBlock(bps);

        while (Time.time < nextNote)
        {
            yield return null;
        }

        _rhythmIndex++;

        while (_rhythmIndex < _rhythm.RhythmNotes.Count)
        {
            bps = beatDuration * _rhythm.RhythmNotes[_rhythmIndex].Beats;
            nextNote = Time.time + bps * _nextNoteDuration;
            ActiveBlock(bps + bps * (1 - _nextNoteDuration));

            while(Time.time < nextNote)
            {
                yield return null;
            }

            _rhythmIndex++;
        }
    }
}
