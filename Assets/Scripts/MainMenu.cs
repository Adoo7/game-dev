using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
   public void playGame()
   {
       Application.LoadLevel(1);  //loads the first level NEEDS TO BE CHANGED
   }

   public void instructions()
   {
       Application.LoadLevel(1);  //loads the instructions screen
   }

    public void credits()
    {
        Application.LoadLevel(1);  //loads the credits screen
    }

    public void back()
    {
        Application.LoadLevel(0);  //loads the main menu
    }


   public void quitGame()
   {
       Debug.Log("QUIT!");
       Application.Quit();
   }
}
