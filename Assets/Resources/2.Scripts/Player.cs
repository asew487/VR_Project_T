using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject hammerLeft;
    [SerializeField] GameObject hammerRight;
    [SerializeField] GameObject anvil;

    public void Active(bool active)
    {
        hammerLeft.SetActive(active);
        hammerRight.SetActive(active);
        anvil.SetActive(active);
    }
}
