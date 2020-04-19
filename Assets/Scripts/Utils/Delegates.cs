using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delegates
{
    // I know there is a more standard way of doign this in C# but tihs works
    // too
    public delegate void EmptyDel();

    // Delegate used to start a transition countdown
    public delegate void TransitionDel(string name, float timeLeft);

    // We use this in a timer to chek if we should continue or not
    public delegate bool CheckAndMaybeContinueDel();
}
