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

    Dictionary<string, string> stage_map = new Dictionary<string, string>()
    {
        { "What is this?", "Level01_GrompIntro" },
        { "Nice hat sir", "Level02_LincolnIntro" },
        { "Ow!", "Level03_BarbershopIntro" },
        { "Yeah, yeah, I get it", "Level04_PushButton" },
        { "I can't get through!", "Level05_JumpOverDoor" },
        { "Hmm...", "Level06_ExtendHatOverSpikes" },
        { "Ow 2!", "Level07_HatHook" },
        { "The Full Quartet", "Level08_TheFullQuartet" }
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
        foreach (KeyValuePair<string, string> sceneTuple in stage_map)
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
            textChild.GetComponent<Text>().text = sceneTuple.Key;
            button.GetComponent<Button>().onClick.AddListener(delegate{SwitchScene(sceneTuple.Value);});

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
