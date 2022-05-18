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
                    ""name"": ""MouseClic"",
                    ""type"": ""Value"",
                    ""id"": ""009899f5-a799-4705-b46d-0428b7a53a3a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""MoveCamera"",
                    ""type"": ""Value"",
                    ""id"": ""9e7b68d8-1a6d-443d-8111-21f96d58244f"",
                    ""expectedControlType"": ""Vector2"",
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
                    ""action"": ""MouseClic"",
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
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // BattleActionMap
        m_BattleActionMap = asset.FindActionMap("BattleActionMap", throwIfNotFound: true);
        m_BattleActionMap_MouseClic = m_BattleActionMap.FindAction("MouseClic", throwIfNotFound: true);
        m_BattleActionMap_MoveCamera = m_BattleActionMap.FindAction("MoveCamera", throwIfNotFound: true);
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
    private readonly InputAction m_BattleActionMap_MouseClic;
    private readonly InputAction m_BattleActionMap_MoveCamera;
    public struct BattleActionMapActions
    {
        private @PlayerControl m_Wrapper;
        public BattleActionMapActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseClic => m_Wrapper.m_BattleActionMap_MouseClic;
        public InputAction @MoveCamera => m_Wrapper.m_BattleActionMap_MoveCamera;
        public InputActionMap Get() { return m_Wrapper.m_BattleActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleActionMapActions set) { return set.Get(); }
        public void SetCallbacks(IBattleActionMapActions instance)
        {
            if (m_Wrapper.m_BattleActionMapActionsCallbackInterface != null)
            {
                @MouseClic.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseClic;
                @MouseClic.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseClic;
                @MouseClic.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseClic;
                @MoveCamera.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMoveCamera;
            }
            m_Wrapper.m_BattleActionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseClic.started += instance.OnMouseClic;
                @MouseClic.performed += instance.OnMouseClic;
                @MouseClic.canceled += instance.OnMouseClic;
                @MoveCamera.started += instance.OnMoveCamera;
                @MoveCamera.performed += instance.OnMoveCamera;
                @MoveCamera.canceled += instance.OnMoveCamera;
            }
        }
    }
    public BattleActionMapActions @BattleActionMap => new BattleActionMapActions(this);
    public interface IBattleActionMapActions
    {
        void OnMouseClic(InputAction.CallbackContext context);
        void OnMoveCamera(InputAction.CallbackContext context);
    }
}
