var scrollSpeed:float ;
//var uvtexture:Texture;//放2d圖
function Update () 
{
var offset = Time.time * scrollSpeed;
renderer.material.SetTextureOffset ("_MainTex", Vector2(-offset,0));
}