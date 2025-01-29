# 3D-Snake Game

![3D-Snake](https://user-images.githubusercontent.com/62818241/201542473-2ff1fb5e-aa0a-424b-8c9c-51aa46c799ce.png)

## 📌 Introduction
**3D-Snake** is an exciting 3D version of the classic Snake game. Players control a snake that grows longer as it eats fruit while avoiding walls, bombs, and its own tail. The goal is to achieve the highest score possible without colliding with obstacles.

## 🔥 Features
- 🐍 **Snake Movement**: The snake moves in 4 possible directions (left, right, up, down).
- 🍏 **Fruit Pickup**: Eating fruit causes the snake to grow longer and increases the score.
- 💣 **Bombs**: Bombs appear randomly, and if the snake collides with one, it results in game over.
- 🧱 **Walls**: Walls are obstacles that can end the game if the snake crashes into them.
- 🐍 **Tail Growth**: Each fruit consumed causes the snake to grow by adding a new node to its tail.
- 🏆 **Score System**: The score increases as the snake eats fruit, and the score is displayed on the screen.
- 🔊 **Audio Feedback**: Sound effects play when picking up fruit or dying.

---

## 🏗️ How It Works

### 📌 **AudioManager**

Handles the audio effects for picking up fruit and for when the player dies.

```csharp
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip pickUp_Sound, dead_Sound;
    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Play_pickUp_Sound()
    {
        AudioSource.PlayClipAtPoint(pickUp_Sound, transform.position);
    }

    public void Play_dead_Sound()
    {
        AudioSource.PlayClipAtPoint(dead_Sound, transform.position);
    }
}
```

### 📌 **DeactivePickup**

Controls the deactivation of fruit and bomb pickups after a random time.

```csharp
public class DeactivePickup : MonoBehaviour
{
    void Start()
    {
        Invoke("Deactive", Random.Range(3f, 6f));
    }

    void Deactive()
    {
        gameObject.SetActive(false);
    }
}
```

### 📌 **GameOver**

Handles the game over logic by waiting for a few seconds before returning to the main menu.

```csharp
public class GameOver : MonoBehaviour
{
    private float x; 
    void Update()
    {
        x += Time.deltaTime;

        if(x > 3f)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
```

### 📌 **GameplayController**

Spawns fruits and bombs at random positions and manages the game score.

```csharp
public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    public GameObject fruit_PickUp, bomb_PickUp;

    private float min_X = -4.25f, max_X = 4.25f, min_Y = -2.26f, max_Y = 2.26f;
    private float Z_pos = 0.73f;

    private Text score_Text;
    private int scoreCount;

    void Start()
    {
        score_Text = GameObject.Find("Score").GetComponent<Text>();
        Invoke("StartSpawning", 0.5f);
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnPickUps());
    }

    IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));

        if(Random.Range(0, 10) >= 2)
        {
            Instantiate(fruit_PickUp, new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), Z_pos), Quaternion.identity);
        }
        else
        {
            Instantiate(bomb_PickUp, new Vector3(Random.Range(min_X, max_X), Random.Range(min_Y, max_Y), Z_pos), Quaternion.identity);
        }
        Invoke("StartSpawning", 0.5f);
    }

    public void IncreaseScore()
    {
        scoreCount++;
        score_Text.text = "Score: " + scoreCount;
    }
}
```

### 📌 **MainMenu**

Allows the player to start the game or quit the application.

```csharp
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
```

### 📌 **PlayerController**

Handles the movement of the snake and its interaction with pickups, walls, bombs, and its tail.

```csharp
public class PlayerController : MonoBehaviour
{
    public float step_Length = 0.2f;
    public float movement_Frequency = 0.1f;

    private List<Vector3> delta_Position;
    private List<Rigidbody> nodes;
    private Rigidbody main_Body;
    private Rigidbody head_Body;
    private Transform tr;

    private float counter;
    private bool move;
    private bool create_Node_At_Tail;

    void Awake()
    {
        tr = transform;
        main_Body = GetComponent<Rigidbody>();
        InitSnakeNodes();
        InitPlayer();
        delta_Position = new List<Vector3>() { new Vector3(-step_Length, 0f), new Vector3(0f, step_Length), new Vector3(step_Length, 0), new Vector3(0f, -step_Length) };
    }

    void FixedUpdate() 
    {
        if(move)
        {
            move = false;
            Move();
        }
    }

    void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());
        head_Body = nodes[0];
    }

    void Move()
    {
        Vector3 dposition = delta_Position[(int)direction];
        Vector3 parentPos = head_Body.position;
        Vector3 prevPosition;

        main_Body.position = main_Body.position + dposition;
        head_Body.position = head_Body.position + dposition;

        for(int i=1; i<nodes.Count; i++)
        {
            prevPosition = nodes[i].position;
            nodes[i].position = parentPos;
            parentPos = prevPosition;
        }

        if(create_Node_At_Tail)
        {
            create_Node_At_Tail = false;
            GameObject newNode = Instantiate(tailPrefab, nodes[nodes.Count - 1].position, Quaternion.identity);
            newNode.tag = "Tail";
            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());
        }
    }

    void CheckMovementFrequency()
    {
        counter += Time.deltaTime;
        if(counter >= movement_Frequency)
        {
            counter = 0;
            move = true;
        }
    }

    public void SetInputDirection(PlayerDirection dir)
    {
        if(dir == PlayerDirection.UP && direction == PlayerDirection.DOWN ||
           dir == PlayerDirection.DOWN && direction == PlayerDirection.UP ||
           dir == PlayerDirection.RIGHT && direction == PlayerDirection.LEFT ||
           dir == PlayerDirection.LEFT && direction == PlayerDirection.RIGHT)
        {
            return;
        }
        direction = dir;
        ForceMove();
    }

    void ForceMove()
    {
        counter = 0;
        move = false;
        Move(); 
    }

    void OnTriggerEnter(Collider target)
    {
        if(target.tag == Tags.FRUIT)
        {
            target.gameObject.SetActive(false);
            create_Node_At_Tail = true;
            GameplayController.instance.IncreaseScore();
            AudioManager.instance.Play_pickUp_Sound();
        }

        if(target.tag == Tags.WALL || target.tag == Tags.BOMB || target.tag == Tags.TAIL )
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("GameOver");
            AudioManager.instance.Play_dead_Sound();
        }
    }
}
```

---

## 🎯 Conclusion

**3D-Snake** offers a fun and challenging experience, adding depth to the classic snake game by incorporating obstacles, pickups, and a scoring system. With a responsive control system, dynamic gameplay, and an engaging scoring model, it's a great starting point for developing 3D snake-based games. 🚀
