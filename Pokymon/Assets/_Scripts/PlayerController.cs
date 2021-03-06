using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
   private bool isMoving;

   public float speed;

   private Vector2 input;


   private Animator _animator;

   public LayerMask solidObjectsPlayer, pokemonLayer;

   private void Awake()
   {

       _animator = GetComponent<Animator>();
   }

   private void Update()
   {
       if (!isMoving)
       { 
           input.x = Input.GetAxisRaw("Horizontal");
           input.y = Input.GetAxisRaw("Vertical");

           if (input.x!=0)
           {
               input.y = 0;
           }

           if (input != Vector2.zero)
           {
               _animator.SetFloat("Move X", input.x);
               _animator.SetFloat("Move Y", input.y);
               var targetPosition = transform.position;
               targetPosition.x += input.x;
               targetPosition.y += input.y;
               if (IsAvailable(targetPosition))
               {
                   StartCoroutine(MoveTowards(targetPosition));    
               }

               

           }
       }
   }

   private void LateUpdate()
   {
       _animator.SetBool("Is Moving", isMoving);
   }

   IEnumerator MoveTowards(Vector3 destination)
   {
       isMoving = true;
        
       while (Vector3.Distance(transform.position, destination) > Mathf.Epsilon)
       {
           transform.position = Vector3.MoveTowards(transform.position,
               destination, speed * Time.deltaTime);
           yield return null;
       }
        
       transform.position = destination;
       isMoving = false;

       CheckForPokemon();


   }
   /// <summary>
   /// El métodoc omporueba que lña zona a la que queremos acceder, este disponible
   /// </summary>
   /// <param name="target">Zona a la que queremos acceder</param>
   /// <returns>Devuelve true, si el target esta disponbible y false en caso contrario.</returns>

   private bool IsAvailable(Vector3 target)
   {
       if (Physics2D.OverlapCircle(target, 0.1f, solidObjectsPlayer)!= null)
       {
           return false;
       }

       return true;
   }

   private void CheckForPokemon()
   {
       if (Physics2D.OverlapCircle(transform.position,0.2f, pokemonLayer)!=null)
       {
           if (Random.Range(0,100)< 15)
           {
               Debug.Log("Empezar Batalla Pokemon");
           }
       }
   }


}
