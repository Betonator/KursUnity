using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] //odpowiada za pokazywanie zmiennej w Inspectorze
    private float speed = 50.0f;//zmienna prywatna odpowiadająca za prędkość poruszania
    //gdyby private zamienić na public to nie potrzeba [SerializeField], ale przez to można 
    //modyfikować zmienną przez inny skrypt
    private Rigidbody rigidBody;//zmienna przechowująca odwołanie się do komponentu Rigidbody (referencja)


    private void Awake() //funkcja wywołuje się raz przed rozpoczęciem gry, służy do przypisywania wartości zmiennych lub inicjalizacji obiektów
    {
        rigidBody = GetComponent<Rigidbody>(); //do zmiennej przypisujemy komponent z naszego obiektu 
    }

    void Update() //funkcja wywołuje się co klatke
    {
        float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime; //tworzymy zmienną lokalną i przypisujemy do niej wartość wciśnietego przycisku (A = -1, D = 1), 
        //mnożymy razy prędkość i czas trwania poprzedniej klatki, w celu sprawienia by ruch był gładszy, a nie skokowy
        float v = Input.GetAxis("Vertical") * speed * Time.fixedTime; //podobnie jak wyżej tylko pobieramy wartości przycisków dla osi pionowej (W = 1, S = -1)

        rigidBody.velocity = new Vector3(h, rigidBody.velocity.y, v);//do wektora przyśpieszenia w komponencie rigidbody, przypisujemy nowe wartości h i v. Instrukcja 
        //rigidbody.velocity.y wstawia obecną wartosć przyśpieszenia w osi Y, po to by nie blokować skoku, gdyby wartość ustawić na 0, to nie można skakać
    }
}
