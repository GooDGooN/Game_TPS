using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatus : MonoBehaviour
{
    private Color healthColor = new Color(50.0f, 200.0f, 100.0f) / 255.0f;
    private Color staminaColor = new Color(255.0f, 150.0f, 0.0f) / 255.0f;
    private Color lowStatusColor = new Color(255.0f, 0.0f, 70.0f) / 255.0f;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text magazineText;

    private void LateUpdate()
    {
        healthColor.a = staminaColor.a = lowStatusColor.a = 1.0f;
        var playerScript = GlobalVarStorage.PlayerScript;
        healthBar.color = Color.Lerp(lowStatusColor, healthColor, ((float)playerScript.Health / playerScript.MaxHealth));
        
        healthBar.fillAmount = (float)playerScript.Health / playerScript.MaxHealth;
        healthText.text = $"{playerScript.Health}/{playerScript.MaxHealth}";

        staminaBar.color = playerScript.IsStaminaRecharge ? lowStatusColor : staminaColor;
        staminaBar.fillAmount = playerScript.Stamina / playerScript.MaxStamina;

        magazineText.text = playerScript.PlayerRifle.CurrentMagazineCapacity.ToString();
    }
}

