using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class HelperMethods
{
    /*public static void doCharacterAction(GameObject gameObject, CharacterTypes characterType)
    {
        Vector2 scale = gameObject.transform.localScale;
        switch (characterType)
        {
            case CharacterTypes.Luigi:
                scale.x = scale.x * 0.5f;
                scale.y = scale.y * 0.5f;
                break;
            case CharacterTypes.Barbershop:
                scale.x = scale.x * 1.5f;
                scale.y = scale.y * 1.5f;
                break;
            default:

                break;
        }
        gameObject.transform.localScale = scale;
    }*/
}

public class main : MonoBehaviour
{
    private GameObject currCharacter;
    private GameObject selectedPrefabCharacter;
    private GameObject toSpawnChar;
    private CharacterLife currCharacterLife;

    public Texture2D lincolnIcon;
    public Texture2D grumpyIcon;
    public Texture2D barbershopIcon;

    public int nLincoln = 99;
    public int nGrumpy = 99;
    public int nBarbershop = 99;

    int nLincolnCurr = 0;
    int nGrumpyCurr = 0;
    int nBarbershopCurr = 0;

    private GameObject luigiPrefab, lincolnPrefab, grumpyPrefab, barbershopPrefab;

    private List<CharacterLife> characterLives;

    private LevelEnd levelEnd;
    private GameObject spawnPoint;
    private Vector2 spawnVector;
    private List<KeyDoorController> keyDoors;

    float horizontal = 0;
    List<KeyInputType> keysPressed = new List<KeyInputType>();

    void Start()
    {
        spawnVector = new Vector2(0, 0);
        spawnPoint = GameObject.Find("SpawnPoint");
        if (spawnPoint)
        {
            spawnVector = spawnPoint.transform.position;
            // making spawn point box invisible
            spawnPoint.GetComponent<SpriteRenderer>().enabled = false;
        }
        levelEnd = GameObject.FindObjectOfType<LevelEnd>();

        characterLives = new List<CharacterLife>();
        luigiPrefab = Resources.Load<GameObject>("very_important_asset");
        lincolnPrefab = Resources.Load<GameObject>("LincolnPrefab");
        grumpyPrefab = Resources.Load<GameObject>("GrumpyPrefab");
        barbershopPrefab = Resources.Load<GameObject>("BarbershopPrefab");
        // Debug.Log("truly what");

        keyDoors = new List<KeyDoorController>();
        var keyDoorControllers = GameObject.FindObjectsOfType<KeyDoorController>();
        foreach(var keyDoor in keyDoorControllers)
        {
            keyDoors.Add(keyDoor);
        }

        selectedPrefabCharacter = lincolnPrefab;
    }

    private IEnumerator StartEndStage()
    {
        yield return new WaitForSeconds(1);
        if (PlayerPrefs.GetInt("lastLevel") == 1)
        {
            PlayerPrefs.SetInt("levelsPassed", 1 + PlayerPrefs.GetInt("levelsPassed"));
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelect");

    }

    void Update() 
    {
        bool toSpawn = false;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelect");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            toSpawnChar = selectedPrefabCharacter;
            toSpawn = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            toSpawnChar = lincolnPrefab;
            toSpawn = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toSpawnChar = grumpyPrefab;
            toSpawn = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            toSpawnChar = barbershopPrefab;
            toSpawn = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            toSpawnChar = luigiPrefab;
            toSpawn = true;
        }

        bool levelWon = levelEnd.GoalIsMet();

        if (levelWon)
        {
            StartCoroutine(StartEndStage());
        }
        else if (toSpawn && !levelWon && canAddNextCharacter())
        {
            if (currCharacter != null && currCharacterLife != null)
            {
                characterLives.Add(currCharacterLife);
            }
            foreach (CharacterLife life in characterLives)
            {
                life.ResetToSpawn(spawnVector);
            }

            ResetComponents();

            currCharacter = Instantiate(toSpawnChar);
            var controllerScript = currCharacter.GetComponent<CharacterController>();
            currCharacter.transform.position = spawnVector;
            
            currCharacterLife = new CharacterLife(currCharacter);
            controllerScript.mainRef = this;
            controllerScript.characterLife = currCharacterLife;
        }
        else if (Input.GetKey(KeyCode.R))
        {
            foreach (CharacterLife life in characterLives)
            {
                Destroy(life.unityObject);
            }   
            characterLives.Clear();
            if (currCharacter)
            {
                Destroy(currCharacter);
            }
            currCharacter = null;
            currCharacterLife = null;

            nBarbershopCurr = 0;
            nGrumpyCurr = 0;
            nLincolnCurr = 0;

            levelEnd.ResetProgress();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (currCharacter != null && currCharacterLife != null)
            {
                characterLives.Add(currCharacterLife);
            }
            foreach (CharacterLife life in characterLives)
            {
                life.ResetToSpawn(spawnVector);
            }
            currCharacter = null;
            currCharacterLife = null;
        }
        else if (currCharacter)
        {
            keysPressed = new List<KeyInputType>();
            if (Input.GetKey(KeyCode.Space))
            {
                keysPressed.Add(KeyInputType.Jump);
            }
            if (Input.GetKey("left"))
            {
                keysPressed.Add(KeyInputType.Left);
            }
            if (Input.GetKey("right"))
            {
                keysPressed.Add(KeyInputType.Right);
            }
            if (Input.GetKey(KeyCode.E))
            {
                keysPressed.Add(KeyInputType.Action);
            }
            horizontal = Input.GetAxis("Horizontal");
        }
    }

