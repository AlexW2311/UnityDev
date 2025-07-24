using UnityEngine;

[RequireComponent(typeof(PlayerStateMachine))]
public class CrouchVisuals : MonoBehaviour
{
    [Header("Heights")]
    public float standingHeight = 2f;
    public float crouchHeight   = 1.0f;
    public float heightLerpSpeed = 12f;

    [Header("View Root (camera or parent)")]
    [SerializeField] private Transform viewRoot; // optional, auto-found if null

    private PlayerStateMachine stateMachine;
    private CapsuleCollider capsule;
    
    // cached values
    private float originalHeight;
    private Vector3 originalCenter;
    private float bottomOffset;

    private float startEyeY;
    private float crouchEyeY;

    private float targetHeight;

    void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
        capsule      = GetComponent<CapsuleCollider>();
        if (capsule == null) capsule = gameObject.AddComponent<CapsuleCollider>();

        if (viewRoot == null) viewRoot = FindViewRoot();
    }

    void Start()
    {
        // Use actual collider values as baseline
        originalHeight = capsule.height;
        originalCenter = capsule.center;
        bottomOffset   = originalCenter.y - originalHeight * 0.5f;

        // Override inspector defaults with real collider height if needed
        if (standingHeight <= 0f) standingHeight = originalHeight;
        targetHeight = standingHeight;

        startEyeY   = viewRoot.localPosition.y;
        crouchEyeY  = startEyeY * (crouchHeight / standingHeight);
    }

    void LateUpdate()
    {
        // Decide target from state
        targetHeight = stateMachine.IsCrouched ? crouchHeight : standingHeight;

        // Lerp collider
        float h = Mathf.Lerp(capsule.height, targetHeight, Time.deltaTime * heightLerpSpeed);
        capsule.height = h;

        float newCenterY = bottomOffset + h * 0.5f;
        capsule.center = new Vector3(originalCenter.x, newCenterY, originalCenter.z);

        // Normalize h for camera lerp
        float t = Mathf.InverseLerp(crouchHeight, standingHeight, h); // 0 crouched -> 1 standing

        Vector3 p = viewRoot.localPosition;
        p.y = Mathf.Lerp(crouchEyeY, startEyeY, t);
        viewRoot.localPosition = p;
    }

    private Transform FindViewRoot()
    {
        Transform t = transform.Find("ViewRoot");
        if (t != null) return t;

        Transform fpv = transform.Find("fpv_camera");
        if (fpv != null) return fpv;

        Camera cam = GetComponentInChildren<Camera>(true);
        return cam != null ? cam.transform : transform;
    }
}
