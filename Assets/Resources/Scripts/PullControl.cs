using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe pour controler l'objet avec des mechaniques de tractions.
/// Permet d'attirer un objet vers la main camera suite a une interaction / une condition.
/// Pour l'instant, on en a besoin que pour attirer les billes avec la lampe torche.
/// a terme il faudra separer les interactions avec les objets des conditions d'action des scripts.
/// Ex : si on veut pouvoir attirer a nous les billes avec la lampe, une classe gere l'action traction
///     une autre gere si on a bien la lampe en main (Mgr/Singleton), une derniere utilise la fonction d'attraction 
///     tout le temps ou lorsque le personage fait qqchose en particulier (controles)
/// </summary>
public class PullControl : MonoBehaviour
{
    /// <summary>
    /// la main camera. Sert de reference spatial.
    /// </summary>
    public GameObject mOriginObj;

    /// <summary>
    /// true : affiche les spheres et raycast
    /// </summary>
    public bool isOnDebug = false;

    private Collider mCollider;

    private RaycastHit mHit;


    // Update is called once per frame
    void Update ()
    {
        // initialaisations
        {
            // main camera encore null, on verifie si on utilise la camera VRTK
            if( mOriginObj == null )
            {
                return;
            }

            if( mCollider == null )
            {
                mCollider = gameObject.GetComponentInChildren<Collider>();
            }

        }

        // a terme il faut un moyen de verifier si les conditions de l'enigme soient reuni pour fair l'action
        if( !areConditionsValid() )
        {
            return;
        }

        // raycasts 
        {
            RaycastHit hit;

            var lOrigine = mOriginObj.transform.position;

            //var lRaycastDirection = this.transform.position - lOrigine;
            var lRaycastDirection = mOriginObj.transform.TransformDirection(Vector3.forward);
            //Debug.Log("pos : bille " + this.transform.position + ". camera " + lOrigine);

            // layer 8 est utilise pour la lampe.
            int layerMask = 1 << 8;

            // on lance des spheres depuis la camera, de rayon "1f", vers l'objet
            if ( Physics.SphereCast( lOrigine, 1f, lRaycastDirection, out hit, Mathf.Infinity, layerMask) )
            {
                mHit = hit;
                // on a touche
                if ( isOnDebug )
                {
                    Debug.Log("Did Hit : " + hit.collider.name);
                }

                    Debug.Log(hit.collider.gameObject.name + " " + this.name);
                // on applique la force de traction
                if ( hit.collider == mCollider )
                {
                    addPullForce(hit.rigidbody, -lRaycastDirection);
                }
                else
                {
                    Debug.Log("pb wrong collider!!");
                }
            }
            else
            {
                // pas touche : on montre les hitbox et raycasts
                if (isOnDebug)
                {
                    Debug.DrawRay( lOrigine, lRaycastDirection * 100,  Color.magenta, 1f );
                    Debug.Log("Did not Hit");
                }
            }
        }

    }

    void OnDrawGizmos()
    {
        if (!isOnDebug)
        {
            return;
        }

        // Draw a yellow sphere at the transform's position
        Gizmos.color = new Color(0.5f, 0f, 0.5f, 0.5f);
        Gizmos.DrawSphere(this.transform.position, 1f);
        
    }

    /// <summary>
    /// Rajoute une force a pObjRigidbody vers la lForceDirection.
    /// Supprime la composante verticale pour que l'objet ne vole pas.
    /// </summary>
    /// <param name="lForceDirection"></param>
    public void addPullForce( Rigidbody pObjRigidbody , Vector3 lForceDirection )
    {
        lForceDirection.y = 0f;
        lForceDirection = lForceDirection.normalized * 0.01f;
        pObjRigidbody.AddForce(lForceDirection, ForceMode.VelocityChange);
    }

    /// <summary>
    /// pour nos besoins futurs. Pour l'instant ce n'est pas en place.
    /// </summary>
    /// <returns></returns>
    private bool areConditionsValid()
    {
        if( mCollider == null )
        {
            Debug.LogWarning("mCollider null dans pullControl de " + this.gameObject.name + ". On n'effectue pas la traction." );
            return false;
        }

        if ( mOriginObj == null)
        {
            Debug.LogWarning("mCamera null dans pullControl de " + this.gameObject.name + ". On n'effectue pas la traction." );
            return false;
        }
        // lEventManager = get singleton / manager
        // return lEventManager.mFlashLightIsFounded
        return true;
    }
}
