using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //TODO: Add reference to script, which contains control names
        //TODO: replace string identity to custom enum
        _keyMap.Add("interact", KeyCode.E);
        _keyMap.Add("move left", KeyCode.A);
        _keyMap.Add("move right", KeyCode.D);
        _keyMap.Add("jump", KeyCode.W);
        _keyMap.Add("flashlight", KeyCode.F);
        _keyMap.Add("sprint", KeyCode.LeftShift);
        _keyMap.Add("fire", KeyCode.Mouse0);
        _keyMap.Add("escape", KeyCode.Escape);
        _keyMap.Add("slot 1", KeyCode.Alpha1);
        _keyMap.Add("slot 2", KeyCode.Alpha2);
        _keyMap.Add("slot 3", KeyCode.Alpha3);
        _keyMap.Add("slot 4", KeyCode.Alpha4);
        _keyMap.Add("slot 5", KeyCode.Alpha5);
        _keyMap.Add("reload", KeyCode.R);
        _keyMap.Add("inventory", KeyCode.I);
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
