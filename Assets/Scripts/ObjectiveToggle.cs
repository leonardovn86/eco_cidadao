using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveToggle : MonoBehaviour
{
    public Toggle toggle;
    public TextMeshProUGUI label;

    void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleChanged);
        UpdateLabelStyle(toggle.isOn);
    }

    void OnToggleChanged(bool isChecked)
    {
        UpdateLabelStyle(isChecked);
    }

    void UpdateLabelStyle(bool isChecked)
    {
        if (isChecked)
        {
            label.text = "<s>" + label.text.Replace("<s>", "").Replace("</s>", "") + "</s>";
        }
        else
        {
            label.text = label.text.Replace("<s>", "").Replace("</s>", "");
        }
    }
}
