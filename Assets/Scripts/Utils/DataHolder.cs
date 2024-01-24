using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
    public static PlayerType ÑurrentPlayerType { get; set; }

    public enum PlayerType
    {
        hostPlayer,
        clientPlayer,
    }
}
