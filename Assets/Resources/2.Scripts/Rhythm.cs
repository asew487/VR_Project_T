using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rhythm", menuName = "Scriptable/Rhythm")]
public class Rhythm : ScriptableObject
{
    [SerializeField] float _bpm;
    [SerializeField] List<RhythmNote> _rhythmNotes;

    public float Bpm => _bpm;
    public List<RhythmNote> RhythmNotes => _rhythmNotes;
}
