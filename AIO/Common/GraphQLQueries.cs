namespace AIO.Common.Request
{
    public class GraphQLQueries
    {
        public static string PlaceLimboBet = @"mutation LimboBet($amount:Float! $multiplierTarget:Float! $currency:CurrencyEnum! $identifier:String!){limboBet(amount:$amount currency:$currency multiplierTarget:$multiplierTarget identifier:$identifier){...CasinoBet state{...CasinoGameLimbo}}}fragment CasinoBet on CasinoBet{id active payoutMultiplier amountMultiplier amount payout updatedAt currency game user{id name balances{...UserBalanceFragment}}}fragment CasinoGameLimbo on CasinoGameLimbo{result multiplierTarget}fragment UserBalanceFragment on UserBalance{available{amount currency}vault{amount currency}}";
    }


}
