using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Input
{
    public class KeyboardControl
    {
        private static KeyboardControl _instance = null;
        
        private Dictionary<string, KeyCode> _keyMap;

        private float _sencityvity = 1f;

        public static KeyboardControl Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new KeyboardControl();
                return _instance;
            }
        }

        private KeyboardControl()
        {
            ResetKeyMap();
        }

        public void ResetKeyMap()
        {
            if (_keyMap != null)
                _keyMap.Clear();
            else
                _keyMap = new Dictionary<string, KeyCode>();

            _keyMap.Add(InputActionType.Interact,   KeyCode.E);
            _keyMap.Add(InputActionType.MoveLeft,   KeyCode.A);
            _keyMap.Add(InputActionType.MoveRight,  KeyCode.D);
            _keyMap.Add(InputActionType.Jump,       KeyCode.W);
            _keyMap.Add(InputActionType.Flashlight, KeyCode.F);
            _keyMap.Add(InputActionType.Sprint,     KeyCode.LeftShift);
            _keyMap.Add(InputActionType.Fire,       KeyCode.Mouse0);
            _keyMap.Add(InputActionType.Escape,     KeyCode.Escape);
            _keyMap.Add(InputActionType.Slot_1,     KeyCode.Alpha1);
            _keyMap.Add(InputActionType.Slot_2,     KeyCode.Alpha2);
            _keyMap.Add(InputActionType.Slot_3,     KeyCode.Alpha3);
            _keyMap.Add(InputActionType.Slot_4,     KeyCode.Alpha4);
            _keyMap.Add(InputActionType.Slot_5,     KeyCode.Alpha5);
            _keyMap.Add(InputActionType.Reload,     KeyCode.R);
            _keyMap.Add(InputActionType.Inventory,  KeyCode.I);
        }
        
        public KeyCode GetKeyByName(string name)
        {
            KeyCode result = KeyCode.None;
            if (_keyMap.TryGetValue(name, out result))
                return result;
            throw new System.Exception("missing key reference");
        }
        
        public float mouseSencitivity
        {
            get
            {
                return _sencitivity;
            }
            set
            {
                if (value > 0 && value < 1)
                {
                    _sencitivity = value;
                }
                else
                {
                    throw new System.Exception("invalid mouse sensitivity argument");
                }
            }
        }

        public bool Reset(string name, KeyCode newInput)
        {
            if (KeyIsUnique(name, newInput))
            {
                _keyMap[name] = newInput;
                return true;
            }
            return false;
        }

        private bool KeyIsUnique(string name, KeyCode newKey)
        {
            foreach (var inputMethod in _keyMap)
            {
                if (inputMethod.Value == newKey && inputMethod.Key != name)
                    return false;
            }
            return true;
        }
    }

    public enum InputActionType
    {
        Interact,
        MoveLeft,
        MoveRight,
        Jump,
        Flashlight,
        Sprint,
        Fire,
        Escape,
        Slot_1,
        Slot_2,
        Slot_3,
        Slot_4,
        Slot_5,
        Reload,
        Inventory,
    }
}
