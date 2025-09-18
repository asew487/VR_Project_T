using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class TimingChecker : MonoBehaviour
{
    [SerializeField] Anvil anvil;
    [SerializeField] AudioClip clip;
    [SerializeField] ParticleSystem particle;
    private Queue<RhythmBlock> notes = new Queue<RhythmBlock>();

    void Start()
    {
        anvil.Pressed += Pressed;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var note = other.GetComponent<RhythmBlock>();
        note.State = RhythmBlockState.Perfact;
        notes.Enqueue(note);
    }

    private void OnTriggerExit(Collider other)
    {
        //var note = other.GetComponent<RhythmBlock>();
        //note.State = RhythmBlockState.Bad;
        if(notes.Count > 0)
        {
            notes.Dequeue();
        }
    }

    public void Pressed()
    {
        if (notes.Count == 0) { return; }

        AudioSource source = GetComponent<AudioSource>();
        source?.Stop();
        source.clip = clip;
        source?.Play();

        particle?.Stop();
        particle?.Play();

        RhythmBlock note = notes.Dequeue();
        UIManager.Instance.AddProductionGaugeValue(note.State);
        note.Release();
    }
}
