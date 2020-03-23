using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private bool jumpRequest = false; //zmienna przechowuje żądanie skoku

    [SerializeField]
    private float velocity = 10.0f; //przyśpieszenie skoku

    [SerializeField]
    private float fallMultiplier = 2.5f; //współczynnik grawitacji, czyli jak grawitacja to 9.81 to zwiększamy ją 1.5f * 9.81f, przez co postać szybciej spada

    [SerializeField]
    private float lowJumpMultiplier = 2.0f; //współczynnik grawitacji, w momencie gdy nie trzymamy spacji, a postać jest na etapie unoszenia. Jak nie jasne to priv

    private Rigidbody rigidBody;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump")) //sprawdzamy czy Jump(spacja) wciśnięty raz
        {
            jumpRequest = true;//ustawienie żadania skoku
        }
    }
    void FixedUpdate() //Funkcja wywoływana w równych odstępach czasu, domyślnie działa 50 razy na sekunde. Służy do obliczeń fizycznych
    {
        //ten if odpowiada za nadanie energii skoku
        if (jumpRequest)//sprawdzamy czy jest żądanie skoku
        {
            rigidBody.AddForce(Vector3.up * velocity, ForceMode.Impulse);//nadanie impulsu o wektorze skierowanym w góre (0,1,0) razy przyśpiesznie. 
            //ForceMode.Impulse ustawia, że to pojedyńczy impuls (tylko raz dodaje przyśpieszenie w danym kierunku.
            jumpRequest = false;//cofnięcie żądania skoku, bo wykonane
        }

        //te ify odpowiadają za wydłużenie i skrócenie skoku
        if (rigidBody.velocity.y < 0.0f)//przyśpieszenie w osi Y jest ujemne gdy obiekt spada
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime; 
            /*alternatywna i prostsza wersja powyższej linijki
             * rigidBody.gravityScale = fallMultiplier
             * poprostu zmieniamy mnożnik grawitacji
             * */
        }
        else if (rigidBody.velocity.y > 0.0f && !Input.GetButton("Jump"))
        {
            rigidBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            /*alternatywna wersja
             * rigidBody.gravityScale = lowJumpMultiplier
             */
        }
        /*ale przy alternatywnych wersja trzeba jeszcze wyzerować mnożnik grawitacji:
         *else{
         *    rigidBody.gravityScale = 1;
         *}
         * takie wykonanie wydaje mi się znacznie lepsze
         */
    }
}
