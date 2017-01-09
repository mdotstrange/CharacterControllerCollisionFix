using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CcCollisionFixer : MonoBehaviour
{
    //Player object with Character Controller
    public GameObject Player;
    // for setting Player Position
    public GameObject playerMidPoint;
    public GameObject OverlapSphereOrigin;
    public float SphereRadius;
    public LayerMask layerMask;
    //the amount the Player is pushed away from the colliding object
    public float pushBack;
    // This bool is for setting the Player speed to 0 when moving it back, I don't know if it did anything so disable it
    //public bool setPlayerSpeedTo0;
    int loop;

    // reference to the Playermovement script
    //playerMovement pm;

    void Reset()
    {
        SphereRadius = 1f;
        pushBack = 0.5f;
        loop = 0;
    }

    void Start()
    {
        //pm = gameObject.GetComponent<playerMovement>();
    }

    void FixedUpdate()
    {
        Vector3 sphere00Origin = OverlapSphereOrigin.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(sphere00Origin, SphereRadius, layerMask, QueryTriggerInteraction.Ignore);
        Vector3 contactPoint;

        if (hitColliders.Length != 0)
        {
            for (int index = 0; index < hitColliders.Length; index++)
            {               
                Vector3 playerPos = playerMidPoint.transform.position;
                bool contactPointSuccess = ClosestPointOnSurface(hitColliders[index], playerPos, 1.0f, out contactPoint);

                if (contactPointSuccess == true)
                {
                    //if(setPlayerSpeedTo0)
                    //{
                    //    //set player movement script to 0
                    //    //I don't know if this does anything 0_0
                    //    pm.speed = 0f;
                    //}
                  
                          
                    Vector3 directionPri = playerPos - contactPoint;
                    Vector3 destination = directionPri * pushBack;


                    RaycastHit hitter;
                     if(Physics.Linecast(sphere00Origin, contactPoint, out hitter, layerMask, QueryTriggerInteraction.Ignore))
                    {
                     
                        Vector3 normal = hitter.normal;
                        //Debug.Log("LineCast hit"  + normal);
                   

                        Player.transform.position = Player.transform.position + normal * pushBack;
                    }
                     else
                    {
                        //Debug.Log("No LineCast hit");            
                        Player.transform.position = Player.transform.position + destination * pushBack;
                    }
                    loop++;
                    if (loop == 5000)
                    {
                        Debug.Log("loooper");
                        break;
                    }

                }

            }
        }
        //if(setPlayerSpeedTo0)
        //{
        //    //put speed back to normal
        //    pm.speed = 15f;
        //}
    }

    //following code is from https://github.com/IronWarrior/SuperCharacterController/blob/master/Assets/SuperCharacterController/SuperCharacterController/Core/SuperCollider.cs
    //I just add a CharacterController type

    public static bool ClosestPointOnSurface(Collider collider, Vector3 to, float radius, out Vector3 closestPointOnSurface)
    {
        if (collider is BoxCollider)
        {
            closestPointOnSurface = CcCollisionFixer.ClosestPointOnSurface((BoxCollider)collider, to);
            return true;
        } 
        else if (collider is SphereCollider)
        {
            closestPointOnSurface = CcCollisionFixer.ClosestPointOnSurface((SphereCollider)collider, to);
            return true;
        } 
        else if (collider is CapsuleCollider)
        {
            closestPointOnSurface = CcCollisionFixer.ClosestPointOnSurface((CapsuleCollider)collider, to);
            return true;
        } else if (collider is CharacterController)
        {
            closestPointOnSurface = CcCollisionFixer.ClosestPointOnSurface((CharacterController)collider, to);
            return true;
        }

        Debug.LogError(string.Format("{0} does not have an implementation for ClosestPointOnSurface; GameObject.Name='{1}'", collider.GetType(), collider.gameObject.name));
        closestPointOnSurface = Vector3.zero;
        return false;
    }

    public static Vector3 ClosestPointOnSurface(SphereCollider collider, Vector3 to)
    {
        Vector3 p;

        p = to - (collider.transform.position + collider.center);
        p.Normalize();

        p *= collider.radius * collider.transform.localScale.x;
        p += collider.transform.position + collider.center;

        return p;
    }

    public static Vector3 ClosestPointOnSurface(BoxCollider collider, Vector3 to)
    {
        // Cache the collider transform
        var ct = collider.transform;

        // Firstly, transform the point into the space of the collider
        var local = ct.InverseTransformPoint(to);

        // Now, shift it to be in the center of the box
        local -= collider.center;

        //Pre multiply to save operations.
        var halfSize = collider.size * 0.5f;

        // Clamp the points to the collider's extents
        var localNorm = new Vector3(
                Mathf.Clamp(local.x, -halfSize.x, halfSize.x),
                Mathf.Clamp(local.y, -halfSize.y, halfSize.y),
                Mathf.Clamp(local.z, -halfSize.z, halfSize.z)
            );

        //Calculate distances from each edge
        var dx = Mathf.Min(Mathf.Abs(halfSize.x - localNorm.x), Mathf.Abs(-halfSize.x - localNorm.x));
        var dy = Mathf.Min(Mathf.Abs(halfSize.y - localNorm.y), Mathf.Abs(-halfSize.y - localNorm.y));
        var dz = Mathf.Min(Mathf.Abs(halfSize.z - localNorm.z), Mathf.Abs(-halfSize.z - localNorm.z));

        // Select a face to project on
        if (dx < dy && dx < dz)
        {
            localNorm.x = Mathf.Sign(localNorm.x) * halfSize.x;
        } else if (dy < dx && dy < dz)
        {
            localNorm.y = Mathf.Sign(localNorm.y) * halfSize.y;
        } else if (dz < dx && dz < dy)
        {
            localNorm.z = Mathf.Sign(localNorm.z) * halfSize.z;
        }

        // Now we undo our transformations
        localNorm += collider.center;

        // Return resulting point
        return ct.TransformPoint(localNorm);
    }

    // Courtesy of Moodie
    public static Vector3 ClosestPointOnSurface(CapsuleCollider collider, Vector3 to)
    {
        Transform ct = collider.transform; // Transform of the collider

        float lineLength = collider.height - collider.radius * 2; // The length of the line connecting the center of both sphere
        Vector3 dir = Vector3.up;

        Vector3 upperSphere = dir * lineLength * 0.5f + collider.center; // The position of the radius of the upper sphere in local coordinates
        Vector3 lowerSphere = -dir * lineLength * 0.5f + collider.center; // The position of the radius of the lower sphere in local coordinates

        Vector3 local = ct.InverseTransformPoint(to); // The position of the controller in local coordinates

        Vector3 p = Vector3.zero; // Contact point
        Vector3 pt = Vector3.zero; // The point we need to use to get a direction vector with the controller to calculate contact point

        if (local.y < lineLength * 0.5f && local.y > -lineLength * 0.5f) // Controller is contacting with cylinder, not spheres
            pt = dir * local.y + collider.center;
        else if (local.y > lineLength * 0.5f) // Controller is contacting with the upper sphere 
            pt = upperSphere;
        else if (local.y < -lineLength * 0.5f) // Controller is contacting with lower sphere
            pt = lowerSphere;

        //Calculate contact point in local coordinates and return it in world coordinates
        p = local - pt;
        p.Normalize();
        p = p * collider.radius + pt;
        return ct.TransformPoint(p);

    }

    // Courtesy of Moodie
    public static Vector3 ClosestPointOnSurface(CharacterController collider, Vector3 to)
    {
        Transform ct = collider.transform; // Transform of the collider

        float lineLength = collider.height - collider.radius * 2; // The length of the line connecting the center of both sphere
        Vector3 dir = Vector3.up;

        Vector3 upperSphere = dir * lineLength * 0.5f + collider.center; // The position of the radius of the upper sphere in local coordinates
        Vector3 lowerSphere = -dir * lineLength * 0.5f + collider.center; // The position of the radius of the lower sphere in local coordinates

        Vector3 local = ct.InverseTransformPoint(to); // The position of the controller in local coordinates

        Vector3 p = Vector3.zero; // Contact point
        Vector3 pt = Vector3.zero; // The point we need to use to get a direction vector with the controller to calculate contact point

        if (local.y < lineLength * 0.5f && local.y > -lineLength * 0.5f) // Controller is contacting with cylinder, not spheres
            pt = dir * local.y + collider.center;
        else if (local.y > lineLength * 0.5f) // Controller is contacting with the upper sphere 
            pt = upperSphere;
        else if (local.y < -lineLength * 0.5f) // Controller is contacting with lower sphere
            pt = lowerSphere;

        //Calculate contact point in local coordinates and return it in world coordinates
        p = local - pt;
        p.Normalize();
        p = p * collider.radius + pt;
        return ct.TransformPoint(p);
    }

}
