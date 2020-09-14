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
    private GameObject button;

    // Start is called before the first frame update
    void Start()
    {
        var canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        GameObject buttonPrefab = Resources.Load<GameObject>("Button");

        bool skipFirst = true;
        int counter = 0;
        foreach (string sceneName in stages)
        {
            if (skipFirst)
            {
                skipFirst = false;
                continue;
            }

            button = Instantiate(buttonPrefab) as GameObject;
            GameObject textChild = button.transform.GetChild(0).gameObject;
            button.transform.position = new Vector2(0, 120 - (counter * 30));
            button.transform.SetParent(canvas.transform, false);
            textChild.GetComponent<Text>().text = sceneName;
            button.GetComponent<Button>().onClick.AddListener(delegate{SwitchScene(sceneName);});

            counter++;
        }
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
