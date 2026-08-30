using TMPro;
using UnityEngine;

public class AmmeterText : MonoBehaviour
{
    [SerializeField] TextMeshPro AmmeterValue;

    public void UpdateAmmeterValue(double currentInAmps)
    {
        double absI = System.Math.Abs(currentInAmps);
        string display;

        if (absI >= 1.0)
        {
            display = string.Format("{0:0.###} A", currentInAmps);
        }
        else if (absI >= 1e-3)
        {
            display = string.Format("{0:0.###} mA", currentInAmps * 1e3);
        }
        else
        {
            display = string.Format("{0:0.###} µA", currentInAmps * 1e6);
        }

        AmmeterValue.text = display;
    }

    public void InitAmmeterValue()
    {
        AmmeterValue = GetComponent<TextMeshPro>();
    }
}
