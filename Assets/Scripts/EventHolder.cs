using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHolder
{
    public static Action OnSomeCellChanged;
    public static Action<PathData> OnPathBuilded;
}
