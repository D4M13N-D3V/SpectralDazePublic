
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [TextArea(3,10)]
    public string LineText;
    public Character LineCharacter;
}
