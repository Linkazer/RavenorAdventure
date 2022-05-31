// GENERATED AUTOMATICALLY FROM 'Assets/Script/Inputs/PlayerControl.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControl : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""BattleActionMap"",
            ""id"": ""e28c39c9-14c2-41c9-9263-7dc9466e124a"",
            ""actions"": [
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""009899f5-a799-4705-b46d-0428b7a53a3a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveCamera"",
                    ""type"": ""Value"",
                    ""id"": ""9e7b68d8-1a6d-443d-8111-21f96d58244f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseLeftClic"",
                    ""type"": ""Button"",
                    ""id"": ""dc179aa0-186f-4e26-8a1f-8255bafcdb18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMiddleClic"",
                    ""type"": ""Value"",
                    ""id"": ""670ec74b-9f63-4b94-9409-56fd5e4f05be"",
                    ""expectedControlType"": ""Integer"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""af4eba63-ce55-481f-826d-d1aa41b0ae85"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""DirectionalArrow"",
                    ""id"": ""2b930294-74ec-463e-901a-e4ec5ae94af0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""95e20de9-6832-46e6-b6b1-b46eaf9cf4fe"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""30774bfb-1506-490e-9ecd-9c8adfbaec86"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b7b7c6f5-b63f-4ad2-83dc-2d6b05777174"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2e9014e3-2a44-4188-b60b-b784b7cbdebf"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ZQSD"",
                    ""id"": ""52a2a487-38b9-4ee8-bd5f-43825a25e5e5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""81a169d6-a851-4e78-a282-3ed0b57e8008"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1e0b3119-4b33-4475-9356-2466bda52f7a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3f512c77-edcd-40c4-b6f4-ed414471504d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""953b2266-4ce0-4c63-9f1f-f1a36e5e68b5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9a6e0a7b-be93-4494-8e7e-5d73d0d5653f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLeftClic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf80f5f5-35d2-45b0-a127-bf33c020bcb0"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMiddleClic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // BattleActionMap
        m_BattleActionMap = asset.FindActionMap("BattleActionMap", throwIfNotFound: true);
        m_BattleActionMap_MousePosition = m_BattleActionMap.FindAction("MousePosition", throwIfNotFound: true);
        m_BattleActionMap_MoveCamera = m_BattleActionMap.FindAction("MoveCamera", throwIfNotFound: true);
        m_BattleActionMap_MouseLeftClic = m_BattleActionMap.FindAction("MouseLeftClic", throwIfNotFound: true);
        m_BattleActionMap_MouseMiddleClic = m_BattleActionMap.FindAction("MouseMiddleClic", throwIfNotFound: true);
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

    // BattleActionMap
    private readonly InputActionMap m_BattleActionMap;
    private IBattleActionMapActions m_BattleActionMapActionsCallbackInterface;
    private readonly InputAction m_BattleActionMap_MousePosition;
    private readonly InputAction m_BattleActionMap_MoveCamera;
    private readonly InputAction m_BattleActionMap_MouseLeftClic;
    private readonly InputAction m_BattleActionMap_MouseMiddleClic;
    public struct BattleActionMapActions
    {
        private @PlayerControl m_Wrapper;
        public BattleActionMapActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_BattleActionMap_MousePosition;
        public InputAction @MoveCamera => m_Wrapper.m_BattleActionMap_MoveCamera;
        public InputAction @MouseLeftClic => m_Wrapper.m_BattleActionMap_MouseLeftClic;
        public InputAction @MouseMiddleClic => m_Wrapper.m_BattleActionMap_MouseMiddleClic;
        public InputActionMap Get() { return m_Wrapper.m_BattleActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleActionMapActions set) { return set.Get(); }
        public void SetCallbacks(IBattleActionMapActions instance)
        {
            if (m_Wrapper.m_BattleActionMapActionsCallbackInterface != null)
            {
                @MousePosition.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMousePosition;
                @MoveCamera.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMoveCamera;
                @MouseLeftClic.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseLeftClic;
                @MouseLeftClic.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseLeftClic;
                @MouseLeftClic.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseLeftClic;
                @MouseMiddleClic.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseMiddleClic;
                @MouseMiddleClic.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseMiddleClic;
                @MouseMiddleClic.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseMiddleClic;
            }
            m_Wrapper.m_BattleActionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @MoveCamera.started += instance.OnMoveCamera;
                @MoveCamera.performed += instance.OnMoveCamera;
                @MoveCamera.canceled += instance.OnMoveCamera;
                @MouseLeftClic.started += instance.OnMouseLeftClic;
                @MouseLeftClic.performed += instance.OnMouseLeftClic;
                @MouseLeftClic.canceled += instance.OnMouseLeftClic;
                @MouseMiddleClic.started += instance.OnMouseMiddleClic;
                @MouseMiddleClic.performed += instance.OnMouseMiddleClic;
                @MouseMiddleClic.canceled += instance.OnMouseMiddleClic;
            }
        }
    }
    public BattleActionMapActions @BattleActionMap => new BattleActionMapActions(this);
    public interface IBattleActionMapActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMoveCamera(InputAction.CallbackContext context);
        void OnMouseLeftClic(InputAction.CallbackContext context);
        void OnMouseMiddleClic(InputAction.CallbackContext context);
    }
}
