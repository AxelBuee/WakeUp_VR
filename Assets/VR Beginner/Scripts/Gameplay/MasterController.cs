using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Master script that will handle reading some input on the controller and trigger special events like Teleport or
/// activating the MagicTractorBeam  + Menu
/// </summary>
public class MasterController : MonoBehaviour
{
    static MasterController s_Instance = null;
    public static MasterController Instance => s_Instance;

    public XRRig Rig => m_Rig;

    [Header("Setup")]
    public bool DisableSetupForDebug = false;
    public Transform StartingPosition;
    public GameObject TeleporterParent;
    
    [Header("Reference")]
    public XRRayInteractor RightTeleportInteractor;
    public XRRayInteractor LeftTeleportInteractor;

    public XRDirectInteractor RightDirectInteractor;
    public XRDirectInteractor LeftDirectInteractor;

    public MagicTractorBeam RightTractorBeam;
    public MagicTractorBeam LeftTractorBeam;
    public XRRayInteractor RightUiMenuInteractor;
    public XRRayInteractor LeftUiMenuInteractor;
    XRRig m_Rig;
    
    InputDevice m_LeftInputDevice;
    InputDevice m_RightInputDevice;

    XRInteractorLineVisual m_RightLineVisual;
    XRInteractorLineVisual m_LeftLineVisual;

    HandPrefab m_RightHandPrefab;
    HandPrefab m_LeftHandPrefab;
    
    XRReleaseController m_RightController;
    XRReleaseController m_LeftController;

    bool m_PreviousRightClicked = false;
    bool m_PreviousLeftClicked = false;

    bool m_LastFrameRightEnable = false;
    bool m_LastFrameLeftEnable = false;

    LayerMask m_OriginalRightMask;
    LayerMask m_OriginalLeftMask;
    
    List<XRBaseInteractable> m_InteractableCache = new List<XRBaseInteractable>(16);

    /// le gameObject contenant le component canvas du menu du jeu
    GameObject mMenuCanvas_Go;


    void Awake()
    {
        s_Instance = this;
        m_Rig = GetComponent<XRRig>();
       
    }

    void OnEnable()
    {
         InputDevices.deviceConnected += RegisterDevices;
    }

    void OnDisable()
    {
        InputDevices.deviceConnected -= RegisterDevices;
    }

    void Start()
    {
        m_RightLineVisual = RightTeleportInteractor.GetComponent<XRInteractorLineVisual>();
        m_RightLineVisual.enabled = false;

        m_LeftLineVisual = LeftTeleportInteractor.GetComponent<XRInteractorLineVisual>();
        m_LeftLineVisual.enabled = false;

        m_RightController = RightTeleportInteractor.GetComponent<XRReleaseController>();
        m_LeftController = LeftTeleportInteractor.GetComponent<XRReleaseController>();

        m_OriginalRightMask = RightTeleportInteractor.interactionLayerMask;
        m_OriginalLeftMask = LeftTeleportInteractor.interactionLayerMask;
        
        if (!DisableSetupForDebug)
        {
            transform.position = StartingPosition.position;
            transform.rotation = StartingPosition.rotation;
            
            if(TeleporterParent != null)
                TeleporterParent.SetActive(false);
        }
        
        InputDeviceCharacteristics leftTrackedControllerFilter = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left;
        List<InputDevice> foundControllers = new List<InputDevice>();
        
        InputDevices.GetDevicesWithCharacteristics(leftTrackedControllerFilter, foundControllers);

        if (foundControllers.Count > 0)
            m_LeftInputDevice = foundControllers[0];
        
        
        InputDeviceCharacteristics rightTrackedControllerFilter = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right;

        InputDevices.GetDevicesWithCharacteristics(rightTrackedControllerFilter, foundControllers);

        if (foundControllers.Count > 0)
            m_RightInputDevice = foundControllers[0];

        if (m_Rig.TrackingOriginMode != TrackingOriginModeFlags.Floor)
            m_Rig.cameraYOffset = 1.8f;

        mMenuCanvas_Go = CCManager.Instance.transform.Find("MenuCanvas").gameObject;
        if (mMenuCanvas_Go == null)
        {
            Debug.LogWarning("Pas de Menu dans cette scene ??");
        }
    }

