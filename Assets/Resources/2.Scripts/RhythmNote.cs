using UnityEngine;

public enum Note
{
    Whole = 1,              // 4 beats
    Half = 2,               // 2 beats
    Quarter = 4,            // 1 beats
    Eighth = 8,             // 1 / 2 beats
    Sixteenth = 16           // 1 / 4 beats
}

[System.Serializable]
public class RhythmNote
{
    [SerializeField] Note note;
    [SerializeField] bool isRest;

    public RhythmNote(Note note, bool isRest)
    {
        this.note = note;
        this.isRest = isRest;
    }
    
    public bool IsRest => isRest;
    public float Beats => note switch
    {
        Note.Whole => 4f,
        Note.Half => 2f,
        Note.Quarter => 1f,
        Note.Eighth => 0.5f,
        Note.Sixteenth => 0.25f,
        _ => 0f
    };
}
