using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe pour controler l'objet avec des mechaniques de tractions.
/// Permet d'attirer un objet vers l'objet renseigne (TODO : suite a une interaction / une condition ).
/// Pour l'instant, on en a besoin que pour attirer les billes avec la lampe torche.
/// a terme il faudra separer les interactions avec les objets des conditions d'action des scripts.
/// Ex : si on veut pouvoir attirer a nous les billes avec la lampe, une classe gere l'action traction
///     une autre gere si on a bien la lampe en main (Mgr/Singleton), une derniere utilise la fonction d'attraction 
///     tout le temps ou lorsque le personage fait qqchose en particulier (controles)
/// </summary>
public class PullControl : MonoBehaviour
{
    /// <summary>
    /// GO vers ou va la force d'attraction.
    /// </summary>
    public GameObject mPullerObj;

    /// <summary>
    /// Pour arreter la fumee si la bille est trop loin de sa pos de depart.
    /// </summary>
    private Vector3 mStartPos;

    /// <summary>
    /// rayon du cercle autour du point d'origine partir de laquel on stop la fumee si la bille est dehors.
    /// </summary>
    public float mStopSmokeRange;

    /// <summary>
    /// multiplie la taille du vecteur normalise avant de l'appliquer en force.
    /// </summary>
    public float mPullForceCoef;

    /// <summary>
    /// true : affiche les spheres, raycasts et log sur la scene
    /// </summary>
    public bool isOnDebug = false;

    private string mDebugStr;

    /// <summary>
    /// collider de l'objet a bouger et qui contient se script
    /// </summary>
    private Collider mCollider;

    /// <summary>
    /// le hit contenant les infos sur le raycast touche par le raycast.
    /// </summary>
    //private RaycastHit mHit;

    /// <summary>
    /// TODO c'est propre aux billes sous le lit, ca n'a rien a faire ici normalement...
    /// Component de system de particules de la bille
    /// </summary>
    private ParticleSystem mParticleSystem;

    private void Start()
    {
        if (mStopSmokeRange <= 0)
        {
            mStopSmokeRange = 1f;
        }

        if (mPullForceCoef <= 0)
        {
            mPullForceCoef = .1f;
        }

        if (mParticleSystem == null)
        {
            mParticleSystem = this.GetComponentInChildren<ParticleSystem>();
        }

        if (mCollider == null)
        {
            mCollider = gameObject.GetComponentInChildren<Collider>();
        }

        mDebugStr = string.Empty;

        mStartPos = this.transform.position;
    }

    /// fixed update a cause de la addForce
    void FixedUpdate()
    {
        // a terme il faut un moyen de verifier si les conditions de l'enigme soient reuni pour fair l'action
        if (!areConditionsValid())
        {
            return;
        }


        // raycasts 
        {
            RaycastHit hit;

            var lOrigine = mPullerObj.transform.position;

            var lRaycastDirection = mPullerObj.transform.TransformDirection(Vector3.forward);

            // layer 8 est utilise pour la lampe.
            int layerMask = 1 << 8;

            // on lance des spheres depuis la camera, de rayon "1f", vers l'objet
            if (Physics.SphereCast(lOrigine, 1f, lRaycastDirection, out hit, Mathf.Infinity, layerMask))
            {
                // on a touche
                //mHit = hit;
                if (isOnDebug)
                {
                    mDebugStr += "Did Hit : " + hit.collider.name + " | ";
                }

                // on applique la force de traction
                if (hit.collider == mCollider)
                {
                    addPullForce(hit.rigidbody, -lRaycastDirection);
                }
                else
                {
                    mDebugStr += "pb wrong collider!!" + " | ";
                }
            }
            else
            {
                // raycast a pas touche : on montre les hitbox et raycasts
                if (isOnDebug)
                {
                    Debug.DrawRay(lOrigine, lRaycastDirection * 100, Color.magenta, 1f);
                    mDebugStr += "Did not Hit" + " | ";
                }

            }

        }

        // TODO c'est propre aux billes sous le lit, ca n'a rien a faire ici normalement...
        // stop les particules de fumee des bille si elle s'est deplacee trop loin
        if (mCollider != null)
        {
            var lDist = (transform.position - mStartPos).magnitude;

            if (isOnDebug)
            {
                mDebugStr += "Dist = " + lDist + " | ";
            }

            if (lDist > mStopSmokeRange)
            {
                if (mParticleSystem != null)
                {
                    mParticleSystem.Stop();
                }
            }
        }

        // print + reset debug
        if (isOnDebug)
        {
            Debug.Log(mDebugStr);
            mDebugStr = string.Empty;
        }
    }

    void OnDrawGizmos()
    {
        if (!isOnDebug)
        {
            return;
        }

        // Draw a purple sphere at the transform's position
        Gizmos.color = new Color(0.5f, 0f, 0.5f, 0.5f);
        Gizmos.DrawSphere(this.transform.position, 1f);

    }

    /// <summary>
    /// Rajoute une force a pObjRigidbody vers la lForceDirection.
    /// Supprime la composante verticale pour que l'objet ne vole pas.
    /// // en fait faudrait attirer l'objet vers sois avec une ia + navmesh soit avec une animation ?
    /// </summary>
    /// <param name="lForceDirection"></param>
    public void addPullForce(Rigidbody pObjRigidbody, Vector3 lForceDirection)
    {
        lForceDirection.y = 0f;
        lForceDirection = lForceDirection.normalized * mPullForceCoef;
        pObjRigidbody.AddForce(lForceDirection, ForceMode.VelocityChange);
    }

    /// <summary>
    /// a faire pour nos besoins futurs.
    /// Verifie qu'il y ait bien les components necessaire en attendant.
    /// </summary>
    /// <returns></returns>
    private bool areConditionsValid()
    {
        if (mCollider == null)
        {
            Debug.LogWarning("mCollider null dans pullControl de " + this.gameObject.name + ". On n'effectue pas la traction.");
            return false;
        }

        if (mPullerObj == null)
        {
            Debug.LogWarning("mObjOrigin null dans pullControl de " + this.gameObject.name + ". On n'effectue pas la traction.");
            return false;
        }

        // un truc du genre
        // var lEventManager = get singleton / manager
        // return lEventManager.mFlashLightIsFounded
        return true;
    }
}
