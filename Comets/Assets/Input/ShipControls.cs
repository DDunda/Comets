//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Input/ShipControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ShipControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ShipControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ShipControls"",
    ""maps"": [
        {
            ""name"": ""Ship"",
            ""id"": ""7565c5ba-9339-4721-bfa5-d33c2154a9f3"",
            ""actions"": [
                {
                    ""name"": ""Turn"",
                    ""type"": ""Value"",
                    ""id"": ""cf63786b-1156-4752-9f9f-23993c1e9d78"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Accelerate"",
                    ""type"": ""Value"",
                    ""id"": ""e0f254e8-b377-4eaa-8516-14c37075630b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""6b68f51e-1443-44d6-a692-22e6e6d5cfea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Button"",
                    ""id"": ""ea2ab490-f907-42b5-8b6a-51f9db0d1efe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Direction"",
                    ""type"": ""Value"",
                    ""id"": ""a79253c9-605d-4915-9856-67bafe5c22fe"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard (WASD)"",
                    ""id"": ""3d19a1ff-324a-43e7-93f5-cd04f33bc8b3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a50df9e5-a3be-4594-b965-e6b17ceb4404"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard (Arrows)"",
                    ""id"": ""6f834ca8-55d4-456a-a5ce-b33600006fb4"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""4f0df2f8-4da0-400f-891a-611c4ae12854"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""de2eaa53-78d6-400c-bf67-91fea8caad49"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""52273424-7ef1-4280-b1e7-55487d5cca2a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""103e97da-7833-4b94-a95f-9930c12de9df"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc5db02a-91fd-497d-bbac-0703eb6bbc44"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae809955-b103-4bff-a2d5-d496a453ca22"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d22348fd-526a-41b4-8838-f99d16fa5e28"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89fbab2e-2870-45b6-acf4-33b6a0feda71"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd23253a-4607-4d6c-a255-7c5fde80b94f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d4f033f-7648-447d-84a2-30de1de8f295"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94e05670-cb4c-4a20-9d0d-cef592768b51"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14214d23-06d6-40c2-8876-7e9c05060fd1"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf56e13f-2d4a-4d35-9566-5d919c8ada63"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Ship (Debug)"",
            ""id"": ""821c3dff-1bcb-48f4-98c8-e4c80b9c6ff3"",
            ""actions"": [
                {
                    ""name"": ""FreezeSpeed"",
                    ""type"": ""Button"",
                    ""id"": ""26a9ea59-777c-4bda-9bed-b38c71654a07"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WarpToCenter"",
                    ""type"": ""Button"",
                    ""id"": ""453fae57-4696-495a-885e-a6f560389633"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpawnComets"",
                    ""type"": ""Button"",
                    ""id"": ""63af5ab0-0fe5-42a2-9e3a-8cf8148511df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""41b596e8-fba5-4e78-8bcb-3d81669d78ad"",
                    ""path"": ""<Keyboard>/#(X)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FreezeSpeed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a798fdeb-fe18-46cf-ba66-f59576bf210c"",
                    ""path"": ""<Keyboard>/#(C)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WarpToCenter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b114e7db-2697-49ed-ae14-b19a78d6734f"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpawnComets"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Ship
        m_Ship = asset.FindActionMap("Ship", throwIfNotFound: true);
        m_Ship_Turn = m_Ship.FindAction("Turn", throwIfNotFound: true);
        m_Ship_Accelerate = m_Ship.FindAction("Accelerate", throwIfNotFound: true);
        m_Ship_Fire = m_Ship.FindAction("Fire", throwIfNotFound: true);
        m_Ship_Brake = m_Ship.FindAction("Brake", throwIfNotFound: true);
        m_Ship_Direction = m_Ship.FindAction("Direction", throwIfNotFound: true);
        // Ship (Debug)
        m_ShipDebug = asset.FindActionMap("Ship (Debug)", throwIfNotFound: true);
        m_ShipDebug_FreezeSpeed = m_ShipDebug.FindAction("FreezeSpeed", throwIfNotFound: true);
        m_ShipDebug_WarpToCenter = m_ShipDebug.FindAction("WarpToCenter", throwIfNotFound: true);
        m_ShipDebug_SpawnComets = m_ShipDebug.FindAction("SpawnComets", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Ship
    private readonly InputActionMap m_Ship;
    private IShipActions m_ShipActionsCallbackInterface;
    private readonly InputAction m_Ship_Turn;
    private readonly InputAction m_Ship_Accelerate;
    private readonly InputAction m_Ship_Fire;
    private readonly InputAction m_Ship_Brake;
    private readonly InputAction m_Ship_Direction;
    public struct ShipActions
    {
        private @ShipControls m_Wrapper;
        public ShipActions(@ShipControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Turn => m_Wrapper.m_Ship_Turn;
        public InputAction @Accelerate => m_Wrapper.m_Ship_Accelerate;
        public InputAction @Fire => m_Wrapper.m_Ship_Fire;
        public InputAction @Brake => m_Wrapper.m_Ship_Brake;
        public InputAction @Direction => m_Wrapper.m_Ship_Direction;
        public InputActionMap Get() { return m_Wrapper.m_Ship; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShipActions set) { return set.Get(); }
        public void SetCallbacks(IShipActions instance)
        {
            if (m_Wrapper.m_ShipActionsCallbackInterface != null)
            {
                @Turn.started -= m_Wrapper.m_ShipActionsCallbackInterface.OnTurn;
                @Turn.performed -= m_Wrapper.m_ShipActionsCallbackInterface.OnTurn;
                @Turn.canceled -= m_Wrapper.m_ShipActionsCallbackInterface.OnTurn;
                @Accelerate.started -= m_Wrapper.m_ShipActionsCallbackInterface.OnAccelerate;
                @Accelerate.performed -= m_Wrapper.m_ShipActionsCallbackInterface.OnAccelerate;
                @Accelerate.canceled -= m_Wrapper.m_ShipActionsCallbackInterface.OnAccelerate;
                @Fire.started -= m_Wrapper.m_ShipActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_ShipActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_ShipActionsCallbackInterface.OnFire;
                @Brake.started -= m_Wrapper.m_ShipActionsCallbackInterface.OnBrake;
                @Brake.performed -= m_Wrapper.m_ShipActionsCallbackInterface.OnBrake;
                @Brake.canceled -= m_Wrapper.m_ShipActionsCallbackInterface.OnBrake;
                @Direction.started -= m_Wrapper.m_ShipActionsCallbackInterface.OnDirection;
                @Direction.performed -= m_Wrapper.m_ShipActionsCallbackInterface.OnDirection;
                @Direction.canceled -= m_Wrapper.m_ShipActionsCallbackInterface.OnDirection;
            }
            m_Wrapper.m_ShipActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Turn.started += instance.OnTurn;
                @Turn.performed += instance.OnTurn;
                @Turn.canceled += instance.OnTurn;
                @Accelerate.started += instance.OnAccelerate;
                @Accelerate.performed += instance.OnAccelerate;
                @Accelerate.canceled += instance.OnAccelerate;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Brake.started += instance.OnBrake;
                @Brake.performed += instance.OnBrake;
                @Brake.canceled += instance.OnBrake;
                @Direction.started += instance.OnDirection;
                @Direction.performed += instance.OnDirection;
                @Direction.canceled += instance.OnDirection;
            }
        }
    }
    public ShipActions @Ship => new ShipActions(this);

    // Ship (Debug)
    private readonly InputActionMap m_ShipDebug;
    private IShipDebugActions m_ShipDebugActionsCallbackInterface;
    private readonly InputAction m_ShipDebug_FreezeSpeed;
    private readonly InputAction m_ShipDebug_WarpToCenter;
    private readonly InputAction m_ShipDebug_SpawnComets;
    public struct ShipDebugActions
    {
        private @ShipControls m_Wrapper;
        public ShipDebugActions(@ShipControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @FreezeSpeed => m_Wrapper.m_ShipDebug_FreezeSpeed;
        public InputAction @WarpToCenter => m_Wrapper.m_ShipDebug_WarpToCenter;
        public InputAction @SpawnComets => m_Wrapper.m_ShipDebug_SpawnComets;
        public InputActionMap Get() { return m_Wrapper.m_ShipDebug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShipDebugActions set) { return set.Get(); }
        public void SetCallbacks(IShipDebugActions instance)
        {
            if (m_Wrapper.m_ShipDebugActionsCallbackInterface != null)
            {
                @FreezeSpeed.started -= m_Wrapper.m_ShipDebugActionsCallbackInterface.OnFreezeSpeed;
                @FreezeSpeed.performed -= m_Wrapper.m_ShipDebugActionsCallbackInterface.OnFreezeSpeed;
                @FreezeSpeed.canceled -= m_Wrapper.m_ShipDebugActionsCallbackInterface.OnFreezeSpeed;
                @WarpToCenter.started -= m_Wrapper.m_ShipDebugActionsCallbackInterface.OnWarpToCenter;
                @WarpToCenter.performed -= m_Wrapper.m_ShipDebugActionsCallbackInterface.OnWarpToCenter;
                @WarpToCenter.canceled -= m_Wrapper.m_ShipDebugActionsCallbackInterface.OnWarpToCenter;
                @SpawnComets.started -= m_Wrapper.m_ShipDebugActionsCallbackInterface.OnSpawnComets;
                @SpawnComets.performed -= m_Wrapper.m_ShipDebugActionsCallbackInterface.OnSpawnComets;
                @SpawnComets.canceled -= m_Wrapper.m_ShipDebugActionsCallbackInterface.OnSpawnComets;
            }
            m_Wrapper.m_ShipDebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @FreezeSpeed.started += instance.OnFreezeSpeed;
                @FreezeSpeed.performed += instance.OnFreezeSpeed;
                @FreezeSpeed.canceled += instance.OnFreezeSpeed;
                @WarpToCenter.started += instance.OnWarpToCenter;
                @WarpToCenter.performed += instance.OnWarpToCenter;
                @WarpToCenter.canceled += instance.OnWarpToCenter;
                @SpawnComets.started += instance.OnSpawnComets;
                @SpawnComets.performed += instance.OnSpawnComets;
                @SpawnComets.canceled += instance.OnSpawnComets;
            }
        }
    }
    public ShipDebugActions @ShipDebug => new ShipDebugActions(this);
    public interface IShipActions
    {
        void OnTurn(InputAction.CallbackContext context);
        void OnAccelerate(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnBrake(InputAction.CallbackContext context);
        void OnDirection(InputAction.CallbackContext context);
    }
    public interface IShipDebugActions
    {
        void OnFreezeSpeed(InputAction.CallbackContext context);
        void OnWarpToCenter(InputAction.CallbackContext context);
        void OnSpawnComets(InputAction.CallbackContext context);
    }
}
