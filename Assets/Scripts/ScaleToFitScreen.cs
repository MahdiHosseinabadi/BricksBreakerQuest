using UnityEngine;
using UnityEngine.UI;

public class ScaleToFitScreen : MonoBehaviour
{
    private SpriteRenderer sr;
    public Image BackGround;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();


        // world height is always camera's orthographicSize * 2
        float worldScreenHeight = Camera.main.orthographicSize * 2;

        // world width is calculated by diving world height with screen heigh
        // then multiplying it with screen width
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // to scale the game object we divide the world screen width with the
        // size x of the sprite, and we divide the world screen height with the
        // size y of the sprite
        transform.localScale = new Vector3(worldScreenWidth / BackGround.sprite.rect.width, worldScreenHeight / BackGround.sprite.rect.height, 1);
        // Logger.Log("Width " + BackGround.sprite.rect.width + "Height " + BackGround.sprite.rect.height);
    }

} // class

