using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSaveData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelect");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
