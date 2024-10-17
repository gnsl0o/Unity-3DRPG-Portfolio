using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager Instance;

    public Slider healthBarPrefab;
    private Queue<Slider> healthBarPool = new Queue<Slider>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Slider GetHealthBar()
    {
        if(healthBarPool.Count > 0)
        {
            return healthBarPool.Dequeue();
        }
        else
        {
            Slider newHealthBar = Instantiate(healthBarPrefab, transform); // ���ο� ü�¹� ����
            newHealthBar.gameObject.SetActive(false); // �ʱ� ���¸� ��Ȱ��ȭ�� ����
            return newHealthBar;
        }
    }

    public void ReturnHealthBar(Slider healthBar)
    {
        healthBar.gameObject.SetActive(false);
        healthBarPool.Enqueue(healthBar);
    }
}
