using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    private string[] stages = new string[]
    {
        "StageSelect",
        "Level01_GrompIntro",
        "Level02_LincolnIntro",
        "Level03_BarbershopIntro",
        "Level04_PushButton",
        "Level05_JumpOverDoor",
        "Level06_ExtendHatOverSpikes"
    };
    private string[] loadedStagePaths;
    private AssetBundle myLoadedAssetBundle;
    private GameObject objToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        // myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/Scenes");
        // loadedStagePaths = myLoadedAssetBundle.GetAllScenePaths();
        // Debug.Log(loadedStagePaths);
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Assests/Scenes/" + stages[0]);
        // UnityEngine.SceneManagement.SceneManager.LoadScene(stages[1]);
        // UnityEngine.SceneManagement.SceneManager.LoadScene(loadedStagePaths[0]);

        // var button = Instantiate(RoomButton, Vector3.zero, Quaternion.identity) as Button;

        objToSpawn = new GameObject("Cool GameObject made from Code");
        objToSpawn.AddComponent<Button>();
        // objToSpawn.AddComponent<Image>();
        // objToSpawn.AddComponent<SpriteRenderer>();
        // objToSpawn.transform.parent = panel;
        objToSpawn.AddComponent<RectTransform>();
        objToSpawn.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        objToSpawn.GetComponent<RectTransform>().position = Vector2.zero;
        // objToSpawn.GetComponent<Image>().color = Color.red;

        // objToSpawn.GetComponent<Button>().colors.normalColor = Color.red;
        ColorBlock colorBlock = objToSpawn.GetComponent<Button>().colors;
        colorBlock.normalColor = new Color(0.0f, 0.5f, 0.0f, 1.0f);
        objToSpawn.GetComponent<Button>().colors = colorBlock;

        objToSpawn.GetComponent<Button>().onClick.AddListener(delegate{SwitchScene(stages[0]);});

    }

    private void SwitchScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
