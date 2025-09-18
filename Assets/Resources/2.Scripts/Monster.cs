using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private Vector3[] track;
    private int trackIndex;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Init(Vector3[] track)
    {
        this.track = track;
        StartCoroutine(MoveToTrack());
    }

    IEnumerator MoveToTrack()
    {
        for (trackIndex = 0; trackIndex + 1 < track.Length; trackIndex++)
        {
            float distance = Vector3.Distance(track[trackIndex], track[trackIndex + 1]);
            float remainingDistance = distance;

            while (remainingDistance > 0)
            {
                transform.position = Vector3.Lerp(track[trackIndex], track[trackIndex + 1], 1 - (remainingDistance / distance));
                remainingDistance -= moveSpeed * Time.deltaTime;
                yield return null;
            }

            yield return null;
        }

        Destroy(gameObject);

        yield break;
    }
}
