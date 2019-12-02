using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //biblioteka odpowiada za sterowanie scenami

public class MenuController : MonoBehaviour
{
    public void startClick()    //gdy przycisk start wciśnięty uruchom scene (poziom 1)
    {
        SceneManager.LoadScene(1);
    }

    public void quitClick() //gdy przycisk quit wciśniety wyłącz program
    {
        Debug.Log("quit");  //wypisuje w konsoli tekst, można jako argument dać zmienną, wtedy wypisze wartość jej lub połączyć z napisem: "napis" + nazwaZmiennej
        Application.Quit(); //wyłącza program
    }
}
