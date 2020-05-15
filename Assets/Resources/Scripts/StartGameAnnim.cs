using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameAnnim : MonoBehaviour
{
    [SerializeField] public Animator animator;//animator du perso
    [SerializeField] public GameObject door; //porte a fermer
    [SerializeField] public Light doorLight; //lumièrer a éteindre
    [SerializeField] public GameObject kidLight; //not object but realy light

    // Start is called before the first frame update
    void Start()
    {
        //lancer l'anim du perso et attendre qu'elle se finisse
        animator.SetBool("playPressed", true);

        //fermer la porte
        StartCoroutine("waitAnnimationAndClose");

        //éteindre la lumière //allumer la veilleuse 
        StartCoroutine("lightOFF");
    }

    public void runGame(){
        
        // //lancer l'anim du perso et attendre qu'elle se finisse
        // animator.SetBool("playPressed", true);
        // StartCoroutine("waitAnnimationEnd");

        // //fermer la porte
        // HingeJoint hinge = door.GetComponent<HingeJoint>();
        // hinge.useSpring = true;

        // //reculer la lumière
        // StartCoroutine("lightOFF");

        // //allumer la veilleuse 
        // kidLight.SetActive(true);
    }

    IEnumerator lightOFF(){
            float steps = 0.2f;
            doorLight.intensity=5;
            while(doorLight.intensity > 0.5){
                yield return new WaitForSeconds(0.1f);
                doorLight.intensity -=  steps;
            }
            kidLight.SetActive(true);
    }
    IEnumerator waitAnnimationAndClose(){
        yield return new WaitForSeconds(5f);
        HingeJoint hinge = door.GetComponent<HingeJoint>();
        hinge.useSpring = true;
    }
    
}
