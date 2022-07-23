using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameOver;
    public bool roomHasDoor;
    public List<Enemy> enemies = new List<Enemy>();

    private PlayerController player;
    private Door exitDoor;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        //player= FindObjectOfType<PlayerController>();
        //exitDoor= FindObjectOfType<Door>();
    }

    public void Update()
    {
        if (player != null)
            gameOver = player.isDead;
        if (enemies.Count == 0 && roomHasDoor)
        {
            exitDoor.OpendDoor();
        }

        UIManager.instance.GameOverUI(gameOver);
    }

    public void IsEnemy(Enemy enemy) //�������� (���ģʽ���۲���ģʽ)
    {
        enemies.Add(enemy);
    }

    public void EnemyIsDead(Enemy enemy) //������������
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0&&roomHasDoor)
        {
            exitDoor.OpendDoor();
        }
    }

    public void IsPlayer(PlayerController controller) //�������    
    {
        player = controller;
    }

    public void IsExitDoor(Door door)   //������
    {
        exitDoor = door;
        roomHasDoor = true;
    }

    public float LoadHealth() //����Ѫ������
    {
        if (!PlayerPrefs.HasKey("playerHealth"))
        {
            PlayerPrefs.SetFloat("playerHealth", player.HP);
        }
        float currentHealth = PlayerPrefs.GetFloat("playerHealth");
        return currentHealth;
    }

    public void SaveData()//�洢 Ѫ�� �� ���� ����
    {
        PlayerPrefs.SetFloat("playerHealth", player.HP);
        PlayerPrefs.SetInt("archiveScene", SceneManager.GetActiveScene().buildIndex + 1);       
        PlayerPrefs.Save();
        Debug.Log("����Ѫ���볡��");
    }

    public void RestartScene()  //���¼��س���
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        PlayerPrefs.DeleteKey("playerHealth");
    }

    public void NextScene() //�����¸�����
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()   //����Ϸ
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.DeleteAll();
        Debug.Log("��������Ϸ");
    }

    public void ContinueGame() //������Ϸ
    {
        if (PlayerPrefs.HasKey("archiveScene"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("archiveScene"));
            Debug.Log("������ǰ�浵");
        }
        else
        {
            Debug.Log("�޴浵����������Ϸ");
            NewGame();
        }
        if(Time.timeScale==0)
            Time.timeScale=1;
    }

    public void GoToMainMenu()//ǰ�����˵�
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()  //�˳���Ϸ
    { 
        Application.Quit();
    }
}
