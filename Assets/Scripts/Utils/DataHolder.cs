using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{
    public static PlayerType �urrentPlayerType { get; set; }

    public enum PlayerType
    {
        hostPlayer,
        clientPlayer,
    }
}
