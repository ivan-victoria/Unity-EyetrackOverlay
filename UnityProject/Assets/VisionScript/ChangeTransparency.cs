//-----------------------------------------------------------------------
// Copyright 2016 Tobii AB (publ). All rights reserved.
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Tobii.Gaming;

/// <summary>
/// Changes the color of the game object's material, when the the game object 
/// is in focus of the user's eye-gaze.
/// </summary>
/// <remarks>
/// Referenced by the Target game objects in the Simple Gaze Selection example scene.
/// </remarks>
[RequireComponent(typeof(GazeAware))]
[RequireComponent(typeof(MeshRenderer))]

public class ChangeTransparency : MonoBehaviour
{

    public Color selectionColor;
    public GameObject text;
    private GazeAware _gazeAwareComponent;
    private UserPresence _userPresence;
    private MeshRenderer _meshRenderer;
    // public Object texta;
    public Color textcolor;
    private Color _deselectionColor;
    // private Color _deselectionColorText;
    private Color _lerpColor;
    private float _fadeSpeed = 0.1f;

    public bool enableHide;
    public bool enableDesktopHide;
    private int counter = 0;

    TextMesh textmesh;
    /// <summary>
    /// Set the lerp color
    /// </summary>
    void Start()
    {
        _gazeAwareComponent = GetComponent<GazeAware>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _lerpColor = _meshRenderer.material.color;
        _deselectionColor = new Color(1f, 1f, 1f, 0.2f);
        _userPresence = TobiiAPI.GetUserPresence();
        //  _deselectionColorText = new Color(1f, 1f, 1f, 0.4f);
        //  textcolor = new Color(0f,0f,0f,1f);
    }

    /// <summary>
    /// Lerping the color
    /// </summary>
    void Update()
    {

        if (_meshRenderer.material.color != _lerpColor)
        {
            _meshRenderer.material.color = Color.Lerp(_meshRenderer.material.color, _lerpColor, _fadeSpeed);
            text.GetComponent<TextMesh>().color = Color.Lerp(_meshRenderer.material.color, textcolor, _fadeSpeed);
            //  Color.black;

        }

        // Change the color of the cube
        if (_gazeAwareComponent.HasGazeFocus)
        {
            SetLerpColor(selectionColor);
            text.GetComponent<TextMesh>().color = textcolor;

        }
        else
        {
            SetLerpColor(_deselectionColor);
        }

        if (enableHide) {

            if (_gazeAwareComponent.HasGazeFocus)
            {
                counter++;
            }

            else
            {
                if (counter > 0)
                {
                    _meshRenderer.enabled = false;
                    text.SetActive(false);
                }
            }

            if (text.activeSelf == false)
            {
                StartCoroutine(updateCoroutine(5, true));
                counter = 0;
            }

        }


        if (enableDesktopHide)
        {
            _userPresence = TobiiAPI.GetUserPresence();
            if (_userPresence.IsUserPresent())
            {
             //   print("A user is present in front of the screen.");
                if (text.activeSelf == false)
                {
                    StartCoroutine(updateCoroutine(1, true));
                }
            }
            else
            {
              //  print("User presence status is: " + _userPresence);
                StartCoroutine(updateCoroutine(5, false));
            }
            
        }
    }

    /// <summary>
    /// Update the color, which should used for the lerping
    /// </summary>
    /// <param name="lerpColor"></param>
    public void SetLerpColor(Color lerpColor)
    {
        this._lerpColor = lerpColor;
    }

    private IEnumerator updateCoroutine(int timer, bool enabler )
    {
        yield return new WaitForSecondsRealtime(timer);
        _meshRenderer.enabled = enabler;
        text.SetActive(enabler);
    }

}
