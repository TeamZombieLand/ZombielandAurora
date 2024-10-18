using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Zombie : MonoBehaviour,IPunObservable
{   

    [Header("Zombie Type")]
    [SerializeField] private ZOMBIE_TYPE zombie_type = ZOMBIE_TYPE.NORMAL;

    [Header("Health")]
    [SerializeField] CanvasGroup health_canvas;
    [SerializeField] Image health_slider;
    [SerializeField] float max_health;
    [SerializeField] float currentHealth;
    [SerializeField] float health_regainTime;

    [Header("Shield Health")]
    [SerializeField] bool isShieldOn=false;
    [SerializeField] GameObject Shield;
    [SerializeField] Image shield_slider;
    [SerializeField] float shield_health;
    [SerializeField] float max_shield_health;

    [Header("NavMesh AI")]
    [SerializeField] NavMeshAgent navmesh_agent;
    [SerializeField] Transform target;


    [Header("Attack Range")]
    [SerializeField] bool isAttacking;
    [SerializeField] float attack_radius;
    [SerializeField] float longattack_radius;
    [SerializeField] float attack_damage;
    [SerializeField] float attack_cooldown;
    [SerializeField] float attack_range;
    [SerializeField] float longattack_range;
    [SerializeField] GameObject AttackParticleNoise;
    [SerializeField] Transform MouthPos;
    [SerializeField] Transform HandPos;
    [SerializeField] GameObject AttackCollider;
    public float Damage { get {return attack_damage; } }

    [Header("Animatios")]
    [SerializeField] Animator animator;
    [SerializeField] int attack_id;
    [SerializeField] int max_attackID;
    [SerializeField] int die_id;
    

    [Header("Walk/Run")]
    [SerializeField] float current_speed;
    [SerializeField] float walk_speed;
    [SerializeField] float run_speed;
    [SerializeField] bool can_run;
    [SerializeField] int walk_id;
    [SerializeField] int run_id;
    [SerializeField] int idle_id;


    [SerializeField] bool isAlive;

    

    [Header("Event Management")]
  
    public static Action OnZombieDisable;

    [SerializeField] Renderer[] renderers;


    [Header("Zombie Sound Management")]
    [SerializeField] AudioClip[] audio_attack_clips;
    [SerializeField] AudioClip[] audio_die_clips;
    [SerializeField] AudioClip[] audio_got_hurt;
    [SerializeField] AudioClip[] audio_zombie_random_sound;
    [SerializeField] AudioSource ZombieAudioSource;

    [SerializeField] PhotonView PV;
    [SerializeField] Player ownerPlayer;

    [SerializeField] Transform mainCam;

    [Header("LONG RANGE ATTAC")]
    [SerializeField] ZombieMeele Melee;

    public bool resetZombieChildRigid = false;
    
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (PV != null)
        {
            ownerPlayer = PV.Owner;
        }
    }

    int targetIndex = 0;

    private void OnEnable()
    {
       
        GetComponent<RagdollToggle>().ToggleRagdoll(false);

        if (resetZombieChildRigid)
        {
            GetComponent<RagdollToggle>().ResetJointsPosition();
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.SetFloat("EdgeWidth", 0);
            renderers[i].material.SetFloat("Dissolve", 0);
        }
        if (CommonReferences.Instance._pState == PlayerState.WORLD)
        {
            if (CommonReferences.Instance.gameState == GameState.MULTIPLAYER)
            {
                target = CommonReferences.Instance.GetRandomPlayer().transform;
            }
            else
            {
                target = CommonReferences.Instance.staticSinglePlayer.transform;
            }
            targetIndex = 0;
        }
        else
        {
            target = CommonReferences.Instance.myCar.transform;
            targetIndex = 1;
        }

        currentHealth = max_health;
        isAlive = true;
        isAttacking = false;

        SetHealthValue();

        switch (zombie_type)
        {
            case ZOMBIE_TYPE.NORMAL:
                {
                    break;
                }
                
            case ZOMBIE_TYPE.SPEEDRUNNER:
                {
                    break;
                }
            case ZOMBIE_TYPE.REGAINHEALTH:
                {
                    break;
                }
            case ZOMBIE_TYPE.BOSS:
                {
                    break;
                }
            case ZOMBIE_TYPE.SHIELD:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }


        attack_id = Random.Range(0, max_attackID);
        die_id= Random.Range(0, 2);

        animator.SetFloat("GlobalSpeed", 1);
        animator.SetFloat("die_id", die_id);
        animator.SetFloat("attack_id", attack_id);
        animator.SetFloat("die_id", die_id);

        can_run = Random.Range(0, 3) > 1;
        current_speed = can_run ? run_speed : walk_speed;
        

    
        if (PV != null && PV.IsMine)
        {
            navmesh_agent.enabled = true;
        }
        else if(PV==null)
        {

            navmesh_agent.enabled = true;
        }

        walk_id = Random.Range(5, 8);
        idle_id = Random.Range(0, 5);
        run_id = Random.Range(8, 10);
        animator.Play("Movement");

        CommonReferences.OnPlayerStateChange += HandlePlayerStateChange;

        if (PV!=null && !PV.IsMine)
        {
            navmesh_agent.enabled = false;
        }

      


    }

    private void OnDisable()
    {
        CommonReferences.OnPlayerStateChange -= HandlePlayerStateChange;
        navmesh_agent.enabled = false;
        OnZombieDisable?.Invoke();

    }

    private void HandlePlayerStateChange(PlayerState state)
    {
        if (state == PlayerState.WORLD)
        {
            ChangeTarget(0);
        }
        else
        {
            ChangeTarget(1);
        }
    }

    public void ChangeTarget(int index)
    {
        if (index == 0)
        {
            if (CommonReferences.Instance.gameState == GameState.MULTIPLAYER)
            {
                target = CommonReferences.Instance.GetPlayer().transform;
            }
            else
            {
                target = CommonReferences.Instance.staticSinglePlayer.transform;
            }
        }
        else
        {
            target = CommonReferences.Instance.myCar.transform;
        }

        targetIndex = index;
    }
    private void SetHealthValue()
    {
        health_slider.fillAmount = currentHealth / max_health;
        if (isShieldOn)
        {
            shield_slider.fillAmount = shield_health / max_shield_health;
            shield_slider.transform.parent.gameObject.SetActive(shield_slider.fillAmount > 0);
        }

        health_slider.transform.parent.gameObject.SetActive(health_slider.fillAmount > 0);
        
    }

    private void LateUpdate()
    {
        

        if (health_slider.gameObject.activeInHierarchy)
        {
            if (mainCam == null)
            {
                if(CommonReferences.Instance.gameState == GameState.MULTIPLAYER)
                {
                    mainCam = CommonReferences.Instance.myCamera;
                }
                else
                {
                    mainCam = CommonReferences.Instance.staticSinglePlayer.GetComponentInChildren<Camera>().transform;
                }
            }
            Vector3 lookRot= mainCam.transform.position - this.transform.position;
            lookRot.y = 0;            
            health_slider.transform.parent.rotation=Quaternion.LookRotation(lookRot);
            
        }
    }
    void Update()
    {
        if (PV != null)
        {
            if (!PV.IsMine) return;


            if (ownerPlayer != PV.Owner)
            {
                ownerPlayer = PV.Owner;
                FindNewTarget();
            }
        }


        if (AbilitiesManager.Instance.usingFreezePower)
        {
            if (navmesh_agent != null)
            {
                navmesh_agent.speed = 0;
                animator.SetFloat("GlobalSpeed", 0);
            }
        }
        else
        {
            if (navmesh_agent != null)
            {
                navmesh_agent.speed = current_speed;
                animator.SetFloat("GlobalSpeed", 1);
            }
        }


        if (!target) return;
        if (!isAlive) return;

        if (isAttacking) return;

        float dist = Vector3.Distance(target.position, this.transform.position);

        if (dist <= ((targetIndex == 0) ? (attack_id==2)?longattack_radius :attack_radius : (attack_id == 2) ? longattack_radius : (attack_radius*2f)))
        {
            if (navmesh_agent == null) return;

            current_speed = can_run ? run_speed : walk_speed;

            if (navmesh_agent.isOnNavMesh)
            {
                navmesh_agent.SetDestination(target.position);
            }
            else
            {
                navmesh_agent.Warp(this.transform.position);
            }


          

            if (attack_id != 2)
            {
                if (dist <= ((targetIndex == 0) ? attack_range : attack_range * 2f))
                {


                    //ATtack Target
                    Attack();
                    //Face Target
                    FaceTarget();
                }
            }
            else
            {
                if (dist <= ((targetIndex == 0) ? longattack_range : longattack_range * 1.3f))
                {


                    //ATtack Target
                    Attack(true);
                    //Face Target
                    FaceTarget();
                }
            }

            

        }
        else
        {

            if (navmesh_agent.isOnNavMesh)
            {
                navmesh_agent.SetDestination(target.position);
            }
            else
            {
                navmesh_agent.Warp(this.transform.position);
            }

            current_speed = run_speed;

            if (soundCouroutine == null)
            {
                soundCouroutine = StartCoroutine(playRandomSound());
            }
        }


        if (!AbilitiesManager.Instance.usingFreezePower)
        {
            animator.SetFloat("speed", can_run ? (navmesh_agent.velocity.magnitude > 0.5f ? run_id : idle_id) : (navmesh_agent.velocity.magnitude > 0.5f ? walk_id : idle_id));

        }
    }

        #region Sound Management

        Coroutine soundCouroutine;
    public void PlayClip(AudioClip clip, bool checkIfPlaying = false)
    {
        if (checkIfPlaying)
        {
            if (ZombieAudioSource.isPlaying) return;
        }
        ZombieAudioSource.clip = clip;
        ZombieAudioSource.Play();
    }
    IEnumerator playRandomSound()
    {
        yield return new WaitForSeconds(Random.Range(3, 6));
        PlayClip(audio_zombie_random_sound[Random.Range(0,audio_zombie_random_sound.Length)],true);
        yield return new WaitForSeconds(Random.Range(2, 6));
        soundCouroutine = null;

    }
    #endregion
    private void Attack(bool isLongRange = false)
    {
        if (isAttacking) return;
        if(CommonReferences.Instance.gameState == GameState.MULTIPLAYER)
        {
            PV.RPC("RPC_Attack", RpcTarget.Others);
        }
        navmesh_agent.enabled = false;
        isAttacking = true;     
        
        animator.SetFloat("attack_id", attack_id);
        animator.SetBool("attack", true);
        StartCoroutine(resetAnimBool("attack"));

        ZombieAudioSource.clip = audio_attack_clips[Random.Range(0, audio_attack_clips.Length)];

        if (isLongRange)
        {
            StartCoroutine("throwMelee");
           
        }

        ZombieAudioSource.Play();

        if (attack_id == 3 && max_attackID == 4)
        {
            Instantiate(AttackParticleNoise, MouthPos);
        }

        StartCoroutine("ResetAttack");        
    }

    IEnumerator throwMelee()
    {
        yield return new WaitForSeconds(0.5f);

        if (CommonReferences.Instance.gameState == GameState.SINGLEPLAYER)
        {
            ZombieMeele melee = Instantiate(Melee, HandPos.transform.position, Quaternion.Euler(0, transform.eulerAngles.y, 0));
            melee.Throw(target, Damage);
        }
        else
        {
            GameObject melee = PhotonNetwork.Instantiate(Melee.name, HandPos.transform.position, Quaternion.Euler(0, transform.eulerAngles.y, 0));
            melee.transform.GetComponent<ZombieMeele>().Throw(target, Damage);
        }
    }

    [PunRPC]
    public void RPC_Attack()
    {
        navmesh_agent.enabled = false;
        isAttacking = true;              

        ZombieAudioSource.clip = audio_attack_clips[Random.Range(0, audio_attack_clips.Length)];

        ZombieAudioSource.Play();

        if (attack_id == 3 && max_attackID == 4)
        {
            Instantiate(AttackParticleNoise, MouthPos);
        }

        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine("ResetAttack");
        }
    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        if (attack_id != 2)
        {
            AttackCollider.SetActive(true);
        }
        yield return new WaitForSeconds(0.3f);
        AttackCollider.SetActive(false);
        yield return new WaitForSeconds(attack_cooldown-0.8f);
        isAttacking = false;

        if (PV != null && PV.IsMine)
        {
            navmesh_agent.enabled = true;
        }
        else if(CommonReferences.Instance.gameState == GameState.SINGLEPLAYER)
        {
            navmesh_agent.enabled = true;
        }
        attack_id = Random.Range(0, max_attackID);
    }

    private void FaceTarget()
    {
        Vector3 dir = (target.position - this.transform.position).normalized;
        dir.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }



    [PunRPC]
    public void TookDamage(float damage, bool headshot)
    {
        if (isAlive)
        {
        
            currentHealth -= damage;
            LeanTween.cancel(health_canvas.gameObject);
            health_canvas.alpha = 1;
            LeanTween.alphaCanvas(health_canvas, 0, 0.5f).setFrom(1).setDelay(1);
            if (currentHealth <= 0)
            {
                PlayClip(audio_die_clips[Random.Range(0, audio_die_clips.Length)]);

                StopCoroutine("ResetAttack");
                navmesh_agent.enabled = false;
                currentHealth = 0;
                isAlive = false;
                animator.SetBool("die",true);
                StartCoroutine(resetAnimBool("die"));

                StartCoroutine(disableZombie());
                PV.RPC("disableZombieRPC", RpcTarget.Others);
            }
            else
            {
                PlayClip(audio_got_hurt[Random.Range(0, audio_got_hurt.Length)]);
            }
            SetHealthValue();
        }
    }

    [PunRPC]
    public void disableZombieRPC()
    {
        StartCoroutine(disableZombie());
    }
    [PunRPC]
    public void Spawned()
    {
       
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        animator.Play("Movement");
    }
    IEnumerator resetAnimBool(string _value)
    {
        yield return new WaitForSecondsRealtime(0.05f);
        animator.SetBool(_value, false);
    }
    public void TakeDamage(float damage,bool headshot,bool iAmKillder = true)
    {
        if (isAlive)
        {

            currentHealth -= damage;
            LeanTween.cancel(health_canvas.gameObject);
            health_canvas.alpha = 1;
            LeanTween.alphaCanvas(health_canvas,0,0.5f).setFrom(1).setDelay(1);
            if (currentHealth <= 0)
            {
                PlayClip(audio_die_clips[Random.Range(0, audio_die_clips.Length)]);
                
                StopCoroutine("ResetAttack");
                navmesh_agent.enabled = false;
                currentHealth = 0;
                isAlive = false;
                animator.SetBool("die", true);
                StartCoroutine(resetAnimBool("die"));

                StartCoroutine(disableZombie());
                UIManager.insta.zombiesKilled++;

                if (CommonReferences.Instance.gameState == GameState.MULTIPLAYER)
                {
                    if (iAmKillder)
                    {
                        ScoreManager.Instance.coinsGot += (Random.Range(20, 50) * (UIManager.insta.headshotCombo > 0 ? UIManager.insta.headshotCombo : 1));
                        ScoreManager.Instance.zombiesKilled += 1;
                    }
                }
                else
                {
                    ScoreManager.Instance.coinsGot += (Random.Range(20, 50) * (UIManager.insta.headshotCombo > 0 ? UIManager.insta.headshotCombo : 1));
                    ScoreManager.Instance.zombiesKilled += 1;
                }
                
                if (headshot)
                {
                    UIManager.insta.headshots++;
                    UIManager.insta.showHeadShotComboMsg();
                }
            }
            else
            {
                PlayClip(audio_got_hurt[Random.Range(0, audio_got_hurt.Length)]);                
            }
            SetHealthValue();
        }

      
    }

    [SerializeField] AnimationClip[] die_clips;
    IEnumerator disableZombie()
    {   
        float t = die_clips[die_id].length;
        float transitionRate = 0;
        yield return new WaitForSeconds(t);

        while (transitionRate<1)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.SetFloat("EdgeWidth", Mathf.Lerp(renderers[i].material.GetFloat("EdgeWidth"),Mathf.PingPong(Time.time,0.1f),Time.deltaTime*transitionRate));
                renderers[i].material.SetFloat("Dissolve", Mathf.Lerp(renderers[i].material.GetFloat("Dissolve"), 1, Time.deltaTime * transitionRate));

            }
            transitionRate += Time.deltaTime / t;
            yield return null;
        }        
        this.gameObject.SetActive(false);
    }
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else if (stream.IsReading)
        {
            float newHealth = (float)stream.ReceiveNext();
            if(currentHealth != newHealth)
            {
                currentHealth = newHealth;
               
            }           
        }
    }

    private void FindNewTarget()
    {

        Debug.Log("FIND NEW TARGET");
        if (CommonReferences.Instance.gameState == GameState.MULTIPLAYER)
        {
            target = CommonReferences.Instance.GetRandomPlayer();
        }
        else
        {
            target = CommonReferences.Instance.staticSinglePlayer.transform;
        }
        targetIndex = 0;
        if (PV.IsMine)
        {
            navmesh_agent.enabled = true;
        }
    }
}

public enum ZOMBIE_TYPE
{
    NORMAL,SPEEDRUNNER,REGAINHEALTH,BOSS,SHIELD
}
