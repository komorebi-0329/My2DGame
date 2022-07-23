using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject healthBar;

    [Header("UI Elements")]
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject bossHealthBar;
    public GameObject PlayOverMenu;
    public Slider bossHealthBarSlider;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateHealth(float currentHealth)
    {
        switch (currentHealth)
        {
            case 3:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 2:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 0:
                healthBar.transform.GetChild(0).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }

    public void SetBossHealth(float health) //���BossѪ��  
    {
        bossHealthBarSlider.maxValue = health;      
    }

    public void UpdateBossHealth(float health)  //����bossѪ��
    {
        bossHealthBarSlider.value = health;
    }
    public void CloseBossHealthBar() //�ر�BossѪ��
    {
        bossHealthBar.SetActive(false);
    }

    public void PauseGame() //��ͣ��Ϸ
    { 
        pauseMenu.SetActive(true);
        Time.timeScale = 0; 
    }

    public void ResumeGame()    //�ص���Ϸ
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameOverUI(bool playerDead) //GameOverMenu
    {
        gameOverMenu.SetActive(playerDead);
    }
}
