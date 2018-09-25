using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A monobehaviour that holds information that the Dialogue Manager can access.
/// </summary>
public class DialogueUI : MonoBehaviour
{
    /// <summary>
    /// The parent of everything related to the Dialogue UI. ( should be another child of canvas)
    /// </summary>
    public GameObject Parent;

    /// <summary>
    /// The panel that the character stuff is children to.
    /// </summary>
    public GameObject CharacterView;
    /// <summary>
    /// The character name text label
    /// </summary>
    public Text CharacterName;
    /// <summary>
    /// The character profile raw image
    /// </summary>
    public RawImage CharacterImage;

    /// <summary>
    /// The message scrollbox for messages
    /// </summary>
    public GameObject MessageView;
    /// <summary>
    /// The options scrollbox for options
    /// </summary>
    public GameObject OptionsView;
    /// <summary>
    /// The message scrollbox content view for options
    /// </summary>
    public GameObject OptionsViewButtonContainer;
    /// <summary>
    /// The message text label
    /// </summary>
    public Text MessageText;
}
