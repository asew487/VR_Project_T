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

    private void ActiveBlock(RhythmNote rhythmNote)
    {
        for(int i = 0; i < _rhythmBlocks.Length; i++)
        {
            
            if(_rhythmBlocks[i].gameObject.activeSelf == false)
            {

            }
        }
    }

    IEnumerator PlayRhythm()
    {
        while(_rhythmIndex < _rhythm.RhythmNotes.Count)
        {
            Debug.Log(Time.time);
            float beatDuration = 60f / _rhythm.Bpm;
            float bps = beatDuration * _rhythm.RhythmNotes[_rhythmIndex].Beats;
            float nextNote = Time.time + bps;
            // 다음박 예비박 시작하기 - 스케일 bps 속도 동아 증가

            //yield return new WaitForSeconds(bps);
            while(Time.time < nextNote)
            {
                yield return null;
            }

            _rhythmIndex++;
        }
    }
}
