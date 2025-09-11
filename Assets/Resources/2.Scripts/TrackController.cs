using UnityEngine;

public enum TrackType
{
    Red,
    Blue
}

public class TrackController : MonoBehaviour
{
    [SerializeField] private LineRenderer redTrackLineOne;
    [SerializeField] private LineRenderer redTrackLineTwo;
    [SerializeField] private LineRenderer blueTrackLineOne;
    [SerializeField] private LineRenderer blueTrackLineTwo;

    private Vector3[] redTrackOne = new Vector3[5];
    private Vector3[] redTrackTwo = new Vector3[5];
    private Vector3[] blueTrackOne = new Vector3[5];
    private Vector3[] blueTrackTwo = new Vector3[5];


    void Start()
    {
        redTrackLineOne.GetPositions(redTrackOne);
        redTrackLineTwo.GetPositions(redTrackTwo);
        blueTrackLineOne.GetPositions(blueTrackOne);
        blueTrackLineTwo.GetPositions(blueTrackTwo);
    }

    void Update()
    {
        
    }

    public Vector3[] GetTrack(TrackType type, int index)
    {
        switch (type)
        {
            case TrackType.Red:
                if (index <= 0) { return redTrackOne; }
                else if (index >= 1) { return redTrackTwo; }
                break;
            case TrackType.Blue:
                if (index <= 0) { return blueTrackOne; }
                else if (index >= 1) { return blueTrackTwo; }
                break;
        }

        return null;
    }
}
