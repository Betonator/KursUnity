using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Dropdown qualityDropdown;    //zmienna ustawień jakoście

    [SerializeField]
    private Dropdown resolutionDropdown;    //zmienna ustawień rozdzielczości

    private Resolution[] resolutions;   //tablica wszystkich rozdzielczości

    private void Start()
    {
        //Cursor.visible = true;    w ten sposób pokazujemy kursor, by schować true -> false
        //Time.timeScale = 0;   w ten sposób zatrzymujemy gre, by włączyć 0 -> 1. Jeśli chcemy spowolnić to poprostu zmniejszyć wartość

        qualityDropdown.value = QualitySettings.GetQualityLevel();  //ustawia wartość dropdowna na tą która jest w programie aktualna, by nie było rozbieżnosci
        qualityDropdown.RefreshShownValue();    //aktualizuje zmieny

        resolutions = Screen.resolutions;   //pobieranie do tablicy wszystkich dostępnych rozdzielczości (trzeba tak, bo każdy komputer może mieć inne dostępne)

        resolutionDropdown.ClearOptions();  //czyszczenie aktualnych opcji w dropdownie

        List<string> options = new List<string>();  //tworzenie listy możliwych rozdzielczości. Trzeba tak zrobić, bo domyślnie są one przechowywane jako liczby i tutaj formatujemy
        int currentResolutionIndex = 0; //to się przyda do ustawienia w dropdown aktualnej rozdielczości tak jak w qualityDropdown
        for(int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + "x" + resolutions[i].height; //tworzenie rodzielczości jako napis, np. 1920x1080
            options.Add(option);    //dodanie do listy

            //sprawdzamy czy rozdielczość po której iterujemy jest równa domyślnie ustawionej, jeśli tak to zapisujemy jako domyślną
            if(resolutions[i].width == Screen.currentResolution.width && 
            resolutions[i].height == Screen.currentResolution.height)   
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); //dodawanie opcji do dropdowna
        resolutionDropdown.value = currentResolutionIndex;  //ustawienie domyślnie wybranego
        resolutionDropdown.RefreshShownValue(); //akrualizacja zmian
    }

    public void setFullscreen(bool fullscreen)  //ustawianie pełnego ekranu
    {
        Screen.fullScreen = fullscreen;
    }

    public void setQuality(int qualityIndex)    //ustawianie jakości
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setResolution(int resolutionIndex)  //ustawianie rozdzielczości
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
