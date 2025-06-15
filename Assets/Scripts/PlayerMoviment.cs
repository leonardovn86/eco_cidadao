using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

namespace CityPeople
{

    public class PlayerMoviment : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Autoplay random animation clips")]
        private bool AutoPlayAnimations = true;
        [SerializeField]
        [Tooltip("Overrides palette materials, skips other objects")]
        private Material PaletteOverride;
        public string CurrentPaletteName { get; private set; }

        private AnimationClip[] myClips;
        public Animator animator;
        public const string people_pal_prefix = "people_pal";
        private List<Renderer> _paletteMeshes;

        public float moveSpeed = 5f;
        public float mouseSensitivity = 1.0f;

        public Transform orientation;     // Onde rotaciona o corpo
        public Transform cameraTransform; // Para rotacionar câmera vertical

        private Vector2 inputMove;
        private Vector2 inputLook;
        private float xRotation;

        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction lookAction;

        private InputAction runAction;
        public float runMultiplier = 3f; // Multiplicador de velocidade ao correr

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                moveAction = playerInput.actions["Move"];
                lookAction = playerInput.actions["Look"];
                runAction = playerInput.actions["Sprint"];
            }


            var AllRenderers = gameObject.GetComponentsInChildren<Renderer>();
            _paletteMeshes = new List<Renderer>();
            foreach (Renderer r in AllRenderers)
            {
                var matName = r.sharedMaterial.name;
                var len = Math.Min(people_pal_prefix.Length, matName.Length);
                if (matName[0..len] == CityPeople.people_pal_prefix)
                {
                    _paletteMeshes.Add(r);
                }
            }
            if (_paletteMeshes.Count > 0)
            {
                CurrentPaletteName = _paletteMeshes[0].sharedMaterial.name;
            }

            if (PaletteOverride != null)
            {
                SetPalette(PaletteOverride);
            }
        }

        private void OnEnable()
{
    moveAction?.Enable();
    lookAction?.Enable();
    runAction?.Enable();
}

private void OnDisable()
{
    moveAction?.Disable();
    lookAction?.Disable();
    runAction?.Disable();
}

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            animator = GetComponent<Animator>();
            if (animator != null)
            {
                myClips = animator.runtimeAnimatorController.animationClips;
                if (AutoPlayAnimations)
                {
                    PlayAnyClip();
                    StartCoroutine(ShuffleClips());
                }
            }

            if (AutoPlayAnimations)
            {
                //collider for detect clicks near the character
                CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
                //average character dimentions
                collider.center = new Vector3(0f, 0.8f, 0f);
                collider.radius = 0.3f;
                collider.height = 1.77f;
                collider.direction = 1;
            }

        }

        public void SetPalette(Material mat)
        {
            if (mat != null)
            {
                if (mat.name[0..people_pal_prefix.Length] == CityPeople.people_pal_prefix)
                {
                    CurrentPaletteName = mat.name;
                    foreach (Renderer r in _paletteMeshes)
                    {
                        r.material = mat;
                    }
                }
                else
                {
                    Debug.Log("Material name should start with 'palete_pal...' by convention.");
                }
            }
        }

        public void PlayAnyClip()
        {
            if (animator.GetFloat("Speed") < 0.1f) // Apenas se o personagem estiver parado
            {
                var cl = myClips[Random.Range(0, myClips.Length)];
                animator.CrossFadeInFixedTime(cl.name, 1.0f, -1, Random.value * cl.length);
            }
        }


        IEnumerator ShuffleClips()
        {
            while (true)
            {
                yield return new WaitForSeconds(15.0f + Random.value * 5.0f);
                PlayAnyClip();
            }
        }

      void Update()
{
    inputMove = moveAction.ReadValue<Vector2>();
    inputLook = lookAction.ReadValue<Vector2>();

    Vector3 move = orientation.forward * inputMove.y + orientation.right * inputMove.x;
    move.y = 0f;

    float currentSpeed = moveSpeed;
    if (runAction.IsPressed())
    {
        currentSpeed *= runMultiplier;
        animator.SetBool("IsRunning", true);
    }
    else
    {
        animator.SetBool("IsRunning", false);
    }

    // Aqui ajustado:
    //animator.SetFloat("Speed", move.magnitude);
    animator.SetFloat("Speed", inputMove.magnitude * (runAction.IsPressed() ? runMultiplier : 1f));


    transform.position += move.normalized * currentSpeed * Time.deltaTime;

    orientation.Rotate(Vector3.up * inputLook.x * mouseSensitivity);

    xRotation -= inputLook.y * mouseSensitivity;
    xRotation = Mathf.Clamp(xRotation, -80f, 80f);
    cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
}


    }
}