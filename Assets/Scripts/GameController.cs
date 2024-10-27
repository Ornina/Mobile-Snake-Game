using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public float snakeSpeed = 1;

    const float WIDTH = 3.4f;
    const float HEIGHT = 7f;

    public BodyPart bodyPrefab = null;
    public Sprite tailSprite = null;
    public Sprite bodySprite = null;

    public SnakeHead snakeHead = null;
    public GameObject rockPrefab = null;
    public GameObject eggPrefab = null;
    public GameObject goldEggPrefab = null;
    public GameObject spikePrefab = null;

    public bool alive = true;
    public bool waitingToPlay = true;

    int level = 0;
    int noOfSpikes = 0;  
    int noOfEggsForNextLevel = 0;
    public int score = 0;
    public int hiScore = 0;

    public Text scoreText = null;
    public Text hiScoreText = null;
    public Text gameOverText = null;
    public Text tapToPlayText = null;
    public Text MadeByText = null;

    List<Egg> eggs = new List<Egg>();
    List<Spike> spikes = new List<Spike>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        MadeByText.gameObject.SetActive(true);
        Debug.Log("Starting SnakeGame !!!!!!!");
        CreateWalls();
        alive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingToPlay)
        {
            foreach(Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    StartGamePlay();
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                StartGamePlay();
            }
        }
    }

    void StartGamePlay()
    {
        score = 0;
        level = 0;
        scoreText.text = "Score = 0" + score;
        hiScoreText.text = "HiScore = 0" + hiScore;

        tapToPlayText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        MadeByText.gameObject.SetActive(false);

        waitingToPlay = false;
        alive = true;

        KillOldEggs();
        Kill”ldSpikes();

        LevelUp();
    }

    void LevelUp()
    {
        level++;
        noOfSpikes = level;

        noOfEggsForNextLevel = 4 + (level * 2);

        snakeSpeed = 1f + (level / 4);
        if (snakeSpeed > 6)
            snakeSpeed = 6;
        snakeHead.ResetSnake();
        CreateEgg();
       
        CreateSpike();
 
    }


    public void GameOver()
    {
        alive = false;
        waitingToPlay = true;

        gameOverText.gameObject.SetActive(true);
        tapToPlayText.gameObject.SetActive(true);
    }

    public void EggEaten(Egg egg)
    {
        score++;
        noOfEggsForNextLevel--;

        if (noOfEggsForNextLevel == 0)
        {
            score += 10;
            LevelUp();
        }

       
        else if (noOfEggsForNextLevel == 1) //last egg
            CreateEgg(true);
        else
            CreateEgg(false);

        if (score > hiScore)
        {

            hiScore = score;

            hiScoreText.text = "HiScore = 0" + hiScore;
        }

        scoreText.text = "Score = 0" + score;

        eggs.Remove(egg);
        Destroy(egg.gameObject);


    }

    void CreateWalls()
    {
        float z = -1f;

        Vector3 start = new Vector3(-WIDTH, -HEIGHT, z);
        Vector3 finish = new Vector3(-WIDTH, +HEIGHT, z);
        CreateWall(start, finish);

        start = new Vector3(+WIDTH, -HEIGHT, z);
        finish = new Vector3(+WIDTH, +HEIGHT, z);
        CreateWall(start, finish);

        start = new Vector3(-WIDTH, -HEIGHT, z);
        finish = new Vector3(+WIDTH, -HEIGHT, z);
        CreateWall(start, finish);

        start = new Vector3(-WIDTH, +HEIGHT, z);
        finish = new Vector3(+WIDTH, +HEIGHT, z);
        CreateWall(start, finish);
    }

    void CreateWall(Vector3 start, Vector3 finish)
    {
        float distance = Vector3.Distance(start, finish);
        int noOfRocks = (int)(distance * 3f);
        Vector3 delta = (finish - start) / noOfRocks;

        Vector3 position = start;
        for (int rock = 0; rock <= noOfRocks;  rock++)
        {
            float rotation = Random.Range(0, 360f);
            float scale = Random.Range(1.5f, 2f);
            CreateRock(position, scale, rotation);
            position = position + delta;
        }
    }

    void CreateRock(Vector3 position, float scale, float rotation)
    {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0, 0, rotation));
        rock.transform.localScale = new Vector3(scale, scale, 1);

    }

    void CreateEgg(bool golden = false)
    {
        Vector3 position;
        position.x = -WIDTH + Random.Range(1f, (WIDTH * 2) - 2f);
        position.y = -HEIGHT + Random.Range(1f, (HEIGHT * 2) - 2f);
        position.z = -1f;
        Egg egg = null;
        if (golden)
           egg = Instantiate(goldEggPrefab, position, Quaternion.identity).GetComponent<Egg>();
        else
           egg = Instantiate(eggPrefab, position, Quaternion.identity).GetComponent<Egg>();

        eggs.Add(egg);
    }

    void CreateSpike()
    {
        Vector3 position2;
        position2.x = -WIDTH + Random.Range(1f, (WIDTH * 2) - 2f);
        position2.y = -HEIGHT + Random.Range(1f, (HEIGHT * 2) - 2f);
        position2.z = -1f;
        
        Spike spike = Instantiate(spikePrefab, position2, Quaternion.identity).GetComponent<Spike>();
        spikes.Add(spike);
    }

    void KillOldEggs()
    {
        foreach(Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }

    void Kill”ldSpikes()
    {
        foreach (Spike spike in spikes)
        {
            Destroy(spike.gameObject);
        }
        spikes.Clear();
    }
}
