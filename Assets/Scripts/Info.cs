using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [Header("Info Objects")]
    public Text healthText;

    private int _lastHealth = -1;

    void Update()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        if (Globals.health != _lastHealth)
        {
            _lastHealth = Globals.health;
            healthText.text = string.Join(" ", Enumerable.Repeat("♥", Globals.health));
        }
    }

}
