using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class options : MonoBehaviour
{
   public void StartGame(){
    SceneManager.LoadScene("inGame");
   }

   public void QuitGame(){
    Application.Quit();
   }
}
