using OZeroBlackjack.Business.RequestModel;

namespace OZeroBlackjack.Business
{
    public interface IDecisionProcessor
    {
        string Process(TurnParams parameters);
    }
}