    void RegisterDevices(InputDevice connectedDevice)
    {
        if (connectedDevice.isValid)
        {
            if ((connectedDevice.characteristics & InputDeviceCharacteristics.HeldInHand) == InputDeviceCharacteristics.HeldInHand)
            {
                if ((connectedDevice.characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
                {
                    m_LeftInputDevice = connectedDevice;
                }
                else if ((connectedDevice.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
                {
                    m_RightInputDevice = connectedDevice;
                }
            }
        }
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        
        // active les UI controlleur si besoin seulement
        
        RightTeleportUpdate();
        LeftTeleportUpdate();
        MenuUpdate();
        InMenu_ControllersUpdate();
    }

    /// Control UI menu visibility. Inspired from TeleportUpdate function.
    void MenuUpdate()
    {
        if( mMenuCanvas_Go == null )
        {
            return;
        }

        Vector2 axisRightInput;
        Vector2 axisLeftInput;
        m_RightInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisRightInput);
        m_LeftInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisLeftInput);

        if( axisRightInput.y < - 0.5f || axisLeftInput.y < - 0.5f )
        {
            mMenuCanvas_Go.SetActive(true);
        } 
        else if( !CCManager.Instance.mIsWaitingToPlay )
        {
            mMenuCanvas_Go.SetActive(false);
        }
    }

    // active / desactive les GOs controller du XRRig si le menu est
    // active ou desactive.
    void InMenu_ControllersUpdate()
    {
        if( mMenuCanvas_Go == null )
        {
            RightUiMenuInteractor.gameObject.SetActive( false );
            LeftUiMenuInteractor.gameObject.SetActive( false );
            return;
        }

        if( mMenuCanvas_Go.activeInHierarchy )
        {
            RightUiMenuInteractor.gameObject.SetActive( true );
            LeftUiMenuInteractor.gameObject.SetActive( true );

            RightDirectInteractor.gameObject.SetActive( false );
            LeftDirectInteractor.gameObject.SetActive( false );
            
            RightTeleportInteractor.gameObject.SetActive( false );
            LeftTeleportInteractor.gameObject.SetActive( false );
        }
        else
        {
            RightDirectInteractor.gameObject.SetActive( true );
            LeftDirectInteractor.gameObject.SetActive( true );
            
            RightTeleportInteractor.gameObject.SetActive( true );
            LeftTeleportInteractor.gameObject.SetActive( true );

            RightUiMenuInteractor.gameObject.SetActive( false );
            LeftUiMenuInteractor.gameObject.SetActive( false );
        }
    }
    void RightTeleportUpdate()
    {
        Vector2 axisInput;
        m_RightInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisInput);
        
        m_RightLineVisual.enabled = axisInput.y > 0.5f;
        
        RightTeleportInteractor.InteractionLayerMask = m_LastFrameRightEnable ? m_OriginalRightMask : new LayerMask();
        
        if (axisInput.y <= 0.5f && m_PreviousRightClicked)
        {
            m_RightController.Select();
        }

        
        if (axisInput.y <= -0.5f)
        {
            if(!RightTractorBeam.IsTracting)
                RightTractorBeam.StartTracting();
        }
        else if(RightTractorBeam.IsTracting)
        {
            RightTractorBeam.StopTracting();
        }

        //if the right animator is null, we try to get it. It's not the best performance wise but no other way as setup
        //of the model by the Interaction Toolkit is done on the first update.
        if (m_RightHandPrefab == null)
        {
            m_RightHandPrefab = RightDirectInteractor.GetComponentInChildren<HandPrefab>();
        }

        m_PreviousRightClicked = axisInput.y > 0.5f;

        if (m_RightHandPrefab != null)
        {
            m_RightHandPrefab.Animator.SetBool("Pointing", m_PreviousRightClicked);
        }

        m_LastFrameRightEnable = m_RightLineVisual.enabled;
    }

    void LeftTeleportUpdate()
    {
        Vector2 axisInput;
        m_LeftInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisInput);
        
        m_LeftLineVisual.enabled = axisInput.y > 0.5f;
        
        LeftTeleportInteractor.InteractionLayerMask = m_LastFrameLeftEnable ? m_OriginalLeftMask : new LayerMask();
        
        if (axisInput.y <= 0.5f && m_PreviousLeftClicked)
        {
            m_LeftController.Select();
        }
        
        if (axisInput.y <= -0.5f)
        {
            if(!LeftTractorBeam.IsTracting)
                LeftTractorBeam.StartTracting();
        }
        else if(LeftTractorBeam.IsTracting)
        {
            LeftTractorBeam.StopTracting();
        }
        
        //if the left animator is null, we try to get it. It's not the best performance wise but no other way as setup
        //of the model by the Interaction Toolkit is done on the first update.
        if (m_LeftHandPrefab == null)
        {
            m_LeftHandPrefab = LeftDirectInteractor.GetComponentInChildren<HandPrefab>();
        }

        m_PreviousLeftClicked = axisInput.y > 0.5f;
        
        if (m_LeftHandPrefab != null)
            m_LeftHandPrefab.Animator.SetBool("Pointing", m_PreviousLeftClicked);
        
        m_LastFrameLeftEnable = m_LeftLineVisual.enabled;
    }
}
