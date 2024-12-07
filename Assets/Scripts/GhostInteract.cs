using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostInteract : MonoBehaviour {
    [Header("Throw Settings")]
    [Range(0f, 1f)]
    public float throwChance = 0.5f; // Chance to throw an item (0.5 = 50%)
    public float throwForce = 25f; // Force applied to the thrown item
    public float minThrowInterval = 2f; // Minimum time between throw attempts
    public float maxThrowInterval = 5f; // Maximum time between throw attempts

    private List<Rigidbody> nearbyItems = new List<Rigidbody>(); // Track items in range

    private void Awake() {
        // Ensure the collider is set up correctly
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider == null || !collider.isTrigger) {
            Debug.LogError("This script requires a SphereCollider set to 'Is Trigger'.");
        }
    }

    private void Start() {
        // Start the time-driven throw routine
        StartCoroutine(ThrowItemRoutine());
    }

    private IEnumerator ThrowItemRoutine() {
        while (true) {
            // Wait for a random interval between min and max
            float randomInterval = Random.Range(minThrowInterval, maxThrowInterval);
            yield return new WaitForSeconds(randomInterval);

            // Attempt to throw an item
            AttemptThrowItem();
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Add items that enter the collider to the list
        Rigidbody itemRigidbody = other.GetComponent<Rigidbody>();
        if (itemRigidbody != null) {
            Debug.Log($"Item entered trigger: {other.gameObject.name}");
            nearbyItems.Add(itemRigidbody);
        }
    }

    private void OnTriggerExit(Collider other) {
        // Remove items that leave the collider from the list
        Rigidbody itemRigidbody = other.GetComponent<Rigidbody>();
        if (itemRigidbody != null) {
            Debug.Log($"Item exited trigger: {other.gameObject.name}");
            nearbyItems.Remove(itemRigidbody);
        }
    }

    private void AttemptThrowItem() {
        Debug.Log("Attempting to throw item...");

        // If there are no items in range, log and exit
        if (nearbyItems.Count == 0) {
            Debug.Log("No items to throw.");
            return;
        }

        // Random chance to decide whether to throw an item
        if (Random.value <= throwChance) {
            // Pick a random item from the list
            Rigidbody itemToThrow = nearbyItems[Random.Range(0, nearbyItems.Count)];

            // Calculate a throw direction
            Vector3 throwDirection = (itemToThrow.position - transform.position).normalized;

            // Apply force to the item
            itemToThrow.AddForce(throwDirection * throwForce, ForceMode.Impulse);

            // Remove the item from the list after throwing
            nearbyItems.Remove(itemToThrow);

            Debug.Log($"Ghost threw item: {itemToThrow.gameObject.name}");
        } else {
            Debug.Log("Throw chance failed. Ghost did not throw an item.");
        }
    }
}
