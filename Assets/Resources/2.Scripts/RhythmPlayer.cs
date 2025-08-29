using System.Collections;
using UnityEngine;

public class RhythmPlayer : MonoBehaviour
{
    [SerializeField] Rhythm _rhythm;
    [SerializeField] RhythmBlock[] _rhythmBlocks;
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
        for(int i = 0; i < _rhythmBlocks.Length; i++)
        {
            if(_rhythmBlocks[i].gameObject.activeSelf == false)
            {
                _rhythmBlocks[i].gameObject.SetActive(true);
                _rhythmBlocks[i].Init(bps);
                break;
            }
        }
    }

    IEnumerator PlayRhythm()
    {
        float beatDuration = 60f / _rhythm.Bpm;
        float bps = beatDuration * _rhythm.RhythmNotes[_rhythmIndex].Beats;
        float nextNote = Time.time + bps;
        ActiveBlock(bps);

        while (Time.time < nextNote)
        {
            yield return null;
        }

        _rhythmIndex++;

        while (_rhythmIndex < _rhythm.RhythmNotes.Count)
        {
            bps = beatDuration * _rhythm.RhythmNotes[_rhythmIndex].Beats;
            nextNote = Time.time + bps;
            ActiveBlock(bps);

            while(Time.time < nextNote)
            {
                yield return null;
            }

            _rhythmIndex++;
        }
    }
}
