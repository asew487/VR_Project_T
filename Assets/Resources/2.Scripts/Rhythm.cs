using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rhythm", menuName = "Scriptable/Rhythm")]
public class Rhythm : ScriptableObject
{
    [SerializeField] float bpm;
    [SerializeField] Track[] redTrackRhythmNotes = new Track[2];
    [SerializeField] Track[] blueTrackRhythmNotes = new Track[2];

    public float Bpm => bpm;
    public Track[] RedTrackRhythmNotes => redTrackRhythmNotes;
    public Track[] BlueTrackRhythmNotes => blueTrackRhythmNotes;
}
