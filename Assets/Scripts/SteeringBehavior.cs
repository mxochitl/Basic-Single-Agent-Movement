using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the place to put all of the various steering behavior methods we're going
/// to be using. Probably best to put them all here, not in NPCController.
/// </summary>

public struct SteeringOutput
{
    public Vector3 linear;
    public float angular;
}

public class SteeringBehavior : MonoBehaviour {

    // The agent at hand here, and whatever target it is dealing with
    public NPCController agent;
    public NPCController target;

    // Below are a bunch of variable declarations that will be used for the next few
    // assignments. Only a few of them are needed for the first assignment.

    // For pursue and evade functions
    public float maxPrediction;
    public float maxAcceleration;

    // For arrive function
    public float maxSpeed;
    public float targetRadiusL;
    public float slowRadiusL;
    public float timeToTarget;

    // For Face function
    public float maxRotation;
    public float maxAngularAcceleration;
    public float targetRadiusA;
    public float slowRadiusA;

    // For wander function
    public float wanderOffset;
    public float wanderRadius;
    public float wanderRate;
    private float wanderOrientation;

    // Holds the path to follow
    public GameObject[] Path;
    public int current = 0;

    protected void Start() {
        agent = GetComponent<NPCController>();
        //wanderOrientation = agent.orientation;
    }


    public Vector3 Seek() {
        Vector3 steering = new Vector3(0.0F, 0.0F, 0.0F);
    
        steering = target.position - agent.position;

        // if (target.position - agent.position).magnitude is in slow radius
        // call dynamicArive instead 
        // else
        float d = (target.position - agent.position).magnitude;

        if(d < slowRadiusL){
            DynamicArrive();
        }
        else{
            steering.Normalize();
            steering *= maxAcceleration;
        }

        return steering;

	}


    public Vector3 Flee() {
        Vector3 steering = new Vector3();
    
        steering = agent.position - target.position;

        steering.Normalize();
        steering *= maxAcceleration;

        return steering;

	}

    public SteeringOutput DynamicArrive() {

        /* This functions follows the pseudo-code for arrive in the book
         * AI for Games by Ian Millington
         */

        //        public float maxSpeed;
        //public float targetRadiusL;
        //public float slowRadiusL;
        //public float timeToTarget;

        // Create the structure to hold our output
        SteeringOutput steering = new SteeringOutput();

        // Get the direction to the target
        Vector3 direction = target.position - agent.position;
        float distance = direction.magnitude;
        
        // Check if we are there, return no steering
        if (distance < targetRadiusL) {
            return steering; // Return None
        }

        // If we are outside of the slowRadius, then go max speed
        if (distance > slowRadiusL) {
            //float targetSpeed = maxSpeed;
            target.maxSpeed = maxSpeed;
        }


        // Otherwise calculate a scaled speed
        else {
            float targetSpeed = maxSpeed * (distance / slowRadiusL);
            // The target velocity combines speed and direction 
            //Vector3 targetVelocity = direction;
            //targetVelocity.Normalize();
            //targetVelocity *= targetSpeed;

            target.velocity = direction;
            target.velocity.Normalize();
            target.velocity *= target.maxSpeed;

            // Acceleration tries to get to the target velocty 
            steering.linear = target.velocity - agent.velocity;
            steering.linear /= timeToTarget;

            // Check if acceleration is too fast 
            if (steering.linear.magnitude > maxAcceleration) {
                steering.linear.Normalize();
                steering.linear *= maxAcceleration;
            }

         
        }

        // Output the steering
        steering.angular = 0;
        return steering;

        //// The target velocity combines speed and direction 
        //Vector3 targetVelocity = direction;
        //targetVelocity.Normalize();
        //targetVelocity *= targetSpeed;

        //targetSpeed = maxSpeed * (distance / slowRadius)
        //targetVelocitytargetSpeedatdirection 
        //# Acceleration tries to get to the target velocity
        //steering.linear= targetVelocity- character.velocity
        //steering.linear= steering.linear/timeToTarget

    }

}
