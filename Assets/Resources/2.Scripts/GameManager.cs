using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private RhythmPlayer rhythmPlayer;
    private FadeController fadeCtrl;
    private Player player;
    private bool isGameEnd;
    private bool isEventEnd;

    public Action WeaponActiveTrue;
    public bool IsEventEnd
    {
        get { return isEventEnd; }
        set { isEventEnd = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        rhythmPlayer = FindAnyObjectByType<RhythmPlayer>();
        fadeCtrl = FindAnyObjectByType<FadeController>();
        player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if(rhythmPlayer.EndTrack >= 4) 
        {   
            var note = FindAnyObjectByType<RhythmBlock>();
            if(note == null)
            {
                rhythmPlayer.EndTrack = 0;
                isGameEnd = true;
            }
        }

        if(isGameEnd == true)
        {
            isGameEnd = false;
            player.Active(false);
            fadeCtrl.FadeOut(0, WeaponActiveTrue);
        }

        if (isEventEnd == true)
        {
            isEventEnd = false;

            fadeCtrl.FadeOut(0);
        }
    }
}
