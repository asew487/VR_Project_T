using System.Collections;
using UnityEngine;

public class RhythmPlayer : MonoBehaviour
{
    [SerializeField] Rhythm _rhythm;
    private int _rhythmIndex = 0;
    private float _beatTime = 0;
    private float _beatTimer = 0;
    private bool _isPlaying = false;

    private void Start()
    {
        //StartCoroutine(PlayRhythm());
    }

    private void Update()
    {
        if(_rhythmIndex < _rhythm.RhythmNotes.Count)
        {
            if(_isPlaying == false)
            {
                Debug.Log("��");
                float beatDuration = 60f / _rhythm.Bpm;
                float bps = beatDuration * _rhythm.RhythmNotes[_rhythmIndex].Beats;
                _beatTime = bps;
                _isPlaying = true;
            }

            _beatTimer += Time.deltaTime;

            if (_beatTimer > _beatTime)
            {
                _rhythmIndex++;
                _beatTimer = 0;
                _isPlaying = false;
            }
        }

        
    }

    IEnumerator PlayRhythm()
    {
        float beatDuration = 60f / _rhythm.Bpm; // 1�� �ð�(��)
        float nextTime = Time.time;             // ù �� ����

        while (_rhythmIndex < _rhythm.RhythmNotes.Count)
        {
            float bps = beatDuration * _rhythm.RhythmNotes[_rhythmIndex].Beats;

            Debug.Log($"{Time.time:F3}�� �� �� (���� {bps})");

            // ���� �� �ð� ����
            nextTime += bps;

            // ���� �� �ð��� �� ������ ���
            while (Time.time < nextTime)
                yield return null;

            _rhythmIndex++;
        }
    }
}
