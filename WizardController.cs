using System.Threading.Tasks;
using UnityEngine;

/* Script for handling game logic of a wizard enemy in a shoot-em up type game.
 * Enemy spawn point/attributes are randomly determined by EnemySpawnManager script.
 * The enemy will lock onto a player and continuelly throw food at the player until
 * the enemy is destroyed by the player's thrown food.
 */
public class WizardController : MonoBehaviour
{   
    // Attributes
    public float projectileSpeed;
    public int attackPattern;
    public int food;

    public GameObject target;

    // Time between throws
    private float maxCastTimer = 2f;
    private float castTimer;

    // Thrown food
    private GameObject[] projectilePrefabs;

    void Awake()
    {
        castTimer = maxCastTimer;
        projectilePrefabs = Resources.LoadAll<GameObject>("ProjectilePrefabs");
    }

    void FixedUpdate()
    {
        castTimer -= Time.deltaTime;

        if (castTimer < 0)
        {
            DoAttackPattern();
            castTimer = maxCastTimer;
        }
    }

    // Check if wizard has been hit by a player's thrown food, but using Unity editor physics manager,
    // we ignore other wizard's thrown food.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProjectileController projectile = collision.gameObject.GetComponent<ProjectileController>();

        if (projectile != null)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    // Set base info about a wizard upon creation
    public void SetBaseAttributes(GameObject targetObj, float speed, int pattern, int type, int side)
    {
        target = targetObj;
        projectileSpeed = speed;
        attackPattern = pattern;
        food = type;
        AdjustRotation(side);
      
    }

    // Scalable attack pattern script. Simply add another case if a new projectile pattern is implemented
    // like a curved throw.
    private void DoAttackPattern()
    {
        switch(attackPattern)
        {
            case 1:
                CastStraight();
                break;
            case 2:
                CastDoubleTap();
                break;
            case 3:
                CastCone();
                break;
        }
    }

    // Creates a projectile based on wizard positioning/pre-determined attributes that
    // flies towards the player
    private GameObject CreateTargetProjectile()
    {
        GameObject projectile = Instantiate<GameObject>(projectilePrefabs[food]);
        projectile.GetComponent<ProjectileController>().food = food;

        projectile.layer = 10; // Projectile layer
        projectile.transform.position = gameObject.transform.position + Vector3.right * -1.5f;

        Vector3 targetVector = new Vector3(projectile.transform.position.x - target.transform.position.x,
                                            projectile.transform.position.y - target.transform.position.y, 0f);
        float angle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg + 90f;
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        projectile.transform.localScale = new Vector3(.75f, .75f, .75f);

        return projectile;
    }

    // Simple straight line projectile towards the player
    private void CastStraight()
    {
        CreateTargetProjectile();
    }

    // 2 projectiles back to back in a straight line towards the player
    private async void CastDoubleTap()
    {
        CreateTargetProjectile();

        await Task.Delay(200); //Small delay so the projectiles don't stack up but enough to give player time to eat both if possible

        CreateTargetProjectile();
    }

    // 3 projectiles  in a small cone towards the player
    private void CastCone()
    {
        CreateTargetProjectile();
        GameObject leftProjectile = CreateTargetProjectile();
        GameObject rightProjectile = CreateTargetProjectile();

        leftProjectile.transform.position += Vector3.up * 2.5f;
        rightProjectile.transform.position += Vector3.up * -2.5f;
    }
    
    // Used to turn the sprite, so it is facing towards the  center of the map
    private void AdjustRotation(int side)
    {
        if (side == 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
    }

}
