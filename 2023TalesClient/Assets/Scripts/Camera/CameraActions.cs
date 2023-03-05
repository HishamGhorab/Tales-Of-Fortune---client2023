// GENERATED AUTOMATICALLY FROM 'Assets/Resources/Input Actions/CameraActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CameraActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraActions"",
    ""maps"": [
        {
            ""name"": ""CameraControls"",
            ""id"": ""7b4a7fe7-a2bc-4565-b722-c849297e0100"",
            ""actions"": [
                {
                    ""name"": ""xyMovement"",
                    ""type"": ""Value"",
                    ""id"": ""9e5f1000-b9e6-4828-b5b8-eb101196d050"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseScroll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b9c64d57-79f8-4f70-8361-efe3e8db9d07"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayersNumbers"",
                    ""type"": ""Value"",
                    ""id"": ""74afd47b-0246-4b54-8838-fcd3f8d837ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""6856c024-8cdc-4039-b3a8-1853aca62ad6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""xyMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""772e5254-0e22-4500-b430-f2ebb7cc12cd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""xyMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""430c7715-2666-4533-afab-1acc7e827b2e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""xyMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b74dabee-8248-412d-a144-64be3d4359f1"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""xyMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""18c4d7ee-0383-4063-a747-8452245c9087"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""xyMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3d01ab30-8e06-4f50-b356-8e839b96afc5"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Numbers"",
                    ""id"": ""a11cd753-ac71-481b-abeb-a568d675552a"",
                    ""path"": ""CustomNumbers"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""numberOne"",
                    ""id"": ""c3a161a5-d6a4-4a10-9c9d-bf313175868d"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""numberTwo"",
                    ""id"": ""a407254f-ef6e-4b89-9a62-fc0ff3443c80"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""numberThree"",
                    ""id"": ""f027b33b-e4b4-4c7c-b259-a08352980530"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""numberFour"",
                    ""id"": ""60e6f850-1a64-4395-84f1-6b43f301134b"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""numberFive"",
                    ""id"": ""1a5239ab-e228-4a18-ae9a-8d7b7cdf1f8c"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""numberSix"",
                    ""id"": ""9aa6707f-b1ee-4d87-871d-bec99c169048"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""numberSeven"",
                    ""id"": ""a31784db-4c7a-45c1-984a-b3eb98acddaf"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""numberEight"",
                    ""id"": ""5d93c6e9-ff0a-4b2c-a246-cb745e6961e1"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""numberNine"",
                    ""id"": ""a5492c32-0d6e-420d-96dc-dcc8f8f64d13"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayersNumbers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraControls
        m_CameraControls = asset.FindActionMap("CameraControls", throwIfNotFound: true);
        m_CameraControls_xyMovement = m_CameraControls.FindAction("xyMovement", throwIfNotFound: true);
        m_CameraControls_MouseScroll = m_CameraControls.FindAction("MouseScroll", throwIfNotFound: true);
        m_CameraControls_PlayersNumbers = m_CameraControls.FindAction("PlayersNumbers", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // CameraControls
    private readonly InputActionMap m_CameraControls;
    private ICameraControlsActions m_CameraControlsActionsCallbackInterface;
    private readonly InputAction m_CameraControls_xyMovement;
    private readonly InputAction m_CameraControls_MouseScroll;
    private readonly InputAction m_CameraControls_PlayersNumbers;
    public struct CameraControlsActions
    {
        private @CameraActions m_Wrapper;
        public CameraControlsActions(@CameraActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @xyMovement => m_Wrapper.m_CameraControls_xyMovement;
        public InputAction @MouseScroll => m_Wrapper.m_CameraControls_MouseScroll;
        public InputAction @PlayersNumbers => m_Wrapper.m_CameraControls_PlayersNumbers;
        public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlsActions instance)
        {
            if (m_Wrapper.m_CameraControlsActionsCallbackInterface != null)
            {
                @xyMovement.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnXyMovement;
                @xyMovement.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnXyMovement;
                @xyMovement.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnXyMovement;
                @MouseScroll.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMouseScroll;
                @MouseScroll.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMouseScroll;
                @MouseScroll.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMouseScroll;
                @PlayersNumbers.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnPlayersNumbers;
                @PlayersNumbers.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnPlayersNumbers;
                @PlayersNumbers.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnPlayersNumbers;
            }
            m_Wrapper.m_CameraControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @xyMovement.started += instance.OnXyMovement;
                @xyMovement.performed += instance.OnXyMovement;
                @xyMovement.canceled += instance.OnXyMovement;
                @MouseScroll.started += instance.OnMouseScroll;
                @MouseScroll.performed += instance.OnMouseScroll;
                @MouseScroll.canceled += instance.OnMouseScroll;
                @PlayersNumbers.started += instance.OnPlayersNumbers;
                @PlayersNumbers.performed += instance.OnPlayersNumbers;
                @PlayersNumbers.canceled += instance.OnPlayersNumbers;
            }
        }
    }
    public CameraControlsActions @CameraControls => new CameraControlsActions(this);
    public interface ICameraControlsActions
    {
        void OnXyMovement(InputAction.CallbackContext context);
        void OnMouseScroll(InputAction.CallbackContext context);
        void OnPlayersNumbers(InputAction.CallbackContext context);
    }
}
