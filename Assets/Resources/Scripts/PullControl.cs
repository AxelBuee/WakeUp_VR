using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe pour controler l'objet avec des mechaniques de tractions.
/// Permet d'attirer un objet vers l'objet renseigne.
/// Pour l'instant, on en a besoin que pour attirer les billes avec la lampe torche.
/// a terme il faudra separer les interactions avec les objets des conditions d'action des scripts.
/// Ex : si on veut pouvoir attirer a nous les billes avec la lampe, une classe gere l'action traction
///     une autre gere si on a bien la lampe en main (Mgr/Singleton), une derniere utilise la fonction d'attraction 
///     tout le temps ou lorsque le personage fait qqchose en particulier (controles)
/// </summary>
public class PullControl : MonoBehaviour
{
    /// <summary>
    /// numero du layer utilise pour attirer les objets avec la lampe
    /// ce layer ne collisione pas, il capte seulement les raycasts
    /// </summary>
    static int sLayer = 15; //1 << 15;

    /// <summary>
    /// GO attracteur.
    /// </summary>
    public GameObject mPullerObj;

    public GameObject otherSphere;


    /// <summary>
    /// Collider conenant le ParticleSystem sous le lit.
    /// </summary>
    public Collider mMarbleDeparturCollider;

    /// <summary>
    /// Pour arreter la fumee si la bille est trop loin de sa pos de depart.
    /// </summary>
    private Vector3 mStartPos;

    /// <summary>
    /// rayon du cercle autour du point d'origine partir de laquel on stop la fumee si la bille est dehors.
    /// Si on veut eviter que la fumee s'arrete "sur un coup de chance"
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
    /// collider de l'objet a bouger et qui contient ce script
    /// </summary>
    private SphereCollider mCollider;

    ///<summary>
    /// rayon du sphereCollider captant les raycast de la lampe
    ///<summary>
    public float mGhostHitboxRadius;

    /// <summary>
    /// le rigidbody de l'objet a attirer, et qui est donc dans la hierarchi de this
    /// </summary>
    private Rigidbody mRigidbody;

    /// <summary>
    /// le hit contenant les infos sur le raycast touche par le raycast.
    /// </summary>
    //private RaycastHit mHit;

    /// <summary>
    /// TODO c'est propre aux billes sous le lit, ca n'a rien a faire ici normalement...
    /// Component de system de particules de la bille
    /// </summary>
    private ParticleSystem mParticleSystem;

    /// <summary>
    /// component Light de la lampe dont le parent est active/desactive;
    /// </summary>
    private Light mLight;

    private void Start()
    {
        //inits des vals avec vals par defaut si besoin
        if (mStopSmokeRange <= 0)
        {
            mStopSmokeRange = 1f;
        }

        if (mPullForceCoef <= 0)
        {
            mPullForceCoef = .1f;
        }

        if( mGhostHitboxRadius<= 0 )
        {
            mGhostHitboxRadius = 0.05f;
        }

        if (mCollider == null)
        {
            // on creer le GameObject qui contiendra la hitbox de la bille
            var mColliderGoContainer = new GameObject("GhostHitbox");
            mColliderGoContainer.layer = sLayer;
            mColliderGoContainer.transform.SetParent( this.transform );
            mColliderGoContainer.transform.localPosition = Vector3.zero;
            mColliderGoContainer.transform.localRotation = Quaternion.identity;
            mColliderGoContainer.transform.localScale = Vector3.one;

            // set la parente
            mCollider = mColliderGoContainer.AddComponent<SphereCollider>(); // le collider detecteur de raycast
            var lTrueCollider = gameObject.GetComponent<Collider>() as BoxCollider; // le vrai collider de la bille

            // set le collider
            mCollider.center = lTrueCollider != null ? lTrueCollider.center : Vector3.zero ;
            mCollider.radius = mGhostHitboxRadius;
        }

        if (mRigidbody == null)
        {
            mRigidbody = gameObject.GetComponentInChildren<Rigidbody>();
        }

        if (mMarbleDeparturCollider != null)
        {
            mParticleSystem = mMarbleDeparturCollider.gameObject.GetComponentInChildren<ParticleSystem>();
        }

        if( mPullerObj != null )
        {
            mLight = mPullerObj.transform.parent.GetComponentInChildren<Light>(true);
        }

        mDebugStr = string.Empty;

        mStartPos = mMarbleDeparturCollider.transform.position;
    }

    /// fixed update a cause de la addForce
    void FixedUpdate()
    {
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
                    if( !mParticleSystem.isStopped ) // on veux bloquer la bille et particlesSys.stop une seule frame dans tout le jeu
                    {
                        mParticleSystem.Stop();
                        mRigidbody.AddForce( - mRigidbody.velocity , ForceMode.Impulse );
                        mMarbleDeparturCollider.enabled = false; // on empeche les billes de se rapprocher si elle sont sortie du lit
                        otherSphere.SetActive(false);
                    }

                }
            }
        }

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

            // on lance des rayons depuis mPullerObj, vers sa direction frontaliere.
            // si mPullerObj est dans le collider au moment du cast, la colision ne sera detectee.
            var lLayerMask = 1 << sLayer;
            if (Physics.Raycast(lOrigine, lRaycastDirection, out hit, Mathf.Infinity, lLayerMask) )
            {
                // on a touche
                //mHit = hit;
                if (isOnDebug)
                {
                    mDebugStr += "Did Hit : " + hit.collider.name + " | ";
                }

                // on applique la force de traction si mPullerObj pointe vers mMarbleDepartueCollider
                if ( (mMarbleDeparturCollider != null && hit.collider == mMarbleDeparturCollider) 
                    || hit.collider == mCollider )
                {
                    addPullForce( mRigidbody, mPullerObj.transform.position - this.transform.position );
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

        var lCollider = mMarbleDeparturCollider as SphereCollider;
        Gizmos.DrawSphere( mMarbleDeparturCollider.transform.position, lCollider != null ? lCollider.radius : 0.01f );

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
        pObjRigidbody.AddForce(lForceDirection, ForceMode.Force);
    }

    /// <summary>
    /// Verifie qu'il y ait bien les components necessaire au script.
    /// Retourn false si il manque un component ou si la lampe est eteinte.
    /// </summary>
    /// <returns></returns>
    private bool areConditionsValid()
    {
        if (mCollider == null)
        {
            Debug.LogWarning("mCollider null dans pullControl de " + this.gameObject.name + ". L'objet deteneur du script est bien complet?");
            return false;
        }

        if (mPullerObj == null)
        {
            Debug.LogWarning("mObjOrigin null dans pullControl de " + this.gameObject.name + ". La source d'attraction est bien set?");
            return false;
        }

        if( mLight == null )
        {
            Debug.LogWarning("mLight null dans pullControl de " + this.gameObject.name + ". mObjOrigin (source du mouvment) a bien une lumiere?.");
            return false;
        }

        if( mMarbleDeparturCollider == null )
        {
            Debug.LogWarning("mMarbleDeparturCollider null dans pullControl de " + this.gameObject.name + ". Quel collider detecte qu'on pointe la lampe sous le lit?");
            return false;
        }

        // la bille reste inerte si la lampe est eteinte ...
        if ( !mLight.transform.gameObject.activeSelf )
        {
            return false;
        }

        // un truc du genre
        // var lEventManager = get singleton / manager
        // return lEventManager.mFlashLightIsFounded
        return true;
    }
}
