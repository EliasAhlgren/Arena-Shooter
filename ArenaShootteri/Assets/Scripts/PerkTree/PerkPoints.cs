using UnityEngine;
using UnityEngine.UI;

public class PerkPoints : MonoBehaviour
{

    private Text perkText;

    void Start()
    {
        perkText = GetComponent<Text>();
    }

    void Update()
    {
        perkText.text = PerkTreeReader.Instance.availablePoints.ToString();
    }
}
