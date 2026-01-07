using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{/// <summary>
/// 용암이 흐르는 속도와 차오르는 속도를 지정해주는 변수들이다.
/// </summary>
    [SerializeField] float scrollSpeed = 0.1f;
    [SerializeField] bool isStart = false;
    [SerializeField] float upSpeed = 0.1f;
    Renderer rend;
    /// <summary>
    /// Render 컴포넌트에 직접 연결해서 용암이 흐르는 것처럼 보이게 해줄 것이다.
    /// </summary>
    void Start()
    {
        rend = GetComponent<Renderer>();
        scrollSpeed = 0.05f;
        upSpeed = 0.1f;
    }
    /// <summary>
    /// 점프맵의 경우 2페이즈가 시작될 때 용암이 점점 차오르게 해줄 것이다.
    /// </summary>
    void Update()
    {
        if (isStart) LavaOn();
    }
    /// <summary>
    /// 렌더에 사용하는 텍스처의 오프셋값을 조정해줌으로써
    /// 텍스처가 점점 밀려나는 것이 용암이 흐르는 것처럼 보이게 작용된다.
    /// </summary>
    public void LavaOn()
    {
        isStart = true;

        float moveThis = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, moveThis));
        transform.position += (Vector3.up * upSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 플레이어가 닿으면 게임오버 호출
    /// </summary>
    /// <param name="other"></param>
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
