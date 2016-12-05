using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {

    private new Transform transform;
    private Camera cam;


    private FocusArea focusArea;                            // boundary
    private CameraBound cameraViewBound;                    // camera boundary


    public bool drawFocusArea = false;
    public bool drawCameraBound = false;
    public float height = 0f;
    public float cameraMoveSmoothTime = 0.8f;
    public float cameraZoomSmoothTime = 2f;
    public float cameraFrustumHeightLimit = 8;

    public CameraBoundObject[] bounds;

    float smoothX, smoothY, smoothZ;

    void Awake() {
        transform = GetComponent<Transform>();
        cam = GetComponent<Camera>();

        focusArea = new FocusArea(-Mathf.Infinity, Mathf.Infinity, Mathf.Infinity, -Mathf.Infinity, height);
        cameraViewBound = new CameraBound(focusArea, cameraFrustumHeightLimit, cam.aspect);
    }

    void OnDrawGizmos() {
        if ( drawCameraBound ) {
            Gizmos.color = new Color(0, 0, 1f, 0.2f);
            Gizmos.DrawCube(focusArea.center, new Vector3(cameraViewBound.right-cameraViewBound.left, 0, cameraViewBound.top-cameraViewBound.bottom));
        }
        if ( drawFocusArea ) {
            Gizmos.color = new Color(1f, 0, 0, 0.2f);
            Gizmos.DrawCube(focusArea.center, new Vector3(focusArea.right-focusArea.left, 0, focusArea.top-focusArea.bottom));
        }
    }

    void LateUpdate() {

        focusArea.Update(bounds, height);
        cameraViewBound.Update(focusArea, cameraFrustumHeightLimit, cam.aspect);

        Vector3 focusPoint = focusArea.center;

        float distanceHeight = (cameraViewBound.top-cameraViewBound.bottom) * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float distanceWidth = (cameraViewBound.right-cameraViewBound.left) * 0.5f / (Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * cam.aspect);
        float distance = distanceHeight > distanceWidth ? distanceHeight : distanceWidth;

        focusPoint.x = Mathf.SmoothDamp(transform.position.x, focusPoint.x, ref smoothX, cameraMoveSmoothTime);
        focusPoint.z = Mathf.SmoothDamp(transform.position.z, focusPoint.z, ref smoothZ, cameraMoveSmoothTime);
        focusPoint.y = Mathf.SmoothDamp(transform.position.y, distance, ref smoothY, cameraZoomSmoothTime);


        transform.position = focusPoint;
    }

    struct FocusArea {
        public float left, right,
            top, bottom;

        public Vector3 center;

        public FocusArea(float l, float r, float t, float b, float d) {
            left = l;
            right = r;
            top = t;
            bottom = b;
            center = new Vector3((left+right)/2, d, (bottom+top)/2);
        }

        // Update boundary
        public void Update(CameraBoundObject[] bounds, float d) {
            for ( int i = 0; i < bounds.Length; i++ ) {
                if ( i == 0 ) {
                    left = bounds[i].getBound.left;
                    right = bounds[i].getBound.right;
                    top = bounds[i].getBound.top;
                    bottom = bounds[i].getBound.bottom;
                } else {

                    // left
                    if ( bounds[i].getBound.left < left ) {
                        left = bounds[i].getBound.left;
                    }

                    // right
                    if ( bounds[i].getBound.right > right ) {
                        right = bounds[i].getBound.right;
                    }

                    // top
                    if ( bounds[i].getBound.top > top ) {
                        top = bounds[i].getBound.top;
                    }

                    // bottom
                    if ( bounds[i].getBound.bottom < bottom ) {
                        bottom = bounds[i].getBound.bottom;
                    }
                }
            }// endfor

            center = new Vector3((left+right)/2, d, (bottom+top)/2);
        }
    };

    struct CameraBound {
        public float left, right;
        public float top, bottom;

        public CameraBound(FocusArea focusArea, float heightLimit, float aspect) {
            if ( (focusArea.top - focusArea.bottom) < heightLimit ) {
                top = focusArea.center.y + (heightLimit/2);
                bottom = focusArea.center.y - (heightLimit/2);
            } else {
                top = focusArea.top;
                bottom = focusArea.bottom;
            }

            float widthLimit = (heightLimit * aspect);
            if ( (focusArea.right - focusArea.left) < widthLimit ) {
                right = focusArea.center.x + (widthLimit/2);
                left = focusArea.center.x - (widthLimit/2);
            } else {
                right = focusArea.right;
                left = focusArea.left;
            }

        }
        public void Update(FocusArea focusArea, float heightLimit, float aspect) {

            if ( (focusArea.top - focusArea.bottom) < heightLimit ) {
                top = focusArea.center.y + (heightLimit/2);
                bottom = focusArea.center.y - (heightLimit/2);
            } else {
                top = focusArea.top;
                bottom = focusArea.bottom;
            }

            float widthLimit = (heightLimit * aspect);
            if ( (focusArea.right - focusArea.left) < widthLimit ) {
                right = focusArea.center.x + (widthLimit/2);
                left = focusArea.center.x - (widthLimit/2);
            } else {
                right = focusArea.right;
                left = focusArea.left;
            }
            
        }
    };
}
