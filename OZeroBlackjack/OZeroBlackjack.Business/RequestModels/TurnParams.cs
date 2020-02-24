namespace OZeroBlackjack.Business.RequestModel
{
    public class TurnParams
    {
        public string[] CurrentCards { get; set; }
        public string[] CardHistory { get; set; }
        public int MoneyInHand { get; set; }
    }
}