using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHasProgress
{
    public event EventHandler<OnProgressChangedEventAarry> OnProgressChanged;
    public class OnProgressChangedEventAarry : EventArgs
    {
        public float progressNormalized;
    }
}
