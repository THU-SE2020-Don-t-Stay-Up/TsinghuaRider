using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporter : MonoBehaviour
{
    [Header("传送动画")]
    public float teleportTime = 2.0f;
    public float amplifyScale = 1.5f;
    public float rotationSpeed = 30.0f;
    public Sprite clearSprite;

    [Header("传送逻辑")]
    public string targetScene;

    float timer;
    bool isTeleporting = false;
    bool hasTeleported = false;
    bool cleared = false;
    GameObject target;

    /// <summary>
    /// 为传送点设置要传送的物体，默认为玩家
    /// </summary>
    /// <param name="teleportTarget"></param>
    public void FindTarget(GameObject teleportTarget = null)
    {
        if (teleportTarget == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            target = teleportTarget;
        }
    }

    /// <summary>
    /// 立即传送物体，默认为玩家
    /// </summary>
    /// <param name="teleportTarget"></param>
    public void TeleportNow(GameObject teleportTarget = null)
    {
        FindTarget(teleportTarget);
        StartCoroutine(TeleportAsync());
        hasTeleported = true;
    }

    void Start()
    {
        timer = teleportTime;
    }

    void Update()
    {
        if (isTeleporting)
        {
            // 显示动画
            transform.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime);
            transform.localScale = Vector3.one * amplifyScale;

            // 计算时间
            timer -= Time.deltaTime;
            if (timer < 0 && !hasTeleported)
            {
                TeleportNow();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == target)
        {
            //Debug.Log("Across Scene Teleporting " + target);
            isTeleporting = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == target)
        {
            isTeleporting = false;
            // 退出传送后重置计时和动画
            timer = teleportTime;
            transform.localScale = Vector3.one;
            transform.rotation = Quaternion.identity;
        }
    }

    IEnumerator TeleportAsync()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (target != null)
        {
            // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
            SceneManager.MoveGameObjectToScene(target, SceneManager.GetSceneByName(targetScene));
        }

        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }


    public void clear()
    {
        cleared = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = clearSprite;
    }
}
