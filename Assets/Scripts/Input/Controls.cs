//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/Input/Controls.inputactions
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

public partial class @Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""SigilCreatingUI"",
            ""id"": ""70420c8d-d3f9-448f-8958-45cc3dab5a28"",
            ""actions"": [
                {
                    ""name"": ""Delete"",
                    ""type"": ""Button"",
                    ""id"": ""8ccd02ad-9358-40c5-8942-090c9ba0c56f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""New"",
                    ""type"": ""Button"",
                    ""id"": ""a66a0e3a-9d37-4740-bae6-476a81c901cf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectRecognized"",
                    ""type"": ""Button"",
                    ""id"": ""474c8e53-f27c-47f0-9536-8afa738bea6b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3b6d1181-9c7f-4cb1-9f5b-518a3e913739"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90de0f14-f3c7-43f1-936f-b5a9c01846b6"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87718150-44f6-4e8f-9a14-56b0f5503ca4"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectRecognized"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Move"",
            ""id"": ""b247404a-9fcf-4bf3-a0c2-2ea73f69ce4d"",
            ""actions"": [
                {
                    ""name"": ""Direction"",
                    ""type"": ""Value"",
                    ""id"": ""61dcbc09-655d-46da-9e83-ef90cfa56d5b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4271fd62-f102-4561-af8e-78397c95c3ce"",
                    ""path"": ""<Joystick>/stick"",
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
            ""name"": ""Draw"",
            ""id"": ""8180be9d-9e8f-4520-9b34-6fe273809ff1"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""03add1ce-e451-4256-9d1c-458d0e496335"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Contact"",
                    ""type"": ""Button"",
                    ""id"": ""7452fa12-faa5-466c-a921-78453d0a5976"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""94787778-87a4-4178-9976-1ab7126ef9d9"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""528ebb1c-0209-488a-8723-1e34f407391f"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Contact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Aim"",
            ""id"": ""8e65c0ed-8c57-427d-abf3-117dc72e6565"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""3f734fce-d8f1-44c0-8e9b-6e250165294d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Contact"",
                    ""type"": ""Button"",
                    ""id"": ""e75f59a8-fd81-4005-8462-7a953ef7a520"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f1ff5918-d48e-4057-96fb-2dc191e6b422"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e355618-4ce9-4dce-a24d-776b9fb1fdff"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Contact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Swing"",
            ""id"": ""0e4dd5a9-0596-4958-858a-930817ae8404"",
            ""actions"": [
                {
                    ""name"": ""Direction"",
                    ""type"": ""Value"",
                    ""id"": ""3fe8d924-6c4e-4b06-b620-8991b7e77972"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Contact"",
                    ""type"": ""Button"",
                    ""id"": ""f7cd804e-a0c9-4a4a-95f6-d56967637271"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2fc436a5-22c2-48f3-8f3e-2b7de1ee7f15"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8dc6f905-0318-4315-9c79-40d6d6cd861d"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Contact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // SigilCreatingUI
        m_SigilCreatingUI = asset.FindActionMap("SigilCreatingUI", throwIfNotFound: true);
        m_SigilCreatingUI_Delete = m_SigilCreatingUI.FindAction("Delete", throwIfNotFound: true);
        m_SigilCreatingUI_New = m_SigilCreatingUI.FindAction("New", throwIfNotFound: true);
        m_SigilCreatingUI_SelectRecognized = m_SigilCreatingUI.FindAction("SelectRecognized", throwIfNotFound: true);
        // Move
        m_Move = asset.FindActionMap("Move", throwIfNotFound: true);
        m_Move_Direction = m_Move.FindAction("Direction", throwIfNotFound: true);
        // Draw
        m_Draw = asset.FindActionMap("Draw", throwIfNotFound: true);
        m_Draw_Position = m_Draw.FindAction("Position", throwIfNotFound: true);
        m_Draw_Contact = m_Draw.FindAction("Contact", throwIfNotFound: true);
        // Aim
        m_Aim = asset.FindActionMap("Aim", throwIfNotFound: true);
        m_Aim_Position = m_Aim.FindAction("Position", throwIfNotFound: true);
        m_Aim_Contact = m_Aim.FindAction("Contact", throwIfNotFound: true);
        // Swing
        m_Swing = asset.FindActionMap("Swing", throwIfNotFound: true);
        m_Swing_Direction = m_Swing.FindAction("Direction", throwIfNotFound: true);
        m_Swing_Contact = m_Swing.FindAction("Contact", throwIfNotFound: true);
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

    // SigilCreatingUI
    private readonly InputActionMap m_SigilCreatingUI;
    private List<ISigilCreatingUIActions> m_SigilCreatingUIActionsCallbackInterfaces = new List<ISigilCreatingUIActions>();
    private readonly InputAction m_SigilCreatingUI_Delete;
    private readonly InputAction m_SigilCreatingUI_New;
    private readonly InputAction m_SigilCreatingUI_SelectRecognized;
    public struct SigilCreatingUIActions
    {
        private @Controls m_Wrapper;
        public SigilCreatingUIActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Delete => m_Wrapper.m_SigilCreatingUI_Delete;
        public InputAction @New => m_Wrapper.m_SigilCreatingUI_New;
        public InputAction @SelectRecognized => m_Wrapper.m_SigilCreatingUI_SelectRecognized;
        public InputActionMap Get() { return m_Wrapper.m_SigilCreatingUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SigilCreatingUIActions set) { return set.Get(); }
        public void AddCallbacks(ISigilCreatingUIActions instance)
        {
            if (instance == null || m_Wrapper.m_SigilCreatingUIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_SigilCreatingUIActionsCallbackInterfaces.Add(instance);
            @Delete.started += instance.OnDelete;
            @Delete.performed += instance.OnDelete;
            @Delete.canceled += instance.OnDelete;
            @New.started += instance.OnNew;
            @New.performed += instance.OnNew;
            @New.canceled += instance.OnNew;
            @SelectRecognized.started += instance.OnSelectRecognized;
            @SelectRecognized.performed += instance.OnSelectRecognized;
            @SelectRecognized.canceled += instance.OnSelectRecognized;
        }

        private void UnregisterCallbacks(ISigilCreatingUIActions instance)
        {
            @Delete.started -= instance.OnDelete;
            @Delete.performed -= instance.OnDelete;
            @Delete.canceled -= instance.OnDelete;
            @New.started -= instance.OnNew;
            @New.performed -= instance.OnNew;
            @New.canceled -= instance.OnNew;
            @SelectRecognized.started -= instance.OnSelectRecognized;
            @SelectRecognized.performed -= instance.OnSelectRecognized;
            @SelectRecognized.canceled -= instance.OnSelectRecognized;
        }

        public void RemoveCallbacks(ISigilCreatingUIActions instance)
        {
            if (m_Wrapper.m_SigilCreatingUIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ISigilCreatingUIActions instance)
        {
            foreach (var item in m_Wrapper.m_SigilCreatingUIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_SigilCreatingUIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public SigilCreatingUIActions @SigilCreatingUI => new SigilCreatingUIActions(this);

    // Move
    private readonly InputActionMap m_Move;
    private List<IMoveActions> m_MoveActionsCallbackInterfaces = new List<IMoveActions>();
    private readonly InputAction m_Move_Direction;
    public struct MoveActions
    {
        private @Controls m_Wrapper;
        public MoveActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Direction => m_Wrapper.m_Move_Direction;
        public InputActionMap Get() { return m_Wrapper.m_Move; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveActions set) { return set.Get(); }
        public void AddCallbacks(IMoveActions instance)
        {
            if (instance == null || m_Wrapper.m_MoveActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MoveActionsCallbackInterfaces.Add(instance);
            @Direction.started += instance.OnDirection;
            @Direction.performed += instance.OnDirection;
            @Direction.canceled += instance.OnDirection;
        }

        private void UnregisterCallbacks(IMoveActions instance)
        {
            @Direction.started -= instance.OnDirection;
            @Direction.performed -= instance.OnDirection;
            @Direction.canceled -= instance.OnDirection;
        }

        public void RemoveCallbacks(IMoveActions instance)
        {
            if (m_Wrapper.m_MoveActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMoveActions instance)
        {
            foreach (var item in m_Wrapper.m_MoveActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MoveActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MoveActions @Move => new MoveActions(this);

    // Draw
    private readonly InputActionMap m_Draw;
    private List<IDrawActions> m_DrawActionsCallbackInterfaces = new List<IDrawActions>();
    private readonly InputAction m_Draw_Position;
    private readonly InputAction m_Draw_Contact;
    public struct DrawActions
    {
        private @Controls m_Wrapper;
        public DrawActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_Draw_Position;
        public InputAction @Contact => m_Wrapper.m_Draw_Contact;
        public InputActionMap Get() { return m_Wrapper.m_Draw; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DrawActions set) { return set.Get(); }
        public void AddCallbacks(IDrawActions instance)
        {
            if (instance == null || m_Wrapper.m_DrawActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DrawActionsCallbackInterfaces.Add(instance);
            @Position.started += instance.OnPosition;
            @Position.performed += instance.OnPosition;
            @Position.canceled += instance.OnPosition;
            @Contact.started += instance.OnContact;
            @Contact.performed += instance.OnContact;
            @Contact.canceled += instance.OnContact;
        }

        private void UnregisterCallbacks(IDrawActions instance)
        {
            @Position.started -= instance.OnPosition;
            @Position.performed -= instance.OnPosition;
            @Position.canceled -= instance.OnPosition;
            @Contact.started -= instance.OnContact;
            @Contact.performed -= instance.OnContact;
            @Contact.canceled -= instance.OnContact;
        }

        public void RemoveCallbacks(IDrawActions instance)
        {
            if (m_Wrapper.m_DrawActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDrawActions instance)
        {
            foreach (var item in m_Wrapper.m_DrawActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DrawActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DrawActions @Draw => new DrawActions(this);

    // Aim
    private readonly InputActionMap m_Aim;
    private List<IAimActions> m_AimActionsCallbackInterfaces = new List<IAimActions>();
    private readonly InputAction m_Aim_Position;
    private readonly InputAction m_Aim_Contact;
    public struct AimActions
    {
        private @Controls m_Wrapper;
        public AimActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_Aim_Position;
        public InputAction @Contact => m_Wrapper.m_Aim_Contact;
        public InputActionMap Get() { return m_Wrapper.m_Aim; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AimActions set) { return set.Get(); }
        public void AddCallbacks(IAimActions instance)
        {
            if (instance == null || m_Wrapper.m_AimActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_AimActionsCallbackInterfaces.Add(instance);
            @Position.started += instance.OnPosition;
            @Position.performed += instance.OnPosition;
            @Position.canceled += instance.OnPosition;
            @Contact.started += instance.OnContact;
            @Contact.performed += instance.OnContact;
            @Contact.canceled += instance.OnContact;
        }

        private void UnregisterCallbacks(IAimActions instance)
        {
            @Position.started -= instance.OnPosition;
            @Position.performed -= instance.OnPosition;
            @Position.canceled -= instance.OnPosition;
            @Contact.started -= instance.OnContact;
            @Contact.performed -= instance.OnContact;
            @Contact.canceled -= instance.OnContact;
        }

        public void RemoveCallbacks(IAimActions instance)
        {
            if (m_Wrapper.m_AimActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IAimActions instance)
        {
            foreach (var item in m_Wrapper.m_AimActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_AimActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public AimActions @Aim => new AimActions(this);

    // Swing
    private readonly InputActionMap m_Swing;
    private List<ISwingActions> m_SwingActionsCallbackInterfaces = new List<ISwingActions>();
    private readonly InputAction m_Swing_Direction;
    private readonly InputAction m_Swing_Contact;
    public struct SwingActions
    {
        private @Controls m_Wrapper;
        public SwingActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Direction => m_Wrapper.m_Swing_Direction;
        public InputAction @Contact => m_Wrapper.m_Swing_Contact;
        public InputActionMap Get() { return m_Wrapper.m_Swing; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SwingActions set) { return set.Get(); }
        public void AddCallbacks(ISwingActions instance)
        {
            if (instance == null || m_Wrapper.m_SwingActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_SwingActionsCallbackInterfaces.Add(instance);
            @Direction.started += instance.OnDirection;
            @Direction.performed += instance.OnDirection;
            @Direction.canceled += instance.OnDirection;
            @Contact.started += instance.OnContact;
            @Contact.performed += instance.OnContact;
            @Contact.canceled += instance.OnContact;
        }

        private void UnregisterCallbacks(ISwingActions instance)
        {
            @Direction.started -= instance.OnDirection;
            @Direction.performed -= instance.OnDirection;
            @Direction.canceled -= instance.OnDirection;
            @Contact.started -= instance.OnContact;
            @Contact.performed -= instance.OnContact;
            @Contact.canceled -= instance.OnContact;
        }

        public void RemoveCallbacks(ISwingActions instance)
        {
            if (m_Wrapper.m_SwingActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ISwingActions instance)
        {
            foreach (var item in m_Wrapper.m_SwingActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_SwingActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public SwingActions @Swing => new SwingActions(this);
    public interface ISigilCreatingUIActions
    {
        void OnDelete(InputAction.CallbackContext context);
        void OnNew(InputAction.CallbackContext context);
        void OnSelectRecognized(InputAction.CallbackContext context);
    }
    public interface IMoveActions
    {
        void OnDirection(InputAction.CallbackContext context);
    }
    public interface IDrawActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnContact(InputAction.CallbackContext context);
    }
    public interface IAimActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnContact(InputAction.CallbackContext context);
    }
    public interface ISwingActions
    {
        void OnDirection(InputAction.CallbackContext context);
        void OnContact(InputAction.CallbackContext context);
    }
}
