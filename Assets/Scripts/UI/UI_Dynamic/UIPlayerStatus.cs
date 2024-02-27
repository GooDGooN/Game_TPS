using CharacterNamespace;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    private int lastKill = 0;
    [SerializeField] private TMP_Text killCount;
    [SerializeField] private TMP_Text remainEnemyCount;
    [SerializeField] private TMP_Text playTime;
    private StringBuilder playTimeString = new StringBuilder();

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

        if(lastKill < GameManager.KillCount)
        {
            lastKill = GameManager.KillCount;
            KillIncreaseEffect();
        }
        killCount.text = lastKill.ToString();

        remainEnemyCount.text = EnemySpawner.Instance.CurrentEnemyCount().ToString();

        //playTime.text = $"{(int)GameManager.SurviveTime / 60}:{(int)GameManager.SurviveTime % 60}";
        //playTime.text = string.Format($"{0:D2}", (int)GameManager.SurviveTime / 60);
        /*playTimeString.AppendFormat($"{0:D2}", GameManager.SurviveTime / 60);
        playTimeString.Append(":");
        playTimeString.AppendFormat($"{0:D2}", GameManager.SurviveTime + 60 % 60);*/

        var min = GameManager.SurviveTime / 60;
        var sec = (GameManager.SurviveTime + 60) % 60;
        //playTime.text = $"{ string.Format($"{0:D2}", min) }:{ string.Format($"{0:D2}", sec) }";
        playTime.text = $"{min:D2}:{sec:D2}";
        playTimeString.Clear();
    }

    public void KillIncreaseEffect()
    {
        killCount.gameObject.transform.localScale = Vector3.one * 1.5f;
        killCount.gameObject.transform.DOScale(Vector3.one, 0.5f);
    }
}

