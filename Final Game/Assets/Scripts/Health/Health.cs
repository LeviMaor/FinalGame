using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

   
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spritRenderer;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spritRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage (float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if(currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
               
                foreach(Behaviour component in components)
                {
                    component.enabled = false;
                }
                dead = true;

                if (tag == "Player")
                    SceneManager.LoadScene("LoseScreen");


            }
        }

    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spritRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spritRenderer.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
