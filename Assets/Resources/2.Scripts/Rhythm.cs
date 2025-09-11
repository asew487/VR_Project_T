using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rhythm", menuName = "Scriptable/Rhythm")]
public class Rhythm : ScriptableObject
{
    [SerializeField] float bpm;
    [SerializeField] List<RhythmNote> redRhythmNotes;
    [SerializeField] List<RhythmNote> blueRhythmNotes;

    public float Bpm => bpm;
    public List<RhythmNote> RedRhythmNotes => redRhythmNotes;
    public List<RhythmNote> BlueRhythmNotes => blueRhythmNotes;
}
