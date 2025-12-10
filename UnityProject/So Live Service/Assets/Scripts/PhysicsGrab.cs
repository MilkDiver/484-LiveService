using UnityEngine;

public class PhysicsGrab : MonoBehaviour
{
    [Header("References")]
    public Camera cam;

    [Header("Grab settings")]
    public float grabRange = 5f;                 // how far you can reach to pick up objects
    public float maxHoldDistance = 1.5f;         // maximum distance of the hold point from camera
    public float moveSpeed = 5f;                 // units per second the grabbed object follows
    public float rotateSpeed = 720f;             // degrees per second for rotation

    [Tooltip("Set to true while the player is holding the grab button (input should set this).")]
    public bool tryGrabbing = false;

    [SerializeField] private Rigidbody grabbedRb;

    private Transform holdPoint;

    // original rigidbody state for restoration on drop
    private Transform originalParent;
    private bool originalUseGravity;
    private bool originalIsKinematic;
    private CollisionDetectionMode originalCollisionDetectionMode;
    private RigidbodyInterpolation originalInterpolation;

    void Start()
    {
        // Create a camera-relative hold point that we will clamp by raycast.
        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.SetParent(cam.transform, false);
        holdPoint.localPosition = new Vector3(0f, 0f, maxHoldDistance);
        holdPoint.localRotation = Quaternion.identity;
    }

    void Update()
    {
        // While player is holding the grab input, clamp holdPoint distance so it never goes through walls.
        if (tryGrabbing)
            UpdateHoldPointDistance();
    }

    void FixedUpdate()
    {
        if (grabbedRb != null)
            MoveHeldObjectPhysics();
    }

    // Move the hold point along the camera forward but clamp distance to the first hit up to maxHoldDistance.
    // Ignore the grabbed object itself when deciding the clamp distance.
    private void UpdateHoldPointDistance()
    {
        Vector3 origin = cam.transform.position;
        Vector3 dir = cam.transform.forward;

        // If nothing is grabbed, a simple raycast is fine.
        if (grabbedRb == null)
        {
            if (Physics.Raycast(origin, dir, out RaycastHit hit, maxHoldDistance, ~0, QueryTriggerInteraction.Ignore))
            {
                float dist = Mathf.Max(0.05f, hit.distance - 0.01f);
                holdPoint.localPosition = new Vector3(0f, 0f, dist);
            }
            else
            {
                holdPoint.localPosition = new Vector3(0f, 0f, maxHoldDistance);
            }
            return;
        }

        // When holding something, raycast all hits and pick the nearest hit that is NOT part of the grabbed object.
        RaycastHit[] hits = Physics.RaycastAll(origin, dir, maxHoldDistance, ~0, QueryTriggerInteraction.Ignore);
        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            foreach (var h in hits)
            {
                if (h.collider == null) continue;

                // If the hit collider belongs to the grabbed object, skip it.
                if (h.collider.attachedRigidbody == grabbedRb) continue;

                // Also skip colliders that are children of the grabbed object transform (in case attachedRigidbody is null).
                if (h.collider.transform.IsChildOf(grabbedRb.transform)) continue;

                float dist = Mathf.Max(0.05f, h.distance - 0.01f);
                holdPoint.localPosition = new Vector3(0f, 0f, dist);
                return;
            }
        }

        // No valid obstruction -> full reach
        holdPoint.localPosition = new Vector3(0f, 0f, maxHoldDistance);
    }

    // Call to attempt grabbing the object under crosshair (center of screen)
    public void TryGrab()
    {
        if (grabbedRb != null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            var rb = hit.collider.GetComponent<Rigidbody>();
            if (rb == null) return;

            grabbedRb = rb;

            // save original state
            originalParent = rb.transform.parent;
            originalUseGravity = rb.useGravity;
            originalIsKinematic = rb.isKinematic;
            originalCollisionDetectionMode = rb.collisionDetectionMode;
            originalInterpolation = rb.interpolation;

            // prepare rb for being driven by MovePosition while still participating in collisions
            grabbedRb.useGravity = false;
            grabbedRb.isKinematic = false;
            grabbedRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            grabbedRb.interpolation = RigidbodyInterpolation.Interpolate;

            grabbedRb.transform.SetParent(null);

            // zero velocities to avoid sudden pops
            grabbedRb.linearVelocity = Vector3.zero;
            grabbedRb.angularVelocity = Vector3.zero;

            // ensure holdPoint distance is up to date immediately
            UpdateHoldPointDistance();
        }
    }

    // Physics-driven follow so collisions are respected
    private void MoveHeldObjectPhysics()
    {
        var rb = grabbedRb;
        if (rb == null) return;

        Vector3 targetPos = holdPoint.position;
        Quaternion targetRot = holdPoint.rotation;

        // Move toward target respecting moveSpeed and FixedDeltaTime
        float maxMove = moveSpeed * Time.fixedDeltaTime;
        Vector3 nextPos = Vector3.MoveTowards(rb.position, targetPos, maxMove);
        rb.MovePosition(nextPos);

        float maxDegrees = rotateSpeed * Time.fixedDeltaTime;
        Quaternion nextRot = Quaternion.RotateTowards(rb.rotation, targetRot, maxDegrees);
        rb.MoveRotation(nextRot);
    }

    public void Drop()
    {
        if (grabbedRb == null) return;

        // restore original rigidbody settings
        grabbedRb.useGravity = originalUseGravity;
        grabbedRb.isKinematic = originalIsKinematic;
        grabbedRb.collisionDetectionMode = originalCollisionDetectionMode;
        grabbedRb.interpolation = originalInterpolation;
        grabbedRb.transform.SetParent(originalParent);

        grabbedRb = null;
    }
}