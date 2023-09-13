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
                    ""type"": ""Button"",
                    ""id"": ""670ec74b-9f63-4b94-9409-56fd5e4f05be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseRightClic"",
                    ""type"": ""Button"",
                    ""id"": ""a3b1b768-4b46-45bf-9a75-27299d3243fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseScroll"",
                    ""type"": ""Value"",
                    ""id"": ""dff447dc-c6a6-416d-8147-fd7c565e7f98"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectCharacter_1"",
                    ""type"": ""Button"",
                    ""id"": ""c63ca657-732e-431d-b180-72b047e5a91c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectCharacter_2"",
                    ""type"": ""Button"",
                    ""id"": ""e1e3ae82-99d8-4e65-8e97-604f2dd09b67"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectCharacter_3"",
                    ""type"": ""Button"",
                    ""id"": ""79d328c4-ecb6-43e8-a1a5-ddaa67cb2864"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectCharacter_4"",
                    ""type"": ""Button"",
                    ""id"": ""ed8be3bb-88fe-4467-b180-249de731f3e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectSpell_1"",
                    ""type"": ""Button"",
                    ""id"": ""14058a40-215c-43a8-8e48-f36f1c25d520"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectSpell_2"",
                    ""type"": ""Button"",
                    ""id"": ""e3cbb61e-565e-4f49-a869-2f01d764ada8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectSpell_3"",
                    ""type"": ""Button"",
                    ""id"": ""905b5830-7545-4cbf-85b9-2a58c5b80449"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectSpell_4"",
                    ""type"": ""Button"",
                    ""id"": ""924fbd86-834b-40b0-9721-1033361007a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectSpell_5"",
                    ""type"": ""Button"",
                    ""id"": ""17f8fb8e-3858-481c-8eaf-7d91c334c35f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EndTurn"",
                    ""type"": ""Button"",
                    ""id"": ""a8718280-3649-4381-a95c-882f2da543ea"",
                    ""expectedControlType"": ""Button"",
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
                    ""interactions"": """",
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
                },
                {
                    ""name"": """",
                    ""id"": ""d28594bc-0379-449e-8e8e-ec68ad6ece17"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseRightClic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16d004e8-f227-4ced-a5e6-6e4971c91745"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02648a1e-bfb0-4dec-8db7-ce9dc84b0a6a"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectCharacter_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1cb7701-a5c7-43c1-8891-a42c5f39f105"",
                    ""path"": ""<Keyboard>/f2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectCharacter_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f84c8dd-2dd4-4194-a742-de788b0102b3"",
                    ""path"": ""<Keyboard>/f3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectCharacter_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ae83578-7e87-48cb-968f-313b6fdc507f"",
                    ""path"": ""<Keyboard>/f4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectCharacter_4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8f036a4-daf2-436c-bc73-7eb78985a4dd"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectSpell_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d411f78d-1003-4d5f-884f-8b8962314b1b"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectSpell_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e13f664-dd0f-4619-99af-111228dbf3e2"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectSpell_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1fa6c44c-933f-4688-9c5f-1836a592feba"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectSpell_4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""669cfc1b-04db-408d-b3c0-b5640af60be9"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectSpell_5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a12048bc-39cd-4c24-a497-1e9c2d9d2f12"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EndTurn"",
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
        m_BattleActionMap_MouseRightClic = m_BattleActionMap.FindAction("MouseRightClic", throwIfNotFound: true);
        m_BattleActionMap_MouseScroll = m_BattleActionMap.FindAction("MouseScroll", throwIfNotFound: true);
        m_BattleActionMap_SelectCharacter_1 = m_BattleActionMap.FindAction("SelectCharacter_1", throwIfNotFound: true);
        m_BattleActionMap_SelectCharacter_2 = m_BattleActionMap.FindAction("SelectCharacter_2", throwIfNotFound: true);
        m_BattleActionMap_SelectCharacter_3 = m_BattleActionMap.FindAction("SelectCharacter_3", throwIfNotFound: true);
        m_BattleActionMap_SelectCharacter_4 = m_BattleActionMap.FindAction("SelectCharacter_4", throwIfNotFound: true);
        m_BattleActionMap_SelectSpell_1 = m_BattleActionMap.FindAction("SelectSpell_1", throwIfNotFound: true);
        m_BattleActionMap_SelectSpell_2 = m_BattleActionMap.FindAction("SelectSpell_2", throwIfNotFound: true);
        m_BattleActionMap_SelectSpell_3 = m_BattleActionMap.FindAction("SelectSpell_3", throwIfNotFound: true);
        m_BattleActionMap_SelectSpell_4 = m_BattleActionMap.FindAction("SelectSpell_4", throwIfNotFound: true);
        m_BattleActionMap_SelectSpell_5 = m_BattleActionMap.FindAction("SelectSpell_5", throwIfNotFound: true);
        m_BattleActionMap_EndTurn = m_BattleActionMap.FindAction("EndTurn", throwIfNotFound: true);
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
    private readonly InputAction m_BattleActionMap_MouseRightClic;
    private readonly InputAction m_BattleActionMap_MouseScroll;
    private readonly InputAction m_BattleActionMap_SelectCharacter_1;
    private readonly InputAction m_BattleActionMap_SelectCharacter_2;
    private readonly InputAction m_BattleActionMap_SelectCharacter_3;
    private readonly InputAction m_BattleActionMap_SelectCharacter_4;
    private readonly InputAction m_BattleActionMap_SelectSpell_1;
    private readonly InputAction m_BattleActionMap_SelectSpell_2;
    private readonly InputAction m_BattleActionMap_SelectSpell_3;
    private readonly InputAction m_BattleActionMap_SelectSpell_4;
    private readonly InputAction m_BattleActionMap_SelectSpell_5;
    private readonly InputAction m_BattleActionMap_EndTurn;
    public struct BattleActionMapActions
    {
        private @PlayerControl m_Wrapper;
        public BattleActionMapActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_BattleActionMap_MousePosition;
        public InputAction @MoveCamera => m_Wrapper.m_BattleActionMap_MoveCamera;
        public InputAction @MouseLeftClic => m_Wrapper.m_BattleActionMap_MouseLeftClic;
        public InputAction @MouseMiddleClic => m_Wrapper.m_BattleActionMap_MouseMiddleClic;
        public InputAction @MouseRightClic => m_Wrapper.m_BattleActionMap_MouseRightClic;
        public InputAction @MouseScroll => m_Wrapper.m_BattleActionMap_MouseScroll;
        public InputAction @SelectCharacter_1 => m_Wrapper.m_BattleActionMap_SelectCharacter_1;
        public InputAction @SelectCharacter_2 => m_Wrapper.m_BattleActionMap_SelectCharacter_2;
        public InputAction @SelectCharacter_3 => m_Wrapper.m_BattleActionMap_SelectCharacter_3;
        public InputAction @SelectCharacter_4 => m_Wrapper.m_BattleActionMap_SelectCharacter_4;
        public InputAction @SelectSpell_1 => m_Wrapper.m_BattleActionMap_SelectSpell_1;
        public InputAction @SelectSpell_2 => m_Wrapper.m_BattleActionMap_SelectSpell_2;
        public InputAction @SelectSpell_3 => m_Wrapper.m_BattleActionMap_SelectSpell_3;
        public InputAction @SelectSpell_4 => m_Wrapper.m_BattleActionMap_SelectSpell_4;
        public InputAction @SelectSpell_5 => m_Wrapper.m_BattleActionMap_SelectSpell_5;
        public InputAction @EndTurn => m_Wrapper.m_BattleActionMap_EndTurn;
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
                @MouseRightClic.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseRightClic;
                @MouseRightClic.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseRightClic;
                @MouseRightClic.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseRightClic;
                @MouseScroll.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseScroll;
                @MouseScroll.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseScroll;
                @MouseScroll.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnMouseScroll;
                @SelectCharacter_1.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_1;
                @SelectCharacter_1.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_1;
                @SelectCharacter_1.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_1;
                @SelectCharacter_2.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_2;
                @SelectCharacter_2.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_2;
                @SelectCharacter_2.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_2;
                @SelectCharacter_3.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_3;
                @SelectCharacter_3.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_3;
                @SelectCharacter_3.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_3;
                @SelectCharacter_4.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_4;
                @SelectCharacter_4.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_4;
                @SelectCharacter_4.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectCharacter_4;
                @SelectSpell_1.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_1;
                @SelectSpell_1.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_1;
                @SelectSpell_1.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_1;
                @SelectSpell_2.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_2;
                @SelectSpell_2.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_2;
                @SelectSpell_2.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_2;
                @SelectSpell_3.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_3;
                @SelectSpell_3.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_3;
                @SelectSpell_3.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_3;
                @SelectSpell_4.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_4;
                @SelectSpell_4.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_4;
                @SelectSpell_4.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_4;
                @SelectSpell_5.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_5;
                @SelectSpell_5.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_5;
                @SelectSpell_5.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnSelectSpell_5;
                @EndTurn.started -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnEndTurn;
                @EndTurn.performed -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnEndTurn;
                @EndTurn.canceled -= m_Wrapper.m_BattleActionMapActionsCallbackInterface.OnEndTurn;
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
                @MouseRightClic.started += instance.OnMouseRightClic;
                @MouseRightClic.performed += instance.OnMouseRightClic;
                @MouseRightClic.canceled += instance.OnMouseRightClic;
                @MouseScroll.started += instance.OnMouseScroll;
                @MouseScroll.performed += instance.OnMouseScroll;
                @MouseScroll.canceled += instance.OnMouseScroll;
                @SelectCharacter_1.started += instance.OnSelectCharacter_1;
                @SelectCharacter_1.performed += instance.OnSelectCharacter_1;
                @SelectCharacter_1.canceled += instance.OnSelectCharacter_1;
                @SelectCharacter_2.started += instance.OnSelectCharacter_2;
                @SelectCharacter_2.performed += instance.OnSelectCharacter_2;
                @SelectCharacter_2.canceled += instance.OnSelectCharacter_2;
                @SelectCharacter_3.started += instance.OnSelectCharacter_3;
                @SelectCharacter_3.performed += instance.OnSelectCharacter_3;
                @SelectCharacter_3.canceled += instance.OnSelectCharacter_3;
                @SelectCharacter_4.started += instance.OnSelectCharacter_4;
                @SelectCharacter_4.performed += instance.OnSelectCharacter_4;
                @SelectCharacter_4.canceled += instance.OnSelectCharacter_4;
                @SelectSpell_1.started += instance.OnSelectSpell_1;
                @SelectSpell_1.performed += instance.OnSelectSpell_1;
                @SelectSpell_1.canceled += instance.OnSelectSpell_1;
                @SelectSpell_2.started += instance.OnSelectSpell_2;
                @SelectSpell_2.performed += instance.OnSelectSpell_2;
                @SelectSpell_2.canceled += instance.OnSelectSpell_2;
                @SelectSpell_3.started += instance.OnSelectSpell_3;
                @SelectSpell_3.performed += instance.OnSelectSpell_3;
                @SelectSpell_3.canceled += instance.OnSelectSpell_3;
                @SelectSpell_4.started += instance.OnSelectSpell_4;
                @SelectSpell_4.performed += instance.OnSelectSpell_4;
                @SelectSpell_4.canceled += instance.OnSelectSpell_4;
                @SelectSpell_5.started += instance.OnSelectSpell_5;
                @SelectSpell_5.performed += instance.OnSelectSpell_5;
                @SelectSpell_5.canceled += instance.OnSelectSpell_5;
                @EndTurn.started += instance.OnEndTurn;
                @EndTurn.performed += instance.OnEndTurn;
                @EndTurn.canceled += instance.OnEndTurn;
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
        void OnMouseRightClic(InputAction.CallbackContext context);
        void OnMouseScroll(InputAction.CallbackContext context);
        void OnSelectCharacter_1(InputAction.CallbackContext context);
        void OnSelectCharacter_2(InputAction.CallbackContext context);
        void OnSelectCharacter_3(InputAction.CallbackContext context);
        void OnSelectCharacter_4(InputAction.CallbackContext context);
        void OnSelectSpell_1(InputAction.CallbackContext context);
        void OnSelectSpell_2(InputAction.CallbackContext context);
        void OnSelectSpell_3(InputAction.CallbackContext context);
        void OnSelectSpell_4(InputAction.CallbackContext context);
        void OnSelectSpell_5(InputAction.CallbackContext context);
        void OnEndTurn(InputAction.CallbackContext context);
    }
}
