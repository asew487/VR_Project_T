using System.Collections;
using UnityEngine;

public class WeaponEvent : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject playerHand;
    [SerializeField] float grabSpeed;

    private RhythmPlayer rhythmPlayer;

    void Start()
    {
        rhythmPlayer = FindAnyObjectByType<RhythmPlayer>();
        GameManager.Instance.WeaponActiveTrue = Event;
    }

    void Update()
    {
        
    }

    public void Event()
    {
        weapon.SetActive(true);
        StartCoroutine(GrabAnimation());
    }

    IEnumerator GrabAnimation()
    {
        yield return new WaitForSeconds(1f);

        Vector3 startPos = weapon.transform.position;
        float distance = Vector3.Distance(startPos, playerHand.transform.position);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            weapon.transform.position = Vector3.Lerp(startPos, playerHand.transform.position, 1 - (remainingDistance / distance));
            remainingDistance -= grabSpeed * Time.deltaTime;
            yield return null;
        }

        weapon.transform.position = playerHand.transform.position;
        weapon.transform.rotation = playerHand.transform.rotation;
        weapon.transform.parent = playerHand.transform;

        yield return new WaitForSeconds(1f);

        rhythmPlayer.MonsterSpawnStart();

        yield return null;
    }
}
