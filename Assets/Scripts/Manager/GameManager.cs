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

    public void IsEnemy(Enemy enemy) //声明敌人 (设计模式：观察者模式)
    {
        enemies.Add(enemy);
    }

    public void EnemyIsDead(Enemy enemy) //声明敌人死亡
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0&&roomHasDoor)
        {
            exitDoor.OpendDoor();
        }
    }

    public void IsPlayer(PlayerController controller) //声明玩家    
    {
        player = controller;
    }

    public void IsExitDoor(Door door)   //声明门
    {
        exitDoor = door;
        roomHasDoor = true;
    }

    public float LoadHealth() //加载血量数据
    {
        if (!PlayerPrefs.HasKey("playerHealth"))
        {
            PlayerPrefs.SetFloat("playerHealth", player.HP);
        }
        float currentHealth = PlayerPrefs.GetFloat("playerHealth");
        return currentHealth;
    }

    public void SaveData()//存储 血量 与 场景 数据
    {
        PlayerPrefs.SetFloat("playerHealth", player.HP);
        PlayerPrefs.SetInt("archiveScene", SceneManager.GetActiveScene().buildIndex + 1);       
        PlayerPrefs.Save();
        Debug.Log("保存血量与场景");
    }

    public void RestartScene()  //重新加载场景
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        PlayerPrefs.DeleteKey("playerHealth");
    }

    public void NextScene() //加载下个场景
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()   //新游戏
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.DeleteAll();
        Debug.Log("创建新游戏");
    }

    public void ContinueGame() //继续游戏
    {
        if (PlayerPrefs.HasKey("archiveScene"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("archiveScene"));
            Debug.Log("加载先前存档");
        }
        else
        {
            Debug.Log("无存档，创建新游戏");
            NewGame();
        }
        if(Time.timeScale==0)
            Time.timeScale=1;
    }

    public void GoToMainMenu()//前往主菜单
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()  //退出游戏
    { 
        Application.Quit();
    }
}
