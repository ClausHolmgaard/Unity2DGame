using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Stat {

    [SerializeField]
    private BarHandler bar;

    private float _maxValue;
    private float _currentValue;

    public float maxValue {
        get {
            return _maxValue;
        }

        set {
            _maxValue = value;
            if (bar != null) {
                bar.maxValue = _maxValue;
            }
        }
    }

    public float currentValue {
        get {
            return _currentValue;
        }

        set {
            _currentValue = Mathf.Clamp(value, 0, maxValue);
            if (bar != null) {
                bar.setValue(_currentValue);
            }
        }
    }

    public void reduceValue(float val) {
        currentValue -= val;
    }

    public void increaseValue(float val) {
        currentValue += val;
    }

}
