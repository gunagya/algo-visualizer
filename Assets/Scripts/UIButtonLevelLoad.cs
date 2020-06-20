using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIButtonLevelLoad : MonoBehaviour {
	
	public string LevelToLoad;
	
	public void loadLevel() {
        //Load the level from LevelToLoad
        Debug.Log("Going back to main menu");
		SceneManager.LoadScene(LevelToLoad);
	}
}
