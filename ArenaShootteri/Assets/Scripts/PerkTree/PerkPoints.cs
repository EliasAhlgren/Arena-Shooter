using UnityEngine;
using UnityEngine.UI;

public class PerkPoints : MonoBehaviour
{
    public PerkHub perkHub;

    private Text perkText;
    private float perkPoints = 0;

    void Start()
    {
        perkText = GetComponent<Text>();
        perkText.text = PerkTreeReader.Instance.availablePoints.ToString();
        perkPoints = PerkTreeReader.Instance.availablePoints;
    }

    void Update()
    {
        if (perkPoints != PerkTreeReader.Instance.availablePoints)
        {
            perkText.text = PerkTreeReader.Instance.availablePoints.ToString();
            perkPoints = PerkTreeReader.Instance.availablePoints;
            perkHub.RefreshButtons();
        }
        
    }
}
