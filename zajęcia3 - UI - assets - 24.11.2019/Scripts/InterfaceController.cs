using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //trzezba dodać tą biblioteke, by korzystać z elementów UI

public class InterfaceController : MonoBehaviour
{
    [SerializeField]
    private Canvas[] uiCanvas;  //tablica canvasów

    private int currentCanvasIndex = 0; //indeks aktywnego canvasa

    private void Start()    //resetowanie canvasów
    {
        uiCanvas[0].enabled = true; //canvas 0 jest aktywny
        for(int i = 1; i < uiCanvas.Length; i++)    //pozostałe canvasy nieaktywne
        {
            uiCanvas[i].enabled = false;
        }
    }

    public void ChangeCanvas(int canvasIndex)   //funkcja przełączająca canvasy
    {
        uiCanvas[currentCanvasIndex].enabled = false;   //wyłącz aktywny canvas
        currentCanvasIndex = canvasIndex;   //ustawa indeks aktywnego canvasu
        uiCanvas[currentCanvasIndex].enabled = true;    //ustaw nowy canvas jako aktywny
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //gdy Escape wciśnięty wróć do canvas 0, w tym przypadku Menu
        {
            ChangeCanvas(0);
        }
    }
}