    void FixedUpdate()
    {
        if (currCharacter)
        {
            var controllerScript = currCharacter.GetComponent<CharacterController>();
            controllerScript.takeActions(keysPressed, horizontal);
            currCharacterLife.TrackInput(Time.fixedDeltaTime, keysPressed, horizontal);
        }
        foreach (CharacterLife life in characterLives)
        {
            life.UpdateFromHistory(Time.fixedDeltaTime);
        }
    }

    void OnGUI()
    {
        GUIContent lincolnContent = new GUIContent();
        lincolnContent.image = lincolnIcon;
        lincolnContent.text = nLincoln.ToString();
        if (GUI.Button(new Rect(10, 10, 100, 50), lincolnContent))
        {
            selectedPrefabCharacter = lincolnPrefab;
            print("EMANCIPATION TIME");
        }

        GUIContent grumpyContent = new GUIContent();
        grumpyContent.image = grumpyIcon;
        grumpyContent.text = nGrumpy.ToString();
        if (GUI.Button(new Rect(10, 70, 100, 50), grumpyContent))
        {
            selectedPrefabCharacter = grumpyPrefab;
            print("Feelin' Grompy");
        }

        GUIContent barbershopContent = new GUIContent();
        barbershopContent.image = barbershopIcon;
        barbershopContent.text = nBarbershop.ToString();
        if (GUI.Button(new Rect(10, 130, 100, 50), barbershopContent))
        {
            selectedPrefabCharacter = barbershopPrefab;
            print("Doo wop doo wah");
        }
    }

    void ResetComponents()
    {
        if (levelEnd != null)
        {
            levelEnd.ResetProgress();
        }
        foreach (var keyDoor in keyDoors)
        {
            keyDoor.ResetKeyDoor();
        }
    }

    void LevelWin()
    {
        Debug.Log("you won");
    }

    bool canAddNextCharacter()
    {
        bool canAdd = false;
        // Enforce character type limits
        var toSpawnType = toSpawnChar.GetComponent<CharacterController>().characterType;
        switch (toSpawnType)
        {
            case CharacterTypes.Barbershop:
                if(nBarbershop > nBarbershopCurr)
                {
                    canAdd = true;
                    nBarbershopCurr += 1;
                }
                break;
            case CharacterTypes.Grompy:
                if (nGrumpy > nGrumpyCurr)
                {
                    canAdd = true;
                    nGrumpyCurr += 1;
                }
                break;
            case CharacterTypes.Lincoln:
                if (nLincoln > nLincolnCurr)
                {
                    canAdd = true;
                    nLincolnCurr += 1;
                }
                break;

            default:
                break;
        }

        return canAdd;
    }
}



public enum KeyInputType
{
  Left,
  Right,
  Jump,
  Action,
  None
}

public class CharacterLife
{
    
    public List<Tuple<float, List<KeyInputType>, float>> history;
    public List<KeyInputType> keysPressedEmpty;
    private int currentPositionInArray;
    public GameObject unityObject;
    Vector3 initTransformPosition;
    Vector3 initTransformScale;

    public CharacterLife(GameObject obj)
    {
        history = new List<Tuple<float, List<KeyInputType>, float>>();
        keysPressedEmpty = new List<KeyInputType>();
        currentPositionInArray = 0;
        unityObject = obj;
        initTransformPosition = unityObject.transform.position;
        initTransformScale = unityObject.transform.localScale;
    }

    public void TrackInput(float timeDelta, List<KeyInputType> keysPressed, float horizontal)
    {
        history.Add(new Tuple<float, List<KeyInputType>, float>(timeDelta, keysPressed, horizontal));
    }

    public void UpdateFromHistory(float timeDelta)
    {
        var controllerScript = unityObject.GetComponent<CharacterController>();
        if (currentPositionInArray < history.Count)
        {
            controllerScript.takeActions(history[currentPositionInArray].Item2, history[currentPositionInArray].Item3);
            currentPositionInArray++;
        }
        else
        {
            controllerScript.takeActions(keysPressedEmpty, 0f);
        }
    }

    // public void DestroyStuff()
    // {
        // Destroy(unityObject);
        // history.Clear();
    // }

    public void ResetToSpawn(Vector2 spawnVector)
    {
        unityObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        unityObject.transform.position = spawnVector;
        unityObject.transform.localScale = initTransformScale;
        var controllerScript = unityObject.GetComponent<CharacterController>();
        controllerScript.Resurrect();

        currentPositionInArray = 0;
        
    }
}