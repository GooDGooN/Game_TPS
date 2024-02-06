using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatus : MonoBehaviour
{
    private const float divideValue = 255.0f;
    private Color healthColor = new Color(50.0f / divideValue, 200.0f / divideValue, 100.0f / divideValue);
    private Color staminaColor = new Color(255.0f / divideValue, 150.0f / divideValue, 0.0f / divideValue);
    private Color lowStatusColor = new Color(255.0f / divideValue, 0.0f, 70.0f / divideValue);

    private Color magazineColor = new Color(50.0f / divideValue, 200.0f / divideValue, 255.0f / divideValue);

    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text magazineText;

    [SerializeField] private Image magazineIcon;

    private void LateUpdate()
    {
        var player = PlayerControl.Instance;
        healthBar.color = Color.Lerp(lowStatusColor, healthColor, ((float)player.Health / player.MaxHealth));
        
        healthBar.fillAmount = (float)player.Health / player.MaxHealth;
        healthText.text = $"{player.Health}/{player.MaxHealth}";

        staminaBar.color = player.IsStaminaRecharge ? lowStatusColor : staminaColor;
        staminaBar.fillAmount = player.Stamina / player.MaxStamina;

        magazineText.text = player.PlayerRifle.CurrentMagazineCapacity.ToString();
        magazineText.color = magazineIcon.color = (player.PlayerRifle.CurrentMagazineCapacity <= 0) ? lowStatusColor : magazineColor;
    }
}

