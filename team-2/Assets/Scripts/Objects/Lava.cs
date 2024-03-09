using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.1f;
    [SerializeField] bool isStart = false;
    [SerializeField] float upSpeed = 0.1f;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        scrollSpeed = 0.05f;
        upSpeed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart) LavaOn();
    }

    public void LavaOn()
    {
        isStart = true;

        float moveThis = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, moveThis));
        transform.position += (Vector3.up * upSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player.live)
            {
                player.live = false;
                GameManager.Instance.GameOver(MonsterType.Lava);
            }
        }
    }
}
