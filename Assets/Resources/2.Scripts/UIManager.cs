using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    [SerializeField] Image[] productionGauge = new Image[10];
    [SerializeField] TextMeshProUGUI text;

    private float productionGaugeValue;
    private float badValue;
    private float perfactValue;

    public float BadValue { get { return badValue; } set { badValue = value; } }
    public float PerfactValue { get { return perfactValue; } set { perfactValue = value; } }

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
        foreach (Image gauge in productionGauge)
        {
            gauge.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }

    public void AddProductionGaugeValue(RhythmBlockState state)
    {
        switch (state)
        {
            case RhythmBlockState.Bad:
                productionGaugeValue += badValue;
                break;
            case RhythmBlockState.Perfact:
                productionGaugeValue += perfactValue;
                break;
        }

        for (int i = 0; i < productionGaugeValue / 10; i++)
        {
            productionGauge[i].gameObject.SetActive(true);
        }

        Mathf.Clamp(productionGaugeValue, 0, 100);
        text.text = productionGaugeValue.ToString() + "%";
    }
}
