using UniRx;
namespace Kitchen.Scripts.Player.Models
{
    public class PlayerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3ReactiveProperty Direction { get; set; }
    }
}
