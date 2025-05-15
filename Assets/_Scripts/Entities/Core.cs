// Made by Niek Melet on 15/5/2025

using UnityEngine;

namespace FistFury.Entities
{
    /// <summary>
    /// Overall abstract class for all the entities, containing all the core data.
    /// </summary>
    public abstract class Core : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Rigidbody2D Body { get; private set; }

        // TODO: Add more fields, e.g. GroundCheck
    }
}
