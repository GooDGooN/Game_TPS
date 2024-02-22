using CharacterNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] ItemPrefabs;
    private List<ItemType> randomItem = new List<ItemType>();
    private Coroutine coroutine;

    private void Awake()
    {
        for(int i = 0; i < ItemPrefabs.Length; i++)
        {
            if(i != (int)ItemType.Heal)
            {
                randomItem.Add((ItemType)i);
            }
        }
    }

    private void Update()
    {
        SetLimitList();
        if (GameManager.IsGameStart && coroutine == null)
        {
            coroutine = StartCoroutine(Spawning());
        }

    }

    private void SetLimitList()
    {
        var player = PlayerControl.Instance;
        if (player.AtkSpeed <= 0.1f && randomItem.Contains(ItemType.AttackSpeed))
        {
            randomItem.Remove(ItemType.AttackSpeed);
        }
        if (player.PlayerRifle.MaxImumMagazineCapacity >= 50 && randomItem.Contains(ItemType.Magazine))
        {
            randomItem.Remove(ItemType.Magazine);
        }
        if (player.MoveSpeedMutiplier >= 2.0f && randomItem.Contains(ItemType.MoveSpeed))
        {
            randomItem.Remove(ItemType.MoveSpeed);
        }
        if (player.ReloadMotionSpeedMultiplier >= 2.0f && randomItem.Contains(ItemType.Reload))
        {
            randomItem.Remove(ItemType.Reload);
        }
        if (player.StaminaRechargeMultiplier >= 2.0f && randomItem.Contains(ItemType.Stamina))
        {
            randomItem.Remove(ItemType.Stamina);
        }

        if(player.Health == 100 && randomItem.Contains(ItemType.Heal))
        {
            randomItem.Remove(ItemType.Heal);
        }
        else if (player.Health != 100 && !randomItem.Contains(ItemType.Heal))
        {
            randomItem.Add(ItemType.Heal);
        }
    }

    private IEnumerator Spawning()
    {
        while(PlayerControl.Instance != null) 
        {
            yield return new WaitForSeconds(5.0f);
            
            var randomNum = Random.Range(0, randomItem.Count);
            var randomPos = GameManager.Instance.GetRandomSpawnPosition(true);
            Instantiate(ItemPrefabs[randomNum], randomPos, Quaternion.identity, transform);
        }
    }
}
