using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyProperty MyTarget;
    private RectTransform myRectTransform;
    [SerializeField] private Image healthBar;


    private void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
    }

    private void Awake()
    {
        healthBar.color = Color.clear;
    }

    private void LateUpdate()
    {
        healthBar.fillAmount = MyTarget.Health / (float)MyTarget.MaxHealth;
        if (MyTarget.isActiveAndEnabled)
        {
            transform.position = Camera.main.WorldToScreenPoint(MyTarget.transform.position);
            if(myRectTransform.position.z < 0.0f)
            {
                healthBar.color = Color.clear;  
                GetComponent<Image>().color = Color.clear;
            }
            else
            {
                healthBar.color = Color.white;
                GetComponent<Image>().color = Color.white;
            }
            if(MyTarget.Health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
