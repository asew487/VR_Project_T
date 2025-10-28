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
    
    // Importer에서 값을 설정할 수 있게 하는 메서드들
    public void SetBpm(float value) => bpm = value;
    public void SetRedTracks(Track[] tracks) => redTrackRhythmNotes = tracks;
    public void SetBlueTracks(Track[] tracks) => blueTrackRhythmNotes = tracks;
}
