using AIO.Common.Response;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AIO.Common.Request
{




    public class APIClientManager
    {

        public CookieContainer cc;
        RestClient SharedRestClient;

        public string ApiUrl { get; private set; }
        public string URL { get; set; }
        public string Domain { get; set; }
        public string ApiKey { get; set; }

        public APIClientManager()
        {

        }

        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void CreateOrUseDefaultRestClient(bool dispose = false)
        {

            if (dispose == true)
            {
                SharedRestClient = null;
                cc = null;
            }

            if (SharedRestClient != null)
            {
                return;
            }

            ApiUrl = "https://api." + Domain + "/graphql";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            this.cc = new CookieContainer();

            SharedRestClient = new RestClient();
            SharedRestClient.BaseUrl = new Uri(ApiUrl);
            SharedRestClient.CookieContainer = this.cc;
            SharedRestClient.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";

        }

        private RestRequest CreateDefaultRestRequest(string apiKey)
        {
            RestRequest restRequest = new RestRequest(Method.POST);
            restRequest.AddHeader("authorization", string.Format("Bearer {0}", apiKey));
            restRequest.AddHeader("x-access-token", apiKey);
            restRequest.AddHeader("Content-type", "application/json");
            return restRequest;
        }

        public async Task<IRestResponse> Authorize()
        {

            try
            {

                BetQuery payload = new BetQuery
                {
                    operationName = "initialUserRequest",
                    variables = new BetClass() { },
                    query = "query initialUserRequest {\n  user {\n    ...UserAuth\n    __typename\n  }\n}\n\nfragment UserAuth on User {\n  id\n  name\n  email\n  hasPhoneNumberVerified\n  hasEmailVerified\n  hasPassword\n  intercomHash\n  createdAt\n  hasTfaEnabled\n  mixpanelId\n  hasOauth\n  isKycBasicRequired\n  isKycExtendedRequired\n  isKycFullRequired\n  kycBasic {\n    id\n    status\n    __typename\n  }\n  kycExtended {\n    id\n    status\n    __typename\n  }\n  kycFull {\n    id\n    status\n    __typename\n  }\n  flags {\n    flag\n    __typename\n  }\n  roles {\n    name\n    __typename\n  }\n  balances {\n    ...UserBalanceFragment\n    __typename\n  }\n  activeClientSeed {\n    id\n    seed\n    __typename\n  }\n  previousServerSeed {\n    id\n    seed\n    __typename\n  }\n  activeServerSeed {\n    id\n    seedHash\n    nextSeedHash\n    nonce\n    blocked\n    __typename\n  }\n  __typename\n}\n\nfragment UserBalanceFragment on UserBalance {\n  available {\n    amount\n    currency\n    __typename\n  }\n  vault {\n    amount\n    currency\n    __typename\n  }\n  __typename\n}\n"
                };

                CreateOrUseDefaultRestClient();

                var request = CreateDefaultRestRequest(ApiKey);

                request.AddJsonBody(payload);

                var restResponse = await SharedRestClient.ExecuteAsync(request);

                return restResponse;

            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<IRestResponse> SendToVault(string currency, decimal sentamount)
        {
            try
            {

                BetQuery payload = new BetQuery
                {
                    variables = new BetClass()
                    {
                        currency = currency.ToLower(),
                        amount = sentamount

                    },
                    query = "mutation CreateVaultDeposit($currency: CurrencyEnum!, $amount: Float!) {\n  createVaultDeposit(currency: $currency, amount: $amount) {\n    id\n    amount\n    currency\n    user {\n      id\n      balances {\n        available {\n          amount\n          currency\n          __typename\n        }\n        vault {\n          amount\n          currency\n          __typename\n        }\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n"
                };

                CreateOrUseDefaultRestClient();

                var request = CreateDefaultRestRequest(ApiKey);

                request.AddJsonBody(payload);

                var restResponse = await SharedRestClient.ExecuteAsync(request);

                return restResponse;

            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);

                return null;
            }
        }

        public async Task<IRestResponse> SendTip()
        {

            try
            {

                //BetQuery payload = new BetQuery
                //{
                //    variables = new BetClass()
                //    {
                //        seed = RandomString(10)

                //    },
                //    query = "mutation RotateSeedPair($seed: String!) {\n  rotateSeedPair(seed: $seed) {\n    clientSeed {\n      user {\n        id\n        activeClientSeed {\n          id\n          seed\n          __typename\n        }\n        activeServerSeed {\n          id\n          nonce\n          seedHash\n          nextSeedHash\n          __typename\n        }\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n"
                //};

                //CreateOrUseDefaultRestClient();

                //var request = CreateDefaultRestRequest(ApiKey);

                //request.AddJsonBody(payload);

                //var restResponse = await SharedRestClient.ExecuteAsync(request);

                //return restResponse;
                throw new NotImplementedException();

            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<IRestResponse> CheckBalance()
        {
            try
            {

                BetQuery payload = new BetQuery
                {
                    query = "query UserBalances {\n  user {\n    id\n    balances {\n      available {\n        amount\n        currency\n        __typename\n      }\n      vault {\n        amount\n        currency\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n"
                };

                CreateOrUseDefaultRestClient();

                var request = CreateDefaultRestRequest(ApiKey);

                request.AddJsonBody(payload);

                var restResponse = await SharedRestClient.ExecuteAsync(request);

                return restResponse;


            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
                return null;
            }

        }



        public async Task<IRestResponse> PlaceRouletteBet(string currencySelected)
        {
            try
            {

                BetQuery payload = new BetQuery
                {
                    variables = new BetClass()
                    {
                        //numbers = number.Count > 0 ? number : new List<Values> { },
                        //rows = row.Count > 0 ? row : new List<Values> { },
                        //colors = color.Count > 0 ? color : new List<Values> { },
                        //parities = parity.Count > 0 ? parity : new List<Values> { },
                        //ranges = range.Count > 0 ? range : new List<Values> { },
                        currency = currencySelected,
                        identifier = RandomString(21)

                    },
                    query = "mutation RouletteBet($currency: CurrencyEnum!, $colors: [RouletteBetColorsInput!], $numbers: [RouletteBetNumbersInput!], $parities: [RouletteBetParitiesInput!], $ranges: [RouletteBetRangesInput!], $rows: [RouletteBetRowsInput!], $identifier: String!) {\n  rouletteBet(\n    currency: $currency\n    colors: $colors\n    numbers: $numbers\n    parities: $parities\n    ranges: $ranges\n    rows: $rows\n    identifier: $identifier\n  ) {\n    ...CasinoBet\n    state {\n      ...RouletteStateFragment\n    }\n  }\n}\n\nfragment CasinoBet on CasinoBet {\n  id\n  active\n  payoutMultiplier\n  amountMultiplier\n  amount\n  payout\n  updatedAt\n  currency\n  game\n  user {\n    id\n    name\n  }\n}\n\nfragment RouletteStateFragment on CasinoGameRoulette {\n  result\n  colors {\n    amount\n    value\n  }\n  numbers {\n    amount\n    value\n  }\n  parities {\n    amount\n    value\n  }\n  ranges {\n    amount\n    value\n  }\n  rows {\n    amount\n    value\n  }\n}\n"
                };

                CreateOrUseDefaultRestClient();

                var request = CreateDefaultRestRequest(ApiKey);

                request.AddJsonBody(payload);

                var restResponse = await SharedRestClient.ExecuteAsync(request);

                return restResponse;

            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
                return null;
            }
        }
        public async Task<IRestResponse> PlaceKenoBet(BetQuery payload)
        {
            try
            {

                CreateOrUseDefaultRestClient();

                var request = CreateDefaultRestRequest(ApiKey);

                request.AddJsonBody(payload);

                var restResponse = await SharedRestClient.ExecuteAsync(request);

                return restResponse;

            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
                return null;
            }
        }
        public async Task<IRestResponse> PlaceLimboBet(BetQuery payload)
        {
            try
            {

                CreateOrUseDefaultRestClient();

                var request = CreateDefaultRestRequest(ApiKey);

                request.AddJsonBody(payload);

                var restResponse = await SharedRestClient.ExecuteAsync(request);

                return restResponse;

            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
                return null;
            }
        }
        public async Task<IRestResponse> PlaceDiceBet(BetQuery payload)
        {
            try
            {
                CreateOrUseDefaultRestClient();

                var request = CreateDefaultRestRequest(ApiKey);

                request.AddJsonBody(payload);

                var restResponse = await SharedRestClient.ExecuteAsync(request);

                return restResponse;

            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
                return null;
            }
        }




        public async Task<IRestResponse> ResetSeeds()
        {
            try
            {

                BetQuery payload = new BetQuery
                {
                    variables = new BetClass()
                    {
                        seed = RandomString(10)

                    },
                    query = "mutation RotateSeedPair($seed: String!) {\n  rotateSeedPair(seed: $seed) {\n    clientSeed {\n      user {\n        id\n        activeClientSeed {\n          id\n          seed\n          __typename\n        }\n        activeServerSeed {\n          id\n          nonce\n          seedHash\n          nextSeedHash\n          __typename\n        }\n        __typename\n      }\n      __typename\n    }\n    __typename\n  }\n}\n"
                };

                CreateOrUseDefaultRestClient();

                var request = CreateDefaultRestRequest(ApiKey);

                request.AddJsonBody(payload);

                var restResponse = await SharedRestClient.ExecuteAsync(request);


                return restResponse;


            }
            catch (Exception ex)
            {
                //luaPrint(ex.Message);
                return null;
            }

        }

    }



    public class GraphQLQueries
    {
        public static string PlaceLimboBet = @"mutation LimboBet($amount:Float! $multiplierTarget:Float! $currency:CurrencyEnum! $identifier:String!){limboBet(amount:$amount currency:$currency multiplierTarget:$multiplierTarget identifier:$identifier){...CasinoBet state{...CasinoGameLimbo}}}fragment CasinoBet on CasinoBet{id active payoutMultiplier amountMultiplier amount payout updatedAt currency game user{id name balances{...UserBalanceFragment}}}fragment CasinoGameLimbo on CasinoGameLimbo{result multiplierTarget}fragment UserBalanceFragment on UserBalance{available{amount currency}vault{amount currency}}";
    }


}
