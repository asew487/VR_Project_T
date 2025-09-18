using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Track
{
    [SerializeField] List<RhythmNote> rhythmNoteTrack = new List<RhythmNote>();

    public List<RhythmNote> RhythmNoteTrack => rhythmNoteTrack;
}
