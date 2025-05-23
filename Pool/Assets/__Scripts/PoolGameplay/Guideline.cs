using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guideline : MonoBehaviour
{
    #region Singleton
        public static Guideline instance;
        private void Awake() {
            instance = this;
        }
    #endregion
    
    private LineRenderer lineRenderer;
    [SerializeField] private Transform circleIndicator;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        
        ToggleLineRenderer(true);
    }

    public void ToggleLineRenderer(bool toggleOn) {
        lineRenderer.enabled = toggleOn;
    }

    public void SetGuidelinePoints(Vector3[] points) {
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
    
    public void SetGuidelinePoints(Vector3[] points, Vector3 circleIndicatorPosition) {
        SetGuidelinePoints(points);
        circleIndicator.gameObject.SetActive(true);
        circleIndicator.transform.position = circleIndicatorPosition;
    }
}
